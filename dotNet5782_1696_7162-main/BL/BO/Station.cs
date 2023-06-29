using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int FreeChargeSlots { get; set; }
        public IEnumerable<DroneAtCharge> ChargingDrones { get; set; }
        public override string ToString()
        {
            return $"station{ID}: name= {Name}, free slots= {FreeChargeSlots}," +
                   $"location = {Location},";
        }
    }
}

