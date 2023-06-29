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
        /// <summary>
        /// delete customer from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id">customer's ID</param>
        public void RemoveCustomer(int id)
        {
            List<DO.Customer> customers = XmlTools.LoadListFromXMLSerializer<DO.Customer>(customersFilePath);

            int index = customers.FindIndex(x => x.ID == id && x.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the customer didn't found.");
            Customer c = customers[index];
            c.Deleted = true;
            customers[index] = c;

            XmlTools.SaveListToXMLSerializer<DO.Customer>(customers, customersFilePath);
        }

        /// <summary>
        /// delete drone from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id">drone's ID</param>
        public void RemoveDrone(int id)
        {
            List<DO.Drone> drones = XmlTools.LoadListFromXMLSerializer<DO.Drone>(dronesFilePath);

            int index = drones.FindIndex(x => x.ID == id && x.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the drone did not found.");
            Drone d = drones[index];
            d.Deleted = true;
            drones[index] = d;

            XmlTools.SaveListToXMLSerializer<DO.Drone>(drones, dronesFilePath);
        }

        /// <summary>
        /// delete station from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id">station's ID</param>
        public void RemoveStation(int id)
        {
            List<DO.Station> stations = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsFilePath);

            int index = stations.FindIndex(x => x.ID == id && x.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station did not found.");
            Station s = stations[index];
            s.Deleted = true;
            stations[index] = s;

            XmlTools.SaveListToXMLSerializer<DO.Station>(stations, stationsFilePath);
        }

        /// <summary>
        /// delete package from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id">package's ID</param>
        public void RemovePackage(int id)
        {
            List<DO.Package> packages = XmlTools.LoadListFromXMLSerializer<DO.Package>(packagesFilePath);

            int index = packages.FindIndex(x => x.ID == id && x.Deleted == false);
            if (index == -1)
                throw new IdIsNotExistExeption("the station did not found.");
            Package p = packages[index];
            p.Deleted = true;
            packages[index] = p;

            XmlTools.SaveListToXMLSerializer<DO.Package>(packages, packagesFilePath);
        }
    }
}
