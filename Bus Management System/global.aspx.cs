using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class global : System.Web.UI.Page
    {
        BusinessLayer bl = new BusinessLayer();
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


        // weryfikacja logowania
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

            // sprawdzenie poświadczeń
            istnieje = vl.VerifyUser(ref iD);


            // user nie istnieje, albo wprowadzono dane z błedem
            if (!istnieje)
            {
                ClearTextbox();
                lb_errorMsg.Visible = true;
                lb_errorMsg.Text = "Błędny Login lub Hasło! ";
            }
            // user istnieje - wiedz tworzenie danych sesji
            else
            {
                DataTable loggedUserData = bl.GetUserData(iD);

                // Konstruktor obiektu SESJA
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


                    // tworzenie ciasteczka z danymi aktualnie zalogowanego operatora
                    // sprawdzenie, czy ptzypadkiem takie ciasteczko już nie zostało stworzone, 
                    // bo jeśli tak - to kasujemy je!
                    if (Request.Cookies["Bus"] != null)
                    {
                        Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                    }

                    DateTime loginDate = DateTime.Now;
                    CreateNewBusCookie(sessionName, iD, loginDate);

                    // wprowadzenie danych o zalogowaniu operatora do bazy i wywołanie odpowiedniego panelu
                    // to jeszcze trzeba przekonfigurować, żeby całkowicie pominąć busPanel
                    if (bl.UserLogIn(iD, loginDate))
                        Response.Redirect("busPanel.aspx");
                    // w przypadku błędu dodania do bazy informacji o zalogowaniu użytkownika 
                    // skasowanie ciasteczka i zamknięcie sesji - co jest jednoznaczne z wylogowaniem
                    else
                    {
                        if (Request.Cookies["Bus"] != null)
                        {
                            Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                        }
                        Session.Abandon();
                        Response.Redirect("global.aspx");
                    }
                }
                else
                {
                    ClearTextbox();
                    Response.Write("<script> alert('Błąd - proszę wprowadzić inne poświadczenia' ) </script>");
                }
            }
        }


        private void ClearTextbox()
        {
            inp_name.Text = "";
            inp_2ndName.Text = "";
            inp_pesel.Text = "";
        }


        private void CreateNewBusCookie(string sessionName, int iD, DateTime loginDate)
        {
            HttpCookie BusCookie = new HttpCookie("Bus");
            BusCookie.Values["userId"] = sessionName;
            BusCookie.Values["Id"] = iD.ToString();
            // dodać gdzieś sprawdzanie ile czasu już upłyneło
            BusCookie.Values["loginTime"] = loginDate.ToString();
            BusCookie.Values["busNb"] = "";
            BusCookie.Expires = DateTime.Now.AddHours(8);
            Response.Cookies.Add(BusCookie);
        }



    }
}