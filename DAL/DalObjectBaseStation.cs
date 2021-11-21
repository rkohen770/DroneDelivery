using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject:IDal
    {
        #region ADD
        /// <summary>
        /// Add a base station to the list of stations
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The station name</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <param name="chargeSlots">Several arguments</param>
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots)
        {
            if (DataSource.stations.Exists(station => station.Id == id))
            {
                throw new ExistingFigureException("the station exists allready");
            }
            else
            {
                Station s = new Station
                {
                    Id = id,
                    Name = name,
                    Longitude = longitude,
                    Lattitude = lattitude,
                    ChargeSlots = chargeSlots
                };
                DataSource.stations.Add(s);//Adding the new station to the array;
            }
        }
        #endregion

        #region Get item
        /// <summary>
        /// return base station by station ID.
        /// </summary>
        /// <param name="stationId">station ID</param>
        /// <returns>statoin</returns>
        public Station BaseStationView(int stationId)
        {
            if (!DataSource.stations.Exists(station => station.Id == stationId))
            {
                throw new NoDataExistsException("the base station not exists in the list of station");
            }
            //find the station in the array of stations and return it.
            return DataSource.stations.Find(s => s.Id == stationId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public IEnumerable<Station> GetAllBaseStations()
        {
            return from station in DataSource.stations
                   select station.Clone();
        }

        /// <summary>
        /// return base stations with available charging stations
        /// </summary>
        /// <returns>list of station with availible charge station</returns>
        public IEnumerable<Station> GetAllStationsWithAvailableChargingStations()
        {
            return from station in DataSource.stations
                   where station.ChargeSlots > 0
                   select station.Clone();
        }
        #endregion
    }
}
