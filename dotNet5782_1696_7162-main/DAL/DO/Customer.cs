using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public bool Deleted { get; set; }
        public override string ToString()
        {
            return $"Customer #{ID}: Name: {Name}, Phone: {Phone}," +
                   $"\n {Location}";
        }
    }
}

