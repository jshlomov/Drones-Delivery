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
        public void AddDrone(Drone d, int stationId)
        {
            try
            {
                data.AddDrone(new()
                {
                    ID = d.ID,
                    Model = d.Model,
                    MaxWeight = (DO.WeightCategories)d.MaxWeight
                });
                dronesListBL.Add(new()
                {
                    ID = d.ID,
                    Model = d.Model,
                    MaxWeight = d.MaxWeight,
                    Status = DroneStatuses.InRepair,
                    Battery = Math.Round(GetRandomNumberInRange(20, 40), 2),
                    CurrentLocation = convertLocationToBL(data.GetStation(stationId).TheLocation)
                });
                data.SendDroneToCharge(d.ID, stationId);
            }
            catch (DO.IdExistException ex)
            {
                throw new IdExistException("ERROR", ex);
            }
        }

        public Drone GetDrone(int id)
        {
            try
            {
                Package package = null;
                DroneToList d = dronesListBL.Find(i => i.ID == id);
                if (d == null)
                    throw new IdIsNotExistExeption("the id did not found.");
                if (d.PackageID != 0)
                    package = GetPackage(d.PackageID);
                Drone drone = new()
                {
                    ID = d.ID,
                    Model = d.Model,
                    MaxWeight = d.MaxWeight,
                    Status = d.Status,
                    Battery = d.Battery,
                    Location = d.CurrentLocation,
                    Package = package != null ? convertPackageToPackageOnDeliveryBL(package) : null
                };
                if (package != null)
                    drone.Package.Situation = package.PickedUp == null ? false : true;
                return drone;
            }
            catch (IdIsNotExistExeption ex)
            {
                throw ex;
            }
        }

        public IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> filter = null)
        {
            return dronesListBL.Where(x => filter == null ? true : filter(x));
        }

        public void UpdateDroneName(Drone drone)
        {
            try
            {
                data.UpdateDroneName(drone.ID, drone.Model);
                dronesListBL.Find(i => i.ID == drone.ID).Model = drone.Model;
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public void SendDroneToCharge(Drone drone)
        {
            try
            {
                Station nearStation = GetStation(getNearestStation(drone.Location).Item1);
                double minBattery = getNearestStation(drone.Location).Item2 * free;

                if (drone.Status == DroneStatuses.OnDelivery)
                    throw new NotImplementedException("the drone in a middle of delivery.");
                if (nearStation.FreeChargeSlots == 0)
                    throw new NotImplementedException("the station doesn't have free slotes to charge.");
                if (drone.Battery <= minBattery)
                    throw new NotImplementedException($"the battery is not enough to get to the near station.");
                if (data.GetDronesCharge().Where(i => i.DroneID == drone.ID).Any() == true)
                    throw new NotImplementedException("the drone is already in charge.");

                DroneToList d = dronesListBL.Find(i => i.ID == drone.ID);
                d.Battery -= minBattery;
                d.CurrentLocation = nearStation.Location;
                d.Status = DroneStatuses.InRepair;
                dronesListBL[dronesListBL.FindIndex(i => i.ID == drone.ID)] = d;
                data.SendDroneToCharge(drone.ID, nearStation.ID);
                data.catchFreeSlote(nearStation.ID);
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("the drone's ID does not exist");
            }
        }

        public void releaseDroneFromCharging(Drone drone, double chargingTime)
        {
            int index = dronesListBL.FindIndex(i => i.ID == drone.ID);
            if (dronesListBL[index].Status != DroneStatuses.InRepair)
                throw new NotImplementedException("the drone is not in charge!");

            double newBattery = dronesListBL[index].Battery + chargingRate * chargingTime;
            dronesListBL[index].Battery = (dronesListBL[index].Battery + chargingRate * chargingTime) >= 100 ? 100 : newBattery;
            dronesListBL[index].Status = DroneStatuses.Available;

            data.releaseFreeSlote(data.GetDronesCharge().First(x => x.DroneID == drone.ID).StationID);
            data.releaseDroneFromCharging(dronesListBL[index].ID);
        }

        public void RemoveDrone(int id)
        {
            if (dronesListBL.Find(x => x.ID == id).Status != DroneStatuses.OnDelivery)
            {
                try
                {
                    dronesListBL.RemoveAll(x => x.ID == id);
                    data.RemoveDrone(id);
                }
                catch (DO.IdIsNotExistExeption ex)
                {
                    throw new IdIsNotExistExeption("the drone's ID does not exist");
                }
            }
            else
            {
                throw new NotImplementedException("the drone is on a delivery");
            }
        }


        #region converting and helping methods

        private DO.Drone convertDroneToDal(Drone d)
        {
            return new DO.Drone()
            {
                ID = d.ID,
                Model = d.Model,
                MaxWeight = (DO.WeightCategories)d.MaxWeight
            };
        }

        #endregion
    }
}
