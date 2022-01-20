using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public sealed partial class DalXml : IDal
    {
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

    }
}
