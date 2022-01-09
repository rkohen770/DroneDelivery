using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace DalApi
{
    partial class DalObject : IDal
    {
        #region ADD
        /// <summary>
        /// Add a drone to the list of existing drones
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">The drone model</param>
        /// <param name="maxWeight">Weight category (light, medium, heavy)</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight)
        {
            if (DataSource.drones.Exists(drone => drone.DroneID == id))
            {
                throw new DroneAlreadyExistException(id, model, "the drone exists allready");
            }
            else
            {
                Drone d = new Drone
                {
                    DroneID = id,
                    DroneModel = model,
                    MaxWeight = maxWeight
                };
                DataSource.drones.Add(d); //Adding the new drone to the array
            }
        }
        #endregion

        #region Uppdate
        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public int SendingDroneForCharging(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (!DataSource.stations.Exists(station => station.StationID == stationId))
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }
            for (int sIndex = 0; sIndex < DataSource.stations.Count; sIndex++)
            {
                if (DataSource.stations[sIndex].StationID == stationId)//We found the place of the station in the array of stations
                {
                    Station station = DataSource.stations[sIndex];
                    station.ChargeSlots--;//We will update the number of loading locations
                    DataSource.stations[sIndex] = station;
                    break;
                }
            }
            DroneCharge droneCharge = new DroneCharge { DroneID = droneId, StationID = stationId };//Add a instance of an instance loading entity
            DataSource.droneCharges.Add(droneCharge);//Add a load of drones to the array
            return stationId;
        }

        /// <summary>
        /// Release the UAV from a charge at the base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (!DataSource.stations.Exists(station => station.StationID == stationId))
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }
            for (int sIndex = 0; sIndex < DataSource.stations.Count; sIndex++)
            {
                if (DataSource.stations[sIndex].StationID == stationId)//We found the place of the station in the array of stations
                {
                    Station station = DataSource.stations[sIndex];
                    station.ChargeSlots++;//We will update the number of loading locations
                    DataSource.stations[sIndex] = station;
                    break;
                }
            }
            for (int dCIndex = 0; dCIndex < DataSource.stations.Count; dCIndex++)
            {
                if (DataSource.droneCharges[dCIndex].StationID == stationId && DataSource.droneCharges[dCIndex].DroneID == droneId)//We found the place of the station in the array of stations
                {
                    DataSource.droneCharges.RemoveAt(dCIndex);//remove a load of drones to the array
                    break;
                }
            }
        }

        /// <summary>
        /// Update Drone Modle at a base station
        /// </summary>
        /// <param name="droneId">drone id to update</param>
        /// <param name="model">new model</param>
        public void UpdateDroneModle(int droneId, string model)
        {
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            else
            {
                int dIndex = DataSource.drones.FindIndex(d => d.DroneID == droneId);

                if (DataSource.drones[dIndex].DroneModel != model)
                {
                    Drone drone = DataSource.drones[dIndex];
                    drone.DroneModel = model;
                    DataSource.drones[dIndex] = drone;
                    return;
                }

            }
        }
        #endregion

        #region Get item
        /// <summary>
        /// return drone by drone ID to print
        /// </summary>
        /// <param name="droneId">drone ID to print</param>
        /// <returns>drone to show</returns>
        public Drone GetDrone(int droneId)
        {
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            //find the place of the drone in the array of drones
            return DataSource.drones.Find(d => d.DroneID == droneId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            return from drone in DataSource.drones
                   select drone.Clone();
        }

        /// <summary>
        /// List of drones loaded at a specific station
        /// </summary>
        /// <param name="stationId">base station ID</param>
        /// <returns>List of drones loaded at a specific station</returns>
        public IEnumerable<int> GetDronesInChargingsAtStation(int stationId, Predicate<DroneCharge> p)
        {
            if (!DataSource.stations.Exists(station => station.StationID == stationId))
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }
            return from droneCharge in DataSource.droneCharges
                   where p(droneCharge)
                   select droneCharge.DroneID;
        }

        public IEnumerable<Drone> GetDronesByPredicat(Predicate<Drone> p)
        {
            return from drone in DataSource.drones
                   where p(drone)
                   select drone;
        }
        #endregion

        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns>An array of the amount of power consumption of a drone for each situation</returns>
        public double[] PowerConsumptionRequest()
        {
            double[] result = {DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,
                DataSource.Config.DroneChargingRate };
            return result;
        }

    }
}