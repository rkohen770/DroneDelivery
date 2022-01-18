using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class BlObject : BLApi.IBL
    {

        #region ADD
        /// <summary>
        /// Add drone
        /// </summary>
        /// <param name="droneId">Manufacturer's serial number</param>
        /// <param name="model">Drone model</param>
        /// <param name="maxWeight">Maximum weight</param>
        /// <param name="stationId">Number of stations to put the drone for initial charging</param>
        public void AddDroneBo(int droneId, string model, BO.WeightCategories maxWeight, int stationId)
        {
            try
            {
                //add drone fields in BL.
                Station s = dal.GetBaseStation(stationId);
                DroneForList drone = new()
                {
                    DroneID = droneId,
                    DroneModel = model,
                    MaxWeight = maxWeight,
                    DroneBattery = random.Next(20, 41),
                    DroneStatus = DroneStatus.Maintenance,
                    CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude }
                };

                // Add the drone to the list of drones when charging from a base station
                droneForLists.Add(drone);

                //Add drone in DAL to data source.
                dal.AddDrone(droneId, model, (DO.WeightCategories)maxWeight);

                dal.SendingDroneForCharging(droneId, stationId);
            }
            catch (DO.DroneAlreadyExistException e)
            {
                throw new BO.DroneAlreadyExistException(e.ID, e.Model, e.Message, e.InnerException);
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadBaseStationIDException e)
            {
                throw new BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
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
            try
            {
                dal.GetDrone(id);
                //update in dal
                dal.UpdateDroneModle(id, model);

                //update in BL
                int dIndex = droneForLists.FindIndex(d => d.DroneID == id);//Obtain an index for the location where the package ID is located
                if (droneForLists[dIndex].DroneModel != model)
                {
                    DroneForList drone = droneForLists[dIndex];
                    drone.DroneModel = model;
                    droneForLists[dIndex] = drone;
                }
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }


        /// <summary>
        /// Sending a drone for charging
        /// </summary>
        /// <param name="id">ID drone for charging</param>
        public int SendingDroneForCharging(int id)
        {
            try
            {
                dal.GetDrone(id);
                DroneForList drone = droneForLists.Find(d => d.DroneID == id);
                if (drone != null)
                {
                    if (drone.DroneStatus == DroneStatus.Available)
                    {
                        Station station = dal.GetClosestStation(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude, true);
                        double minDistans = (int)getDistanceBetweenTwoPoints(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude, station.Lattitude, station.Longitude);
                        if (drone.DroneBattery >= minDistans * dal.PowerConsumptionRequest()[0])
                        {
                            //update in BL
                            drone.DroneBattery -= minDistans * dal.PowerConsumptionRequest()[0];
                            drone.CurrentLocation = new Location { Lattitude = station.Lattitude, Longitude = station.Longitude };
                            drone.DroneStatus = DroneStatus.Maintenance;

                            int dIndex = droneForLists.FindIndex(d => d.DroneID == id);
                            droneForLists[dIndex] = drone;

                            //update in DAL
                            return dal.SendingDroneForCharging(id, station.StationID);

                        }
                        else
                        {
                            throw new StatusDroneNotAllowException(id, drone.DroneStatus, "sent for charging", "The drone cannot be sent for charging because there " +
                                "is not enough fuel to reach the nearest available charging station.");
                        }
                    }
                    else
                    {
                        throw new StatusDroneNotAllowException(id, drone.DroneStatus, "sent for charging", "The drone can not be sent for charging because it is not available");
                    }
                }
                else
                {
                    throw new BO.BadDroneIDException(id, "the drone not exists in the list of drones");
                }
            }
            catch (DO.BadBaseStationIDException e)
            {
                throw new BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }

        }

        /// <summary>
        /// Release drone from charging
        /// </summary>
        /// <param name="id">ID drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateReleaseDroneFromCharging(int id, TimeSpan chargingTime)
        {
            try
            {
                dal.GetDrone(id);
                DroneForList drone = droneForLists.Find(d => d.DroneID == id);
                if (drone != null)
                {
                    if (drone.DroneStatus == DroneStatus.Maintenance)
                    {
                        //update in BL
                        drone.DroneBattery += chargingTime.TotalMilliseconds * 1000 * dal.PowerConsumptionRequest()[4];
                        if (drone.DroneBattery >= 100)
                            drone.DroneBattery = 100;
                        drone.DroneStatus = DroneStatus.Available;

                        int dIndex = droneForLists.FindIndex(d => d.DroneID == id);
                        droneForLists[dIndex] = drone;

                        //update in DAL 
                        //We will find the charging station by the location of the drone
                        Station station = dal.GetAllBaseStations().
                            Where(s => s.Lattitude == drone.CurrentLocation.Lattitude && s.Longitude == drone.CurrentLocation.Longitude).
                            FirstOrDefault();
                        dal.ReleasDroneFromCharging(id, station.StationID);//sending for dal
                    }
                    else
                    {
                        throw new StatusDroneNotAllowException(id, drone.DroneStatus, "relese from charging", "The drone can not be relese from charging because it is not in maintenance");
                    }
                }
                else
                {
                    throw new BO.BadDroneIDException(id, "the drone not exists in the system");
                }
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadBaseStationIDException e)
            {
                throw new BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
        }

        #endregion

        #region GET ITEM
        /// <summary>
        /// Drone display
        /// </summary>
        /// <param name="droneId">drone id</param>
        /// <returns>drone</returns>
        public Drone GetDrone(int droneId)
        {
            try
            {
                DO.Drone drone = dal.GetDrone(droneId);
                DroneForList drone_l = droneForLists.Find(d => d.DroneID == droneId);
                if (drone_l.DroneStatus == DroneStatus.Delivery)
                {
                    var parcel = dal.GetAllParcels().ToList().Find(p => p.DroneID == droneId);
                    var customer_sender = dal.GetCustomer(parcel.SenderID);
                    var customer_target = dal.GetCustomer(parcel.TargetID);

                    ParcelInTransfer parcelInTransfer = new ParcelInTransfer()
                    {
                        ParcelID = parcel.ParcelID,
                        Priorities = (BO.Priorities)parcel.priority,
                        Weight = (BO.WeightCategories)parcel.Weight,
                        SenderOfParcel = new() { CustomerID = customer_sender.CustomerID, CustomerName = customer_sender.Name },
                        TargetToParcel = new() { CustomerID = customer_target.CustomerID, CustomerName = customer_target.Name },
                        Collection = new() { Lattitude = customer_sender.Lattitude, Longitude = customer_sender.Longitude },
                        DeliveryDestination = new() { Lattitude = customer_target.Lattitude, Longitude = customer_target.Longitude },
                        TransportDistance = getDistanceBetweenTwoPoints(customer_sender.Lattitude, customer_sender.Longitude, customer_target.Lattitude, customer_target.Longitude)
                    };

                    parcelInTransfer.ParcelStatus = (parcel.PickedUp == null) ? ParcelStatusInTransfer.AwaitingCollection :
                        parcelInTransfer.ParcelStatus = ParcelStatusInTransfer.OnTheWayToDestination;

                    return new()
                    {
                        DroneID = droneId,
                        DroneModel = drone.DroneModel,
                        Weight = drone_l.MaxWeight,
                        DroneBattery = drone_l.DroneBattery,
                        DroneStatus = drone_l.DroneStatus,
                        ParcelInTransfer = parcelInTransfer,
                        CurrentLocation = drone_l.CurrentLocation

                    };
                }
                else
                {
                    return new()
                    {
                        DroneID = droneId,
                        DroneModel = drone.DroneModel,
                        Weight = drone_l.MaxWeight,
                        DroneBattery = drone_l.DroneBattery,
                        DroneStatus = drone_l.DroneStatus,
                        ParcelInTransfer = null,
                        CurrentLocation = drone_l.CurrentLocation

                    };
                }
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST
        /// <summary>
        /// Displays the list of drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<DroneForList> GetAllDronesBo()
        {
            try
            {
                List<DroneForList> list = new();
                foreach (var drone_l in droneForLists)
                {
                    DroneForList droneFor = CloneDrone(GetDrone(drone_l.DroneID));
                    if (drone_l.DroneStatus == DroneStatus.Delivery)
                    {
                        droneFor.ParcelNumIsTransferred = dal.GetAllParcelsWithoutSpecialDron(p => p.DroneID == drone_l.DroneID)
                            .FirstOrDefault().ParcelID;
                    }
                    list.Add(droneFor);
                }
                return list;
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }

        public IEnumerable<DroneForList> GetDronesByPredicat(Predicate<DroneForList> p)
        {
            List<DroneForList> list = new();

            foreach (var drone in droneForLists.FindAll(p))
            {
                DroneForList droneFor = CloneDrone(GetDrone(drone.DroneID));
                if (drone.DroneStatus == DroneStatus.Delivery)
                {
                    droneFor.ParcelNumIsTransferred = dal.GetAllParcelsWithoutSpecialDron(p => p.DroneID == drone.DroneID)
                        .FirstOrDefault().ParcelID;
                }
                list.Add(droneFor);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Converts from object drone to object drone for list
        /// </summary>
        /// <param name="drone">drone</param>
        /// <returns>drone for list</returns>
        public DroneForList CloneDrone(BO.Drone drone)
        {
            return new DroneForList
            {
                DroneID = drone.DroneID,
                DroneModel = drone.DroneModel,
                MaxWeight = (BO.WeightCategories)drone.Weight,
                DroneBattery = drone.DroneBattery,
                DroneStatus = drone.DroneStatus,
                CurrentLocation = drone.CurrentLocation
            };
        }
    }
}