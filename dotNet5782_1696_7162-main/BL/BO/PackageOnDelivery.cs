using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class PackageOnDelivery
    {
        public int ID { get; set; }
        public CustAtPackage Sender { get; set; }
        public CustAtPackage Target { get; set; }
        public bool Situation { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public Location PickingUp { get; set; }
        public Location Delivering { get; set; }
        public double Distance { get; set; }
        public override string ToString()
        {
            return $"Package {ID}: Sender: {Sender}, Reciver: {Target},\n Situation= {Situation}, Priority= {Priority}, Weight= {Weight},\n" +
                $"Location of PickingUp= {PickingUp}\n Location of Delivering= {Delivering},\n Distance= {Math.Round(Distance, 2)}";
        }
    }
}

