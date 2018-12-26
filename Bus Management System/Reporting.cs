using System;
using System.IO;

namespace Bus_Management_System
{
    public class Reporting : Bus
    {
        // przygotowanie i obrobka pliku tekstowego z logami operacji
        public void PrepareLogFile(/*User loggedUser, */string data)
        {
            // tworzymy nazwę pliku ktory bedzie uzupelniany danymi
            string dataLogic = Server.MapPath("~/logi/" + (string)Session["FirstName"] + "_" + (string)Session["LastName"] + "__" + data + ".txt");

            //FileStream fs = null;

            if (!File.Exists(dataLogic))
            {
                using (FileStream fs = File.Create(dataLogic))
                {

                }
                //fs.Dispose();

                string newLine = Environment.NewLine;
                newLine += "Operation; ";
                newLine += "Operation Status; ";
                newLine += "Created; ";
                newLine += "Accepted; ";
                newLine += "Start Load; ";
                newLine += "Start Drive; ";
                newLine += "Start Unload; ";
                newLine += "End Operation; ";
                newLine += "Start Latitude; ";
                newLine += "Start Longitude; ";
                newLine += "Current Latitude; ";
                newLine += "Current Longitude; ";
                newLine += "Speed; ";
                newLine += "Accuracy; ";
                newLine += "Old Distance to Target; ";
                newLine += "Distance to Target; ";
                newLine += "Old Distance to Shengen; ";
                newLine += "Distance to Shengen; ";
                newLine += "Old Distance to NonShengen; ";
                newLine += "Distance to NonShengen; ";
                newLine += "Predicted Distance; ";
                newLine += "Alert Nb; ";
                newLine += "Loop Time; ";
                newLine += "Measurement Time";

                using (StreamWriter writer = new StreamWriter(dataLogic, true))
                {
                    writer.WriteLine(newLine);
                    writer.Dispose();
                }
            }
            // i dodajemy nazwe pliku do zmiennej sesyjnej
            Session["LogFilePath"] = dataLogic;
        }

        // dodajemy do pliku log sesji dane z kontrolek
        public void SaveUserFieldsValues(/*User loggedUser*/)
        {
            string newLine = Environment.NewLine;
            if (Session["Operation"] != null)
                newLine += (int)Session["Operation"] + "; ";
            else
                newLine += " ; ";
            newLine += (int)Session["OperationStatus"] + "; ";
            newLine += (string)Session["Created"] + "; ";
            newLine += (string)Session["Accepted"] + "; ";
            newLine += (string)Session["StartLoad"] + "; ";
            newLine += (string)Session["StartDrive"] + "; ";
            newLine += (string)Session["StartUnload"] + "; ";
            newLine += (string)Session["EndOp"] + "; ";
            newLine += (string)Session["StartLat"] + "; ";
            newLine += (string)Session["StartLon"] + "; ";
            newLine += (string)Session["CurrentLat"] + "; ";
            newLine += (string)Session["CurrentLon"] + "; ";
            newLine += (string)Session["Speed"] + "; ";
            newLine += (string)Session["Accuracy"] + "; ";
            newLine += (string)Session["OldDistanceT"] + "; ";
            newLine += (string)Session["DistanceT"] + "; ";
            newLine += (string)Session["OldDistanceS"] + "; ";
            newLine += (string)Session["DistanceS"] + "; ";
            newLine += (string)Session["OldDistanceN"] + "; ";
            newLine += (string)Session["DistanceN"] + "; ";
            newLine += (string)Session["PredictedDistance"] + "; ";
            if (Session["Alert"] != null)
                newLine += (int)Session["Alert"] + "; ";
            else
                newLine += " ; ";
            newLine += (int)Session["LoopTime"] + "; ";
            newLine += DateTime.Now.ToString("HH:mm:ss");

            string dataLogic = (string)Session["LogFilePath"];

            if (File.Exists(dataLogic))
            {
                using (StreamWriter writer = new StreamWriter((string)Session["LogFilePath"], true))
                {
                    writer.WriteLine(newLine);
                    writer.Dispose();
                }
            }
        }
    }
}