using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    internal partial class DalObject : DalApi.IDal
    {
        /// <summary>
        ///// adding station to the data base
        /// </summary>
        public void AddStation(Station station)
        {
            int index = DataSource.Stations.FindIndex(i => i.ID == station.ID);
            if (index == -1)
                throw new IdExistException("the station is already exist.");
            DataSource.Stations.Add(station);
        }
        
        public void UpdateStation(Station s)
        {
            int index = DataSource.Stations.FindIndex(i => i.ID == s.ID);
            if (index == -1)
                throw new IdIsNotExistExeption("the ID was not exist.");
            DataSource.Stations[index] = s;
        }



        /// <summary>
        /// return specific station with the same ID.
        /// </summary>
        /// <param name="id">station's ID</param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            int index = DataSource.Stations.FindIndex(i => i.ID == id);
            if (index == -1)
                throw new IdIsNotExistExeption("the id was'nt found.");
            return DataSource.Stations[index];

        }

        /// <summary>
        /// return a list with all Stations.
        /// </summary>
        public IEnumerable<Station> GetAllStations(Predicate<Station> filter = null)
        {
            return DataSource.Stations.FindAll(x => filter == null ? true : filter(x) && x.Deleted == false);
        }

        public void RemoveStation(int id)
        {
            int index = DataSource.Stations.FindIndex(x => x.ID == id);
            Station s = DataSource.Stations[index];
            s.Deleted = true;
            DataSource.Stations[index] = s;
        }

        #region manage the slots methodes
        /// <summary>
        /// update that the station has one more free slote. (when the drone leaving.)
        /// </summary>
        /// <param name="stationId"></param>
        public void releaseFreeSlote(int stationId)
        {
            int index = DataSource.Stations.FindIndex(i => i.ID == stationId);
            if (index != -1)
            {
                Station s = DataSource.Stations[index];
                s.FreeChargeSlots++;
                DataSource.Stations[index] = s;
                return;
            }
        }

        /// <summary>
        /// update that the station has one less free slote. (when the drone comming.)
        /// </summary>
        /// <param name="stationId"></param>
        public void catchFreeSlote(int stationId)
        {
            int index = DataSource.Stations.FindIndex(i => i.ID == stationId);
            if (index != -1)
            {
                Station s = DataSource.Stations[index];
                s.FreeChargeSlots--;
                DataSource.Stations[index] = s;
                return;
            }
        }

        #endregion
    }
}
