using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
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

        #region Update
        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {
            if (!DataSource.stations.Exists(drone => drone.Id == id))
            {
                throw new NoDataExistsException("the station not exists in the list of stations");
            }
            else
            {
                int sIndex = DataSource.stations.FindIndex(d => d.Id == id);
                Station station = DataSource.stations[sIndex];
                station.Name = nameBaseStation;

                List<DroneCharge> charges = DataSource.droneCharges.FindAll(c=>c.StationId==station.Id);
                station.ChargeSlots = totalAmountOfChargingStations - charges.Count();
                DataSource.stations[sIndex] = station;
            }
        }

        /// <summary>
        /// Update Base Station Name
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name</param>
        public void UpdateBaseStationName(int id, int nameBaseStation)
        {
            if (!DataSource.stations.Exists(drone => drone.Id == id))
            {
                throw new NoDataExistsException("the station not exists in the list of stations");
            }
            else
            {
                int sIndex = DataSource.stations.FindIndex(d => d.Id == id);

                Station station = DataSource.stations[sIndex];
                station.Name = nameBaseStation;
                DataSource.stations[sIndex] = station;
            }
        }

        /// <summary>
        /// Update base station total number of charge slots
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationCharging(int id, int totalAmountOfChargingStations)
        {
            if (!DataSource.stations.Exists(drone => drone.Id == id))
            {
                throw new NoDataExistsException("the station not exists in the list of stations");
            }
            else
            {
                int sIndex = DataSource.stations.FindIndex(d => d.Id == id);
                Station station = DataSource.stations[sIndex];

                List<DroneCharge> charges = DataSource.droneCharges.FindAll(c => c.StationId == station.Id);
                station.ChargeSlots = totalAmountOfChargingStations - charges.Count();
                DataSource.stations[sIndex] = station;
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

        /// <summary>
        /// A function that returns a minimum distance between a point and a base station
        /// </summary>
        /// <param name="senderLattitude">Lattitude of sender</param>
        /// <param name="senderLongitude">longitude of sender</param>
        /// <returns>Base station closest to the point</returns>
        public Station GetClosestStation(double senderLattitude, double senderLongitude)
        {
            double minDistance = 1000000000000;
            Station station = new();
            foreach (var s in DataSource.stations)
            {
                double dictance = Math.Sqrt(Math.Pow(s.Lattitude - senderLattitude, 2) + Math.Pow(s.Longitude - senderLongitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                    station = s;
                }
            }
            return station;
        }


        /// <summary>
        /// A function that calculates the distance between a customer's location and a base station for charging
        /// </summary>
        /// <param name="targetId">target Id</param>
        /// <returns>Minimum distance to the nearest base station</returns>
        public double GetDistanceBetweenLocationAndClosestBaseStation(int targetId)
        {
            double minDistance = 1000000000000;
            Customer target = CustomerView(targetId);
            foreach (var s in DataSource.stations)
            {
                double dictance = Math.Sqrt(Math.Pow(s.Lattitude - target.Lattitude, 2) + Math.Pow(s.Longitude - target.Longitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                }
            }
            return minDistance;
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
