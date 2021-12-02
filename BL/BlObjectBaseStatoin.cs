using IBL.BO;
using IBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        #region ADD
        /// <summary>
        /// Add base station
        /// </summary>
        /// <param name="id">Station number</param>
        /// <param name="nameBaseStation">Model base station</param>
        /// <param name="location">Location of the bus stop</param>
        /// <param name="numOfAvailableChargingPositions">Number of charging stations available</param>
        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions)
        {
            try
            {
                //add baseStation fields in BL.
                BaseStation baseStation = new BaseStation()
                {
                    Id = id,
                    NameBaseStation = nameBaseStation,
                    Location = location,
                    NumOfAvailableChargingPositions = numOfAvailableChargingPositions
                };
                //Add baseStation in DAL to data source.
                dal.AddStation(id, nameBaseStation, location.Longitude, location.Lattitude, numOfAvailableChargingPositions);
            }
            catch (IDAL.DO.BaseStationAlreadyExistException exception )
            {
                throw new IBL.BO.BaseStationAlreadyExistException(id,nameBaseStation, exception.Message,exception.InnerException);
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {
            //update in BL
            try
            {
                Station station = dal.GetBaseStation(id);
                if (nameBaseStation != 0)
                {
                    if (totalAmountOfChargingStations != 0)
                    {
                        dal.UpdateBaseStationData(id, nameBaseStation, totalAmountOfChargingStations);
                    }
                    else
                    {
                        dal.UpdateBaseStationName(id, nameBaseStation);
                    }
                }
                else
                {
                    dal.UpdateBaseStationCharging(id, totalAmountOfChargingStations);
                }
            }
            catch(IDAL.DO.BadBaseStationIDException exception)
            {
                throw new IBL.BO.BadBaseStationIDException(id, exception.Message, exception.InnerException);
            }
        }
        #endregion

        #region GET ITEM
        /// <summary>
        /// Base station view
        /// </summary>
        /// <param name="baseStationId">base Station ID</param>
        /// <returns>Base station thet requested</returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            try
            {
                Station station = dal.GetBaseStation(baseStationId);
                List<int> dronesIdInChrging = dal.GetDronesInChargingsAtStation(baseStationId).ToList();
                List<DroneInCharging> dronesInCarging = new();
                foreach (var droneId in dronesIdInChrging)
                {
                    var drone_l = droneForLists.Find(d => d.Id == droneId);
                    DroneInCharging drone_c = new() { Id = droneId, Battery = drone_l.Battery };
                    dronesInCarging.Add(drone_c);
                }

                return new BaseStation()
                {
                    Id = baseStationId,
                    NameBaseStation = station.Name,
                    Location = new() { Lattitude = station.Lattitude, Longitude = station.Longitude },
                    NumOfAvailableChargingPositions = station.ChargeSlots,
                    DroneInChargings = dronesInCarging
                };
            }
            catch (IDAL.DO.BadBaseStationIDException e)
            {
                throw new IBL.BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationsBo()
        {
            List<BaseStationForList> list = new();
            foreach (var station in dal.GetAllBaseStations())
            {
                BaseStationForList stationForList = cloneBaseStation(GetBaseStation(station.Id));
                list.Add(stationForList);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationWhithAvailibleCharging()
        {
            List<BaseStationForList> list = new();
            foreach (var station in dal.GetAllStationsWithAvailableChargingStations())
            {
                BaseStationForList stationForList = cloneBaseStation(GetBaseStation(station.Id));
                list.Add(stationForList);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseStation"></param>
        /// <returns></returns>
        private BaseStationForList cloneBaseStation(BaseStation baseStation)
        {
            return new BaseStationForList
            {
                Id = baseStation.Id,
                NameBaseStation = baseStation.NameBaseStation.ToString(),
                NumOfAvailableChargingPositions = baseStation.NumOfAvailableChargingPositions,
                NumOfBusyChargingPositions = baseStation.DroneInChargings.Count()
            };
        }


    }

}
