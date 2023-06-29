using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class DroneToList
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int PackageID { get; set; }
        public override string ToString()
        {
            return $"Drone {ID}: model= {Model}, possible weight= {MaxWeight}, battery= {Battery}, status= {Status}, package= {PackageID} " +
                   $"\nlocation= {CurrentLocation}.";
        }

    }
}

