using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class LoggedUser
    {

        DataAccessLayer dal = new DataAccessLayer();
        SqlCommand sqlcmd = new SqlCommand();

        public DataTable getLoggedUserData(int iD, ref string errorMsg)
        {
            string sqlQuery = "SELECT Employee_CompanyId, Employee_Imie, Employee_Nazwisko, Employee_Priv FROM Employees_Basic WHERE Id = @userId";
            sqlcmd.CommandText = sqlQuery;
            sqlcmd.Parameters.AddWithValue("@userId", iD);
            errorMsg = "";

            try
            {
                DataTable dt = dal.GetDataTable(sqlcmd);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    errorMsg = "Brak danych dotyczących szukanego urzytkownika";
                    return dt;
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Błąd podczas pobierania danych o zweryfikowanym użytkowniku! : (" + ex + ")";
                return null;
            }
        }
    }
}