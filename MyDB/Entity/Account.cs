using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyDB.Entity
{
    public class Account
    {
        string fname { get; set; }
        string lname { get; set; }
        string email { get; set; }
        DateTime dob { get; set; }
        string password { get; set; }
        string cno { get; set; }
        int cvv { get; set; }
        public Account() { }
        public Account(string Fname, string Lname, string Email, string Password, string Cno, int CVV)
        {
            fname = Fname;
            lname = Lname;
            email = Email;
            password = Password;
            cno = Cno;
            cvv = CVV;
        }

        public int Create()
        {
            int result = 0;
            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltbyte = new byte[8];
            rng.GetBytes(saltbyte);
            string salt = Convert.ToBase64String(saltbyte);

            SHA512Managed hashing = new SHA512Managed();

            string pwd = password + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            string finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            byte[] Key = cipher.Key;
            byte[] IV = cipher.IV;
            try
            {
                using (SqlConnection con = new SqlConnection(MyDB))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account (FName, LName, Email, PasswordHash, PasswordSalt, Dob, CNo, CVV) VALUES(@FName,@LName, @Email, @PasswordHash, @PasswordSalt, @DoB, @CNo, @CVV)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FName", fname);
                            cmd.Parameters.AddWithValue("@LName", lname);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DoB", DateTime.Now);
                            cmd.Parameters.AddWithValue("@CNo", encryptData(cno, Key, IV));
                            cmd.Parameters.AddWithValue("@CVV", encryptData(cvv.ToString(), Key, IV));
                            cmd.Connection = con;
                            con.Open();
                            SqlCommand sqlCmd = new SqlCommand("INSERT INTO Pass_Hist (Id) VALUES ((SELECT Id FROM Account WHERE Email=@Email))");
                            sqlCmd.Parameters.AddWithValue("@Email", email);
                            sqlCmd.Connection = con;
                            result = cmd.ExecuteNonQuery();
                            sqlCmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }
        public byte[] encryptData(string data, byte[] Key, byte[] IV)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
        public bool ValidateAccount(string email, string password)
        {
            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(MyDB);
            string cmd = "SELECT Count(*) FROM ACCOUNT WHERE Email=@Email";
            SqlCommand sqlCmd = new SqlCommand(cmd, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Email", email);

            int ans = 0;

            try
            {
                sqlConn.Open();
                int result = (Int32)sqlCmd.ExecuteScalar();
                if (result == 1)
                {
                    string dbHash = get_hash(email, "PasswordHash", true);
                    string dbSalt = get_salt(email, "PasswordSalt", true);
                    SHA512Managed hashing = new SHA512Managed();

                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = password + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (userHash == dbHash)
                        {
                            ans = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                sqlConn.Close();
            }
            return Convert.ToBoolean(ans);
        }
        public string get_hash(string email, string ToTake, bool valid)
        {
            string ans = "";
            string cmd;

            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(MyDB);
            if (valid)
            {
                cmd = "SELECT " + ToTake + "  FROM ACCOUNT WHERE Email=@Email";
            }
            else
            {

                cmd = "SELECT " + ToTake + " FROM Pass_hist WHERE Id=@id";
            }
            SqlCommand sqlCmd = new SqlCommand(cmd, sqlConn);

            if (valid)
            {
                sqlCmd.Parameters.AddWithValue("@Email", email);
            }
            else
            {
                sqlConn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT Id FROM Account WHERE Email=@Email", sqlConn);
                cmd1.Parameters.AddWithValue("@Email", email);
                sqlCmd.Parameters.AddWithValue("@Id", cmd1.ExecuteScalar().ToString());
                sqlConn.Close();
            }

            try
            {
                sqlConn.Open();
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[ToTake] != null && reader[ToTake] != DBNull.Value)
                    {
                        ans = reader[ToTake].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                sqlConn.Close();
            }

            return ans;
        }
        public string get_salt(string email, string ToTake, bool valid)
        {
            string ans = "";
            string cmd;

            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(MyDB);

            if (valid)
            {
                cmd = "SELECT " + ToTake + "  FROM ACCOUNT WHERE Email=@Email";
            }
            else
            {
                
                cmd = "SELECT " + ToTake + " FROM Pass_hist WHERE Id=@id";
            }
            SqlCommand sqlCmd = new SqlCommand(cmd, sqlConn);
            
            if (valid)
            {
                sqlCmd.Parameters.AddWithValue("@Email", email);
            }
            else
            {
                sqlConn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT Id FROM Account WHERE Email=@Email", sqlConn);
                cmd1.Parameters.AddWithValue("@Email", email);
                sqlCmd.Parameters.AddWithValue("@Id", cmd1.ExecuteScalar().ToString());
                sqlConn.Close();
            }

            try
            {
                sqlConn.Open();
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[ToTake] != null && reader[ToTake] != DBNull.Value)
                    {
                        ans = reader[ToTake].ToString();
                    }
                    else
                    {
                        ans = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                sqlConn.Close();
            }

            return ans;
        }
        public bool ValidatePasswords(string email, string password)
        {
            bool result = false;
            bool flag = false;
            string id;

            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection myConn = new SqlConnection(MyDB);
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Account WHERE Email=@Email", myConn);

            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                myConn.Open();
                id = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                myConn.Close();
            }
            if (id != null)
            {
                string pass1salt = get_salt(email, "Pass1Salt", false);
                string pass1hash = get_hash(email, "Pass1Hash", false);
                string pass2salt = get_salt(email, "Pass2Salt", false);
                string pass2hash = get_hash(email, "Pass2Hash", false);
                SHA512Managed hashing = new SHA512Managed();

                if (pass1salt != null && pass1hash != null)
                {
                    if (pass2salt != null && pass2hash != null)
                    {
                        flag = true;
                        result = false;
                        string pwd1WithSalt = password + pass1salt;
                        byte[] hash1WithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd1WithSalt));
                        string user1Hash = Convert.ToBase64String(hash1WithSalt);

                        string pwd2WithSalt = password + pass2salt;
                        byte[] hash2WithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd2WithSalt));
                        string user2Hash = Convert.ToBase64String(hash2WithSalt);

                        if (user1Hash != pass1hash && user2Hash != pass2hash)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        string pwdWithSalt = password + pass1salt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash != pass1hash)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }

            int updresult = UpdPassHist(id, email, password, get_salt(email, "PasswordSalt", true), get_hash(email, "PasswordHash", true), flag);

            result = Convert.ToBoolean(updresult);
            return result;
        }
        public int UpdPassHist(string id, string email, string new_pass, string pass1salt, string pass1hash, bool flag)
        {
            string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection myConn = new SqlConnection(MyDB);
            int result;

            if (flag == true)
            {
                string new_pass2salt = get_salt(email, "Pass1Salt", false);
                string new_pass2hash = get_hash(email, "Pass1Hash", false);

                SqlCommand extracmd = new SqlCommand("UPDATE Pass_hist SET Pass2Salt=@Pass2Salt, Pass2Hash=@Pass2Hash WHERE Id=@Id", myConn);

                extracmd.Parameters.AddWithValue("@Pass2Salt", new_pass2salt);
                extracmd.Parameters.AddWithValue("@Pass2Hash", new_pass2hash);
                extracmd.Parameters.AddWithValue("@Id", Convert.ToInt16(id));

                try
                {
                    myConn.Open();
                    result = extracmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    myConn.Close();
                }
            }
            SqlCommand cmd = new SqlCommand("UPDATE Pass_hist SET Pass1Salt=@Pass1Salt, Pass1Hash=@Pass1Hash WHERE Id=@Id", myConn);

            cmd.Parameters.AddWithValue("@Pass1Salt", pass1salt);
            cmd.Parameters.AddWithValue("@Pass1Hash", pass1hash);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt16(id));

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltbyte = new byte[8];
            rng.GetBytes(saltbyte);
            string salt = Convert.ToBase64String(saltbyte);

            SHA512Managed hashing = new SHA512Managed();

            string pwd = new_pass + salt;
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            string finalHash = Convert.ToBase64String(hashWithSalt);

            SqlCommand newcmd = new SqlCommand("UPDATE Account SET PasswordSalt=@PasswordSalt, PasswordHash=@PasswordHash WHERE Email=@Email", myConn);
            newcmd.Parameters.AddWithValue("@PasswordSalt", salt);
            newcmd.Parameters.AddWithValue("@PasswordHash", finalHash);
            newcmd.Parameters.AddWithValue("@Email", email);


            try
            {
                myConn.Open();
                result = cmd.ExecuteNonQuery();
                result = newcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                myConn.Close();
            }

            return result;
        }
    }
}
