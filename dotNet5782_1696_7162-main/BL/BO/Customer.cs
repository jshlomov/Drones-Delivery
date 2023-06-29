using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<PackageInCust> PackageSentBy { get; set; }
        public IEnumerable<PackageInCust> PackageRecivedBy { get; set; }
        public override string ToString()
        {
            string s = $"customer {ID}: name= {Name}, phone= {Phone}, location= {Location}\n";
            s += "sent by\n";
            foreach (var item in PackageSentBy)
            {
                s += item + "\n";
            }
            s += "recived by\n";
            foreach (var item in PackageRecivedBy)
            {
                s += item + "\n";
            }
            return s;
        }
    }
}

