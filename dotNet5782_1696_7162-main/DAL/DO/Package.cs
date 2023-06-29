using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Package
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int TargetID { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? assigning { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public bool Deleted { get; set; }
        public override string ToString()
        {
            return $"Package #{ID}: Sender Id= {SenderID}, Target Id= {TargetID}, Weight= {Weight}, " +
                   $"Priority= {Priority}, Drone Id= {DroneId}, \ntime of: Creation= {Creation}, Assigning= {assigning}," +
                   $"\nPicked Up= {PickedUp}, Delivered= {Delivered}.";
        }
    }
}

