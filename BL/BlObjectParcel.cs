﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
namespace BO
{
    public partial class BlObject : BLApi.IBL
    {
        #region ADD
        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public int AddParcelBo(int senderId, int targetId, BO.WeightCategories weight, BO.Priorities priority)
        {
            try
            {
                //Add parcel in DAL to data source and get the parcel id that was created.
                int parcelId = dal.AddParcel(senderId, targetId, (DO.WeightCategories)weight, (DO.Priorities)priority);

                //Find a customer sending
                DO.Customer customerS = dal.GetCustomer(senderId);
                CustomerInParcel sender = new CustomerInParcel() { CustomerId = customerS.Id, CustomerName = customerS.Name };
                //Find a receiving customer
                DO.Customer customerT = dal.GetCustomer(targetId);
                CustomerInParcel target = new CustomerInParcel() { CustomerId = customerT.Id, CustomerName = customerT.Name };

                //add per fields in BL.
                BO.Parcel parcel = new BO.Parcel()
                {
                    ParcelId = parcelId,
                    SenderOfParcel = sender,
                    TargetToParcel = target,
                    Weight = weight,
                    Priorities = priority,
                    DroneInParcel = null,
                    Requested = DateTime.Now,
                    Scheduled = null,
                    PickedUp = null,
                    ParcelDelivery = null
                };
                return parcelId;
            }
            catch (DO.ParcelAlreadyExistException e)
            {
                throw new BO.ParcelAlreadyExistException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Assign parcel to drone
        /// </summary>
        /// <param name="droneId">ID drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateAssignParcelToDrone(int droneId)
        {
            try
            {
                DO.Drone drone = dal.GetDrone(droneId);
                if (droneForLists.Exists(d => d.DroneId == droneId && d.DroneStatus == DroneStatus.Available))
                {
                    DroneForList dr = droneForLists.Find(d => d.DroneId == droneId);

                    List<DO.Parcel> parcels = dal.GetAllParcels().Where(p => p.DroneId == 0).ToList();
                    // list parcels ordered by priority and weight
                    List<DO.Parcel> orderedParcels = (from parcel in parcels
                                                      orderby parcel.priority descending,
                                                      parcel.Weight ascending,
                                                      getDistanceBetweenTwoPoints(dr.CurrentLocation.Lattitude, dr.CurrentLocation.Longitude, dal.GetCustomer(parcel.SenderId).Lattitude, dal.GetCustomer(parcel.SenderId).Longitude)
                                                      where parcel.Weight <= drone.MaxWeight
                                                      select parcel).ToList();

                    // choose the first parcel from the list of parcels
                    //************ DO.Parcel theParcel = orderedParcels.FirstOrDefault();
                    foreach (var theParcel in orderedParcels)
                    {
                        // finds the customer's location
                        DO.Customer customer = dal.GetCustomer(theParcel.SenderId);
                        double distanceForBattery = getDistanceBetweenTwoPoints(dr.CurrentLocation.Lattitude, dr.CurrentLocation.Longitude, customer.Lattitude, customer.Longitude) * dal.PowerConsumptionRequest()[0] +
                            dal.GetDistanceBetweenLocationsOfParcels(theParcel.SenderId, theParcel.TargetId) * dal.PowerConsumptionRequest()[(int)theParcel.Weight + 1] +
                            dal.GetDistanceBetweenLocationAndClosestBaseStation(theParcel.TargetId) + dal.PowerConsumptionRequest()[0];


                        //Find the nearest package
                        //All of the above only on condition that the drone manages to reach the sender,
                        //deliver the package to the destination and reach the nearest station(from the delivery destination)
                        //in order to be loaded(if there is an additional need)
                        if (distanceForBattery <= dr.DroneBattery)
                        {
                            int dIndex = droneForLists.FindIndex(d => d.DroneId == droneId);
                            dr.DroneStatus = DroneStatus.Delivery;
                            droneForLists[dIndex] = dr;

                            dal.AssigningParcelToDrone(theParcel.Id, droneId);
                            return;
                        }
                    }
                    throw new BatteryOfDroneNotAllowException(droneId, dr.DroneBattery, "To Assign Parcel To Drone", "The battery in the drone is not enough to make the shipment.\n Please enter another drone number.");
                }
                else
                {
                    throw new BO.BadDroneIDException(droneId, "A drone is not available to associate a drone package");
                }
            }
            catch (DO.BadParcelIDException e)
            {
                throw new BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        public void UpdateCollectionParcelByDrone(int droneId)
        {
            try
            {
                //the drone collect a parcel only if the parcel has been assigned to it and haven't picked up yet
                var drone = dal.GetDrone(droneId);
                var drone_l = droneForLists.Find(d => d.DroneId == droneId && d.DroneStatus == DroneStatus.Delivery);
                foreach (var parcel in dal.GetAllParcelsWithoutSpecialDron(p => p.DroneId == droneId && p.Delivered == null))
                {
                    //check if the parcel was assigned
                    if (parcel.Scheduled == null)
                    {
                        throw new Exception("the parcel wasn't assigned to the drone!");
                    }
                    //check if the parcel was picked up
                    if (parcel.PickedUp != null)
                    {
                        throw new Exception("the parcel was picked up already!");
                    }
                    else
                    {
                        //update in BL
                        DO.Customer customer = dal.GetCustomer(parcel.SenderId);//finds the sender 
                                                                                //calculate the distance frome the current location of the drone- to the customer
                        double distance = getDistanceBetweenTwoPoints(drone_l.CurrentLocation.Lattitude, drone_l.CurrentLocation.Longitude, customer.Lattitude, customer.Longitude);
                        // update the location of the drone to where the senderk
                        drone_l.CurrentLocation = new Location { Lattitude = customer.Lattitude, Longitude = customer.Longitude };
                        drone_l.DroneBattery -= distance * dal.PowerConsumptionRequest()[(int)parcel.Weight + 1];
                        int dIndex = droneForLists.FindIndex(d => d.DroneId == droneId);
                        droneForLists[dIndex] = drone_l;

                        //update in dal
                        dal.PackagCollectionByDrone(parcel.Id);
                    }
                }
            }
            catch (DO.BadParcelIDException e)
            {
                throw new BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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

        /// <summary>
        /// Delivery parcel by drone
        /// </summary>
        /// <param name="id">ID drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateDeliveryParcelByDrone(int droneId)
        {
            try
            {
                //the drone collect a parcel only if the parcel has been assigned to it and hasn't been picked up yet
                var drone = dal.GetDrone(droneId);
                var drone_l = droneForLists.Find(d => d.DroneId == droneId && d.DroneStatus == DroneStatus.Delivery);

                foreach (var parcel in dal.GetAllParcelsWithoutSpecialDron(p => p.DroneId == droneId))
                {

                    //check if the parcel was assigned
                    if (parcel.PickedUp == null)
                    {
                        throw new Exception("the parcel wasn't picked up yet!");
                    }
                    //check if the parcel was picked up
                    if (parcel.Delivered != null)
                    {
                        throw new Exception("the parcel delivered already!");
                    }
                    else
                    {
                        //update in BL
                        DO.Customer customer = dal.GetCustomer(parcel.TargetId);//finds the sender 
                                                                                //calculate the distance frome the current location of the drone- to the customer
                        double distance = dal.GetDistanceBetweenLocationsOfParcels(parcel.SenderId, parcel.TargetId);
                        // update the location of the drone to where the senderk
                        drone_l.CurrentLocation = new Location { Lattitude = customer.Lattitude, Longitude = customer.Longitude };
                        drone_l.DroneBattery -= distance * dal.PowerConsumptionRequest()[0];
                        drone_l.DroneStatus = DroneStatus.Available;
                        int dIndex = droneForLists.FindIndex(d => d.DroneId == droneId);
                        droneForLists[dIndex] = drone_l;

                        //update in dal
                        dal.DeliveryPackageToCustomer(parcel.Id);
                    }
                }
            }
            catch (DO.BadParcelIDException e)
            {
                throw new BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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

        #region GET ITEM
        /// <summary>
        /// Parcel view
        /// </summary>
        /// <param name="parcelId">parcel id</param>
        /// <returns>parcel</returns>
        public BO.Parcel GetParcel(int parcelId)
        {
            try
            {
                var parcel = dal.GetParcel(parcelId);
                var sender = GetCustomer(parcel.SenderId);
                var target = GetCustomer(parcel.TargetId);
                BO.Parcel parcel_BO = new BO.Parcel()
                {
                    ParcelId = parcelId,
                    SenderOfParcel = new() { CustomerId = sender.CustomerId, CustomerName = sender.NameOfCustomer },
                    TargetToParcel = new() { CustomerId = target.CustomerId, CustomerName = target.NameOfCustomer },
                    Weight = (BO.WeightCategories)parcel.Weight,
                    Priorities = (BO.Priorities)parcel.priority,
                    Requested = parcel.Requested,
                    Scheduled = parcel.Scheduled,
                    PickedUp = parcel.PickedUp,
                    ParcelDelivery = parcel.Delivered
                };
                DroneInParcel droneInParcel = new();
                if (parcel.DroneId != 0)
                {
                    dal.GetDrone(parcel.DroneId);
                    var drone = droneForLists.Find(d => d.DroneId == parcel.DroneId);
                    droneInParcel.DroneId = drone.DroneId;
                    droneInParcel.DroneBattery = drone.DroneBattery;
                    droneInParcel.CurrentLocation = drone.CurrentLocation;
                }
                return parcel_BO;
            }
            catch (DO.BadParcelIDException e)
            {
                throw new BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
            catch (DO.BadDroneIDException e)
            {
                throw new BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST 
        /// <summary>
        /// Displays the list of parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<ParcelForList> GetAllParcelsBo()
        {
            try
            {
                List<ParcelForList> ParcelForList = new();
                foreach (var parcel in dal.GetAllParcels())
                {
                    ParcelForList parcelForList = clonParcel(GetParcel(parcel.Id));
                    ParcelForList.Add(parcelForList);
                }
                return ParcelForList;
            }
            catch (DO.BadParcelIDException e)
            {
                throw new BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcels that have not yet been assigned to the drone</returns>
        public IEnumerable<ParcelForList> GetAllParcelsNotYetAssociatedWithDrone()
        {
            List<ParcelForList> list = new();
            foreach (var parcel in dal.GetAllParcelsWithoutSpecialDron(x => x.Id > 0 && x.DroneId == 0))
            {
                ParcelForList parcelForList = clonParcel(GetParcel(parcel.Id));
                list.Add(parcelForList);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Converts from object parcel to object parcel for list
        /// </summary>
        /// <param name="parcel">parcel</param>
        private ParcelForList clonParcel(BO.Parcel parcel)
        {
            return new()
            {
                ParcelId = parcel.ParcelId,
                CustomerNameSend = parcel.SenderOfParcel.CustomerName,
                CustomerNameTarget = parcel.TargetToParcel.CustomerName,
                Weight = parcel.Weight,
                Priorities = parcel.Priorities,
                //find the status of parcel.
                ParcelStatus = (parcel.ParcelDelivery != null) ? BO.ParcelStatus.Provided :
                    (parcel.PickedUp != null) ? BO.ParcelStatus.WasCollected :
                    (parcel.Scheduled != null) ? BO.ParcelStatus.Associated : BO.ParcelStatus.Defined
            };
        }


    }
}
