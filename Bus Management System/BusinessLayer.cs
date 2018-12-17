using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public class BusinessLayer
    {
        private DataAccessLayer dal = new DataAccessLayer();
        private SqlCommand sqlCmd = new SqlCommand();


        public DataTable GetUserData(int iD)
        {
            sqlCmd.CommandText = "SELECT Employee_CompanyId, Employee_Imie, Employee_Nazwisko, Employee_Priv FROM Employees_Basic WHERE Id = @userId";
            sqlCmd.Parameters.AddWithValue("@userId", iD);
            try
            {
                DataTable dt = dal.GetDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // zapisanie w systemie informacji o zalogowaniu użytykownika
        public bool UserLogIn(int iD, DateTime loginDate)
        {
            sqlCmd.CommandText = "INSERT INTO Employees_Status (Employee_Id, Employee_Login) VALUES (@iD, @loginDate)";
            sqlCmd.Parameters.AddWithValue("@iD", iD);
            sqlCmd.Parameters.AddWithValue("@loginDate", loginDate);
            try
            {
                dal.QueryExecution(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UserLogOut(HttpCookie cookie)
        {
            bool result = false;
            sqlCmd.CommandText = "INSERT INTO Employees_Status (Employee_Id, Employee_Logout) VALUES (@userId, @logOut)";
            sqlCmd.Parameters.AddWithValue("@userId", Convert.ToInt32(cookie.Values["Id"]));
            sqlCmd.Parameters.AddWithValue("@logOut", DateTime.Now);
            try
            {
                dal.QueryExecution(sqlCmd);
                result = true;
            }
            catch (Exception ex)
            {
                return false;
            }

            if (result)
            {
                sqlCmd.CommandText = "UPDATE VEHICLES SET Bus_Status = @busStatus WHERE VehicleNb = @busNb";
                sqlCmd.Parameters.AddWithValue("@busStatus", 1);
                sqlCmd.Parameters.AddWithValue("@busNb", cookie.Values["busNb"].ToString());
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.CommandText = "";
                    sqlCmd.Parameters.Clear();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        // pobranie listy zdefiniowanych bramek pasażerskich
        public DataSet GetGates()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT Id, GateNb FROM Gates";
            try
            {
                ds = dal.GetDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
            }
            return ds;
        }


        // pobranie informacji specyficznych o wyjściu pasażerskim dla panelu Operatora
        public DataSet GetGate(int gate)
        {
            sqlCmd.CommandText = "SELECT * FROM Gates WHERE Id = @gateId";
            sqlCmd.Parameters.AddWithValue("@gateId", gate);
            try
            {
                DataSet ds = dal.GetDataSet(sqlCmd);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // pobranie listy zdefiniowanych stanowisk postojowych samolotów
        public DataSet GetStations()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT Id, StationNb FROM Stations";
            try
            {
                ds = dal.GetDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        // pobranie listy zdefiniowanych portów lotniczych
        public DataSet GetAirPort()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT Id, IATA_Name FROM AirPorts";
            try
            {
                ds = dal.GetDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        // pobranie listy zdefiniowanych autobusów
        public DataSet GetBus()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT * FROM Vehicles";
            try
            {
                ds = dal.GetDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        // pobieranie listy dostępnych autobusów
        public DataSet GetIdleBusList(int commandTextNumber, string bus)
        {
            DataSet ds = new DataSet();
            switch (commandTextNumber)
            {
                case 1:
                    {
                        sqlCmd.CommandText = "SELECT Id, VehicleNb FROM Vehicles WHERE Bus_Status = @busStatus AND Work_Status = @workStatus";
                        sqlCmd.Parameters.AddWithValue("@busStatus", 2);
                        sqlCmd.Parameters.AddWithValue("@workStatus", 0);
                    }
                    break;
                case 2:
                    {
                        sqlCmd.CommandText = "SELECT Id, VehicleNb FROM Vehicles WHERE VehicleNb = @bus";
                        sqlCmd.Parameters.AddWithValue("@bus", bus);
                    }
                    break;
            }
            try
            {
                ds = dal.GetDataSet(sqlCmd);
                sqlCmd.CommandText = "";
                sqlCmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        // pobranie informacji o zleceniu dla panelu Operator
        public DataSet GetOperations(string bus)
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT * FROM Operations WHERE Bus=(SELECT Id FROM Vehicles WHERE VehicleNb = @busNb) AND Finished = @finished";
            sqlCmd.Parameters.AddWithValue("@busNb", bus);
            sqlCmd.Parameters.AddWithValue("@finished", 0);
            try
            {
                ds = dal.GetDataSet(sqlCmd);
                return ds;
            }
            catch(Exception ex)
            {
                return ds;
            }
        }
        
        // dodanie nowej operacji
        public Boolean AddNewOperation(NewOperation newOp)
        {
            DateTime addOpDate = DateTime.Now;
            DateTime zeroDate = new DateTime(1999, 01, 01);

            bool success = true;

            sqlCmd.CommandText = "INSERT INTO Operations (Employee_Id, Operation, GodzinaRozkladowa, FlightNb, AirPort, " +
                                                             "Pax, Gate, PPS, Bus, RadioGate, RadioNeon, Created, Accepted, " +
                                                             "StartLoad, StartDrive, StartUnload, EndOp, Finished) " +
                                                     "VALUES (@employee_Id, @operation, @godzinaRozkladowa, @flightNb, @airPort, " +
                                                             "@pax, @gate, @pps, @bus, @radioGate, @radioNeon, @created, @accepted, " +
                                                             "@startLoad, @startDrive, @startUnload, @endOp, @finished)";
            sqlCmd.Parameters.AddWithValue("@employee_Id", 2);
            sqlCmd.Parameters.AddWithValue("@operation", newOp.Operation);
            sqlCmd.Parameters.AddWithValue("@godzinaRozkladowa", newOp.GodzinaRozkładowa);
            sqlCmd.Parameters.AddWithValue("@flightNb", newOp.FlightNb);
            sqlCmd.Parameters.AddWithValue("@airPort", newOp.AirPort);
            sqlCmd.Parameters.AddWithValue("@pax", newOp.Pax);
            sqlCmd.Parameters.AddWithValue("@gate", newOp.Gate);
            sqlCmd.Parameters.AddWithValue("@pps", newOp.PPS);
            sqlCmd.Parameters.AddWithValue("@bus", newOp.Bus);
            sqlCmd.Parameters.AddWithValue("@radioGate", newOp.RadioGate);
            sqlCmd.Parameters.AddWithValue("@radioNeon", newOp.RadioNeon);
            sqlCmd.Parameters.AddWithValue("@created", addOpDate);
            sqlCmd.Parameters.AddWithValue("@accepted", zeroDate);
            sqlCmd.Parameters.AddWithValue("@startLoad", zeroDate);
            sqlCmd.Parameters.AddWithValue("@startDrive", zeroDate);
            sqlCmd.Parameters.AddWithValue("@startUnload", zeroDate);
            sqlCmd.Parameters.AddWithValue("@endOp", zeroDate);
            sqlCmd.Parameters.AddWithValue("@finished", 0);

            try
            {
                dal.QueryExecution(sqlCmd);
            }
            catch (Exception ex)
            {
                success = false;
            }

            if (success)
            {
                sqlCmd.CommandText = "UPDATE Vehicles SET Work_Status = 1 WHERE VehicleNb = @busNb";
                sqlCmd.Parameters.AddWithValue("@busNb", newOp.BusNb);
                try
                {
                    dal.QueryExecution(sqlCmd);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        // pobranie danych specyficznych o stanowisku postojowym dla panelu Operatora
        public DataSet GetPPS(int pps)
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT * FROM Stations WHERE Id = @ppsNb";
            sqlCmd.Parameters.AddWithValue("@ppsNb", pps);
            try
            {
                ds = dal.GetDataSet(sqlCmd);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        // pobranie informacji o porcie lotniczym do panelu Operatora
        public string GetAirPort(int airPort, ref int shengen, ref string fullName, ref string country)
        {
            sqlCmd.CommandText = "SELECT a.IATA_Name, a.Full_Name, b.Country_name, b.Shengen FROM AirPorts AS a " +
                                 "INNER JOIN Countries AS b ON a.Country_Id=b.Id WHERE a.Id = @airPortNb ";
            sqlCmd.Parameters.AddWithValue("@airPortNb", airPort);
            try
            {
                DataSet ds = dal.GetDataSet(sqlCmd);
                shengen = Convert.ToInt32(GetCountry(ds.Tables[0].Rows[0].Field<int>("Shengen")));
                return ds.Tables[0].Rows[0].Field<string>("IATA_Name");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        // ustalenie strefy bezpieczeństwa
        public int GetCountry(int country)
        {
            sqlCmd.CommandText = "SELECT Shengen FROM Countries WHERE Id = @countryId";
            sqlCmd.Parameters.AddWithValue("@countryId", country);
            try
            {
                DataSet ds = dal.GetDataSet(sqlCmd);
                return ds.Tables[0].Rows[0].Field<int>("Shengen");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // sprawdzenie poświadczeń wprowadzonych przez użytkownika
        public Boolean VerifyUser(VerifyLayer vl, ref int iD)
        {
            sqlCmd.CommandText = "SELECT Id FROM Employees_Basic WHERE Employee_Imie = @userName AND Employee_Nazwisko = @user2ndName AND Employee_PESEL = @userPESEL";
            sqlCmd.Parameters.AddWithValue("@userName", vl.Imię);
            sqlCmd.Parameters.AddWithValue("@user2ndName", vl.Nazwisko);
            sqlCmd.Parameters.AddWithValue("@userPESEL", vl.Pesel);
            try
            {
                DataTable dt = dal.GetDataTable(sqlCmd);
                if (dt.Rows.Count > 0)
                {
                    iD = (int)dt.Rows[0][0];
                    return true;
                }
                else
                {
                    iD = 0;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // pobranie danych dokontrolki GridView na paneli Alokatora
        public DataSet GetGridData()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT a.Id, a.RadioGate, a.Created, a.GodzinaRozkladowa, a.FlightNb, a.Pax, a.RadioNeon, " +
                                 "a.Accepted, a.StartLoad, a.StartDrive, a.StartUnload, a.EndOp, " +
                                 "b.Operation, c.StationNb, d.IATA_Name, e.GateNb, f.VehicleNb, g.Shengen " +
                                 "FROM Operations AS a " +
                                 "INNER JOIN OperationType AS b ON a.Operation=b.Id " +
                                 "INNER JOIN Stations AS c ON a.PPS = c.Id " +
                                 "INNER JOIN AirPorts AS d ON a.AirPort = d.Id " +
                                 "INNER JOIN Gates AS e ON a.Gate = e.Id " +
                                 "INNER JOIN Vehicles AS f ON a.Bus = f.Id " +
                                 "INNER JOIN Countries AS g ON a.AirPort = d.Id  AND d.Country_Id = g.Id";
            try
            {
                ds = dal.GetDataSet(sqlCmd);
                return ds;
            }
            catch(Exception ex)
            {
                return ds;
            }
        }

        // pobranie informacji do ddl OperationList panelu Alokator
        public DataSet GetOperationList()
        {
            DataSet ds = new DataSet();
            sqlCmd.CommandText = "SELECT * FROM OperationType";
            try
            {
                return ds = dal.GetDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
    }
}