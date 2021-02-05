using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSecAssignment
{
    public partial class Home : System.Web.UI.Page
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
                else
                {
                    Name.Text = "Welcome " + client.getName(Session["LoggedIn"].ToString());
                }
            }
            else
            {
                Response.Redirect("Login.aspx?YouTried=ToXSS", false);
            }
        }

        protected void logOutBtn_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["Authentication Token"] != null)
            {
                Response.Cookies["Authentication Token"].Value = string.Empty;
                Response.Cookies["Authentication Token"].Expires = DateTime.Now.AddMonths(-20);
            }
            Response.Redirect("Login.aspx", true);
        }

        protected void chgPassBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", false);
        }
    }
}