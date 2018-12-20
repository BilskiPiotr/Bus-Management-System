using System;
using System.IO;

namespace Bus_Management_System
{
    public class Reporting : Bus
    {
        // przygotowanie i obrobka pliku tekstowego z logami operacji
        public void PrepareLogFile(User loggedUser, string data)
        {
            // tworzymy nazwę pliku ktory bedzie uzupelniany danymi
            string dataLogic = Server.MapPath("~/logi/" + loggedUser.FirstName + "_" + loggedUser.LastName + "__" + data + ".txt");

            FileStream fs = null;

            if (!File.Exists(dataLogic))
            {
                using (fs = File.Create(dataLogic))
                {

                }
                fs.Close();

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
                    writer.Close();
                }
            }
            // i dodajemy nazwe pliku do zmiennej sesyjnej
            loggedUser.LogFilePath = dataLogic;
        }

        // dodajemy do pliku log sesji dane z kontrolek
        public void SaveUserFieldsValues(User loggedUser)
        {
            string newLine = Environment.NewLine;
            newLine += loggedUser.Operation.ToString() + "; ";
            newLine += loggedUser.OperationStatus.ToString() + "; ";
            newLine += loggedUser.Created + "; ";
            newLine += loggedUser.Accepted + "; ";
            newLine += loggedUser.StartLoad + "; ";
            newLine += loggedUser.StartDrive + "; ";
            newLine += loggedUser.StartUnload + "; ";
            newLine += loggedUser.EndOp + "; ";
            newLine += loggedUser.StartLat.ToString() + "; ";
            newLine += loggedUser.StartLon.ToString() + "; ";
            newLine += loggedUser.CurrentLat.ToString() + "; ";
            newLine += loggedUser.CurrentLon.ToString() + "; ";
            newLine += loggedUser.Speed.ToString() + "; ";
            newLine += loggedUser.Accuracy.ToString() + "; ";
            newLine += loggedUser.OldDistanceT.ToString() + "; ";
            newLine += loggedUser.DistanceT.ToString() + "; ";
            newLine += loggedUser.OldDistanceS.ToString() + "; ";
            newLine += loggedUser.DistanceS.ToString() + "; ";
            newLine += loggedUser.OldDistanceN.ToString() + "; ";
            newLine += loggedUser.DistanceN.ToString() + "; ";
            newLine += loggedUser.PredictedDistance.ToString() + "; ";
            newLine += loggedUser.Alert.ToString() + "; ";
            newLine += loggedUser.LoopTime.ToString() + "; ";
            newLine += DateTime.Now.ToString("HH:mm:ss");

            string dataLogic = loggedUser.LogFilePath;

            if (File.Exists(dataLogic))
            {
                using (StreamWriter writer = new StreamWriter(loggedUser.LogFilePath, true))
                {
                    writer.WriteLine(newLine);
                    writer.Close();
                }
            }
        }
    }
}