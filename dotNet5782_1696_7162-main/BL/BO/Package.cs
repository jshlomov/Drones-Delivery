using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Package
    {
        public int ID { get; set; }
        public CustAtPackage Sender { get; set; }
        public CustAtPackage Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneAtPackage Drone { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? assigning { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return $"Package #{ID}:\n" +
                   $"Sender= {Sender},\n" +
                   $"Target= {Target},\n" +
                   $"Weight= {Weight}, Priority= {Priority},\n" +
                   $"Drone's data: {Drone}," +
                   $"\ntime of: Creation= {Creation},\n\tAssigning= {assigning}," +
                   $"\n\tPicked Up= {PickedUp},\n\tDelivered= {Delivered}.";
        }
    }
}

