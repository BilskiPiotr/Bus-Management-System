using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class ToDegrees
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        //public string[] Coordinates()
        //{

        //}

        public string TransformToDegrees(string coordinate)
        {
            // zwraca pierwsze dwa znaki dostarczonej wartości latitude
            string stopnie = coordinate.Substring(0, 2);
            string minuty = Convert.ToString(Convert.ToInt32(coordinate.Substring(3)) / 60);

            string result = "";
            return result;
        }
    }
}