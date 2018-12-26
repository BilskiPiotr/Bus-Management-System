using System;
using System.Data;
using System.Device.Location;
using System.Globalization;

namespace Bus_Management_System
{
    public class DataManipulate : Bus
    {
        // naniesienie aktualnych danych lokalizacyjnych
        public void UpdateGPSData(User loggedUser)
        {
            //loggedUser.Speed = speed;
            //loggedUser.Accuracy = accuracy;
            //loggedUser.CurrentLat = currentLat;
            //loggedUser.CurrentLon = currentLon;
        }

        // przeliczenie współrzędnych pobranych z urządzenia GPS do czytelnych współrzędnych w stopniach
        public void TranslateCoordToDegree(User loggedUser)
        {
            double latitude = loggedUser.CurrentLat;
            double longitude = loggedUser.CurrentLon;

            string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            latitude = Math.Abs(latitude);
            double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            longitude = Math.Abs(longitude);
            double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            loggedUser.StartLocLatDegree = String.Format(Convert.ToString(Math.Truncate(latitude) + "° " + +Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek));
            loggedUser.StartLocLonDegree = String.Format(Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek));
        }

        // pobranie danych lokalizacyjnych stanowiska postojowego samolotu
        public void GetPPSData(User loggedUser, int ppsId)
        {
            DataSet pps = bl.GetPPS(ppsId);
            loggedUser.Pps = pps.Tables[0].Rows[0].Field<string>("StationNb");
            loggedUser.PpsLat = Convert.ToDouble(pps.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            loggedUser.PpsLon = Convert.ToDouble(pps.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            pps.Dispose();
        }

        // pobranie danych lokalizacyjnych wyjścia pasażerskiego
        public void GetGateData(User loggedUser, int gateId)
        {
            DataSet gate = bl.GetGate(gateId);
            loggedUser.Gate = gate.Tables[0].Rows[0].Field<string>("GateNb");
            loggedUser.GateLat = Convert.ToDouble(gate.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            loggedUser.GateLon = Convert.ToDouble(gate.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            gate.Dispose();
        }

        //ustawienie wartości aktualnej operacji w zmiennej sesyjnej
        public void GetOperationData(User loggedUser, DataSet ds)
        {
            loggedUser.Operation = ds.Tables[0].Rows[0].Field<int>("Operation");
            loggedUser.FlightNb = ds.Tables[0].Rows[0].Field<string>("FlightNb");
            loggedUser.Pax = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
            loggedUser.RadioGate = ds.Tables[0].Rows[0].Field<string>("RadioGate").ToString();
            loggedUser.RadioNeon = ds.Tables[0].Rows[0].Field<string>("RadioNeon").ToString();
            loggedUser.GodzinaRozkladowa = (ds.Tables[0].Rows[0].Field<DateTime>("GodzinaRozkladowa")).ToString("HH:mm");

            GetSpecyficAirPortData(loggedUser, ds.Tables[0].Rows[0].Field<int>("AirPort"));
        }

        // ustalenie stref bezpieczeństwa dla obsługiwanego portu
        public void GetSpecyficAirPortData(User loggedUser, int airPort)
        {
            if (airPort != 0)
            {
                DataSet ds = bl.GetAirPort(airPort);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    loggedUser.AirPort = ds.Tables[0].Rows[0].Field<string>("IATA_Name");
                    loggedUser.Shengen = Convert.ToInt32(ds.Tables[0].Rows[0].Field<int>("Shengen"));
                    //loggedUser.Shengen = Convert.ToInt32(bl.GetCountry(ds.Tables[0].Rows[0].Field<int>("Shengen")));
                    loggedUser.PortName = ds.Tables[0].Rows[0].Field<string>("Full_Name");
                    loggedUser.Country = ds.Tables[0].Rows[0].Field<string>("Country_name");
                }
                ds.Dispose();
            }
        }

        // Ustalenie na jakim etapie jest aktualna operacja
        public void SetOperationStatus(User loggedUser, DataSet ds)
        {
            /* operationStatus wartości możliwe:
             * 0 - brak zlecenia       <-
             * 1 - zlecenie utworzone    |
             * 2 - zlecenie przyjete     |
             * 3 - rozpoczęty załadunek  |
             * 4 - dowóz pasażerów       |
             * 5 - rozpoczęty wyładunek >|
             */
            int operationStatus = 0;
            loggedUser.Created = (ds.Tables[0].Rows[0].Field<DateTime>("Created")).ToString("HH:mm");
            loggedUser.Accepted = (ds.Tables[0].Rows[0].Field<DateTime>("Accepted")).ToString("HH:mm");
            loggedUser.StartLoad = (ds.Tables[0].Rows[0].Field<DateTime>("StartLoad")).ToString("HH:mm");
            loggedUser.StartDrive = (ds.Tables[0].Rows[0].Field<DateTime>("StartDrive")).ToString("HH:mm");
            loggedUser.StartUnload = (ds.Tables[0].Rows[0].Field<DateTime>("StartUnload")).ToString("HH:mm");
            loggedUser.EndOp = (ds.Tables[0].Rows[0].Field<DateTime>("EndOp")).ToString("HH:mm");
            if (loggedUser.Created != "00:00")
                operationStatus = 1;
            if (loggedUser.Accepted != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartLoad != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartDrive != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartUnload != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.EndOp != "00:00")
                operationStatus = 0;

            loggedUser.OperationStatus = operationStatus;
        }

        // określenie odległości
        public void CheckDistance(User loggedUser)
        {
            switch (loggedUser.OperationStatus)
            {
                case 2:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            Distance(loggedUser, loggedUser.PpsLat, loggedUser.PpsLon);
                        }
                        else
                        {
                            Distance(loggedUser, loggedUser.GateLat, loggedUser.GateLon);
                        }
                    }
                    break;
                case 4:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            Distance(loggedUser, loggedUser.GateLat, loggedUser.GateLon);
                        }
                        else
                        {
                            Distance(loggedUser, loggedUser.PpsLat, loggedUser.PpsLon);
                        }
                    }
                    break;
            }
        }

        // przeliczenie wszystkich odległości
        private double Distance(User loggedUser, double destLat, double destLon)
        {
            GeoCoordinate busPosition = new GeoCoordinate(loggedUser.CurrentLat, loggedUser.CurrentLon);
            GeoCoordinate targetPosition = new GeoCoordinate(destLat, destLon);
            GeoCoordinate shengen = new GeoCoordinate(52.17035, 20.97174);
            GeoCoordinate nonShengen = new GeoCoordinate(52.17224, 20.9702);

            // zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            double distanceT = Math.Round(busPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);
            double distanceS = Math.Round(busPosition.GetDistanceTo(shengen), 2, MidpointRounding.AwayFromZero);
            double distanceN = Math.Round(busPosition.GetDistanceTo(nonShengen), 2, MidpointRounding.AwayFromZero);

            loggedUser.OldDistanceT = loggedUser.DistanceT;
            loggedUser.DistanceT = distanceT;

            loggedUser.OldDistanceS = loggedUser.DistanceS;
            loggedUser.DistanceS = distanceS;

            loggedUser.OldDistanceN = loggedUser.DistanceN;
            loggedUser.DistanceN = distanceN;

            return distanceT;
        }

        // przeliczenie przewidywanej odległości do celu ze względu na prędkość
        public void SetPredictedDistance(User loggedUser)
        {
            double predictedDistance = 0.0d;

            if (loggedUser.OperationStatus == 2 || loggedUser.OperationStatus == 4)
            {
                // prędkość obiektu jest większa niż 3km / h
                if (loggedUser.DistanceT > 30.0d)
                {
                    if (loggedUser.Operation == 1)
                    {
                        switch (loggedUser.Shengen)
                        {
                            case 1:
                                {
                                    if (loggedUser.OldDistanceN > loggedUser.DistanceN)
                                        predictedDistance = Math.Round((loggedUser.DistanceN - (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                                    else
                                    if (loggedUser.OldDistanceN < loggedUser.DistanceN)
                                        predictedDistance = Math.Round((loggedUser.DistanceN + (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                                    else
                                        predictedDistance = loggedUser.DistanceN;
                                }
                                break;
                            case 0:
                                {
                                    if (loggedUser.OldDistanceS > loggedUser.DistanceS)
                                        predictedDistance = Math.Round((loggedUser.DistanceS - (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                                    else
                                    if (loggedUser.OldDistanceS < loggedUser.DistanceS)
                                        predictedDistance = Math.Round((loggedUser.DistanceS + (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                                    else
                                        predictedDistance = loggedUser.DistanceS;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (loggedUser.OldDistanceT > loggedUser.DistanceT)
                            predictedDistance = Math.Round((loggedUser.DistanceT - (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                        else
                        if (loggedUser.OldDistanceT < loggedUser.DistanceT)
                            predictedDistance = Math.Round((loggedUser.DistanceN + (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                        else
                            predictedDistance = loggedUser.DistanceT;
                    }
                }
                loggedUser.PredictedDistance = predictedDistance;
            }
        }
    }
}