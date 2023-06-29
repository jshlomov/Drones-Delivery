using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class PackageToList
    {
        public int ID { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? assigning { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return $"Package {ID}: sender name= {SenderName}, target name= {TargetName}, weight= {Weight}, priority= {Priority}";
        }
    }
}

