using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    class conditions
    {
        public static bool BestWeigt(Package p)
        {
            return p.Weight > 0;
        }

        public static void sort(List<Package> packages)
        {
            packages.Sort((a, b) => (a.Priority.CompareTo(b.Priority)));
        }

    }
}
