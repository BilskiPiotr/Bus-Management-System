using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class OpCookie
    {
        static BusinessLayer bl = new BusinessLayer();

        public OpCookie()
        { }

        public static HttpCookie CreateCookie(DataSet ds)
        {
            int shengen = 0;
            string portName = "";
            string country = "";

            // utworzenie nowego instancji ciasteczka
            HttpCookie cookie = new HttpCookie("opCookie");

            // dodawanie kolejnych wartości do listy wartoąści ciasteczka operacji
            cookie.Values["operation"] = ds.Tables[0].Rows[0].Field<int>("Operation").ToString();
            cookie.Values["godzinaRozkladowa"] = (ds.Tables[0].Rows[0].Field<DateTime>("GodzinaRozkladowa")).ToString("HH:mm");
            cookie.Values["created"] = (ds.Tables[0].Rows[0].Field<DateTime>("Created")).ToString("HH:mm");
            cookie.Values["accepted"] = (ds.Tables[0].Rows[0].Field<DateTime>("Accepted")).ToString("HH:mm");
            cookie.Values["startLoad"] = (ds.Tables[0].Rows[0].Field<DateTime>("StartLoad")).ToString("HH:mm");
            cookie.Values["startDrive"] = (ds.Tables[0].Rows[0].Field<DateTime>("StartDrive")).ToString("HH:mm");
            cookie.Values["startUnload"] = (ds.Tables[0].Rows[0].Field<DateTime>("StartUnload")).ToString("HH:mm");
            cookie.Values["endOp"] = (ds.Tables[0].Rows[0].Field<DateTime>("EndOp")).ToString("HH:mm");
            cookie.Values["flightNb"] = ds.Tables[0].Rows[0].Field<string>("FlightNb").ToString();
            cookie.Values["pax"] = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
            cookie.Values["airPort"] = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen, ref portName, ref country);
            cookie.Values["pps"] = bl.GetPPS(ds.Tables[0].Rows[0].Field<int>("PPS"));
            cookie.Values["gate"] = bl.GetGate(ds.Tables[0].Rows[0].Field<int>("Gate"));
            cookie.Values["radioGate"] = ds.Tables[0].Rows[0].Field<string>("RadioGate").ToString();
            cookie.Values["radioNeon"] = ds.Tables[0].Rows[0].Field<string>("RadioNeon").ToString();
            cookie.Values["shengen"] = shengen.ToString();
            cookie.Values["portName"] = portName;
            cookie.Values["country"] = country;
            cookie.Values["startLocLat"] = "";
            cookie.Values["startLocLon"] = "";
            cookie.Values["startLocLatDegree"] = "";
            cookie.Values["startLocLonDegree"] = "";
            cookie.Values["gateLocLat"] = "";
            cookie.Values["gateLocLon"] = "";
            cookie.Values["ppsLocLat"] = "";
            cookie.Values["ppsLocLon"] = "";


            // ustawienie czasu wygaśnięcia automatycznego
            //cookie.Expires = DateTime.Now.AddMinutes(90);

            // i zwrócenie gotowego ciasteczka
            return cookie;
        }
    }
}