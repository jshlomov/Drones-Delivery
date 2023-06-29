using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace ConsoleUI_BL
{
    class Program
    {
        public enum Choise { EXIT, ADD, UPDATE, DISPALY, LIST };
        public static BlApi.IBL Bo = BlApi.BlFactory.GetBL();
        static void Main(string[] args)
        {
            try
            {
                List<BO.DroneToList> drones = Bo.GetAllDrones(x => x.Status == BO.DroneStatuses.Available).ToList();
                foreach (var item in drones)
                {
                    Drone d = Bo.GetDrone(item.ID);
                    if (d.Status == DroneStatuses.Available)
                    {
                        Bo.SendDroneToCharge(d);
                        Console.WriteLine("aa");
                    }
                }
                List<StationToList> stations = Bo.GetAllStations().ToList();
                foreach (var item in stations)
                {
                    Station s = Bo.GetStation(item.ID);
                    
                    foreach (var i in s.ChargingDrones)
                    {
                        Console.WriteLine(i);
                    }
                }
                List<CustToList> custToLists = Bo.GetAllCustomers().ToList();
                foreach (var item in custToLists)
                {
                    Customer customer = Bo.GetCustomer(item.ID);
                    Console.WriteLine(customer);
                    //foreach (var i in customer.PackageSentBy)
                    //{
                    //    Console.WriteLine();
                    //}
                }
            }
            catch (NotImplementedException ex)
            {

                Console.WriteLine(ex); ;
            }


            //SendDroneToCharge();
            //Choise choise = (Choise)int.Parse(Console.ReadLine());
            //switch (choise)
            //{
            //    case Choise.ADD:
            //        AddChoise ac = (AddChoise)int.Parse(Console.ReadLine());
            //        switch (ac)
            //        {
            //            case AddChoise.STATION:
            //                AddStation();
            //                break;
            //            case AddChoise.DRONE:
            //                AddDrone();
            //                break;
            //            case AddChoise.CUSTOMER:
            //                AddCustomer();
            //                break;
            //            case AddChoise.PACKAGE:
            //                AddPackage();
            //                break;
            //            case AddChoise.EXIT:
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    case Choise.UPDATE:
            //        UpdateChoise uc = (UpdateChoise)int.Parse(Console.ReadLine());
            //        switch (uc)
            //        {
            //            case UpdateChoise.DRONE:
            //                UpdateDrone();
            //                break;
            //            case UpdateChoise.STATION:
            //                UpdateStation();
            //                break;
            //            case UpdateChoise.CUSTOMER:
            //                UpdateCustomer();
            //                break;
            //            case UpdateChoise.DRONE_TO_CHARGE:
            //                SendDroneToCharge();
            //                break;
            //            case UpdateChoise.DRONE_FROM_CHARGE:
            //                releaseDroneFromCharging();
            //                break;
            //            case UpdateChoise.ASSIGN_PACK:
            //                break;
            //            case UpdateChoise.PICK_UP_PACK:
            //                break;
            //            case UpdateChoise.DELIVER_PACK:
            //                break;
            //            case UpdateChoise.EXIT:
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //        //        case Choise.DISPALY:
            //        //            break;
            //        //        case Choise.LIST:
            //        //            break;
            //        //        case Choise.EXIT:
            //        //            break;
            //        //        default:
            //        //            break;
            //}
        }

        public static void AddStation()
        {
            Station station = new Station();
            Console.WriteLine($"");
            station.ID = int.Parse(Console.ReadLine());
            station.Name = Console.ReadLine();
            station.Location.Longitude = double.Parse(Console.ReadLine());
            station.Location.Latitude = double.Parse(Console.ReadLine());
            station.FreeChargeSlots = int.Parse(Console.ReadLine());
            station.ChargingDrones = new List<DroneAtCharge>();
            Bo.AddStation(station);
        }

        public static void AddDrone()
        {
            WeightCategories weight;
            Drone drone = new();
            drone.ID = int.Parse(Console.ReadLine());
            drone.Model = Console.ReadLine();
            WeightCategories.TryParse(Console.ReadLine(), out weight);
            drone.MaxWeight = weight;
            Console.WriteLine("station ID to put the drone:");
            int stationId = int.Parse(Console.ReadLine());
            Bo.AddDrone(drone, stationId);
        }

        public static void AddCustomer()
        {
            Customer customer = new();
            customer.ID = int.Parse(Console.ReadLine());
            customer.Name = Console.ReadLine();
            customer.Phone = Console.ReadLine();
            customer.Location.Longitude = double.Parse(Console.ReadLine());
            customer.Location.Latitude = double.Parse(Console.ReadLine());
            Bo.AddCustomer(customer);
        }

        public static void AddPackage()
        {
            WeightCategories w;
            Priorities p;
            Package package = new();
            package.Sender.ID = int.Parse(Console.ReadLine());
            package.Target.ID = int.Parse(Console.ReadLine());
            WeightCategories.TryParse(Console.ReadLine(), out w);
            package.Weight = w;
            Priorities.TryParse(Console.ReadLine(), out p);
            package.Priority = p;
            Bo.AddPackage(package);
        }

        public static void UpdateDrone()
        {
            Console.WriteLine();
            int id = int.Parse(Console.ReadLine());
            Drone d = Bo.GetDroneExist(id); 
            Console.WriteLine();
            d.Model = Console.ReadLine();
            Bo.UpdateDroneName(d);
        }

        public static void UpdateStation()
        {
            Console.WriteLine();
            int id = int.Parse(Console.ReadLine());
            Station s = Bo.GetStation(id);
            Console.WriteLine();
            s.Name = Console.ReadLine();
            Console.WriteLine();
            Bo.UpadateStation(s);
        }

        public static void UpdateCustomer()
        {
            Console.WriteLine();
            int id = int.Parse(Console.ReadLine());
            Customer c = Bo.GetCustomer(id);
            Console.WriteLine();
            c.Name = Console.ReadLine();
            Console.WriteLine();
            c.Phone = Console.ReadLine();
            Bo.UpdateCustomer(c);
        }

        public static void SendDroneToCharge()
        {
            Console.WriteLine("enter id:");
            int id = int.Parse(Console.ReadLine());
            Drone drone = Bo.GetDroneExist(id);
            Bo.SendDroneToCharge(drone);
        }

        public static void releaseDroneFromCharging()
        {
            Console.WriteLine();
            int id = int.Parse(Console.ReadLine());
            Drone drone = Bo.GetDroneExist(id);
            Console.WriteLine();
            double chargeingTime = double.Parse(Console.ReadLine());
            Bo.releaseDroneFromCharging(drone, chargeingTime);
        }

        //public static void assigningPackageToDrone()
        //{
        //    Console.WriteLine();
        //    int id = int.Parse(Console.ReadLine());
        //    Package package = Bo.get
        //}

    }
}

