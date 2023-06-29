//Yonatan Shlomov 319217162
//Asher Elos 311271696

using System;
using System.Collections.Generic;
using DO;
using DalApi;
using ConsoleUI;

namespace ConsoleUI
{
    class Program
    {
        public enum Choise { NON, ADD, UPDATE, DISPALY, LIST, EXIT };
        public static DalApi.IDal obj = DalApi.DalFactory.GetDal();
        public static void Main(string[] args)
        {
            //Location l = new Location { Latitude = 2, Longitude = 1 };
            //Location k = new Location { Latitude = 2, Longitude = 1 };
            //Choise choise = 0;
            //while (choise != Choise.EXIT)
            //{
            //    Console.WriteLine($"Press 1 for add options\n" +
            //                      $"Press 2 for update options\n" +
            //                      $"Press 3 for display options\n" +
            //                      $"Press 4 for dispaly lists options\n" +
            //                      $"Press 5 to exit");

            //    choise = (Choise)int.Parse(Console.ReadLine());
            //    switch (choise)
            //    {
            //        case Choise.ADD:
            //            AddMain();
            //            break;
            //        case Choise.UPDATE:
            //            UpdateMain();
            //            break;
            //        case Choise.DISPALY:
            //            DisplayMain();
            //            break;
            //        case Choise.LIST:
            //            DisplayListMain();
            //            break;
            //        case Choise.EXIT:
            //            break;
            //        default:
            //            Console.WriteLine("invalid choise");
            //            break;
            //    }
            //}
        }//main

        ///// <summary>
        ///// adding an object to the data base according to the user choise.
        ///// </summary>
        //public static void AddMain()
        //{
        //    int choise;
        //    Console.WriteLine("press 1 to add station\n" +
        //                      "press 2 to add drone\n" +
        //                      "press 3 to add customer\n" +
        //                      "press 4 to add package");
        //    choise = int.Parse(Console.ReadLine());
        //    switch (choise)
        //    {
        //        case 1:
        //            addStation();
        //            break;
        //        case 2:
        //            addDrone();
        //            break;
        //        case 3:
        //            addCustomer();
        //            break;
        //        case 4:
        //            addPackage();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        ///// <summary>
        ///// update data in the database according to the user choise.
        ///// </summary>
        //public static void UpdateMain()
        //{
        //    int choise;
        //    Console.WriteLine("press 1 to assign package to drone\n" +
        //                      "press 2 to update that a package collected by a drone\n" +
        //                      "press 3 to update that a package delivered\n" +
        //                      "press 4 to send drone to charge in a base station\n" +
        //                      "press 5 to Release drone from charging in the base station");
        //    choise = int.Parse(Console.ReadLine());
        //    switch (choise)
        //    {
        //        case 1:
        //            AssignPackageToDrone();
        //            break;
        //        case 2:
        //            ColectingPackage();
        //            break;
        //        case 3:
        //            PackageDelivered();
        //            break;
        //        case 4:
        //            SendDroneToCharge();
        //            break;
        //        case 5:
        //            releaseDroneFromCharging();
        //            break;
        //        default:
        //            Console.WriteLine("invalid choise");
        //            break;
        //    }
        //}

        ///// <summary>
        ///// displaying spacific object from the database according to the user choise.
        ///// </summary>
        //public static void DisplayMain()
        //{
        //    int choise, id;
        //    Console.WriteLine("press 1 to display station\n" +
        //                      "press 2 to display drone\n" +
        //                      "press 3 to display customer\n" +
        //                      "press 4 to display package");
        //    choise = int.Parse(Console.ReadLine());
        //    switch (choise)
        //    {
        //        case 1:
        //            Console.WriteLine("Enter station ID:");
        //            int.TryParse(Console.ReadLine(), out id);
        //            Station station = obj.GetStation(id);
        //            Console.WriteLine(station);
        //            break;
        //        case 2:
        //            Console.WriteLine("Enter drone ID");
        //            int.TryParse(Console.ReadLine(), out id);
        //            Drone drone = obj.GetDrone(id);
        //            Console.WriteLine(drone);
        //            break;
        //        case 3:
        //            Console.WriteLine("Enter customer ID:");
        //            int.TryParse(Console.ReadLine(), out id);
        //            Customer customer = obj.GetCustomer(id);
        //            Console.WriteLine(customer);
        //            break;
        //        case 4:
        //            Console.WriteLine("Enter package ID:");
        //            int.TryParse(Console.ReadLine(), out id);
        //            Package package = obj.GetPackage(id);
        //            Console.WriteLine(package);
        //            break;
        //        default:
        //            Console.WriteLine("invalid choise!");
        //            break;
        //    }
        //}

