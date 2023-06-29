using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;
using DalXml;



namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBL()
        {
            return BlApi.BL.instance;
        }
    }

    internal partial class BL : IBL
    {
        Random rand;
        DalApi.IDal data;

        //this list save all drones data and will use in all program.
        List<DroneToList> dronesListBL = new List<DroneToList>();

        // data of using battery per weight and data of charging rate
        double free;
        double lightCarry;
        double mediumCarry;
        double heavyCarry;
        double chargingRate;

        // constructor : initioalize all drones to bl.
        // initioalize: location, status and battery. (the information dependence in packages data.)
        private BL()
        {
            try

            {
                rand = new Random();
                data = DalXml.DalFactory.GetDalXml();

                // intioalzize electric data:
                double[] electraData = data.GetConsumptionOfElectricity();
                free = electraData[0];
                lightCarry = electraData[1];
                mediumCarry = electraData[2];
                heavyCarry = electraData[3];
                chargingRate = electraData[4];

                #region constructor: initialize drones

                IEnumerable<DO.Drone> drones = data.GetAllDrones();
                IEnumerable<DO.Package> packages = data.GetAllPackages();
                IEnumerable<DO.Customer> customers = data.GetAllCustomers();
                IEnumerable<DO.Station> stations = data.GetAllStations();

                foreach (var i in drones)
                {
                    DroneToList drone = new DroneToList { ID = i.ID, Model = i.Model, MaxWeight = (WeightCategories)i.MaxWeight };
                    if (packages.Any(x => x.DroneId == i.ID && x.Delivered == null) == true)
                    {
                        DO.Package package = packages.First(x => x.DroneId == i.ID && x.Delivered == null);
                        drone.Status = DroneStatuses.OnDelivery;
                        drone.PackageID = package.ID;

                        var senderLocation = convertLocationToBL(customers.First(x => x.ID == package.SenderID).Location);
                        if (package.PickedUp == null)
                            drone.CurrentLocation = senderLocation;
                        else if (package.PickedUp != null)
                            drone.CurrentLocation = convertLocationToBL(data.GetStation(getNearestStation(senderLocation).Item1).TheLocation);
                        drone.Battery = Math.Round(GetRandomNumberInRange(getMinimumBatteryToDrone(drone, package.ID), 100), 2);
                    }
                    else
                    {
                        if (data.GetDronesCharge().Any(i => i.DroneID == drone.ID) == false)
                        {
                            drone.Status = DroneStatuses.Available;
                            var locations = packages.Where(x => x.Delivered != null);
                            if (locations.Any())
                                drone.CurrentLocation = GetCustomer(locations.ToList()[rand.Next(0, locations.Count())].TargetID).Location;
                            else
                                throw new NotImplementedException("there is no target then recived package");
                            drone.Battery = Math.Round(GetRandomNumberInRange(getNearestStation(drone.CurrentLocation).Item2 * free, 100), 2);
                        }
                        else
                        {
                            drone.Status = DroneStatuses.InRepair;
                            drone.CurrentLocation = convertLocationToBL(data.GetStation(data.GetDronesCharge().First(i => i.DroneID == drone.ID).StationID).TheLocation);
                            drone.Battery = Math.Round(GetRandomNumberInRange(0, 20), 2);
                        }
                    }
                    dronesListBL.Add(drone);
                }
                #endregion

            }
            catch (DO.IdExistException Ex)
            {
                throw new IdExistException("ERORR", Ex);
            }
            catch (DO.IdIsNotExistExeption Ex)
            {
                throw new IdIsNotExistExeption("ERORR", Ex);
            }
            catch (NotImplementedException ex)
            {
                throw new NotImplementedException("ERORR", ex);
            }

        }//constractor BL

        #region singleton
        /// <summary>
        /// static constructor for singelton
        /// </summary>
        static BL() { }
        internal static BL instance { get; } = new BL();
        #endregion

        #region Location methods
        private Location convertLocationToBL(DO.Location loc)
        {
            if (loc != null)
                return new Location { Latitude = loc.Latitude, Longitude = loc.Longitude };
            return null;
        }

        private bool locationsComparisonBL(Location loc1, Location loc2)
        {
            return (loc1.Latitude == loc2.Latitude && loc1.Longitude == loc2.Longitude);
        }

        #endregion

        #region all distance and charging methodes

        private double getMinimumBatteryToDrone(DroneToList drone, int packageID, DroneStatuses status = DroneStatuses.OnDelivery)
        {
            var p = data.GetAllPackages().First(x => x.ID == packageID);
            double elctricWeight = getWeightRate(p.ID);
            return getNearestStation(GetCustomer(p.TargetID).Location).Item2 * free +
                   (p.Delivered == null ? getDistance(GetCustomer(p.SenderID).Location, GetCustomer(p.TargetID).Location) * elctricWeight : 0) +
                   (p.PickedUp == null ? getDistance(drone.CurrentLocation, GetCustomer(p.SenderID).Location) * free : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns>reurn: item1= the nearest station. item2= the distance between the locations.</returns>
        private (int stationID, double) getNearestStation(Location location)
        {
            IEnumerable<DO.Station> dalStations = data.GetAllStations();
            DO.Station s = (from i in dalStations
                            orderby getDistance(location, convertLocationToBL(i.TheLocation))
                            select i).First();
            return (s.ID, getDistance(location, convertLocationToBL(s.TheLocation)));
        }

        /// <summary>
        /// checking if 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private bool isBatteryEnoughToAllPath(Drone drone, PackageToList package)
        {
            double elecWeight = getWeightRate(package.ID);

            Package p = GetPackage(package.ID);
            Location locOfSender = GetCustomer(p.Sender.ID).Location;
            Location locOfTarget = GetCustomer(p.Target.ID).Location;

            double puth = (getDistance(drone.Location, locOfSender) * free + (getDistance(locOfSender, locOfTarget)) * elecWeight
                + getNearestStation(locOfTarget).Item2 * free);

            return puth <= drone.Battery ? true : false;
        }

        private double getWeightRate(int id)
        {
            try
            {
                double elecWeight;
                switch (data.GetPackage(id).Weight)
                {
                    case DO.WeightCategories.Lite:
                        elecWeight = lightCarry;
                        break;
                    case DO.WeightCategories.Medium:
                        elecWeight = mediumCarry;
                        break;
                    case DO.WeightCategories.heavy:
                        elecWeight = heavyCarry;
                        break;
                    default:
                        throw new NotImplementedException("invalid weight.");
                }
                return elecWeight;
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption(ex.Message);
            }

        }

        private double getDistance(Location location1, Location location2)
        {
            double rlat1 = Math.PI * location1.Latitude / 180;
            double rlat2 = Math.PI * location2.Latitude / 180;
            double theta = location1.Longitude - location2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344; // return in KM
        }

        #endregion

        /// <summary>
        /// Get Random Number In Range include negative numbers and fractions
        /// </summary>
        private double GetRandomNumberInRange(double minNumber, double maxNumber)
        {
            return new Random().NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}

