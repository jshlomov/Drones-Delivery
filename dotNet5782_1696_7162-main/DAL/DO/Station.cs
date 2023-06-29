using System;


namespace DO
{
    public struct Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FreeChargeSlots { get; set; }
        public Location TheLocation { get; set; }
        public bool Deleted { get; set; }
        public override string ToString()
        {
            return $"Station #{ID}: Name: {Name}, Free Charge Slots: {FreeChargeSlots}," +
                   $"\nlocation: {TheLocation}.";
        }
    }
}

