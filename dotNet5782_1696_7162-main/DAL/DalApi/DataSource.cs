using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    internal class DataSource
    {
        static DataSource() { Initialize(); }

        internal static List<Station> Stations = new();
        internal static List<Drone> Drones = new();
        internal static List<Customer> Customers = new();
        internal static List<Package> Packages = new();
        internal static List<DroneCharge> DronesCharges = new();

        internal class Config
        {
            internal static int packegeRunId = 1;
            public static double freeWeight { get { return 1; } }
            public static double lightCarry { get { return 2; } }
            public static double mediumCarry { get { return 2.5; } }
            public static double heavyCarry { get { return 3; } }
            public static double chargingRate { get { return 10; } }
        }

        public static Random rand = new Random();

        /// <summary>
        /// instaling part of the data
        /// </summary>
        public static void Initialize()
        {

            #region stations
            Stations = new List<Station>
            {
                new Station { ID = 1, FreeChargeSlots = 9, Name = "Bursa", TheLocation = new Location { Latitude=  32.083901, Longitude = 34.801305} },
                new Station { ID = 2, FreeChargeSlots = 7, Name = "Azrieli", TheLocation = new Location { Latitude=  32.074898, Longitude = 34.792488 } },
                new Station { ID = 3, FreeChargeSlots = 7, Name = "Tahana Mercazit", TheLocation = new Location { Latitude=  32.056230, Longitude = 34.780118 } }
            };
            #endregion

            #region Drones:
            // i didn't use random because they are dependent with other data.
            Drones = new List<Drone>
            {
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = rand.Next(1000, 2000).ToString(),
                    MaxWeight = WeightCategories.Lite,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "A 2019",
                    MaxWeight = WeightCategories.heavy,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "B 2015",
                    MaxWeight = WeightCategories.Medium,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "B 2014",
                    MaxWeight = WeightCategories.heavy,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "C 2012",
                    MaxWeight = WeightCategories.Medium,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "C 2012",
                    MaxWeight = WeightCategories.Lite,
                },
                new Drone
                {
                    ID = rand.Next(1000, 9999),
                    Model = "C 2012",
                    MaxWeight = WeightCategories.heavy,
                }
            };
            #endregion

            #region Customers:
            for (int i = 0; i < 10; i++)
            {
                //Location l = new Location
                //{
                //    Latitude = Math.Round(GetRandomNumberInRange(32.097275, 32.037493), 6),//Tel Aviv. from west to east
                //    Longitude = Math.Round(GetRandomNumberInRange(34.750518, 34.803144), 6)//Tel Aviv. from north to south
                //};
                Customer Customer = new()
                {
                    ID = rand.Next(100000000, 1000000000), // 9 digids
                    Name = ((Names)i).ToString(), // i maked list of names called in enum "Names" (look at the cs file "Enums")
                    Phone = "05" + Convert.ToString(rand.Next(0, 10)) + "-" + Convert.ToString(rand.Next(0, 10000000)).PadLeft(7, '0'),
                    Location = new Location
                    {
                        Latitude = Math.Round(GetRandomNumberInRange(32.097275, 32.037493), 6),//Tel Aviv. from west to east
                        Longitude = Math.Round(GetRandomNumberInRange(34.750518, 34.803144), 6)//Tel Aviv. from north to south
                    }
            };
                Customers.Add(Customer);
            }
            #endregion

            #region Packages:
            // the first 7 Packages was delivered, that is what you see in the loop.(we have only 5 Drones....
            // and one of them in repair and the secend Available)
            Package Package;
            for (int i = 0; i < 7; i++)
            {
                Package = new()  //all of the data instalig with random
                {
                    ID = Config.packegeRunId++,
                    SenderID = Customers[0].ID,
                    TargetID = Customers[1].ID, //its can not be a same person
                    DroneId = Drones[rand.Next(0, Drones.Count)].ID,  //its doesnt metter which drone took the package
                    Weight = (WeightCategories)rand.Next(0, 3),
                    Priority = (Priorities)rand.Next(0, 3),
                    Creation = new DateTime(2021, rand.Next(1, 4), rand.Next(1, 28), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)), // Random date and time on 2021
                    assigning = new DateTime(2021, rand.Next(4, 7), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                    PickedUp = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                    Delivered = new DateTime(2021, rand.Next(10, 13), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60))
                };
                Packages.Add(Package);
            }

            //the Packages during a delivery, the first two
            //not delivered yet and the last not pickedup.
            Packages.Add(new Package()
            {
                ID = Config.packegeRunId++,
                SenderID = Customers[rand.Next(0, Customers.Count / 2)].ID,
                TargetID = Customers[rand.Next(Customers.Count / 2, Customers.Count)].ID,
                DroneId = Drones[0].ID,
                Weight = WeightCategories.Lite,
                Priority = Priorities.ordinary,
                Creation = new DateTime(2021, rand.Next(1, 4), rand.Next(1, 28), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                assigning = new DateTime(2021, rand.Next(4, 7), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                PickedUp = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                Delivered = null
            });

            Packages.Add(new()
            {
                ID = Config.packegeRunId++,
                SenderID = Customers[1].ID,
                TargetID = Customers[2].ID,
                DroneId = Drones[1].ID,
                Weight = WeightCategories.heavy,
                Priority = Priorities.urgently,
                Creation = new DateTime(2021, rand.Next(1, 4), rand.Next(1, 28), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                assigning = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                PickedUp = DateTime.Now,
                Delivered = null
            });

            Packages.Add(new()
            {
                ID = Config.packegeRunId++,
                SenderID = Customers[rand.Next(0, Customers.Count / 2)].ID,
                TargetID = Customers[rand.Next(Customers.Count / 2, Customers.Count)].ID,
                DroneId = Drones[2].ID,
                Weight = WeightCategories.Medium,
                Priority = Priorities.Emergency,
                Creation = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                assigning = DateTime.Now,
                PickedUp = null,
                Delivered = null
            });
            Packages.Add(new()
            {
                ID = Config.packegeRunId++,
                SenderID = Customers[rand.Next(0, Customers.Count / 2)].ID,
                TargetID = Customers[rand.Next(Customers.Count / 2, Customers.Count)].ID,
                DroneId = 0,
                Weight = WeightCategories.Medium,
                Priority = Priorities.urgently,
                Creation = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                assigning = null,
                PickedUp = null,
                Delivered = null
            });
            Packages.Add(new()
            {
                ID = Config.packegeRunId++,
                SenderID = Customers[rand.Next(0, Customers.Count / 2)].ID,
                TargetID = Customers[rand.Next(Customers.Count / 2, Customers.Count)].ID,
                DroneId = 0,
                Weight = WeightCategories.Medium,
                Priority = Priorities.urgently,
                Creation = new DateTime(2021, rand.Next(7, 10), rand.Next(1, 31), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                assigning = null,
                PickedUp = null,
                Delivered = null
            });
            #endregion
        }

        /// <summary>
        /// helping method: 
        /// Get Random Number In Range include negative numbers and fractions
        /// </summary>
        public static double GetRandomNumberInRange(double minNumber, double maxNumber)
        {
            return new Random().NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}