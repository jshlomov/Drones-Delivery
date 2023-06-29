using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            double latidute = Latitude;
            double longitude = Longitude;
            string sexaDecLatidute()
            {
                string ch = "N";
                if (latidute < 0)
                {
                    ch = "s";
                    latidute = -latidute;
                }
                int min = (int)(60 * (latidute - (int)latidute));
                double sec = (latidute - (int)latidute) * 3600 - min * 60;
                return $"{(int)latidute}° {min}‘ {sec}‘‘ {ch}";
            }
            string sexaDecLongitude()
            {
                string ch = "E";
                if (longitude < 0)
                {
                    ch = "W";
                    longitude = -longitude;
                }
                int min = (int)(60 * (longitude - (int)longitude));
                double sec = (longitude - (int)longitude) * 3600 - min * 60;
                return $"{(int)longitude}° {min}‘ {sec}‘‘ {ch}";
            }
            return $"location: {{ {sexaDecLatidute()} , {sexaDecLongitude()} }}";
        }
    }
}


