﻿using BO;
using BLApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace BL
{
    public partial class BlObject : IBL
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
                    BaseStationID = id,
                    NameBaseStation = nameBaseStation,
                    Location = location,
                    NumOfAvailableChargingPositions = numOfAvailableChargingPositions
                };
                //Add baseStation in DAL to data source.
                dal.AddStation(id, nameBaseStation, location.Longitude, location.Lattitude, numOfAvailableChargingPositions);
            }
            catch (DO.BaseStationAlreadyExistException exception)
            {
                throw new BO.BaseStationAlreadyExistException(id, nameBaseStation, exception.Message, exception.InnerException);
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
                DO.Station station = dal.GetBaseStation(id);
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
            catch (DO.BadBaseStationIDException exception)
            {
                throw new BO.BadBaseStationIDException(id, exception.Message, exception.InnerException);
            }
        }
        #endregion

        #region DISPLAY
        /// <summary>
        /// Base station view
        /// </summary>
        /// <param name="baseStationId">base Station ID</param>
        /// <returns>Base station thet requested</returns>
        public BaseStation GetBaseStation(int baseStationId)
        {
            try
            {
                DO.Station station = dal.GetBaseStation(baseStationId);
                List<int> dronesIdInChrging = dal.GetDronesInChargingsAtStation(baseStationId, droneCharge => droneCharge.StationID == baseStationId).ToList();
                List<DroneInCharging> dronesInCarging = new();
                foreach (var droneId in dronesIdInChrging)
                {
                    var drone_l = droneForLists.Find(d => d.DroneID == droneId);
                    DroneInCharging drone_c = new() { DroneID = droneId, DroneBattery = drone_l.DroneBattery };
                    dronesInCarging.Add(drone_c);
                }

                return new BaseStation()
                {
                    BaseStationID = baseStationId,
                    NameBaseStation = station.StationName,
                    Location = new() { Lattitude = station.Lattitude, Longitude = station.Longitude },
                    NumOfAvailableChargingPositions = station.ChargeSlots,
                    DroneInChargings = dronesInCarging
                };
            }
            catch (DO.BadBaseStationIDException e)
            {
                throw new BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST
        /// <summary>
        /// Displays a list of base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationsBo()
        {
            List<BaseStationForList> list = new();
            foreach (var station in dal.GetAllBaseStations())
            {
                BaseStationForList stationForList = cloneBaseStation(GetBaseStation(station.StationID));
                list.Add(stationForList);
            }
            return list;
        }

        /// <summary>
        /// Display of base stations with available charging stations
        /// </summary>
        /// <returns>list of base stations with available charging stations</returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationWhithAvailibleCharging(Predicate<DO.Station> p)
        {
            List<BaseStationForList> list = new();
            foreach (var station in dal.GetAllStationsBy(p))
            {
                BaseStationForList stationForList = cloneBaseStation(GetBaseStation(station.StationID));
                list.Add(stationForList);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Converts from object base station to object base station for list
        /// </summary>
        /// <param name="baseStation">base station</param>
        /// <returns> base station for list</returns>
        private BaseStationForList cloneBaseStation(BaseStation baseStation)
        {
            return new BaseStationForList
            {
                BaseStationID = baseStation.BaseStationID,
                NameBaseStation = baseStation.NameBaseStation.ToString(),
                NumOfAvailableChargingPositions = baseStation.NumOfAvailableChargingPositions,
                NumOfBusyChargingPositions = baseStation.DroneInChargings.Count()
            };
        }


    }

}