        ///// <summary>
        ///// dispalying a list of data according to the user choise.
        ///// </summary>
        //public static void DisplayListMain()
        //{
        //    int choise;
        //    Console.WriteLine("press 1 to display all Stations\n" +
        //                      "press 2 to display all Drones\n" +
        //                      "press 3 to display all Customers\n" +
        //                      "press 4 to dispaly all Packages\n" +
        //                      "press 5 to display all that not assigning to a drone\n" +
        //                      "press 6 to dispaly all Stations that have free charge slots");
        //    int.TryParse(Console.ReadLine(), out choise);
        //    switch (choise)
        //    {
        //        case 1:
        //            IEnumerable<Station> stationList = obj.GetAllStations();
        //            PrintList(stationList);
        //            break;
        //        case 2:
        //            IEnumerable<Drone> droneList = obj.GetAllDrones();
        //            PrintList(droneList);
        //            break;
        //        case 3:
        //            IEnumerable<Customer> customerList = obj.GetAllCustomers();
        //            PrintList(customerList);
        //            break;
        //        case 4:
        //            IEnumerable<Package> packageList = obj.GetAllPackages();
        //            PrintList(packageList);
        //            Console.WriteLine();
        //            break;
        //        case 5:
        //            IEnumerable<Package> unassignedPackage = obj.GetUnassignedPackages();
        //            PrintList(unassignedPackage);
        //            break;
        //        case 6:
        //            IEnumerable<Station> stationsWithFreeSlots = obj.GetStationsWithFreeSlots();
        //            PrintList(stationsWithFreeSlots);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        ///// <summary>
        ///// adding station to the data base by geting a data from the user.
        ///// </summary>
        //public static void addStation()
        //{
        //    int id, freeChSlots; double longitude, latitude; string name;
        //    Console.WriteLine("Enter: station's ID");
        //    int.TryParse(Console.ReadLine(), out id);
        //    Console.WriteLine("Enter: station's Name");
        //    name = Console.ReadLine();
        //    Console.WriteLine("Enter: station's Free charge slots");
        //    int.TryParse(Console.ReadLine(), out freeChSlots);
        //    Console.WriteLine("Enter station's location: longitude, latitude");
        //    double.TryParse(Console.ReadLine(), out longitude);
        //    double.TryParse(Console.ReadLine(), out latitude);
        //    Location l = new Location() { Latitude = latitude, Longitude = longitude };
        //    Station stat = new Station()
        //    {
        //        Id = id,
        //        Name = name,
        //        FreeChargeSlots = freeChSlots,
        //        TheLocation = l
        //    };
            
            
        //    obj.AddStation(stat);
        //}

        ///// <summary>
        ///// adding drone to the data base by geting a data from the user.
        ///// </summary>
        //public static void addDrone()
        //{
        //    int id; double battery; WeightCategories weight; DroneStatuses status; string model;
        //    Console.WriteLine("Enter: dron's ID");
        //    int.TryParse(Console.ReadLine(), out id);
        //    Console.WriteLine("Enter: dron's Model");
        //    model = Console.ReadLine();
        //    Console.WriteLine("Enter: dron's maxWeight(Light - 0, Medium - 1, Heavy - 2)");
        //    WeightCategories.TryParse(Console.ReadLine(), out weight);
        //    Console.WriteLine("Enter: dron's status(Available - 0, InRepair - 1, OnDelivery - 2)");
        //    DroneStatuses.TryParse(Console.ReadLine(), out status);
        //    Console.WriteLine("Enter:  battery");
        //    double.TryParse(Console.ReadLine(), out battery);
        //    Drone drone = new Drone
        //    {
        //        Id = id,
        //        Model = model,
        //        MaxWeight = weight,
        //    };
        //    obj.AddDrone(drone);
        //}

