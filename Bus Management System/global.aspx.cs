using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class global : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            Server.ClearError();
        }

        protected void Bt_resetLogin_Click(object sender, EventArgs e)
        {
            inp_name.Text = "";
            inp_2ndName.Text = "";
            inp_pesel.Text = "";
            inp_name.Focus();
        }

        protected void Bt_submitLogin_Click(object sender, EventArgs e)
        {
            VerifyLayer vl = new VerifyLayer
            {
                imię = inp_name.Text,
                nazwisko = inp_2ndName.Text,
                pesel = inp_pesel.Text
            };
            bool istnieje = false;
            int iD = 0;
            string errorMsg = "";

            istnieje = vl.VerifyUser(ref iD, ref errorMsg);

            if (!istnieje)
            {
                ClearTextbox();
                lb_errorMsg.Visible = true;
                lb_errorMsg.Text = "Błędny Login lub Hasło! ";
            }
            else
            {
                LoggedUser loggedUser = new LoggedUser();
                DataTable loggedUserData = new DataTable();
                loggedUserData = loggedUser.GetLoggedUserData(iD, ref errorMsg);

                // Session object constructor
                if (loggedUserData.Rows.Count > 0)
                {
                    User user = new User
                    {
                        EmployeeId = iD,
                        CompanyId = Convert.ToInt32(loggedUserData.Rows[0][0]),
                        FirstName = (string)loggedUserData.Rows[0][1],
                        LastName = (string)loggedUserData.Rows[0][2],
                        AdminPrivileges = Convert.ToInt32(loggedUserData.Rows[0][3])
                    };
                    string sessionName = (string)loggedUserData.Rows[0][0];
                    Session[sessionName] = user;

                    HttpCookie BusCookie = new HttpCookie("BusManagement");
                    BusCookie.Values["userId"] = sessionName;
                    BusCookie.Values["Id"] = iD.ToString();
                    BusCookie.Values["lastVisit"] = DateTime.Now.ToString();
                    BusCookie.Expires = DateTime.Now.AddHours(8);
                    Response.Cookies.Add(BusCookie);

                    Response.Redirect("busPanel.aspx");
                }
                else
                {
                    ClearTextbox();
                    lb_errorMsg.Visible = true;
                    lb_errorMsg.Text = "Błąd dostępu do danych, proszę podać innedane uwieżytelnienia ";
                }
            }
        }


        private void ClearTextbox()
        {
            inp_name.Text = "";
            inp_2ndName.Text = "";
            inp_pesel.Text = "";
        }
    }
}