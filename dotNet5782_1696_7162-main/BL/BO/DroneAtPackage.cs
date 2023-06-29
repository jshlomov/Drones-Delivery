using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class DroneAtPackage
    {
        public int ID { get; set; }
        public double Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            return $"drone {ID}, battery= {Battery},\n location= {CurrentLocation}.";
        }
    }
}

