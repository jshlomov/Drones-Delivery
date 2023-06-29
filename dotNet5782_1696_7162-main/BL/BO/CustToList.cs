using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class CustToList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SumOfDeliveredPackages { get; set; }
        public int SumOfSendedPackages { get; set; }
        public int SumOfRecivedPackages { get; set; }
        public int SumOfOnWayPackages { get; set; }
        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, phone: {Phone}," +
                   $"D";
        }
    }
}

