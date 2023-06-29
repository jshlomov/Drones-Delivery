using System;


namespace BO
{
    public enum WeightCategories
    {
        Lite, Medium, heavy
    }
    public enum DroneStatuses
    {
        Available, InRepair, OnDelivery
    }
    public enum Priorities
    {
        Ordinary, Urgently, Emergency
    }
    public enum Situations
    {
        Created, Assigned, PickedUp, Delivered
    }

    public enum AddChoise
    {
        EXIT, STATION, DRONE, CUSTOMER, PACKAGE
    }

    public enum UpdateChoise
    {
        EXIT, DRONE, STATION, CUSTOMER, DRONE_TO_CHARGE, DRONE_FROM_CHARGE,
        ASSIGN_PACK, PICK_UP_PACK, DELIVER_PACK
    }
}








//initialize bl old func

//List<DO.Drone> droneDalList = data.GetAllDrones().ToList();
//List<DO.Package> packagesList = data.GetAllPackages().ToList();
//List<DO.Customer> customersList = data.GetAllCustomers().ToList();

//#region constructor: initialize drones

//foreach (DO.Drone item in droneDalList)
//{
//    DroneToList drone = new()
//    {
//        ID = item.ID,
//        Model = item.Model,
//        MaxWeight = (WeightCategories)item.MaxWeight,
//    };


//    int pack = packagesList.FindIndex(i => i.DroneId == item.ID && i.Delivered == null);

//    //if there are drone's id, it's already assigned, so i checking the delivered time only.
//    if (pack != -1)
//    {
//        DO.Package p = packagesList[pack];
//        drone.PackageID = p.ID;
//        drone.Status = DroneStatuses.OnDelivery;
//        if (p.PickedUp == null) //if the package assigned but did not picked up: put the drone in the nearest station.
//        {
//            DO.Customer sender = customersList.Find(i => i.ID == p.SenderID); //find the customer. (to use his location.)
//            drone.CurrentLocation = getNearestStation(convertLocationToBL(sender.Location)).Item1.Location; //find the nearest station.
//        }
//        else if (p.PickedUp != null) // if the package picked up: put the drone in sender location
//        {
//            DO.Customer sender = customersList.Find(i => i.ID == p.SenderID);
//            drone.CurrentLocation = convertLocationToBL(sender.Location);
//        }
//        // calculate the drone's battery by the path it need to pass.
//        drone.Battery = Math.Round(GetRandomNumberInRange(getMinimumBatteryToDrone(item, drone.CurrentLocation), 100), 2);
//    }
//    else // if there is'nt package assigning to this drone:
//    {
//        drone.Status = (DroneStatuses)rand.Next(0, 2); //Random between "available" and "inReapair"
//        if (drone.Status == DroneStatuses.Available)
//        {
//            List<DO.Package> targetP = packagesList.FindAll(i => i.Delivered != null);
//            if (targetP.Any())
//            {
//                int targetID = targetP[rand.Next(0, targetP.Count())].TargetID;
//                DO.Customer customer = customersList.Find(i => i.ID == targetID);
//                drone.CurrentLocation = convertLocationToBL(customer.Location);
//            }
//            else throw new NotImplementedException("there is no target then recived package");
//            drone.Battery = Math.Round(GetRandomNumberInRange(getMinimumBatteryToDrone(item, drone.CurrentLocation), 100), 2);
//        }
//        else
//        {
//            List<DO.Station> stations = data.GetAllStations().ToList();
//            drone.CurrentLocation = convertLocationToBL(stations[rand.Next(0, stations.Count())].TheLocation);
//            drone.Battery = Math.Round(GetRandomNumberInRange(0, 20), 2);
//        }
//    }
//    dronesListBL.Add(drone);




//try
//{
//    IEnumerable<DO.Package> packages = data.GetAllPackages();
//    IEnumerable<DO.Customer> customers = data.GetAllCustomers();

//    if (packages.Any(i => i.DroneId == droneID && i.Delivered == null) || status == DroneStatuses.Available) //if the drone assigned
//    {
//        return (getNearestStation(location).Item2 * free);
//    }
//    else //if the drone available and did not assigning to any package
//    {
//        DO.Package package = packages.First(i => i.DroneId == droneID && i.Delivered == null);
//        Location senderI = convertLocationToBL(customers.First(i => i.ID == package.SenderID).Location);
//        Location targetI = convertLocationToBL(customers.First(i => i.ID == package.TargetID).Location);

//        double elecWeight = getWeightRate(package.ID);

//        //calculate the path and the battery that necessery.                                        
//        if (package.assigning != null && package.PickedUp == null)           //all way, from station to sender to reciver and to the near station.
//            return (getDistance(location, senderI) * free +
//                (getDistance(senderI, targetI)) * elecWeight +
//                getNearestStation(targetI).Item2 * free);

//        else if (package.PickedUp != null)                                    //from sender to reciver and to the near station
//            return (getDistance(location, targetI) * elecWeight +
//                getNearestStation(targetI).Item2 * free);

//        else if (package.Delivered != null)                                   // from reciver to the nearest station.
//            return (getNearestStation(location).Item2 * free);

//        throw new NotImplementedException("there is no path to pass."); ;     //invalid value
//    }
//}
//catch (NotImplementedException ex)
//{
//    throw ex.InnerException;
//}