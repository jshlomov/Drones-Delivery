using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DalObject;
using DalXml;

namespace DalXml
{
    public partial class DalXml : IDal
    {
        #region update customer

        public void UpdateCustomer(Customer c)
        {
            List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);

            int index = customers.FindIndex(i => c.ID == i.ID && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the customer did not found!");
            Customer newCust = customers[index];
            newCust.Name = c.Name;
            newCust.Phone = c.Phone;
            customers[index] = newCust;

            XmlTools.SaveListToXMLSerializer<DO.Customer>(customers, customersFilePath);
        }

        #endregion

        #region update drone

        /// <summary>
        /// updating drone's name
        /// </summary>
        /// <param name="id">drone's ID</param>
        /// <param name="model">The new name(model)</param>
        public void UpdateDroneName(int id, string model)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);

            int Index = drones.FindIndex(i => i.ID == id && i.Deleted == false);
            if (Index == -1)
                throw new IdIsNotExistExeption("the drone wasn't found.");
            Drone d = drones[Index];
            d.Model = model;
            drones[Index] = d;

            XmlTools.SaveListToXMLSerializer<DO.Drone>(drones, dronesFilePath);
        }

        /// <summary>
        /// send drone to charge in base station and define him "in repair".
        /// </summary>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);
            List<DO.DroneCharge> dronescharge = XmlTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronesChargeFilePath);

            int DroneIndex = drones.FindIndex(i => i.ID == droneId && i.Deleted == false);
            int StationIndex = stations.FindIndex(i => i.ID == stationId && i.Deleted == false);
            if (DroneIndex == -1)
                throw new IdIsNotExistExeption("the data is incorrect, drone's id did not found.");
            if (StationIndex == -1)
                throw new IdIsNotExistExeption("the data is incorrect, station's id did not found.");
            DroneCharge dc = new DroneCharge()
            {
                DroneID = droneId,
                StationID = stationId,
                StartCharging = DateTime.Now
            };
            dronescharge.Add(dc);

            XmlTools.SaveListToXMLSerializer<DO.DroneCharge>(dronescharge, dronesChargeFilePath);
        }

        /// <summary>
        /// release drone from charching and makes it available.
        /// </summary>
        /// <param name="droneID"></param>
        public void releaseDroneFromCharging(int droneID)
        {
            List<DO.DroneCharge> dronescharge = XmlTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronesChargeFilePath);

            int index = dronescharge.FindIndex(i => i.DroneID == droneID);
            if (index == -1)
                throw new IdIsNotExistExeption("the id wasn't found.");
            dronescharge.RemoveAt(index);

            XmlTools.SaveListToXMLSerializer<DO.DroneCharge>(dronescharge, dronesChargeFilePath);
        }

        #endregion

        #region update package
        /// <summary>
        /// assigning in the package's properties the id of the drone who will take it.
        /// </summary>
        public void AssignPackageToDrone(int droneId, int packageId)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            int index = packages.FindIndex(i => i.ID == packageId && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the package wasn't found.");
            Package p = packages[index];
            p.DroneId = droneId;
            p.assigning = DateTime.Now;
            packages[index] = p;

            XmlTools.SaveListToXMLSerializer<DO.Package>(packages, packagesFilePath);

        }

        /// <summary>
        /// update in the package's properties that the package picked up in this moment (date and time).
        /// </summary>
        public void ColectingPackage(int packageId)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            int index = packages.FindIndex(i => i.ID == packageId && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the package wasn't found.");
            Package p = packages[index];
            p.PickedUp = DateTime.Now;
            packages[index] = p;

            XmlTools.SaveListToXMLSerializer<DO.Package>(packages, packagesFilePath);
        }

        /// <summary>
        /// update in the package properties that the package delivered in this moment (date and time).
        /// </summary>
        public void PackageDelivering(int packageId)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            int index = packages.FindIndex(i => i.ID == packageId && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the package wasn't found.");
            Package p = packages[index];
            p.Delivered = DateTime.Now;
            packages[index] = p;

            XmlTools.SaveListToXMLSerializer<DO.Package>(packages, packagesFilePath);
        }

        #endregion

        #region update station

        public void UpdateStation(Station s)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = stations.FindIndex(i => i.ID == s.ID && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station wasn't found.");
            stations[index] = s;

            XmlTools.SaveListToXMLSerializer<DO.Station>(stations, stationsFilePath);
        }

        #endregion

        #region manage the slots methodes
        /// <summary>
        /// update that the station has one more free slote. (when the drone leaving.)
        /// </summary>
        /// <param name="stationId"></param>
        public void releaseFreeSlote(int stationId)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = stations.FindIndex(i => i.ID == stationId && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station wasn't found.");
            Station s = stations[index];
            s.FreeChargeSlots++;
            stations[index] = s;

            XmlTools.SaveListToXMLSerializer<DO.Station>(stations, stationsFilePath);
        }

        /// <summary>
        /// update that the station has one less free slote. (when the drone comming.)
        /// </summary>
        /// <param name="stationId"></param>
        public void catchFreeSlote(int stationId)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = stations.FindIndex(i => i.ID == stationId && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station wasn't found.");
            Station s = stations[index];
            s.FreeChargeSlots--;
            stations[index] = s;

            XmlTools.SaveListToXMLSerializer<DO.Station>(stations, stationsFilePath);
        }

        #endregion
    }
}