        ///// <summary>
        ///// adding customer to the data base by geting a data from the user.
        ///// </summary>
        //public static void addCustomer()
        //{
        //    int id; string name, phone; double longitude, latitude;
        //    Console.WriteLine("Enter: customer's ID");
        //    int.TryParse(Console.ReadLine(), out id);
        //    Console.WriteLine("Enter: customer's Name");
        //    name = Console.ReadLine();
        //    Console.WriteLine("Enter: customer's Phone Number");
        //    phone = Console.ReadLine();
        //    Console.WriteLine("Enter: customer's location: longitude, latitude");
        //    double.TryParse(Console.ReadLine(), out longitude);
        //    double.TryParse(Console.ReadLine(), out latitude);
        //    Location l = new Location() { Latitude = latitude, Longitude = longitude };
        //    Customer cust = new Customer
        //    {
        //        Id = id,
        //        Name = name,
        //        Phone = phone,
        //        TheLocation = l
        //    };
        //    obj.AddCustomer(cust);
        //}

        ///// <summary>
        ///// adding package to the data baseby geting a data from the user.
        ///// </summary>
        //public static void addPackage()
        //{
        //    int senderId, targetId;
        //    WeightCategories weight;
        //    Priorities priority;
        //    Console.WriteLine("Enter: Sender's ID");
        //    int.TryParse(Console.ReadLine(), out senderId);
        //    Console.WriteLine("Enter: Target's ID");
        //    int.TryParse(Console.ReadLine(), out targetId);
        //    Console.WriteLine("Enter: Weight(Light, Medium, Heavy)");
        //    WeightCategories.TryParse(Console.ReadLine(), out weight);
        //    Console.WriteLine("Eenter Priority(Ordinary, Urgently, Emergency)");
        //    Priorities.TryParse(Console.ReadLine(), out priority);

        //    Package pack = new Package
        //    {
        //        Id = obj.GetPackageRunID(),
        //        SenderId = senderId,
        //        TargetId = targetId,
        //        DroneId = 0,
        //        Weight = weight,
        //        Priority = priority,
        //        Creation = DateTime.Now,
        //        assigning = DateTime.MinValue,
        //        PickedUp = DateTime.MinValue,
        //        Delivered = DateTime.MinValue
        //    };
        //    obj.AddPackage(pack);
        //}

        ///// <summary>
        ///// assigning in the package's properties the id of the drone who will take it.
        ///// </summary>
        //public static void AssignPackageToDrone()
        //{
        //    int droneId, packageId;
        //    Console.WriteLine("Enter drone's ID:");
        //    int.TryParse(Console.ReadLine(), out droneId);
        //    Console.WriteLine("Enter package's ID:");
        //    int.TryParse(Console.ReadLine(), out packageId);
        //    obj.AssignPackageToDrone(droneId, packageId);
        //}

        ///// <summary>
        ///// update in the package properties that the package picked up in this moment (date and time).
        ///// </summary>
        //public static void ColectingPackage()
        //{
        //    int packageId;
        //    Console.WriteLine("Enter package's ID:");
        //    int.TryParse(Console.ReadLine(), out packageId);
        //    obj.ColectingPackage(packageId);
        //}

        ///// <summary>
        ///// update in the package properties that the package delivered in this moment (date and time).
        ///// </summary>
        //public static void PackageDelivered()
        //{
        //    int packageId;
        //    Console.WriteLine("Enter package's ID:");
        //    int.TryParse(Console.ReadLine(), out packageId);
        //    obj.PackageDelivered(packageId);
        //}

        ///// <summary>
        ///// send drone to charge in base station and define him "in repair".
        ///// </summary>
        //public static void SendDroneToCharge()
        //{
        //    int droneId, stationId;
        //    Console.WriteLine("Enter drone's ID:");
        //    int.TryParse(Console.ReadLine(), out droneId);
        //    Console.WriteLine("enter station's ID");
        //    int.TryParse(Console.ReadLine(), out stationId);
        //    obj.SendDroneToCharge(droneId, stationId);
        //}

        ///// <summary>
        ///// release drone from charching and makes it available.
        ///// </summary>
        //public static void releaseDroneFromCharging()
        //{
        //    int droneId;
        //    Console.WriteLine("Enter drone's ID:");
        //    int.TryParse(Console.ReadLine(), out droneId);
        //    obj.releaseDroneFromCharging(droneId);
        //}

        ///// <summary>
        ///// print all lists
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="list"></param>
        //public static void PrintList<T>(IEnumerable<T> list)where T:struct
        //{
        //    foreach (T item in list)
        //        Console.WriteLine(item);
        //}
    }
}