using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class DroneAtCharge
    {
        public int ID { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return $"Drone in chrge: ID= {ID},  Battery= {Battery}";
        }
    }
}

