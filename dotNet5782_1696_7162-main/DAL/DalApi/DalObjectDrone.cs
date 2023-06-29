using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


//לטפל במחיקה
namespace DalObject
{
    internal partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// adding drone to the data base
        /// </summary>
        public void AddDrone(Drone drone)
        {
            int index = DataSource.Drones.FindIndex(i => i.ID == drone.ID);
            if (index != -1)
                throw new IdExistException("the drone is already exist.");
            DataSource.Drones.Add(drone);
        }

        /// <summary>
        /// updating drone's name
        /// </summary>
        /// <param name="id">drone's ID</param>
        /// <param name="model">The new name(model)</param>
        public void UpdateDroneName(int id, string model)
        {
            int droneIndex = DataSource.Drones.FindIndex(i => i.ID == id);
            if (droneIndex == -1)
                throw new IdIsNotExistExeption("the id wasn't found.");
            Drone d = DataSource.Drones[droneIndex];
            d.Model = model;
            DataSource.Drones[droneIndex] = d;
        }

        /// <summary>
        /// send drone to charge in base station and define him "in repair".
        /// </summary>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            int DroneIndex = DataSource.Drones.FindIndex(i => i.ID == droneId);
            int StationIndex = DataSource.Stations.FindIndex(i => i.ID == stationId);
            if (DroneIndex == -1 || StationIndex == -1)
                throw new IdIsNotExistExeption("the data is incorrect, the id is not found.");
            DroneCharge dc = new DroneCharge()
            {
                DroneID = droneId,
                StationID = stationId,
                StartCharging = DateTime.Now
            };
            DataSource.DronesCharges.Add(dc);
        }

        /// <summary>
        /// release drone from charching and makes it available.
        /// </summary>
        /// <param name="droneID"></param>
        public void releaseDroneFromCharging(int droneID)
        {
            int index = DataSource.DronesCharges.FindIndex(i => i.DroneID == droneID);
            if (index == -1)
                throw new IdIsNotExistExeption("the id wasn't found.");
            DataSource.DronesCharges.RemoveAt(index);
        }

        /// <summary>
        /// return specific drone with the same ID.
        /// </summary>
        /// <param name="id">drone's ID</param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            int index = DataSource.Drones.FindIndex(i => i.ID == id);
            if (index == -1)
                throw new IdIsNotExistExeption("the id is not found.");
            return DataSource.Drones[index];
        }

        /// <summary>
        /// return a list with all Drones.
        /// </summary>
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> filter = null)
        {
            return DataSource.Drones.FindAll(x => filter == null ? true : filter(x) && x.Deleted == false);
        }

        public void RemoveDrone(int id)
        {
            int index = DataSource.Drones.FindIndex(x => x.ID == id);
            Drone d = DataSource.Drones[index];
            d.Deleted = true;
            DataSource.Drones[index] = d;
        }
    }
}
