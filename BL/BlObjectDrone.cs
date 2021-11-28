using IBL.BO;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {

        #region ADD
        /// <summary>
        /// Add drone
        /// </summary>
        /// <param name="droneId">Manufacturer's serial number</param>
        /// <param name="model">Drone model</param>
        /// <param name="maxWeight">Maximum weight</param>
        /// <param name="stationId">Number of stations to put the drone for initial charging</param>
        public void AddDroneBo(int droneId, string model, IBL.BO.WeightCategories maxWeight, int stationId)
        {
            //add drone fields in BL.
            Station s = dal.BaseStationView(stationId);
            DroneForList drone = new()
            {
                Id = droneId,
                Model = model,
                Weight = maxWeight,
                Battery = random.Next(20, 41),
                DroneStatus = DroneStatus.Maintenance,
                CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude }
            };
            dal.SendingDroneForCharging(droneId, stationId);

            // Add the drone to the list of drones when charging from a base station
            droneForLists.Add(drone);

            //Add drone in DAL to data source.
            dal.AddDrone(droneId, model, (IDAL.DO.WeightCategories)maxWeight);
        }
        #endregion

        #region UPDATE 
        /// <summary>
        /// Update the drone data that will allow you to update the drone name only
        /// </summary>
        /// <param name="id">Drone id</param>
        /// <param name="model">Drone model</param>
        public void UpdateNameOfDrone(int id, string model)
        {
            //update in dal
            dal.UpdateDroneModle(id, model);

            //update in BL
            int dIndex = droneForLists.FindIndex(d => d.Id == id);//Obtain an index for the location where the package ID is located
            if (droneForLists[dIndex].Model != model)
            {
                DroneForList drone = droneForLists[dIndex];
                drone.Model = model;
                droneForLists[dIndex] = drone;
            }
        }


        /// <summary>
        /// Sending a drone for charging
        /// </summary>
        /// <param name="id">Id drone for charging</param>
        public void SendingDroneForCharging(int id)
        {
            DroneForList drone = droneForLists.Find(d => d.Id == id);
            if (drone!=null)
            {
                if (drone.DroneStatus==DroneStatus.Available)
                {
                    Station station = dal.GetClosestStation(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude, true);
                    double minDistans = getDistanceBetweenTwoPoints(drone, station);
                    dal.SendingDroneForCharging(id, station.Id);
                    if (drone.Battery >= minDistans*dal.PowerConsumptionRequest()[0])
                    {
                        //update in BL
                        drone.Battery -= minDistans * dal.PowerConsumptionRequest()[0];
                        drone.CurrentLocation = new Location { Lattitude = station.Lattitude, Longitude = station.Longitude };
                        drone.DroneStatus = DroneStatus.Maintenance;
                        
                        int dIndex = droneForLists.FindIndex(d => d.Id == id);
                        droneForLists[dIndex] = drone;

                        //update in DAL
                        dal.SendingDroneForCharging(id, station.Id);
                    }
                    else
                    {
                        throw new NotEnoughFuelException("The drone cannot be sent for charging because there " +
                            "is not enough fuel to reach the nearest available charging station.");
                    }
                }
                else
                {
                    throw new DroneConditionNotConfirmException("The drone can not be sent for charging because it is not available");
                }
            }
            else
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
            }
        }

        /// <summary>
        /// Release drone from charging
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateReleaseDroneFromCharging(int id, TimeSpan chargingTime)
        {
            DroneForList drone = droneForLists.Find(d => d.Id == id);
            if (drone != null)
            {
                if (drone.DroneStatus == DroneStatus.Maintenance)
                {
                    //update in BL
                    drone.Battery -= chargingTime.TotalMilliseconds*1000 * dal.PowerConsumptionRequest()[4];
                    drone.DroneStatus = DroneStatus.Available;

                    int dIndex = droneForLists.FindIndex(d => d.Id == id);
                    droneForLists[dIndex] = drone;

                    //update in DAL 
                    //We will find the charging station by the location of the drone
                    Station station = dal.GetAllBaseStations().
                        Where(s => s.Lattitude == drone.CurrentLocation.Lattitude && s.Longitude == drone.CurrentLocation.Longitude).
                        ToList()[0];
                    dal.ReleasDroneFromCharging(id,station.Id);//sending for dal
                }
                else
                {
                    throw new DroneConditionNotConfirmException("The drone can not be relese from charging because it is not in maintenance");
                }
            }
            else
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
            }
        }

        #endregion

    }
}
