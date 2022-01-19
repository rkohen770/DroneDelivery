using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
    {
        #region Drone

        #region ADD
        /// <summary>
        /// Add a drone to the list of existing drones
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">The drone model</param>
        /// <param name="maxWeight">Weight category (light, medium, heavy)</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var DroneAdd = (from item in ListDrone
                            where item.DroneID == id
                            select item).FirstOrDefault();

            if (DroneAdd.DroneID != 0 && DroneAdd.Available)
                throw new DroneAlreadyExistException(id, model, "The drone alredy exists");

            if (DroneAdd.DroneID != 0 && !DroneAdd.Available)
            {
                DeleteDrone(id);
            }
            Drone s = new Drone
            {
                DroneID = id,
                DroneModel = model,
                MaxWeight = maxWeight,
                Available = true,
            };
            ListDrone.Add(s);
            XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);


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
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }

            Station s = station;
            s.ChargeSlots--;//We will update the number of loading locations
            ListStation.Remove(station);
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);

            DroneCharge droneCharge = new DroneCharge//Add a instance of an instance loading entity
            {
                DroneID = droneId,
                StationID = stationId
            };
            ListDroneCharges.Add(droneCharge);//Add a load of drones to file
            XMLTools.SaveListToXMLSerializer(ListDroneCharges, DroneChargePath);

            return stationId;
        }

        /// <summary>
        /// Release the UAV from a charge at the base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            var charge = (from item in ListDroneCharges
                          where item.StationID == stationId && item.DroneID == droneId && item.Available
                          select item).FirstOrDefault();

            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }

            Station s = station;
            s.ChargeSlots++;//We will update the number of loading locations
            ListStation.Remove(station);
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);

            DeleteDroneGharge(charge);
        }

        /// <summary>
        /// Update Drone Modle at a base station
        /// </summary>
        /// <param name="droneId">drone id to update</param>
        /// <param name="model">new model</param>
        public void UpdateDroneModle(int droneId, string model)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();
            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }

            Drone d = drone;
            d.DroneModel = model;
            ListDrone.Remove(drone);
            ListDrone.Add(d);
            XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);
        }
        #endregion

        #region Get item
        /// <summary>
        /// return drone by drone ID 
        /// </summary>
        /// <param name="droneId">drone ID</param>
        /// <returns>drone to show</returns>
        public Drone GetDrone(int droneId)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var d = (from item in ListDrone
                     where item.DroneID == droneId && item.Available
                     select item).FirstOrDefault();
            if (d.DroneID == 0)
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            return d;
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            return from item in ListDrone
                   where item.Available
                   select item;
        }

        /// <summary>
        /// List of drones loaded at a specific station
        /// </summary>
        /// <param name="stationId">base station ID</param>
        /// <returns>List of drones loaded at a specific station</returns>
        public IEnumerable<int> GetDronesInChargingsAtStation(int stationId, Predicate<DroneCharge> p)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }
            return from droneCharge in ListDroneCharges
                   where p(droneCharge) && droneCharge.Available
                   select droneCharge.DroneID;
        }

        /// <summary>
        /// return drones by predicat
        /// </summary>
        /// <param name="p">predicat</param>
        /// <returns>drones by predicat</returns>
        public IEnumerable<Drone> GetDronesByPredicat(Predicate<Drone> p)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            return from drone in ListDrone
                   where p(drone)
                   select drone;
        }
        #endregion

        #region Delete
        /// <summary>
        /// deletes drone by the ID from the file
        /// </summary>
        /// <param name="droneID"></param>
        public void DeleteDrone(int droneID)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var DroneDelete = (from item in ListDrone
                               where item.DroneID == droneID && item.Available
                               select item).FirstOrDefault();
            if (DroneDelete.DroneID != 0)
            {
                ListDrone.Remove(DroneDelete);
                DroneDelete.Available = false;
                ListDrone.Add(DroneDelete);
                XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);
            }
            else throw new BadBaseStationIDException(droneID, $"The drone: {droneID} doesn't exist in the system");
        }

        /// <summary>
        /// delete drone charge from the file
        /// </summary>
        /// <param name="charge"></param>
        private void DeleteDroneGharge(DroneCharge charge)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);
            ListDroneCharge.Remove(charge);
            charge.Available = false;
            ListDroneCharge.Add(charge);
            XMLTools.SaveListToXMLSerializer(ListDroneCharge, DroneChargePath);
        }
        #endregion

        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns>An array of the amount of power consumption of a drone for each situation</returns>
        public double[] PowerConsumptionRequest()
        {
            XElement dalConfigRoot = XElement.Load(ConfigPath);

            double[] Electricity = (from status in dalConfigRoot
                                    .Element("ElectricityUseRequest")
                                    .Elements() select XmlConvert
                                    .ToDouble(status.Value)).ToArray();
            dalConfigRoot.Save(ConfigPath);
            return Electricity;
        }
        
        #endregion
    }
}
