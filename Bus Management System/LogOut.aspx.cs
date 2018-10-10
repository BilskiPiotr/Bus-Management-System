using System;
using System.Web;

namespace Bus_Management_System
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BusinessLayer bl = new BusinessLayer();

                HttpCookie cookie = Request.Cookies["Bus"];
                if (cookie != null)
                {
                    bl.UserLogOut(cookie);
                    Session.Abandon();
                    Response.Redirect("global.aspx");
                }
            }

        }
    }
}