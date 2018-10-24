using System;
using System.Data;
using System.Data.SqlClient;

namespace Bus_Management_System
{
    public class VerifyLayer
    {
        DataAccessLayer dal = new DataAccessLayer();
        SqlCommand sqlcmd = new SqlCommand();

        public string imię { get; set; }
        public string nazwisko { get; set; }
        public string pesel { get; set; }

        public Boolean VerifyUser(ref int iD)
        {
            string sqlQuery = "SELECT Id FROM Employees_Basic WHERE Employee_Imie = @userName AND Employee_Nazwisko = @user2ndName AND Employee_PESEL = @userPESEL";
            sqlcmd.CommandText = sqlQuery;
            sqlcmd.Parameters.AddWithValue("@userName", imię);
            sqlcmd.Parameters.AddWithValue("@user2ndName", nazwisko);
            sqlcmd.Parameters.AddWithValue("@userPESEL", pesel);
            bool istnieje;

            try
            {
                DataTable dt = dal.GetDataTable(sqlcmd);
                if (dt.Rows.Count > 0)
                {
                    iD = (int)dt.Rows[0][0];
                    istnieje = true;
                    return istnieje;
                }
                else
                {
                    iD = 0;
                    istnieje = false;
                    return istnieje;
                }
            }
            catch 
            {
                return istnieje = false;
            }
        }
    }
}