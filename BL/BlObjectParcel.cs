using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        #region ADD
        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public int AddParcelBo(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
        {
            try
            {
                //Add parcel in DAL to data source and get the parcel id that was created.
                int parcelId = dal.AddParcel(senderId, targetId, (IDAL.DO.WeightCategories)weight, (IDAL.DO.Priorities)priority);

                //Find a customer sending
                IDAL.DO.Customer customerS = dal.GetCustomer(senderId);
                CustomerInParcel sender = new CustomerInParcel() { CustomerId = customerS.Id, CustomerName = customerS.Name };
                //Find a receiving customer
                IDAL.DO.Customer customerG = dal.GetCustomer(targetId);
                CustomerInParcel getting = new CustomerInParcel() { CustomerId = customerG.Id, CustomerName = customerG.Name };

                //add per fields in BL.
                IBL.BO.Parcel parcel = new IBL.BO.Parcel()
                {
                    ParcelId = parcelId,
                    SenderOfParcel = sender,
                    TargetToParcel = getting,
                    Weight = weight,
                    Priorities = priority,
                    DroneInParcel = null,
                    Requested = DateTime.Now,
                    Scheduled = DateTime.MinValue,
                    PickedUp = DateTime.MinValue,
                    ParcelDelivery = DateTime.MinValue
                };
                return parcelId;
            }
            catch (IDAL.DO.ParcelAlreadyExistException e)
            {
                throw new IBL.BO.ParcelAlreadyExistException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadCustomerIDException e)
            {
                throw new IBL.BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
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
                IDAL.DO.Drone drone = dal.GetDrone(droneId);
                if (droneForLists.Exists(d => d.DroneId == droneId && d.DroneStatus == DroneStatus.Available))
                {
                    DroneForList dr = droneForLists.Find(d => d.DroneId == droneId);

                    List<IDAL.DO.Parcel> parcels = dal.GetAllParcels().Where(p => p.Scheduled == DateTime.MinValue).ToList();
                    // list parcels ordered by priority and weight
                    List<IDAL.DO.Parcel> orderedParcels = (from parcel in parcels
                                                           orderby parcel.priority descending,
                                                           parcel.Weight ascending,
                                                           getDistanceBetweenTwoPoints(dr.CurrentLocation.Lattitude, dr.CurrentLocation.Longitude, dal.GetCustomer(parcel.SenderId).Lattitude, dal.GetCustomer(parcel.SenderId).Longitude)
                                                           where parcel.Weight <= drone.MaxWeight
                                                           select parcel).ToList();

                    // choose the first parcel from the list of parcels
                    IDAL.DO.Parcel theParcel = orderedParcels.FirstOrDefault();
                    // finds the customer's location
                    IDAL.DO.Customer customer = dal.GetCustomer(theParcel.SenderId);
                    //Find the nearest package
                    //All of the above only on condition that the drone manages to reach the sender,
                    //deliver the package to the destination and reach the nearest station(from the delivery destination)
                    //in order to be loaded(if there is an additional need)
                    if (getDistanceBetweenTwoPoints(dr.CurrentLocation.Lattitude, dr.CurrentLocation.Longitude, customer.Lattitude, customer.Longitude) * dal.PowerConsumptionRequest()[0] +
                    dal.GetDistanceBetweenLocationsOfParcels(theParcel.SenderId, theParcel.TargetId) * dal.PowerConsumptionRequest()[(int)theParcel.Weight + 1] +
                    dal.GetDistanceBetweenLocationAndClosestBaseStation(theParcel.TargetId) + dal.PowerConsumptionRequest()[0] <= dr.DroneBattery)
                    {
                        int dIndex = droneForLists.FindIndex(d => d.DroneId == droneId);
                        dr.DroneStatus = DroneStatus.Delivery;
                        droneForLists[dIndex] = dr;

                        dal.AssigningParcelToDrone(theParcel.Id, droneId);
                    }
                    else
                    {
                        throw new BatteryOfDroneNotAllowException(droneId,dr.DroneBattery, "To Assign Parcel To Drone", "The battery in the drone is not enough to make the shipment");
                    }
                    throw new BatteryOfDroneNotAllowException(droneId, dr.Battery, "To Assign Parcel To Drone", "The battery in the drone is not enough to make the shipment.\n Please enter another drone number.");
                }
                else
                {
                    throw new IBL.BO.BadDroneIDException(droneId, "A drone is not available to associate a drone package");
                }
            }
            catch (IDAL.DO.BadParcelIDException e)
            {
                throw new IBL.BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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

        public void UpdateCollectionParcelByDrone(int droneId)
        {
            try
            {
                //the drone collect a parcel only if the parcel has been assigned to it and haven't picked up yet
                var drone = dal.GetDrone(droneId);
                var drone_l = droneForLists.Find(d => d.DroneStatus == DroneStatus.Delivery);
                var parcel = dal.GetAllParcels().ToList().Find(p => p.DroneId == droneId);
                //check if the parcel was assigned
                if (parcel.Scheduled == DateTime.MinValue)
                {
                    throw new Exception("the parcel wasn't assigned to the drone!");
                }
                //check if the parcel was picked up
                if (parcel.PickedUp != DateTime.MinValue)
                {
                    throw new Exception("the parcel was picked up already!");
                }
                else
                {
                    //update in BL
                    IDAL.DO.Customer customer = dal.GetCustomer(parcel.SenderId);//finds the sender 
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
            catch (IDAL.DO.BadParcelIDException e)
            {
                throw new IBL.BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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

        /// <summary>
        /// Delivery parcel by drone
        /// </summary>
        /// <param name="id">ID drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateDeliveryParcelByDrone(int droneId)
        {
            try
            {
                //the drone collect a parcel only if the parcel has been assigned to it and haven't picked up yet
                var drone = dal.GetDrone(droneId);
                var drone_l = droneForLists.Find(d => d.DroneStatus == DroneStatus.Delivery);
                var parcel = dal.GetAllParcels().ToList().Find(p => p.DroneId == droneId);
                //check if the parcel was assigned
                if (parcel.PickedUp == DateTime.MinValue)
                {
                    throw new Exception("the parcel wasn't picked up yet!");
                }
                //check if the parcel was picked up
                if (parcel.Delivered != DateTime.MinValue)
                {
                    throw new Exception("the parcel delivered already!");
                }
                else
                {
                    //update in BL
                    IDAL.DO.Customer customer = dal.GetCustomer(parcel.TargetId);//finds the sender 
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
            catch (IDAL.DO.BadParcelIDException e)
            {
                throw new IBL.BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
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

        #region GET ITEM
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public IBL.BO.Parcel GetParcel(int parcelId)
        {
            try
            {
                var parcel = dal.GetParcel(parcelId);
                var sender = GetCustomer(parcel.SenderId);
                var target = GetCustomer(parcel.TargetId);
                IBL.BO.Parcel parcel_BO = new IBL.BO.Parcel()
                {
                    ParcelId = parcelId,
                    SenderOfParcel = new() { CustomerId = sender.CustomerId, CustomerName = sender.NameOfCustomer },
                    TargetToParcel = new() { CustomerId = target.CustomerId, CustomerName = target.NameOfCustomer },
                    Weight = (IBL.BO.WeightCategories)parcel.Weight,
                    Priorities = (IBL.BO.Priorities)parcel.priority,
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
            catch (IDAL.DO.BadParcelIDException e)
            {
                throw new IBL.BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadCustomerIDException e)
            {
                throw new IBL.BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
            catch (IDAL.DO.BadDroneIDException e)
            {
                throw new IBL.BO.BadDroneIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelForList> GetAllParcelsBo()
        {
            try
            {
                List<ParcelForList> list = new();
                foreach (var parcel in dal.GetAllParcels())
                {
                    ParcelForList parcelForList = clonParcel(GetParcel(parcel.Id));
                    list.Add(parcelForList);
                }
                return list;
            }
            catch (IDAL.DO.BadParcelIDException e)
            {
                throw new IBL.BO.BadParcelIDException(e.ID, e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelForList> GetAllParcelsNotYetAssociatedWithGlider()
        {
            List<ParcelForList> list = new();
            foreach (var parcel in dal.GetAllParcelsWithoutSpecialDron())
            {
                ParcelForList parcelForList = clonParcel(GetParcel(parcel.Id));
                list.Add(parcelForList);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private ParcelForList clonParcel(IBL.BO.Parcel parcel)
        {
            return new()
            {
                ParcelId = parcel.ParcelId,
                CustomerNameSend = parcel.SenderOfParcel.CustomerName,
                CustomerNameTarget = parcel.TargetToParcel.CustomerName,
                Weight = parcel.Weight,
                Priorities = parcel.Priorities,
                //find the status of parcel.
                ParcelStatus = (parcel.ParcelDelivery != DateTime.MinValue) ? IBL.BO.ParcelStatus.Provided :
                    (parcel.PickedUp != DateTime.MinValue) ? IBL.BO.ParcelStatus.WasCollected :
                    (parcel.Scheduled != DateTime.MinValue) ? IBL.BO.ParcelStatus.Associated : IBL.BO.ParcelStatus.Defined
            };
        }


    }
}

