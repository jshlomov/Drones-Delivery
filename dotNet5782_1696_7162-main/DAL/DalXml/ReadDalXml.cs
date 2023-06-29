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
        #region read customer
        /// <summary>
        /// return specific customer with the same ID.
        /// </summary>
        /// <param name="id">customer's ID</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);

            int index = customers.FindIndex(i => i.ID == id && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the customer was'nt found.");
            return customers.FirstOrDefault(x => x.ID == id && x.Deleted == false);
        }

        /// <summary>
        /// get customer include the deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetallCustomersDel(Predicate<Customer> filter = null)
        {
            List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);

            return (from item in customers
                    where filter == null ? true : filter(item)
                    select item).ToList();
        }

        /// <summary>
        /// return a list with all Customers.
        /// </summary>
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> filter = null)
        {
        List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);
            
            var v = from item in customers
                    where item.Deleted == false
                    select item;
            return v.Where(x => filter == null ? true : filter(x));
        }

        #endregion

        #region read drone
        /// <summary>
        /// return specific drone with the same ID.
        /// </summary>
        /// <param name="id">drone's ID</param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);

            int index = drones.FindIndex(i => i.ID == id && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the id is not found.");
            return drones.FirstOrDefault(x => x.ID == id && x.Deleted == false);
        }

        /// <summary>
        /// return a list with all Drones.
        /// </summary>
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> filter = null)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);

            var v = from item in drones
                    where item.Deleted == false
                    select item;
            return v.Where(x => filter == null ? true : filter(x));
        }
        #endregion

        #region read package
        /// <summary>
        /// return specific package with the same ID.
        /// </summary>
        /// <param name="id">package's ID</param>
        /// <returns></returns>
        public Package GetPackage(int id)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            int index = packages.FindIndex(i => i.ID == id);
            if (index == -1)
                throw new IdIsNotExistExeption("the id is not found.");
            return packages[index];
        }

        /// <summary>
        /// return a list with all Packages.
        /// </summary>
        public IEnumerable<Package> GetAllPackages(Predicate<Package> filter = null)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            var v = from item in packages
                    where item.Deleted == false
                    select item;
            return v.Where(x => filter == null ? true : filter(x));
        }
        #endregion

        #region read station
        /// <summary>
        /// return specific station with the same ID.
        /// </summary>
        /// <param name="id">station's ID</param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = stations.FindIndex(i => i.ID == id && i.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station was'nt found.");
            return stations[index];
        }

        /// <summary>
        /// return a list with all Stations.
        /// </summary>
        public IEnumerable<Station> GetAllStations(Predicate<Station> filter = null)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            var v = from item in stations
                    where item.Deleted == false
                    select item;
            return v.Where(x => filter == null ? true : filter(x) );
        }
        #endregion

        public double[] GetConsumptionOfElectricity()
        {
            double[] arr = new double[5];
            arr[0] = Convert.ToDouble(XElement.Load(configFilePath).Element("freeWeight").Value);
            arr[1] = Convert.ToDouble(XElement.Load(configFilePath).Element("lightCarry").Value);
            arr[2] = Convert.ToDouble(XElement.Load(configFilePath).Element("mediumCarry").Value);
            arr[3] = Convert.ToDouble(XElement.Load(configFilePath).Element("heavyCarry").Value);
            arr[4] = Convert.ToDouble(XElement.Load(configFilePath).Element("chargingRate").Value);
            return arr;
        }

        public IEnumerable<DroneCharge> GetDronesCharge()
        {
            List<DO.DroneCharge> dronesCharge = XmlTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronesChargeFilePath);

            var v = from i in dronesCharge
                    select i;
            return v;
        }
    }
}
