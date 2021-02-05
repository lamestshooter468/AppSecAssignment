using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSecAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MyDB = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        MyDBServiceReference.Service1Client client = new MyDBServiceReference.Service1Client();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void registerBtn_Click(object sender, EventArgs e)
        {
            int result = client.Create(tb_fname.Text.ToString(), tb_lname.Text.ToString(), tb_email.Text.ToString(), tb_pass.Text.ToString(), tb_CNo.Text.ToString(), Convert.ToInt16(tb_CVV.Text.ToString()));
            if (result == 1)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                lbl_error.Text = "An error has occurred while creating your account. Please contact administrators.";
                lbl_error.Visible = true;
            }
        }
    }
}