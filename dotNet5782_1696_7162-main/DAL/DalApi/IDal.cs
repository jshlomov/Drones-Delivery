using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalObject;

namespace DalApi
{
    public interface IDal
    {
        //Create functions
        void AddStation(Station station);
        void AddDrone(Drone drone);
        void AddCustomer(Customer cust);
        int AddPackage(Package package);

        //update functions
        void ColectingPackage(int packageId);
        void PackageDelivering(int packageId);
        void SendDroneToCharge(int droneId, int stationId);
        void releaseDroneFromCharging(int droneID);
        public void UpdateDroneName(int id, string model);
        public void UpdateStation(Station s);
        public void catchFreeSlote(int stationId);
        public void releaseFreeSlote(int stationId);
        public void UpdateCustomer(Customer c);
        void AssignPackageToDrone(int droneId, int packageId);

        //Read functions
        Station GetStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Package GetPackage(int id);
        IEnumerable<Station> GetAllStations(Predicate<Station> filter = null);
        IEnumerable<Drone> GetAllDrones(Predicate<Drone> filter = null);
        IEnumerable<Customer> GetAllCustomers(Predicate<Customer> filter = null);
        IEnumerable<Customer> GetallCustomersDel(Predicate<Customer> filter = null);
        IEnumerable<Package> GetAllPackages(Predicate<Package> filter = null);
        IEnumerable<DroneCharge> GetDronesCharge();

        double[] GetConsumptionOfElectricity();

        //delete functions
        public void RemoveStation(int id);
        public void RemoveDrone(int id);
        public void RemovePackage(int id);
        public void RemoveCustomer(int id);
    }
}
