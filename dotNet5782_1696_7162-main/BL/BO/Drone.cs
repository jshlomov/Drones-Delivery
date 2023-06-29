using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Drone
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatuses Status { get; set; }
        public double Battery { get; set; }
        public PackageOnDelivery Package { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            return $"Drone {ID}:\nModel= {Model},\nMaximum Weight= {MaxWeight},\nStatus= {Status},\nBattery= {Battery}," +
                $"\npackage's data= {Package},\n {Location}";
        }
    }
}

