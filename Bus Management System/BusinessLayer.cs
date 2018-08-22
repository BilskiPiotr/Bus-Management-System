using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class BusinessLayer
    {
        DataAccessLayer dal = new DataAccessLayer();
         
        public int OperationType { get; set; }
        public string FlightNb { get; set; }
        public string PaxCount { get; set; }
        public string RadioGate { get; set; }
        public string RadioNeon { get; set; }
        public string PPSNb { get; set; }
        public string Port { get; set; }
        public string GateNb { get; set; }
        public string[] BusSelected { get; set; }
        


        public void ClearFields(BusinessLayer bl)
        {

        }

        public DataSet GetGates()
        {
            string sqlQuery = "SELECT Id, GateNb FROM Gates";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataSet GetStations()
        {
            string sqlQuery = "SELECT Id, StationNb FROM Stations";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataSet GetAirPort()
        {
            string sqlQuery = "SELECT Id, IATA_Name FROM AirPorts";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataTable GetBus()
        {
            string sqlQuery = "SELECT Status, VehicleNb FROM Vehicles";
            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            dt = dal.GetDataTable(sqlCmd);

            return dt;
        }
    }
}