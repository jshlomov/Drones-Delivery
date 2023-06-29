using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class PackageInCust
    {
        public int ID { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public Situations Situation { get; set; }
        public CustAtPackage TheOtherCustomer { get; set; }
        public override string ToString()
        {
            return $"Package {ID}: Weight= {Weight}, Priority= {Priority}, Situation= {Situation}, The Other CUstomer= {TheOtherCustomer}";
        }
    }
}

