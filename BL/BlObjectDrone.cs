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
            try
            {
                //add drone fields in BL.
                Station s = dal.GetBaseStation(stationId);
                DroneForList drone = new()
                {
                    Id = droneId,
                    Model = model,
                    MaxWeight = maxWeight,
                    Battery = random.Next(20, 41),
                    DroneStatus = DroneStatus.Maintenance,
                    CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude }
                };

                // Add the drone to the list of drones when charging from a base station
                droneForLists.Add(drone);

                //Add drone in DAL to data source.
                dal.AddDrone(droneId, model, (IDAL.DO.WeightCategories)maxWeight);

                dal.SendingDroneForCharging(droneId, stationId);
            }
            catch(IDAL.DO.DroneAlreadyExistException e)
            {
                throw new IBL.BO.DroneAlreadyExistException(e.ID, e.Model, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadBaseStationIDException e)
            {
                throw new IBL.BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
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
                int dIndex = droneForLists.FindIndex(d => d.Id == id);//Obtain an index for the location where the package ID is located
                if (droneForLists[dIndex].Model != model)
                {
                    DroneForList drone = droneForLists[dIndex];
                    drone.Model = model;
                    droneForLists[dIndex] = drone;
                }
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }


        /// <summary>
        /// Sending a drone for charging
        /// </summary>
        /// <param name="id">ID drone for charging</param>
        public void SendingDroneForCharging(int id)
        {
            try
            {
                dal.GetDrone(id);
                DroneForList drone = droneForLists.Find(d => d.Id == id);
                if (drone != null)
                {
                    if (drone.DroneStatus == DroneStatus.Available)
                    {
                        Station station = dal.GetClosestStation(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude, true);
                        double minDistans = getDistanceBetweenTwoPoints(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude, station.Lattitude, station.Longitude);
                        if (drone.Battery >= minDistans * dal.PowerConsumptionRequest()[0])
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
                            throw new StatusDroneNotAllowException(id,drone.DroneStatus, "sent for charging", "The drone cannot be sent for charging because there " +
                                "is not enough fuel to reach the nearest available charging station.");
                        }
                    }
                    else
                    {
                        throw new StatusDroneNotAllowException(id,drone.DroneStatus,"sent for charging", "The drone can not be sent for charging because it is not available");
                    }
                }
                else
                {
                    throw new IBL.BO.BadDroneIDException(id, "the drone not exists in the list of drones");
                }
            }
            catch (IDAL.DO.BadBaseStationIDException e)
            {
                throw new IBL.BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
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
                DroneForList drone = droneForLists.Find(d => d.Id == id);
                if (drone != null)
                {
                    if (drone.DroneStatus == DroneStatus.Maintenance)
                    {
                        //update in BL
                        drone.Battery += chargingTime.TotalMilliseconds * 1000 * dal.PowerConsumptionRequest()[4];
                        drone.DroneStatus = DroneStatus.Available;

                        int dIndex = droneForLists.FindIndex(d => d.Id == id);
                        droneForLists[dIndex] = drone;

                        //update in DAL 
                        //We will find the charging station by the location of the drone
                        Station station = dal.GetAllBaseStations().
                            Where(s => s.Lattitude == drone.CurrentLocation.Lattitude && s.Longitude == drone.CurrentLocation.Longitude).
                            ToList()[0];
                        dal.ReleasDroneFromCharging(id, station.Id);//sending for dal
                    }
                    else
                    {
                        throw new StatusDroneNotAllowException(id,drone.DroneStatus, "relese from charging", "The drone can not be relese from charging because it is not in maintenance");
                    }
                }
                else
                {
                    throw new IBL.BO.BadDroneIDException(id, "the drone not exists in the system");
                }
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadBaseStationIDException e)
            {
                throw new IBL.BO.BadBaseStationIDException(e.ID, e.Message, e.InnerException);
            }
        }

        #endregion

        #region GET ITEM
        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public IBL.BO.Drone GetDrone(int droneId)
        {
            try
            {
                IDAL.DO.Drone drone = dal.GetDrone(droneId);
                DroneForList drone_l = droneForLists.Find(d => d.Id == droneId);
                if (drone_l.DroneStatus == DroneStatus.Delivery)
                {
                    var parcel = dal.GetAllParcels().ToList().Find(p => p.DroneId == droneId);
                    var customer_sender = dal.GetCustomer(parcel.SenderId);
                    var customer_target = dal.GetCustomer(parcel.TargetId);

                    ParcelInTransfer parcelInTransfer = new ParcelInTransfer()
                    {
                        Id = parcel.Id,
                        Priorities = (IBL.BO.Priorities)parcel.priority,
                        Weight = (IBL.BO.WeightCategories)parcel.Weight,
                        Sender = new() { Id = customer_sender.Id, Name = customer_sender.Name },
                        Target = new() { Id = customer_target.Id, Name = customer_target.Name },
                        Collection = new() { Lattitude = customer_sender.Lattitude, Longitude = customer_sender.Longitude },
                        DeliveryDestination = new() { Lattitude = customer_target.Lattitude, Longitude = customer_target.Longitude },
                        TransportDistance = getDistanceBetweenTwoPoints(customer_sender.Lattitude, customer_sender.Longitude, customer_target.Lattitude, customer_target.Longitude)
                    };

                    parcelInTransfer.ParcelStatusInTransfer = (parcel.PickedUp == DateTime.MinValue) ? ParcelStatusInTransfer.AwaitingCollection :
                        parcelInTransfer.ParcelStatusInTransfer = ParcelStatusInTransfer.OnTheWayToDestination;

                    return new()
                    {
                        Id = droneId,
                        Model = drone.Model,
                        Weight = drone_l.MaxWeight,
                        Battery = drone_l.Battery,
                        DroneStatus = drone_l.DroneStatus,
                        ParcelInTransfer = parcelInTransfer,
                        CurrentLocation = drone_l.CurrentLocation

                    };
                }
                else
                {
                    return new()
                    {
                        Id = droneId,
                        Model = drone.Model,
                        Weight = drone_l.MaxWeight,
                        Battery = drone_l.Battery,
                        DroneStatus = drone_l.DroneStatus,
                        ParcelInTransfer = null,
                        CurrentLocation = drone_l.CurrentLocation

                    };
                }
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadCustomerIDException e)
            {
                throw new IBL.BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneForList> GetAllDronesBo()
        {
            try
            {
                List<DroneForList> list = new();
                foreach (var drone_l in droneForLists)
                {
                    DroneForList droneFor = clonDrone(GetDrone(drone_l.Id));
                    if (drone_l.DroneStatus == DroneStatus.Delivery)
                    {
                        droneFor.ParcelNumIsTransferred = dal.GetAllParcels().
                            ToList().Find(p => p.DroneId == drone_l.Id).Id;
                    }

                    list.Add(droneFor);
                }
                return list;
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        private DroneForList clonDrone(IBL.BO.Drone drone)
        {
            return new DroneForList
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.Weight,
                Battery = drone.Battery,
                DroneStatus = drone.DroneStatus,
                CurrentLocation = drone.CurrentLocation
            };
        }
    }
}
