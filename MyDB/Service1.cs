using MyDB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyDB
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public int Create(string fname, string lname, string email, string password, string cno, int cvv)
        {
            Account account = new Account(fname, lname, email, password, cno, cvv);
            return account.Create();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool ValidateAccount(string email, string password)
        {
            Account account = new Account();
            return account.ValidateAccount(email, password);
        }

        public bool ValidatePasswords(string email, string password)
        {
            Account account = new Account();
            return account.ValidatePasswords(email, password);
        }
    }
}
