using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
    {
        #region Parcel

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
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var castomer = (from item in ListCustomer
                            where item.CustomerID == senderId
                            select item).FirstOrDefault();
            if (castomer.CustomerID == 0 || !castomer.Available)
                throw new BadCustomerIDException(senderId, "The sender not exists in the file of customers");

            castomer = (from item in ListCustomer
                        where item.CustomerID == targetId
                        select item).FirstOrDefault();
            if (castomer.CustomerID == 0 || !castomer.Available)
                throw new BadCustomerIDException(targetId, "The target not exists in the file of customers");

            else
            {
                List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
                XElement dalConfigRoot = XMLTools.LoadListFromXMLElement(ConfigPath);
                Parcel p = new()
                {

                    ParcelID = Convert.ToInt32(dalConfigRoot.Element("SerialNum").Value),
                    SenderID = senderId,
                    TargetID = targetId,
                    Weight = weight,
                    Priority = priority,
                    Requested = DateTime.Now,
                    DroneID = droneId,
                    Available=true,
                };
                ListParcel.Add(p);
                dalConfigRoot.Element("SerialNum").Value = (p.ParcelID+1).ToString();
                XMLTools.SaveListToXMLElement(dalConfigRoot, ConfigPath);
                XMLTools.SaveListToXMLSerializer(ListParcel, ParcelsPath);//Adding the new parcel to the file
                return p.ParcelID;
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
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            var parcel = (from item in ListParcel
                          where item.ParcelID == parcelId && item.Available
                          select item).FirstOrDefault();
            if (parcel.ParcelID == 0)
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();
            if (drone.DroneID == 0)
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");

            Parcel p = parcel;
            p.DroneID = droneId;//Update the droneid field in the drone package found
            p.Scheduled = DateTime.Now;//Update packet time association field to now.
            ListParcel.Remove(parcel);
            ListParcel.Add(p);
            XMLTools.SaveListToXMLSerializer(ListParcel, ParcelsPath);

        }

        /// <summary>
        /// Package collection by drone
        /// </summary>
        /// <param name="parcelId">Package ID for collection</param>
        public void PackagCollectionByDrone(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            var parcel = (from item in ListParcel
                          where item.ParcelID == parcelId && item.Available
                          select item).FirstOrDefault();
            if (parcel.ParcelID == 0)
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            Parcel p = parcel;
            parcel.PickedUp = DateTime.Now;//Update packet time pickeup field to now.
            ListParcel.Remove(parcel);
            ListParcel.Add(p);
            XMLTools.SaveListToXMLSerializer(ListParcel, ParcelsPath);
        }


        /// <summary>
        /// Delivery package to customer
        /// </summary>
        /// <param name="parcelId">Package ID for delivery</param>
        public void DeliveryPackageToCustomer(int parcelId)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            var parcel = (from item in ListParcel
                          where item.ParcelID == parcelId && item.Available
                          select item).FirstOrDefault();
            if (parcel.ParcelID == 0)
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            Parcel p = parcel;
            parcel.Delivered = DateTime.Now;//Update packet time delivered field to now.
            ListParcel.Remove(parcel);
            ListParcel.Add(p);
            XMLTools.SaveListToXMLSerializer(ListParcel, ParcelsPath);

        }
        public void UpdateParcelData(int id, int droneID)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            var parcel = (from item in ListParcel
                          where item.ParcelID == id && item.Available
                          select item).FirstOrDefault();
            if (parcel.ParcelID == 0)
                throw new BadParcelIDException(id, "the percel not exists in the list of parcels");
            Parcel p = parcel;
            parcel.DroneID = droneID;
            ListParcel.Remove(parcel);
            ListParcel.Add(p);
            XMLTools.SaveListToXMLSerializer(ListParcel, ParcelsPath);

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
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            var parcel = (from item in ListParcel
                          where item.ParcelID == parcelId && item.Available
                          select item).FirstOrDefault();
            if (parcel.ParcelID == 0)
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");

            //find the place of the parcel in the array of parcels
            return parcel;
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual parcel
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            return from item in ListParcel
                   where item.Available
                   select item;
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcel by predicate</returns>
        public IEnumerable<Parcel> GetAllParcelsByPredicat(Predicate<Parcel> p)
        {
            List<Parcel> ListParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            //return all the parcels by predicate
            return from parcel in ListParcel
                   where p(parcel) && parcel.Available
                   select parcel;
        }
        #endregion

        #endregion
    }
}
