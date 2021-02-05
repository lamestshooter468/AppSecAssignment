using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSecAssignment
{
    public partial class Login : System.Web.UI.Page
    {
        int attempts = 0;
        MyDBServiceReference.Service1Client client = new MyDBServiceReference.Service1Client();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["YouTried"] != null)
            {
                if (Request.QueryString["YouTried"] == "ToXSS")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "TriedToXSS", "alert('How about you login first yea?')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "TriedToXSS", "alert('Try something different.')", true);
                }
            }
        }
        public class MyObject
        {
            public string success { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-captcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=&response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject myObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(myObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            if (attempts >= 3)
            {
                lbl_error.Text = "You have typed the wrong email or password 3 times. Your account has been locked out. Please try again later.";
            }
            else
            {
                if (ValidateCaptcha())
                {
                    if (client.ValidateAccount(tb_email.Text, tb_pass.Text))
                    {
                        Session["LoggedIn"] = tb_email.Text.ToString();
                        string guID = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guID;
                        Response.Cookies.Add(new HttpCookie("Authentication Token", guID));
                        Response.Redirect("Home.aspx", false);
                    }
                    else
                    {
                        lbl_error.Text = "Your email or password is incorrect";
                        lbl_error.Visible = true;
                        attempts += 1;
                    }
                }
                else
                {
                    attempts += 1;
                    lbl_error.Text = "Google has detected a bot.";
                    lbl_error.Visible = true;
                }
            }
            
        }
    }
}