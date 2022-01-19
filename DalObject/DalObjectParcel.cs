using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DL
{
    sealed partial class DalObject : IDal
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
            if (!(DataSource.customers.Exists(customer => customer.CustomerID == senderId)))
            {
                throw new BadCustomerIDException(senderId, "The sender not exists in the list of customers");
            }
            if (!(DataSource.customers.Exists(customer => customer.CustomerID == targetId)))
            {
                throw new BadCustomerIDException(targetId, "The target not exists in the list of customers");
            }
            else
            {
                Parcel parcel = new Parcel
                {
                    ParcelID =DataSource.Config.SerialNumber++,
                    SenderID = senderId,
                    TargetID = targetId,
                    Weight = weight,
                    Priority = priority,
                    Requested = DateTime.Now,
                    DroneID = droneId
                };
                DataSource.parcels.Add(parcel);//Adding the new parcel to the array
                return parcel.ParcelID;
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            else
            {
                //We will go through the entire list of the drone, to find a available drone
                for (int pIndex = 0; pIndex < DataSource.parcels.Count; pIndex++)
                {
                    if (DataSource.parcels[pIndex].ParcelID == parcelId)
                    {
                        Parcel parcel = DataSource.parcels[pIndex];//Obtain an index for the location where the package ID is located
                        parcel.DroneID = droneId;//Update the droneid field in the drone package found
                        parcel.Scheduled = DateTime.Now;//Update packet time association field to now.
                        DataSource.parcels[pIndex] = parcel;
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ParcelID == parcelId)//Obtain an index for the location where the package ID is located
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ParcelID == parcelId)//Obtain an index for the location where the package ID is located
                {
                    Parcel parcel = DataSource.parcels[i];
                    parcel.Delivered = DateTime.Now;//Update packet time delivered field to now.
                    DataSource.parcels[i] = parcel;
                    return;
                }
            }
        }

       
        public void UpdateParcelData(int id, int droneID)
        {
            if (!DataSource.parcels.Exists(p => p.ParcelID == id))
            {
                throw new BadParcelIDException(id, "the parcel not exists in the system");
            }
            else
            {
                int pIndex = DataSource.parcels.FindIndex(p => p.ParcelID == id);
                Parcel parcel = DataSource.parcels[pIndex];
                parcel.DroneID = droneID;
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the parcel not exists in the list of parcels");
            }
            //find the place of the parcel in the array of parcels
            return DataSource.parcels.Find(p => p.ParcelID == parcelId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual parcel
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            return (IEnumerable<Parcel>)DataSource.parcels;
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcel without special dron</returns>
        public IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron(Predicate<Parcel> p)
        {
            //return all the parcels without special drone
            return from parcel in DataSource.parcels
                   where p(parcel)
                   select parcel.Clone();
        }
        #endregion

    }
}