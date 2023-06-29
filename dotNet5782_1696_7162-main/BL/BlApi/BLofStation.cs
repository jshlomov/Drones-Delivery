using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;

namespace BlApi
{
    internal partial class BL : IBL
    {
        public void AddStation(Station station)
        {
            try
            {
                DO.Location l = new DO.Location { Latitude = station.Location.Latitude, Longitude = station.Location.Longitude };
                DO.Station dalStation = new()
                {
                    ID = station.ID,
                    Name = station.Name,
                    TheLocation = l,
                    FreeChargeSlots = station.FreeChargeSlots
                };
                data.AddStation(dalStation);
            }
            catch (DO.IdExistException ex)
            {
                throw new IdExistException("ERROR ", ex);
            }
        }

        public void UpadateStation(Station s)
        {
            try
            {
                DO.Station DalStation = data.GetStation(s.ID);
                if (s.Name != null)
                    DalStation.Name = s.Name;
                if (s.FreeChargeSlots >= s.ChargingDrones.Count())
                    DalStation.FreeChargeSlots = s.FreeChargeSlots - s.ChargingDrones.Count();
                else
                    throw new NotImplementedException("there ara more drones in charging!");
                data.UpdateStation(DalStation);
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public Station GetStation(int id)
        {
            try
            {
                return convertStationToBL(data.GetStation(id));
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public IEnumerable<StationToList> GetAllStations(Predicate<StationToList> filter = null)
        {
            return convertStationListToBL().Where(x => filter == null ? true : filter(x));
        }

        public void RemoveStation(int id)
        {
            if (GetStation(id).ChargingDrones.Any() == true)
                throw new NotImplementedException("the station can not be deleted, it has  drones in charge.");
            data.RemoveStation(id);
        }



        #region convert function

        private Station convertStationToBL(DO.Station s)
        {
            Station station = new Station()
            {
                ID = s.ID,
                Name = s.Name,
                FreeChargeSlots = s.FreeChargeSlots,
                Location = convertLocationToBL(s.TheLocation),
                ChargingDrones = getAllDronesInCharge(s.ID)
            };
            return station;
        }

        private IEnumerable<DroneAtCharge> getAllDronesInCharge(int stationID)
        {
            IEnumerable<DO.DroneCharge> dronesCharge = data.GetDronesCharge();
            List<DroneAtCharge> ChargingDronesBL = new List<DroneAtCharge>();
            foreach (var item in dronesCharge)
            {
                if (item.StationID == stationID)
                {
                    double battery = dronesListBL.Find(i => i.ID == item.DroneID).Battery;
                    ChargingDronesBL.Add(new DroneAtCharge { ID = item.DroneID, Battery = battery });
                }
            }
            return ChargingDronesBL;
        }

        private IEnumerable<StationToList> convertStationListToBL()
        {
            IEnumerable<DO.Station> stations = data.GetAllStations();
            List<StationToList> stationsToList = new List<StationToList>();
            foreach (var item in stations)
            {
                stationsToList.Add(new StationToList()
                {
                    ID = item.ID,
                    Name = item.Name,
                    FreeChargeSlots = item.FreeChargeSlots,
                    TakenChargeSlots = data.GetDronesCharge().Where(x => x.StationID == item.ID).Count()
                });
            }
            return stationsToList;
        }

        #endregion
    }
}
