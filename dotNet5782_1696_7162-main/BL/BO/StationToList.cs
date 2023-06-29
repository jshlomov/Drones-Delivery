using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class StationToList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FreeChargeSlots { get; set; }
        public int TakenChargeSlots { get; set; }
        public override string ToString()
        {
            return $"Station {ID}: name= {Name}, free slots= {FreeChargeSlots}, taken slots= {TakenChargeSlots}.";
        }
    }
}

