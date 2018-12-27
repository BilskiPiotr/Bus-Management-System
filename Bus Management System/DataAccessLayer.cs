using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bus_Management_System
{
    public class DataAccessLayer
    {
        //private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BusData"].ConnectionString);
        string connectionString = ConfigurationManager.ConnectionStrings["BusData"].ConnectionString;
        public void GetDataTable(SqlCommand sqlCmd, ref DataTable dt)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                sqlCmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
                {
                    sda.Fill(dt);
                    con.Close();
                    sda.Dispose();
                }
            }
        }

        public void GetDataSet(SqlCommand sqlCmd, ref DataSet ds)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                sqlCmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
                {
                    sda.Fill(ds);
                    con.Close();
                    sda.Dispose();
                }
            }
        }

        public int InsertExecution(SqlCommand sqlcmd)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                sqlcmd.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int value = (int)sqlcmd.ExecuteScalar();
                con.Close();
                con.Dispose();
                return value;
            }
        }

        public void QueryExecution(SqlCommand sqlcmd)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                sqlcmd.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                    sqlcmd.ExecuteNonQuery();
                    con.Close();
                con.Dispose();
            }
        }
    }
}