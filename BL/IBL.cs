using System;
using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BLApi
{
    public interface IBL
    {
        #region ADD
        /// <summary>
        /// Add base station
        /// </summary>
        /// <param name="id">Station number</param>
        /// <param name="nameBaseStation">Model base station</param>
        /// <param name="location">Location of the bus stop</param>
        /// <param name="numOfAvailableChargingPositions">Number of charging stations available</param>
        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions);

        /// <summary>
        /// Add drone
        /// </summary>
        /// <param name="droneId">Manufacturer's serial number</param>
        /// <param name="model">Drone model</param>
        /// <param name="maxWeight">Maximum weight</param>
        /// <param name="stationId">Number of stations to put the drone for initial charging</param>
        public void AddDroneBo(int droneId, string model, BO.WeightCategories maxWeight, int stationId);

        /// <summary>
        /// Absorption of a new customer
        /// </summary>
        /// <param name="id">Customer ID number</param>
        /// <param name="name">The customer's name</param>
        /// <param name="phone">Phone Number</param>
        /// <param name="location">Customer location</param>
        public void AddCustomerBo(int id, string name, string phone, Location location);

        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public int AddParcelBo(int senderId, int targetId, BO.WeightCategories weight, BO.Priorities priority);
        #endregion

        #region UPDATE
        /// <summary>
        /// Update the drone data that will allow you to update the drone name only
        /// </summary>
        /// <param name="id">Drone id</param>
        /// <param name="model">Drone model</param>
        public void UpdateNameOfDrone(int id, string model);

        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations);

        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="newName">New name</param>
        /// <param name="newPhone">New phone</param>
        public void UpdateCustomerData(int id, string newName, string newPhone);

        /// <summary>
        /// Sending a drone for charging
        /// </summary>
        /// <param name="id">ID drone</param>
        public int SendingDroneForCharging(int id);

        /// <summary>
        /// Release drone from charging
        /// </summary>
        /// <param name="id">ID drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateReleaseDroneFromCharging(int id, TimeSpan chargingTime);

        /// <summary>
        /// Assign a package to a drone
        /// </summary>
        /// <param name="id">ID drone</param>
        public void UpdateAssignParcelToDrone(int id);

        /// <summary>
        /// Collection of a package by drone
        /// </summary>
        /// <param name="id">ID drone</param>
        public void UpdateCollectionParcelByDrone(int id);


        /// <summary>
        /// Delivery of a package by drone
        /// </summary>
        /// <param name="id">ID drone</param>
        public void UpdateDeliveryParcelByDrone(int id);

        #endregion

        #region GET ITEM
        /// <summary>
        /// Base station view
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns>Base station show</returns>
        public BaseStation GetBaseStation(int baseStationId);

        /// <summary>
        /// Drone view
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>Drone show</returns>
        public BO.Drone GetDrone(int droneId);

        /// <summary>
        /// Customer view
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Customer show</returns>
        public BO.Customer GetCustomer(int customerId);

        /// <summary>
        /// Parcel view
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>Parcel show</returns>
        public BO.Parcel GetParcel(int parcelId);
        #endregion

        #region GET LISTS
        /// <summary>
        /// Displays a list of base stations
        /// </summary>
        /// <returns>List of base stations</returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationsBo();

        /// <summary>
        /// Displays a list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<DroneForList> GetAllDronesBo();

        /// <summary>
        /// Displays a list of customers
        /// </summary>
        /// <returns>List of customers</returns>
        public IEnumerable<CustomerForList> GetAllCustomersBo();

        /// <summary>
        /// Displays a list of parcels
        /// </summary>
        /// <returns>List of parcels</returns>
        public IEnumerable<ParcelForList> GetAllParcelsBo();

        /// <summary>
        /// Get all parcels not yet associated with the drone
        /// </summary>
        /// <returns>List with all parcels not yet associated with the glider</returns>
        IEnumerable<ParcelForList> GetAllParcelsNotYetAssociatedWithDrone();

        /// <summary>
        /// Getlist with all base station with availible charging
        /// </summary>
        /// <returns>List with all base station with availible charging</returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationWhithAvailibleCharging(Predicate<Station> p);

        public IEnumerable<CustomerForList> GetAllCustomerByPredicate(Predicate<DO.Customer> p);

        public IEnumerable<DroneForList> GetDronesByPredicat(Predicate<DroneForList> p);


        #endregion

    }


}