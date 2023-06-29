using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        //Create
        public void AddStation(Station station);
        public void AddDrone(Drone d, int stationId);
        public void AddCustomer(Customer c);
        public void AddPackage(Package p);

        //Read
        public Drone GetDrone(int id);
        public Station GetStation(int id);
        public Customer GetCustomer(int id);
        public Package GetPackage(int id);

        public IEnumerable<StationToList> GetAllStations(Predicate<StationToList> filter = null);
        public IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> filter = null);
        public IEnumerable<PackageToList> GetAllPackages(Predicate<PackageToList> filter = null);
        public IEnumerable<CustToList> GetAllCustomers(Predicate<CustToList> filter = null);

        //Update
        public void UpdateDroneName(Drone drone);
        public void UpadateStation(Station s);
        public void UpdateCustomer(Customer c);
        public void SendDroneToCharge(Drone drone);
        public void releaseDroneFromCharging(Drone drone, double chargingTime);
        public void assigningPackageToDrone(Drone drone);
        public void ColectingPackageFromSender(Drone drone);
        public void PackageDelivering(Drone drone);

        //Delete
        public void RemoveDrone(int id);
        public void RemovePackage(int id);
        public void RemoveCustomer(int id);
        public void RemoveStation(int id);
    }
}
