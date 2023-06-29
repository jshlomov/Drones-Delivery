using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DalXml;

namespace DalXml
{

    public static class DalFactory
    {
        public static DalXml GetDalXml()
        {
            return DalXml.Instance;
        }
    }

    public partial class DalXml : IDal
    {
        XElement droneRoot;
        public string dronesFilePath = @"dronesListXml";
        public string customersFilePath = @"customersListXml";
        public string packagesFilePath = @"packagesListXml";
        public string stationsFilePath = @"stationsListXml";
        public string dronesChargeFilePath = @"dronesChargeistXml";
        public string configFilePath = @"xml\configXml";

        private DalXml()
        {
            
            if (!File.Exists(XmlTools.dir + dronesFilePath))
                XmlTools.SaveListToXMLSerializer<DO.Drone>(DalObject.DataSource.Drones, dronesFilePath);

            if (!File.Exists(XmlTools.dir + customersFilePath))
                XmlTools.SaveListToXMLSerializer<DO.Customer>(DalObject.DataSource.Customers, customersFilePath);

            if (!File.Exists(XmlTools.dir + packagesFilePath))
                XmlTools.SaveListToXMLSerializer<DO.Package>(DalObject.DataSource.Packages, packagesFilePath);

            if (!File.Exists(XmlTools.dir + stationsFilePath))
                XmlTools.SaveListToXMLSerializer<DO.Station>(DalObject.DataSource.Stations, stationsFilePath);

            if (!File.Exists(XmlTools.dir + dronesChargeFilePath))
                XmlTools.SaveListToXMLSerializer<DO.DroneCharge>(DalObject.DataSource.DronesCharges, dronesChargeFilePath);

            if (!File.Exists(configFilePath))
            {
                XElement packegeRunId = new XElement("packegeRunId", DalObject.DataSource.Config.packegeRunId);
                XElement free = new XElement("freeWeight", DalObject.DataSource.Config.freeWeight);
                XElement lightCarry = new XElement("lightCarry", DalObject.DataSource.Config.lightCarry);
                XElement mediumCarry = new XElement("mediumCarry", DalObject.DataSource.Config.mediumCarry);
                XElement heavyCarry = new XElement("heavyCarry", DalObject.DataSource.Config.heavyCarry);
                XElement chargingRate = new XElement("chargingRate", DalObject.DataSource.Config.chargingRate);

                XElement element = new XElement("config", packegeRunId, free, lightCarry, mediumCarry, heavyCarry, chargingRate);

                element.Save(configFilePath);
            }
        }

        #region singleton

        static DalXml() { }
        internal static DalXml Instance { get; } = new DalXml();
        #endregion
    }
}
