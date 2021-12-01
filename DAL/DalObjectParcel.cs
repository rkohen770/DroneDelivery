using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject:IDal
    {
        #region ADD
        /// <summary>
        /// Receipt of package for delivery
        /// </summary>
        /// <param name="senderId">Sending customer ID</param>
        /// <param name="targetId"> Receiving customer ID</param>
        /// <param name="weight">Weight category (light, medium, heavy)</param>
        /// <param name="priority">Priority (Normal, fast, emergency)</param>
        /// <returns>An array that contains all the packages</returns>
        public int AddParcel(int senderId, int targetId, WeightCategories weight,
            Priorities priority, int droneId = 0)
        {
            if (DataSource.customers.Exists(customer => customer.Id != senderId))
            {
                throw new BadCustomerIDException(senderId,"the sender not exists in the list of customers");
            }
            if (DataSource.customers.Exists(customer => customer.Id != targetId))
            {
                throw new BadCustomerIDException(targetId,"the target not exists in the list of customers");
            }
            else
            {
                Parcel p = new Parcel
                {
                    Id = DataSource.Config.OrdinalParcelNumber++,
                    SenderId = senderId,
                    TargetId = targetId,
                    Weight = weight,
                    priority = priority,
                    Requested = DateTime.Now,
                    DroneId = droneId
                };
                DataSource.parcels.Add(p);//Adding the new parcel to the array
                return p.Id;
            }
        }
        #endregion

        #region Uppdate
        /// <summary>
        /// Assigning a package to a drone
        /// </summary>
        /// <param name="parcelId">Package ID for association</param>
        public void AssigningParcelToDrone(int parcelId, int droneId)
        {
            if (!DataSource.parcels.Exists(parcel => parcel.Id == parcelId))
            {
                throw new BadParcelIDException(parcelId,"the percel not exists in the list of parcels");
            }
            if (!DataSource.drones.Exists(drone => drone.Id == droneId))
            {
                throw new BadDroneIDException(droneId,"the drone not exists in the list of drones");
            }
            else
            {
                //We will go through the entire array of the drone, to find a available drone
                for (int pIndex = 0; pIndex < DataSource.parcels.Count; pIndex++)
                {
                    if (DataSource.parcels[pIndex].Id == parcelId)
                    {
                        Parcel parcel = DataSource.parcels[pIndex];//Obtain an index for the location where the package ID is located
                        parcel.DroneId = droneId;//Update the droneid field in the drone package found
                        parcel.Scheduled = DateTime.Now;//Update packet time association field to now.
                        DataSource.parcels[pIndex] = parcel;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Package collection by drone
        /// </summary>
        /// <param name="parcelId">Package ID for collection</param>
        public void PackagCollectionByDrone(int parcelId)
        {
            if (!DataSource.parcels.Exists(parcel => parcel.Id == parcelId))
            {
                throw new BadParcelIDException(parcelId,"the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].Id == parcelId)//Obtain an index for the location where the package ID is located
                {
                    Parcel parcel = DataSource.parcels[i];
                    parcel.PickedUp = DateTime.Now;//Update packet time pickeup field to now.
                    DataSource.parcels[i] = parcel;
                    return;
                }
            }
        }
        

        /// <summary>
        /// Delivery package to customer
        /// </summary>
        /// <param name="parcelId">Package ID for delivery</param>
        public void DeliveryPackageToCustomer(int parcelId)
        {
            if (!DataSource.parcels.Exists(parcel => parcel.Id == parcelId))
            {
                throw new BadParcelIDException(parcelId,"the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].Id == parcelId)//Obtain an index for the location where the package ID is located
                {
                    Parcel parcel = DataSource.parcels[i];
                    parcel.Delivered = DateTime.Now;//Update packet time delivered field to now.
                    DataSource.parcels[i] = parcel;
                    return;
                }
            }
        }
        #endregion

        #region Get item
        /// <summary>
        /// return parcel by parcel ID to print.
        /// </summary>
        /// <param name="parcelId">parcel ID to print</param>
        /// <returns>parcel to show</returns>
        public Parcel GetParcel(int parcelId)
        {
            if (!DataSource.parcels.Exists(parcel => parcel.Id == parcelId))
            {
                throw new BadParcelIDException(parcelId,"the parcel not exists in the list of parcels");
            }
            //find the place of the parcel in the array of parcels
            return DataSource.parcels.Find(p => p.Id == parcelId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual parcel
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            return from parcel in DataSource.parcels
                   select parcel.Clone();
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcel without special dron</returns>
        public IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron()
        {
            //return all the parcels without special drone
            return from parcel in DataSource.parcels
                   where parcel.Id > 0 && parcel.DroneId == 0
                   select parcel.Clone();
        }
        #endregion

    }
}
