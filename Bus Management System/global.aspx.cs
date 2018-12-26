using System;
using System.Data;
using System.Web;

namespace Bus_Management_System
{
    public partial class global : System.Web.UI.Page
    {
        private BusinessLayer bl = new BusinessLayer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            Server.ClearError();
        }

        // zresetowanie kontrolek na panelu logowania
        protected void Bt_resetLogin_Click(object sender, EventArgs e)
        {
            ClearTextbox();
            inp_name.Focus();
        }

        // weryfikacja logowania
        protected void Bt_submitLogin_Click(object sender, EventArgs e)
        {
            VerifyLayer vl = new VerifyLayer
            {
                Imię = inp_name.Text.Trim(),
                Nazwisko = inp_2ndName.Text.Trim(),
                Pesel = inp_pesel.Text.Trim()
            };
            int iD = 0;
            //int CompanyId = 0;

            // user nie istnieje, albo wprowadzono dane z błedem
            if (!bl.VerifyUser(vl, ref iD))
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
                    //User loggedUser = new User
                    //{
                    //    Id = iD,
                    //    CompanyId = Convert.ToInt32(loggedUserData.Rows[0][0]),
                    //    FirstName = (string)loggedUserData.Rows[0][1],
                    //    LastName = (string)loggedUserData.Rows[0][2],
                    //    AdminPrivileges = Convert.ToInt32(loggedUserData.Rows[0][3]),
                    //    Interval = 0,
                    //    Bus = "",
                    //    OperationStatus = 0
                    //};
                    //string sessionName = (string)loggedUserData.Rows[0][0];
                    //Session[sessionName] = loggedUser;
                    Session["Name"] = loggedUserData.Rows[0][0].ToString();
                    Session["FirstName"] = loggedUserData.Rows[0][1].ToString();
                    Session["LastName"] = loggedUserData.Rows[0][2].ToString();
                    Session["AdminPrivileges"] = loggedUserData.Rows[0][3].ToString();
                    Session["Interval"] = 0;
                    Session["Bus"] = "";
                    Session["Id"] = iD.ToString();
                    Session["OperationStatus"] = 0;


                    // tworzenie ciasteczka z danymi aktualnie zalogowanego operatora
                    // sprawdzenie, czy przypadkiem takie ciasteczko już nie zostało stworzone, 
                    // bo jeśli tak - to kasujemy je!
                    //if (Request.Cookies["Bus"] != null)
                    //{
                    //    Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                    //}

                    DateTime loginDate = DateTime.Now;
                    //CreateNewBusCookie(sessionName, iD, loginDate);

                    // wprowadzenie danych o zalogowaniu operatora do bazy i wywołanie odpowiedniego panelu
                    if (bl.UserLogIn(iD, loginDate))
                    {
                        HomeView((string)Session["AdminPrivileges"]);
                    }
                        
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

        // ustalenie który panel należy wyświetlić i przekierowanie
        private void HomeView(string AdminPrivileges)
        {
            if (Convert.ToInt32(AdminPrivileges) == 1)
                Response.Redirect("Alocator.aspx");
            else
            if (Convert.ToInt32(AdminPrivileges) == 2)
                Response.Redirect("Bus.aspx");
            else
            {
                //// odczytano nieznane poświadczenia
                //if (Request.Cookies["Bus"] != null)
                //{
                //    Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                //}
                Session.Abandon();
                Response.Redirect("Global.aspx");
            }
        }

        // wyczyszczenie textboxów na panelu logowania
        private void ClearTextbox()
        {
            inp_name.Text = "";
            inp_2ndName.Text = "";
            inp_pesel.Text = "";
        }

        // utworzenie ciasteczka o zalogowanym użytkowniku
        //private void CreateNewBusCookie(string sessionName, int iD, DateTime loginDate)
        //{
        //    HttpCookie BusCookie = new HttpCookie("Bus");
        //    BusCookie.Values["userId"] = sessionName;
        //    BusCookie.Values["Id"] = iD.ToString();
        //    BusCookie.Values["loginTime"] = loginDate.ToString();
        //    // domyslnie cookie zaniknie po wyłączeniu przeglądarki, ae ustawiamy na 8h ze względu na kodeks pracy
        //    BusCookie.Expires = loginDate.AddHours(8);
        //    Response.Cookies.Add(BusCookie);
        //}
    }
}