using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
    {

        #region ADD
        /// <summary>
        /// adds station to the file
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The station name</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <param name="chargeSlots">Several arguments</param>
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationAdd = (from item in ListStation
                              where item.StationID == id
                              select item).FirstOrDefault();

            if (StationAdd.StationID != 0 && StationAdd.Available)
                throw new BaseStationAlreadyExistException(id, name, "The station exists");

            if (StationAdd.StationID != 0 && !StationAdd.Available)
            {
                DeleteStation(id);
            }
            Station s = new Station
            {
                StationID = id,
                StationName = name,
                Longitude = longitude,
                Lattitude = lattitude,
                ChargeSlots = chargeSlots,
                Available=true,
            };
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);


        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update Base Station name and total Amount Of Charging Stations
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name and total Amount Of Charging Stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id && item.Available
                              select item).FirstOrDefault();
            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;
                s.StationName = nameBaseStation;

                var listCharge = from item in XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath)
                                 where item.StationID == id && item.Available
                                 select item;

                s.ChargeSlots = totalAmountOfChargingStations - listCharge.Count();

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(id, $"The station: {id} doesn't exist");
        }

        /// <summary>
        /// Update Base Station name
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name</param>
        public void UpdateBaseStationName(int id, int nameBaseStation)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id && item.Available
                              select item).FirstOrDefault();

            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;
                s.StationName = nameBaseStation;

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            throw new BadBaseStationIDException(id, $"The station doesn't exist in the system");
        }

        /// <summary>
        /// Update base station total number of charge slots
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationCharging(int id, int totalAmountOfChargingStations)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id
                              select item).FirstOrDefault();
            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;

                var listCharge =
                    from item in XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath)
                    where item.StationID == id
                    select item;

                s.ChargeSlots = totalAmountOfChargingStations - listCharge.Count();

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(id, $"The station: {id} doesn't exist");
        }
        #endregion

        #region Get item
        /// <summary>
        /// return base station by station ID.
        /// </summary>
        /// <param name="stationId">station ID</param>
        /// <returns>statoin</returns>
        public Station GetBaseStation(int stationId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            Station s = (from item in ListStation
                         where item.StationID == stationId && item.Available
                         select item).FirstOrDefault();
            if (s.StationID == 0)
                throw new BadBaseStationIDException(stationId, "the base station not exists in the system");
            return s;
        }

        /// <summary>
        /// A function that returns a minimum distance between a point and a base station
        /// </summary>
        /// <param name="senderLattitude">Lattitude of sender</param>
        /// <param name="senderLongitude">longitude of sender</param>
        /// <param name="flag">Optional field for selecting a nearby base station flag = false or available nearby base station flag = true</param>
        /// <returns>Base station closest to the point</returns>
        public Station GetClosestStation(double senderLattitude, double senderLongitude, bool flag = false)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            double minDistance = 1000000000000;
            Station station = new();
            if (!flag)
            {
                foreach (var stations in ListStation)
                {
                    double dictance = Math.Sqrt(Math.Pow(stations.Lattitude - senderLattitude, 2) + Math.Pow(stations.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = stations;
                    }
                }
            }
            else
            {
                foreach (var stations in ListStation.Where(s => s.ChargeSlots > 0))
                {
                    double dictance = Math.Sqrt(Math.Pow(stations.Lattitude - senderLattitude, 2) + Math.Pow(stations.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = stations;
                    }
                }
            }
            return station;
        }

        /// <summary>
        /// A function that calculates the distance between a customer's location and a base station for charging
        /// </summary>
        /// <param name="targetId">target ID</param>
        /// <returns>Minimum distance to the nearest base station</returns>
        public double GetDistanceBetweenLocationAndClosestBaseStation(int targetId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            double minDistance = 1000000000000;
            Customer target = GetCustomer(targetId);
            foreach (var s in ListStation)
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
        /// return base stations with 
        /// </summary>
        /// <returns>list of station with </returns>
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> p)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation
                   where p(item) && item.Available
                   select item;

        }

        /// <summary>
        /// return a list of actual base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public IEnumerable<Station> GetAllBaseStations()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation
                   where item.Available
                   select item;
        }
        #endregion

        #region Delete
        /// <summary>
        /// deletes station by the ID from the file
        /// </summary>
        /// <param name="stationID"></param>
        public void DeleteStation(int stationID)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationDelete = (from item in ListStation
                                 where item.StationID == stationID && item.Available
                                 select item).FirstOrDefault();
            if (StationDelete.StationID != 0)
            {
                ListStation.Remove(StationDelete);
                StationDelete.Available = false;
                ListStation.Add(StationDelete);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(stationID, $"The station: {stationID} doesn't exist");
        }

        /// <summary>
        /// deletes all stations in file
        /// </summary>
        public void DeleteAllStation()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (ListStation != null)
            {
                foreach (var item in ListStation)
                {
                    DeleteStation(item.StationID);
                }
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
        }
        #endregion
    }
}
