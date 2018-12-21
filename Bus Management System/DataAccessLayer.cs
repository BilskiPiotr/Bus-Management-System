using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bus_Management_System
{
    public class DataAccessLayer
    {
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BusData"].ConnectionString);

        public DataTable GetDataTable(SqlCommand sqlCmd)
        {
            sqlCmd.Connection = conn;
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataSet GetDataSet(SqlCommand sqlCmd)
        {
            sqlCmd.Connection = conn;
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            conn.Close();
            return ds;
        }

        public byte[] GetByteData(SqlCommand sqlCmd)
        {
            sqlCmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            byte[] bt = (byte[])sqlCmd.ExecuteScalar();
            conn.Close();
            return bt;

        }

        public int InsertExecution(SqlCommand sqlcmd)
        {
            sqlcmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            int value = (int)sqlcmd.ExecuteScalar();
            conn.Close();
            return value;
        }

        public void QueryExecution(SqlCommand sqlcmd)
        {
            sqlcmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            sqlcmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}