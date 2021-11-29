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
        public void AddParcelBo(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
        {
            //Add parcel in DAL to data source and get the parcel id that was created.
            int parcelId = dal.AddParcel(senderId, targetId, (IDAL.DO.WeightCategories)weight, (IDAL.DO.Priorities)priority);

            //Find a customer sending
            IDAL.DO.Customer customerS = dal.GetCustomer(senderId);
            CustomerInParcel sender = new CustomerInParcel() { Id = customerS.Id, Name = customerS.Name };
            //Find a receiving customer
            IDAL.DO.Customer customerG = dal.GetCustomer(targetId);
            CustomerInParcel getting = new CustomerInParcel() { Id = customerG.Id, Name = customerG.Name };

            //add per fields in BL.
            IBL.BO.Parcel parcel = new IBL.BO.Parcel()
            {
                Id = parcelId,
                Sender = sender,
                Getting = getting,
                Weight = weight,
                Priorities = priority,
                DroneInParcel = null,
                CreateParcel = DateTime.Now,
                ParcelAssociation = DateTime.MinValue,
                ParcelCollection = DateTime.MinValue,
                ParcelDelivery = DateTime.MinValue
            };
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Assign parcel to drone
        /// </summary>
        /// <param name="droneId">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateAssignParcelToDrone(int droneId)
        {
            IDAL.DO.Drone drone = dal.GetDrone(droneId);
            if (droneForLists.Exists(d => d.Id == droneId && d.DroneStatus == DroneStatus.Available))
            {
                DroneForList dr = droneForLists.Find(d => d.Id == droneId);

                List<IDAL.DO.Parcel> parcels = dal.GetAllParcels().Where(p => p.Requested == DateTime.MinValue).ToList();

                // list parcels ordered by priority and weight
                List<IDAL.DO.Parcel> orderedParcels = (from parcel in parcels
                                                       orderby parcel.priority descending,
                                                       parcel.Weight ascending
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
                dal.GetDistanceBetweenLocationAndClosestBaseStation(theParcel.TargetId) + dal.PowerConsumptionRequest()[0] <= dr.Battery)
                {
                    int dIndex = droneForLists.FindIndex(d => d.Id == droneId);
                    dr.DroneStatus = DroneStatus.Delivery;
                    droneForLists[dIndex] = dr;

                    dal.AssigningParcelToDrone(theParcel.Id, droneId);
                }
                else
                {
                    throw new ArithmeticException("The battery in the drone is not enough to make the shipment");
                }
            }
            else
            {
                throw new DroneConditionNotConfirmException("A drone is not available to associate a drone package");
            }
        }

        /// <summary>
        /// Collection of a parcek by drone
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateCollectionParcelByDrone(int droneId)
        {
            //the drone collect a parcel only if the parcel has been assigned to it and haven't picked up yet
            var drone = dal.GetDrone(droneId);
            var drone_l = droneForLists.Find(d => d.DroneStatus == DroneStatus.Delivery);
            var parcel = dal.GetAllParcels().ToList().Find(p => p.DroneId == droneId);
            //check if the parcel was assigned
            if (parcel.scheduled == DateTime.MinValue)
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
                drone_l.Battery -= distance * dal.PowerConsumptionRequest()[(int)parcel.Weight + 1];
                int dIndex = droneForLists.FindIndex(d => d.Id == droneId);
                droneForLists[dIndex] = drone_l;

                //update in dal
                dal.PackagCollectionByDrone(parcel.Id);
            }
        }

        /// <summary>
        /// Delivery parcel by drone
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateDeliveryParcelByDrone(int droneId)
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
                drone_l.Battery -= distance * dal.PowerConsumptionRequest()[0];
                drone_l.DroneStatus = DroneStatus.Available;
                int dIndex = droneForLists.FindIndex(d => d.Id == droneId);
                droneForLists[dIndex] = drone_l;

                //update in dal
                dal.DeliveryPackageToCustomer(parcel.Id);
            }
        }
        #endregion
    }
}

