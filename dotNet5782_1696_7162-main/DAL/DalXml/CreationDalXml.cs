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
        #region customer
        /// <summary>
        /// adding customer to the data base
        /// </summary>
        public void AddCustomer(Customer cust)
        {
            List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);

            int index = customers.FindIndex(i => i.ID == cust.ID);

            if (index != -1 && customers[index].Deleted == false)
                throw new IdExistException("the customer is already exist.");
            else if (index != -1)
                customers[index] = cust;
            else
                customers.Add(cust);

            XmlTools.SaveListToXMLSerializer<DO.Customer>(customers, customersFilePath);
        }
        #endregion

        #region drone
        /// <summary>
        /// adding drone to the data base
        /// </summary>
        public void AddDrone(Drone drone)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);

            int index = drones.FindIndex(i => i.ID == drone.ID);

            if (index != -1 && drones[index].Deleted == false)
                throw new IdExistException("the drone is already exist.");
            else if (index != -1)
                drones[index] = drone;
            else
                drones.Add(drone);

            XmlTools.SaveListToXMLSerializer<DO.Drone>(drones, dronesFilePath);
        }
        #endregion

        #region package
        /// <summary>
        /// adding package to the data base
        /// </summary>
        public int AddPackage(Package package)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            package.ID = GetPackageRunID();
            packages.Add(package);

            XmlTools.SaveListToXMLSerializer<DO.Package>(packages, packagesFilePath);

            return package.ID;
        }

        private int GetPackageRunID()
        {
            int runID = Convert.ToInt32(XElement.Load(configFilePath).Element("packegeRunId").Value);
            XElement temp = XElement.Load(configFilePath);
            temp.Element("packegeRunId").Value = Convert.ToString(runID + 1);
            temp.Save(configFilePath);
            return runID;
        }
        #endregion

        #region station
        /// <summary>
        ///// adding station to the data base
        /// </summary>
        public void AddStation(Station station)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = DataSource.Stations.FindIndex(i => i.ID == station.ID);
            if (index != -1 && stations[index].Deleted == false)
                throw new IdExistException("the station is already exist.");
            else if (index != -1)
                stations[index] = station;
            else
                stations.Add(station);

            XmlTools.SaveListToXMLSerializer<DO.Station>(stations, stationsFilePath);
        }
        #endregion
    }
}
