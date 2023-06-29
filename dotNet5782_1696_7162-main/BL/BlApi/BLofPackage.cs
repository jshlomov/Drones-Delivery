using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    internal partial class BL : IBL
    {
        public void AddPackage(Package p)
        {
            //i dont understand if i need to create package's list in the BL
            try
            {
                data.AddPackage(new DO.Package
                {
                    SenderID = p.Sender.ID,
                    TargetID = p.Target.ID,
                    Weight = (DO.WeightCategories)p.Weight,
                    Priority = (DO.Priorities)p.Priority,
                    DroneId = 0,
                    Creation = DateTime.Now,
                    assigning = null,
                    PickedUp = null,
                    Delivered = null
                });
            }
            catch (DO.IdExistException ex)
            {
                throw new IdExistException("ERROR", ex);
            }
        }

        public void assigningPackageToDrone(Drone drone)
        {
            if (drone.Status != DroneStatuses.Available)
                throw new NotImplementedException("the drone is not available.");
            PackageToList bestPackage = GetBestPackageToAssigning(drone);
            if (bestPackage != null)
            {
                data.AssignPackageToDrone(drone.ID, bestPackage.ID);
                int index = dronesListBL.FindIndex(i => i.ID == drone.ID);
                DroneToList d = dronesListBL[index];
                d.PackageID = bestPackage.ID;
                d.Status = DroneStatuses.OnDelivery;
                dronesListBL[index] = d;
            }
            else
                throw new NoBatteryToPath("no battery to path.");
        }

        public void ColectingPackageFromSender(Drone drone)
        {
            if (drone.Package == null)
                throw new NotImplementedException("the drone not assigned to a package!");
            PackageToList p = GetAllPackages().First(i => i.ID == drone.Package.ID && i.PickedUp == null);
            if (p != null)
            {
                data.ColectingPackage(p.ID);

                int index = dronesListBL.FindIndex(i => i.ID == drone.ID);
                DroneToList newDrone = dronesListBL[index];
                newDrone.CurrentLocation = drone.Package.PickingUp;
                newDrone.Battery = drone.Battery - (getDistance(drone.Location, drone.Package.PickingUp) * free);
                dronesListBL[index] = newDrone;
            }
            else
                throw new NotImplementedException("the drone already sended this package!");
        }

        public void PackageDelivering(Drone drone)
        {
            try
            {
                if (drone.Status != DroneStatuses.OnDelivery)
                    throw new NotImplementedException("the drone is not on delivery!");
                if (drone.Package.Situation == false)
                    throw new NotImplementedException("the drone did not pick up the package yet.");

                int index = dronesListBL.FindIndex(i => i.ID == drone.ID);
                double weight = drone.Package.Weight == WeightCategories.heavy ? heavyCarry : (drone.Package.Weight == WeightCategories.Medium ? mediumCarry : lightCarry);
                dronesListBL[index].Battery = drone.Battery - (getDistance(drone.Location, drone.Package.Delivering) * weight);
                dronesListBL[index].CurrentLocation = drone.Package.Delivering;
                dronesListBL[index].Status = DroneStatuses.Available;
                dronesListBL[index].PackageID = 0;
                data.PackageDelivering(drone.Package.ID);
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public Package GetPackage(int id)
        {
            try
            {
                DO.Package p = data.GetPackage(id);
                List<DO.Customer> custList = data.GetAllCustomers().ToList();

                Package package = new()
                {
                    ID = p.ID,
                    Sender = new CustAtPackage { ID = p.SenderID, Name = custList.Find(i => i.ID == p.SenderID).Name },
                    Target = new CustAtPackage { ID = p.TargetID, Name = custList.Find(i => i.ID == p.TargetID).Name },
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority,
                    assigning = p.assigning,
                    Creation = p.Creation,
                    PickedUp = p.PickedUp,
                    Delivered = p.Delivered
                };

                DroneToList d = dronesListBL.Find(i => i.ID == p.DroneId);
                if (d == null)
                    package.Drone = null;
                else
                    package.Drone = new DroneAtPackage { ID = d.ID, Battery = d.Battery, CurrentLocation = d.CurrentLocation };

                return package;
            }
            catch (DO.IdIsNotExistExeption ex)
            {

                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public IEnumerable<PackageToList> GetAllPackages(Predicate<PackageToList> filter = null)
        {
            List<PackageToList> newPackages = convertPackagesToBL();
            return newPackages.Where(x => filter == null ? true : filter(x));
        }

        public void RemovePackage(int id)
        {
            Package p = GetPackage(id);
            if (p.assigning != null && p.Delivered == null)
                throw new NotImplementedException("the package in a middle of delivery!");
            data.RemovePackage(id);
        }


        #region help function

        private PackageToList GetBestPackageToAssigning(Drone drone)
        {
            List<PackageToList> packages = GetAllPackages().ToList();
            IEnumerable<PackageToList> packages1 = from i in packages
                                                   where i.assigning == null
                                                   orderby getDistance(getPackageLocation(i.ID), drone.Location) ascending
                                                   orderby i.Weight descending
                                                   orderby i.Priority descending
                                                   select i;
            if (packages1.Any() == false)
                throw new NoPackageToAssighn("there is no package to assign.");
            return packages1.Where(x => isBatteryEnoughToAllPath(drone, x)).FirstOrDefault();


            #region ...
            //List<PackageToList> packages = GetAllPackages().ToList();
            //List<PackageToList> packages1 = (from i in packages
            //                                 where i.assigning == null
            //                                 orderby getDistance(getPackageLocation(i.ID), drone.Location) ascending
            //                                 orderby i.Weight descending
            //                                 orderby i.Priority descending
            //                                 select i).ToList();
            //foreach (var item in packages1)
            //{
            //    if (isBatteryEnoughToAllPath(drone, item))
            //        return item;
            //}
            //return null;
            #endregion
        }


        private Location getPackageLocation(int id)
        {
            Package p = GetPackage(id);
            if (p.Delivered == null)
                return GetCustomer(p.Sender.ID).Location;
            else
                return GetCustomer(p.Target.ID).Location;
        }

        #endregion

        #region converting function

        private PackageOnDelivery convertPackageToPackageOnDeliveryBL(Package p)
        {
            Location locOfSender = convertLocationToBL(data.GetAllCustomers().ToList().Find(i => i.ID == p.Sender.ID).Location);
            Location locOfTarget = convertLocationToBL(data.GetAllCustomers().ToList().Find(i => i.ID == p.Target.ID).Location);

            PackageOnDelivery packOnDel = new()
            {
                ID = p.ID,
                Priority = p.Priority,
                Sender = p.Sender,
                Target = p.Target,
                Weight = p.Weight,
                PickingUp = locOfSender,
                Delivering = locOfTarget,
                Distance = Math.Round(getDistance(locOfSender, locOfTarget), 2)
            };
            return packOnDel;
        }

        private List<PackageToList> convertPackagesToBL() // i need to change this function after i did "get package" func..
        {
            //Converts a list of base packages from the data layer to a list of base packages of the BL layer.
            List<PackageToList> packagesToList = new List<PackageToList>();
            List<DO.Package> DalPackages = data.GetAllPackages().ToList();
            List<DO.Customer> dalCustList = data.GetallCustomersDel().ToList();
            foreach (DO.Package item in DalPackages)
            {
                packagesToList.Add(new PackageToList
                {
                    ID = item.ID,
                    SenderName = dalCustList.Find(i => i.ID == item.SenderID).Name,
                    TargetName = dalCustList.Find(i => i.ID == item.TargetID).Name,
                    Weight = (WeightCategories)item.Weight,
                    Priority = (Priorities)item.Priority,
                    assigning = item.assigning,
                    Creation = item.Creation,
                    PickedUp = item.PickedUp,
                    Delivered = item.Delivered
                });
            }
            return packagesToList;
        }

        #region ...
        private DO.Package covertPackageToDal(Package p)
        {
            DO.Package pack = new()
            {
                ID = p.ID,
                SenderID = p.Sender.ID,
                TargetID = p.Target.ID,
                Weight = (DO.WeightCategories)p.Weight,
                Priority = (DO.Priorities)p.Priority,
                DroneId = p.Drone.ID,
                assigning = p.assigning,
                Creation = p.Creation,
                PickedUp = p.PickedUp,
                Delivered = p.Delivered
            };
            return pack;
        }

        private PackageInCust convertPackageToPackageInCustBL(Package p)
        {
            Situations sit;
            if (p.assigning == null) sit = Situations.Created;
            else if (p.PickedUp == null) sit = Situations.Assigned;
            else if (p.Delivered == null) sit = Situations.PickedUp;
            else sit = Situations.Delivered;

            return new PackageInCust
            {
                ID = p.ID,
                Weight = p.Weight,
                Priority = p.Priority,
                Situation = sit
            };
        }
        #endregion

        #endregion

    }
}
