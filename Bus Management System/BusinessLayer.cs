using System;
using System.Data;
using System.Data.SqlClient;

namespace Bus_Management_System
{
    public class BusinessLayer
    {
        private DataAccessLayer dal = new DataAccessLayer();
        private bool result = false;

        public void GetUserData(int iD, ref DataTable dt)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT Employee_CompanyId, Employee_Imie, Employee_Nazwisko, Employee_Priv FROM Employees_Basic WHERE Id = @userId";
                sqlCmd.Parameters.AddWithValue("@userId", iD);
                try
                {
                    dal.GetDataTable(sqlCmd, ref dt);
                    sqlCmd.Dispose();
                }
                catch (Exception GetUserData_ex)
                {

                }
            }
        }

        // zapisanie w systemie informacji o zalogowaniu użytykownika
        public bool UserLogIn(int iD, DateTime loginDate)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "INSERT INTO Employees_Status (Employee_Id, Employee_Login) VALUES (@iD, @loginDate)";
                sqlCmd.Parameters.AddWithValue("@iD", iD);
                sqlCmd.Parameters.AddWithValue("@loginDate", loginDate);
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.Dispose();
                    result = true;
                }
                catch (Exception UserLogIn_ex)
                {
                    result = false;
                }
            }
            return result;
        }

        // wylogowanie użytkownika
        public bool UserLogOut(int id, string bus)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "INSERT INTO Employees_Status (Employee_Id, Employee_Logout) VALUES (@userId, @logOut)";
                sqlCmd.Parameters.AddWithValue("@userId", id.ToString());
                sqlCmd.Parameters.AddWithValue("@logOut", DateTime.Now);
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.Dispose();
                    result = true;
                }
                catch (Exception UserLogOut_ex1)
                {

                }
            }
            //jeśli pierwszy upload poszedł poprawnie
            if (result && bus != "")
            {
                try
                {
                    UpdateVehicleStatus(1, bus);
                    result = true;
                }
                catch (Exception UserLogOut_ex2)
                {
                    result = false;
                }
            }
            return result;
        }

        // naniesieniesienie statusu dla wybranego pojazdu
        public void UpdateVehicleStatus(int status, string bus)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "UPDATE VEHICLES SET Bus_Status = @busStatus WHERE VehicleNb = @busNb";
                sqlCmd.Parameters.AddWithValue("@busStatus", status);
                sqlCmd.Parameters.AddWithValue("@busNb", bus);
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.Dispose();
                }
                catch (Exception UpdateVehicleStatus_ex)
                {

                }
            }
        }

        // pobranie listy zdefiniowanych bramek pasażerskich
        public void GetGates(ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT Id, GateNb FROM Gates";
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetGates_ex)
                {
                }
            }
        }

        // pobranie informacji specyficznych o wyjściu pasażerskim dla panelu Operatora
        public void GetGate(int gate, ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT * FROM Gates WHERE Id = @gateId";
                sqlCmd.Parameters.AddWithValue("@gateId", gate);
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetGate_ex)
                {
                }
            }
        }

        // pobranie listy zdefiniowanych stanowisk postojowych samolotów
        public void GetStations(ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT Id, StationNb FROM Stations";
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetStations_ex)
                {
                }
            }
        }

        // pobranie listy zdefiniowanych portów lotniczych
        public void GetAirPort(ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT Id, IATA_Name FROM AirPorts";
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetAirPort_ex)
                {
                }
            }
        }

        // pobieranie listy dostępnych autobusów
        public void GetBus(int commandTextNumber, string bus, ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
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
                    case 3:
                        {
                            sqlCmd.CommandText = "SELECT Id, VehicleNb FROM Vehicles WHERE Bus_Status = @busStatus";
                            sqlCmd.Parameters.AddWithValue("@busStatus", 1);
                        }
                        break;
                    case 4:
                        {
                            sqlCmd.CommandText = "SELECT * FROM Vehicles";
                        }
                        break;
                }
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetBus_ex)
                { }
            }
        }

        // pobranie informacji o zleceniu dla panelu Operator
        public void GetOperations(string bus, ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT * FROM Operations WHERE Bus=(SELECT Id FROM Vehicles WHERE VehicleNb = @busNb) AND Finished = @finished";
                sqlCmd.Parameters.AddWithValue("@busNb", bus);
                sqlCmd.Parameters.AddWithValue("@finished", 0);
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetOperations_ex)
                { }
            }
        }
        
        // dodanie nowej operacji
        public bool AddNewOperation(NewOperation newOp)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                DateTime addOpDate = DateTime.Now;
                DateTime zeroDate = new DateTime(1999, 01, 01);
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
                    sqlCmd.CommandText = "";
                    sqlCmd.Parameters.Clear();
                    result = true;
                }
                catch (Exception AddNewOperation_ex1)
                {
                    result = false;
                }

                if (result)
                {
                    sqlCmd.CommandText = "UPDATE Vehicles SET Work_Status = 1 WHERE VehicleNb = @busNb";
                    sqlCmd.Parameters.AddWithValue("@busNb", newOp.BusNb);
                    try
                    {
                        dal.QueryExecution(sqlCmd);
                        sqlCmd.Dispose();
                        result = true;
                    }
                    catch (Exception AddNewOperation_ex2)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        // pobranie danych specyficznych o stanowisku postojowym dla panelu Operatora
        public void GetPPS(int pps, ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT * FROM Stations WHERE Id = @ppsNb";
                sqlCmd.Parameters.AddWithValue("@ppsNb", pps);
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetPPS_ex)
                {
                }
            }
        }

        // pobranie informacji o porcie lotniczym do panelu Operatora
        public void GetAirPort(int airPort, ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT a.IATA_Name, a.Full_Name, b.Country_name, b.Shengen FROM AirPorts AS a " +
                                     "INNER JOIN Countries AS b ON a.Country_Id=b.Id WHERE a.Id = @airPortNb ";
                sqlCmd.Parameters.AddWithValue("@airPortNb", airPort);
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetAirPort_ex)
                { }
            }
        }

        // ustalenie strefy bezpieczeństwa
        public int GetCountry(int country)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                int shengen = 0;
                sqlCmd.CommandText = "SELECT Shengen FROM Countries WHERE Id = @countryId";
                sqlCmd.Parameters.AddWithValue("@countryId", country);
                try
                {
                    DataSet ds = new DataSet();
                    dal.GetDataSet(sqlCmd, ref ds);
                    shengen = ds.Tables[0].Rows[0].Field<int>("Shengen");
                    ds.Dispose();
                    sqlCmd.Dispose();
                    return shengen;
                }
                catch (Exception GetCountry_ex)
                {
                    return shengen;
                }
            }
        }

        // sprawdzenie poświadczeń wprowadzonych przez użytkownika
        public bool VerifyUser(VerifyLayer vl, ref int iD)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT Id FROM Employees_Basic WHERE Employee_Imie = @userName AND Employee_Nazwisko = @user2ndName AND Employee_PESEL = @userPESEL";
                sqlCmd.Parameters.AddWithValue("@userName", vl.Imię);
                sqlCmd.Parameters.AddWithValue("@user2ndName", vl.Nazwisko);
                sqlCmd.Parameters.AddWithValue("@userPESEL", vl.Pesel);
                DataTable dt = new DataTable();
                try
                {
                    dal.GetDataTable(sqlCmd, ref dt);
                    sqlCmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        iD = (int)dt.Rows[0][0];
                        result = true;
                        dt.Dispose();
                    }
                }
                catch (Exception VerifyUser_ex)
                {
                    result = false;
                }
            }
            return result;
        }

        // pobranie danych dokontrolki GridView na paneli Alokatora
        public void GetGridData(ref DataSet ds)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
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
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception ex)
                {
                }
            }
        }

        // pobranie informacji do ddl OperationList panelu Alokator
        public void GetOperationList(ref DataSet ds)
        {
           using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "SELECT * FROM OperationType";
                try
                {
                    dal.GetDataSet(sqlCmd, ref ds);
                    sqlCmd.Dispose();
                }
                catch (Exception GetOperationList_ex)
                {
                }
            }
        }

        public bool DeleteOperation(string bus, int opId)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.CommandText = "UPDATE Vehicles SET Work_Status = @work_status WHERE VehicleNb = @vehicleNb";
                sqlCmd.Parameters.AddWithValue("@work_status", 0);
                sqlCmd.Parameters.AddWithValue("@vehicleNb", bus);
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.CommandText = "";
                    sqlCmd.Parameters.Clear();
                    result = true;
                }
                catch (Exception DeleteOperation_ex1)
                {
                    result = false;
                    sqlCmd.Dispose();
                }

                if (result)
                {
                    sqlCmd.CommandText = "DELETE FROM Operations WHERE Id=@Id";
                    sqlCmd.Parameters.AddWithValue("@Id", opId);
                    try
                    {
                        dal.QueryExecution(sqlCmd);
                        sqlCmd.Dispose();
                        result = true;
                    }
                    catch (Exception DeleteOperation_ex)
                    {
                        sqlCmd.Dispose();
                        result = false;
                    }
                }
            }
            return result;
        }

        // update danych do istniejącej operacji
        public bool AlocatorRowUpdate(string id, DateTime godzinaRozkladowa, string operation, string flightNb, string airPort,    
                                      string pax, string gate, string pps, string bus, string radioGate, string radioNeon)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                DateTime updateDate = DateTime.Now;
                sqlCmd.CommandText = "UPDATE Operations SET Operation = @operation, " +
                                     "FlightNb = @flightNb, " +
                                     "GodzinaRozkladowa = @opTime, " +
                                     "AirPort = @airPort, " +
                                     "Pax = @pax, " +
                                     "Gate = @gate, " +
                                     "PPS = @pps, " +
                                     "Bus = @bus, " +
                                     "RadioGate = @radioGate, " +
                                     "RadioNeon = @radioNeon, " +
                                     "Created = @editDate " +
                                     "WHERE Id=@id ";
                sqlCmd.Parameters.AddWithValue("@operation", Convert.ToInt32(operation));
                sqlCmd.Parameters.AddWithValue("@flightNb", flightNb);
                sqlCmd.Parameters.AddWithValue("@opTime", godzinaRozkladowa);
                sqlCmd.Parameters.AddWithValue("@airPort", Convert.ToInt32(airPort));
                sqlCmd.Parameters.AddWithValue("@pax", Convert.ToInt32(pax));
                sqlCmd.Parameters.AddWithValue("@gate", Convert.ToInt32(gate));
                sqlCmd.Parameters.AddWithValue("@pps", Convert.ToInt32(pps));
                sqlCmd.Parameters.AddWithValue("@bus", Convert.ToInt32(bus));
                sqlCmd.Parameters.AddWithValue("@radioGate", radioGate);
                sqlCmd.Parameters.AddWithValue("@radioNeon", radioNeon);
                sqlCmd.Parameters.AddWithValue("@editDate", updateDate);
                sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                try
                {
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.CommandText = "";
                    sqlCmd.Parameters.Clear();
                    result = true;
                }
                catch (Exception AlocatorRowUpdate_ex1)
                {
                    sqlCmd.Dispose();
                    result = false;
                }
                if (result)
                {
                    sqlCmd.CommandText = "UPDATE Vehicles SET Work_Status = 1 WHERE Id = @bus ";
                    sqlCmd.Parameters.AddWithValue("@bus", Convert.ToInt32(bus));
                    try
                    {
                        dal.QueryExecution(sqlCmd);
                        sqlCmd.Dispose();
                        result = true;
                    }
                    catch (Exception AlocatorRowUpdate_ex2)
                    {
                        sqlCmd.Dispose();
                        result = false;
                    }
                }
            }
            return result;
        }

        // nanoszenie na bazę danych informacji o kolejnych etapach wykonania operacji
        public bool BusOperationAction(int command, string bus)
        {
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                switch (command)
                {
                    case 1:
                        {
                            sqlCmd.CommandText = "UPDATE Operations SET Accepted = @ActionTime WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)";
                        }
                        break;
                    case 2:
                        {
                            sqlCmd.CommandText = "UPDATE Operations SET StartLoad = @ActionTime WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)";
                        }
                        break;
                    case 3:
                        {
                            sqlCmd.CommandText = "UPDATE Operations SET StartDrive = @ActionTime WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)";
                        }
                        break;
                    case 4:
                        {
                            sqlCmd.CommandText = "UPDATE Operations SET StartUnload = @ActionTime WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)";
                        }
                        break;
                    case 5:
                        {
                            sqlCmd.CommandText = "UPDATE Operations SET EndOp = @ActionTime, Finished = @finished WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)";
                            sqlCmd.Parameters.AddWithValue("@finished", 1);
                        }
                        break;
                }
                try
                {
                    sqlCmd.Parameters.AddWithValue("@busNb", bus);
                    sqlCmd.Parameters.Add("@ActionTime", SqlDbType.DateTime).Value = DateTime.Now;
                    dal.QueryExecution(sqlCmd);
                    sqlCmd.Dispose();
                    result = true;
                }
                catch (Exception BusOperationAction_Ex)
                {
                    sqlCmd.Dispose();
                    result = false;
                }
                return result;
            }
        }
    }
}