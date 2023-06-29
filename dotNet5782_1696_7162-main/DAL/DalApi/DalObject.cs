using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DalObject
{
    internal partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// constructor: using  DataSource.Initialize
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        #region singleton
        static DalObject() { }
        internal static DalObject Instance { get; } = new DalObject();
        #endregion

        public double[] GetConsumptionOfElectricity()
        {
            double[] arr = new double[5];
            arr[0] = DataSource.Config.freeWeight;
            arr[1] = DataSource.Config.lightCarry;
            arr[2] = DataSource.Config.mediumCarry;
            arr[3] = DataSource.Config.heavyCarry;
            arr[4] = DataSource.Config.chargingRate;
            return arr;
        }

        public IEnumerable<DroneCharge> GetDronesCharge()
        {
            List<DroneCharge> dcList = new List<DroneCharge>();
            dcList = DataSource.DronesCharges;
            return dcList;
        }
    }
}

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal()
        {
            return DalObject.DalObject.Instance;
        }
    }

}