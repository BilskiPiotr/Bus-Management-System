using System;
using System.Data;
using System.Device.Location;
using System.Globalization;

namespace Bus_Management_System
{
    public class DataManipulate : Bus
    {
        private DataSet ds = new DataSet();
        // przeliczenie współrzędnych pobranych z urządzenia GPS do czytelnych współrzędnych w stopniach
        public void TranslateCoordToDegree(double latitude, double longitude)
        {
            string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            latitude = Math.Abs(latitude);
            double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            longitude = Math.Abs(longitude);
            double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            Session["StartLocLatDegree"] = String.Format(Convert.ToString(Math.Truncate(latitude) + "° " + +Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek));
            Session["StartLocLonDegree"] = String.Format(Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek));
        }

        // pobranie danych lokalizacyjnych stanowiska postojowego samolotu
        public void GetPPSData(int ppsId)
        {
            bl.GetPPS(ppsId, ref ds);
            Session["Pps"] = ds.Tables[0].Rows[0].Field<string>("StationNb");
            Session["PpsLat"] = Convert.ToDouble(ds.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            Session["PpsLon"] = Convert.ToDouble(ds.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            ds.Clear();
        }

        // pobranie danych lokalizacyjnych wyjścia pasażerskiego
        public void GetGateData(int gateId)
        {
            bl.GetGate(gateId, ref ds);
            Session["Gate"] = ds.Tables[0].Rows[0].Field<string>("GateNb");
            Session["GateLat"] = Convert.ToDouble(ds.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            Session["GateLon"] = Convert.ToDouble(ds.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            ds.Clear();
        }

        //ustawienie wartości aktualnej operacji w zmiennej sesyjnej
        public void GetOperationData(DataSet dsInn)
        {
            Session["Operation"] = dsInn.Tables[0].Rows[0].Field<int>("Operation");
            Session["FlightNb"] = dsInn.Tables[0].Rows[0].Field<string>("FlightNb");
            Session["Pax"] = dsInn.Tables[0].Rows[0].Field<int>("Pax").ToString();
            Session["RadioGate"] = dsInn.Tables[0].Rows[0].Field<string>("RadioGate").ToString();
            Session["RadioNeon"] = dsInn.Tables[0].Rows[0].Field<string>("RadioNeon").ToString();
            Session["GodzinaRozkladowa"] = (dsInn.Tables[0].Rows[0].Field<DateTime>("GodzinaRozkladowa")).ToString("HH:mm");

            GetSpecyficAirPortData(dsInn.Tables[0].Rows[0].Field<int>("AirPort"));
        }

        // ustalenie stref bezpieczeństwa dla obsługiwanego portu
        public void GetSpecyficAirPortData(int airPort)
        {
            bl.GetAirPort(airPort, ref ds);
            Session["AirPort"] = ds.Tables[0].Rows[0].Field<string>("IATA_Name");
            Session["Shengen"] = Convert.ToInt32(bl.GetCountry(ds.Tables[0].Rows[0].Field<int>("Shengen")));
            Session["PortName"] = ds.Tables[0].Rows[0].Field<string>("Full_Name");
            Session["Country"] = ds.Tables[0].Rows[0].Field<string>("Country_name");
            ds.Clear();
        }

        // Ustalenie na jakim etapie jest aktualna operacja
        public void SetOperationStatus(DataSet dsInn)
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
            string data = "";
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("Created").ToString("HH:mm");
            Session["Created"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("Accepted").ToString("HH:mm");
            Session["Accepted"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("StartLoad").ToString("HH:mm");
            Session["StartLoad"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("StartDrive").ToString("HH:mm");
            Session["StartDrive"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("StartUnload").ToString("HH:mm");
            Session["StartUnload"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;
            data = dsInn.Tables[0].Rows[0].Field<DateTime>("EndOp").ToString("HH:mm");
            Session["EdnOp"] = data;
            if (data != "00:00")
                operationStatus = operationStatus + 1;

            Session["OperationStatus"] = operationStatus;
        }

        // określenie odległości
        public void CheckDistance(int operationStatus)
        {
            switch (operationStatus)
            {
                case 2:
                    {
                        if (Session["DistanceT"] != null)
                            Session["OldDistanceT"] = Session["DistanceT"];
                        else
                            Session["OldDistanceT"] = 0;
                        if (Session["DistanceS"] != null)
                            Session["OldDistanceS"] = Session["DistanceS"];
                        else
                            Session["OldDistanceS"] = 0;
                        if (Session["DistanceN"] != null)
                            Session["OldDistanceN"] = Session["DistanceN"];
                        else
                            Session["OldDistanceN"] = 0;
                        if ((int)Session["Operation"] == 1)
                        {
                            Distance((double)Session["PpsLat"], (double)Session["PpsLon"]);
                        }
                        else
                        {
                            Distance((double)Session["GateLat"], (double)Session["GateLon"]);
                        }
                    }
                    break;
                case 4:
                    {
                        if (Session["DistanceT"] != null)
                            Session["OldDistanceT"] = Session["DistanceT"];
                        else
                            Session["OldDistanceT"] = 0;
                        if (Session["DistanceS"] != null)
                            Session["OldDistanceS"] = Session["DistanceS"];
                        else
                            Session["OldDistanceS"] = 0;
                        if (Session["DistanceN"] != null)
                            Session["OldDistanceN"] = Session["DistanceN"];
                        else
                            Session["OldDistanceN"] = 0;
                        if ((int)Session["Operation"] == 1)
                        {
                            Distance((double)Session["GateLat"], (double)Session["GateLon"]);
                        }
                        else
                        {
                            Distance((double)Session["PpsLat"], (double)Session["PpsLon"]);
                        }
                    }
                    break;
            }
        }

        // przeliczenie wszystkich odległości
        private void Distance(double destLat, double destLon)
        {
            GeoCoordinate busPosition = new GeoCoordinate((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
            GeoCoordinate targetPosition = new GeoCoordinate(destLat, destLon);
            GeoCoordinate shengen = new GeoCoordinate(52.17035, 20.97174);
            GeoCoordinate nonShengen = new GeoCoordinate(52.17224, 20.9702);

            // zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            double distanceT = Math.Round(busPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);
            double distanceS = Math.Round(busPosition.GetDistanceTo(shengen), 2, MidpointRounding.AwayFromZero);
            double distanceN = Math.Round(busPosition.GetDistanceTo(nonShengen), 2, MidpointRounding.AwayFromZero);

            //Session["OldDistanceT"] = (double)Session["DistanceT"];
            Session["DistanceT"] = distanceT;

            //Session["OldDistanceS"] = (double)Session["DistanceS"];
            Session["DistanceS"] = distanceS;

            //Session["OldDistanceN"] = (double)Session["DistanceN"];
            Session["DistanceN"] = distanceN;
        }

        // przeliczenie przewidywanej odległości do celu ze względu na prędkość
        public void SetPredictedDistance()
        {
            double predictedDistance = 0.0d;

            if ((int)Session["OperationStatus"] == 2 || (int)Session["OperationStatus"] == 4)
            {
                if ((double)Session["OldDistanceT"] > (double)Session["DistanceT"])
                    predictedDistance = Math.Round(((double)Session["DistanceT"] - ((double)Session["Speed"] * 3)), 2, MidpointRounding.AwayFromZero);
                else
                if ((double)Session["OldDistanceT"] < (double)Session["DistanceT"])
                    predictedDistance = Math.Round(((double)Session["DistanceT"] + ((double)Session["Speed"] * 3)), 2, MidpointRounding.AwayFromZero);
                else
                    predictedDistance = (double)Session["DistanceT"];
            }
            Session["PredictedDistance"] = predictedDistance;
        }
    }
}