using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSecAssignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        MyDBServiceReference.Service1Client client = new MyDBServiceReference.Service1Client();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["Authentication Token"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["Authentication Token"].Value))
                {
                    Response.Redirect("Login.aspx?YouTried=ToXSS", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx?YouTried=ToXSS", false);
            }
        }

        protected void ChgedPass_Click(object sender, EventArgs e)
        {
            if (new_pass.Text == pass_retype.Text)
            {
                if (client.ValidateAccount(Session["LoggedIn"].ToString(), curr_pass.Text))
                {
                    if (client.ValidatePasswords(Session["LoggedIn"].ToString(), new_pass.Text))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Changed", "alert('Password Changed Successfully)", true);
                        Response.Redirect("Home.aspx?", true);
                    }
                    else
                    {
                        errorLbl.Text = "New password cannot match previous 2 passwords.";
                        errorLbl.Visible = true;
                    }
                }
                else
                {
                    errorLbl.Text = "Current password is incorrect.";
                    errorLbl.Visible = true;
                }
            }
            else
            {
                errorLbl.Text = "New passwords do not match.";
                errorLbl.Visible = true;
            }
        }
    }
}