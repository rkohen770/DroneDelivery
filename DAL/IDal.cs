using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        #region ADD
        /// <summary>
        /// Add station
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">Name of station</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="lattitude">Lattitude</param>
        /// <param name="chargeSlots">Several claim positions are vacant</param>
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots);

        /// <summary>
        /// Add drone
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">Drone model</param>
        /// <param name="maxWeight">Maximum weight</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight);

        /// <summary>
        /// Absorption of a new customer to the customer list
        /// </summary>
        /// <param name="id">Customer ID number</param>
        /// <param name="name">The customer's name</param>
        /// <param name="phone">Phone Number</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="lattitude">Lattitude</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude);

        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        /// <param name="droneId">Drone id</param>
        /// <returns></returns>
        public int AddParcel(int senderId, int targetId, WeightCategories weight, Priorities priority, int droneId = 0);
        #endregion

        #region UPDATE
        /// <summary>
        /// Assign a parcel to a drone
        /// </summary>
        /// <param name="parcelId">Parcel id</param>
        /// <param name="droneId">Drone id</param>
        public void AssigningParcelToDrone(int parcelId, int droneId);

        /// <summary>
        /// Parcel collection by drone
        /// </summary>
        /// <param name="parcelId">Parcel id</param>
        public void PackagCollectionByDrone(int parcelId);

        /// <summary>
        /// Delivery parcel to customer
        /// </summary>
        /// <param name="parcelId">parcel id</param>
        public void DeliveryPackageToCustomer(int parcelId);

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone id</param>
        /// <param name="stationId">station id</param>
        public void SendingDroneForCharging(int droneId, int stationId);

        /// <summary>
        /// Release drone from charging at base station
        /// </summary>
        /// <param name="droneId">Drone id</param>
        /// <param name="stationId">Station id</param>
        public void ReleasDroneFromCharging(int droneId, int stationId);

        /// <summary>
        /// Update Drone Modle at a base station
        /// </summary>
        /// <param name="droneId">drone id to update</param>
        /// <param name="model">new model</param>
        public void UpdateDroneModle(int droneId, string model);
        #endregion

        #region GET ITEM
        /// <summary>
        /// Base Station View
        /// </summary>
        /// <param name="stationId">Id station</param>
        /// <returns>Base station to show</returns>
        public Station BaseStationView(int stationId);

        /// <summary>
        /// Drone display
        /// </summary>
        /// <param name="droneId">Id drone</param>
        /// <returns>Dron to show</returns>
        public Drone DroneView(int droneId);

        /// <summary>
        /// Customer view
        /// </summary>
        /// <param name="customerId">Id customer</param>
        /// <returns>Customer to show</returns>
        public Customer CustomerView(int customerId);

        /// <summary>
        /// Parcel view
        /// </summary>
        /// <param name="parcelId">Id parcel</param>
        /// <returns>parcel to show</returns>
        public Parcel ParcelView(int parcelId);
        #endregion

        #region GET LIST
        /// <summary>
        /// Displays a list of base stations
        /// </summary>
        /// <returns>List of base stations</returns>
        public IEnumerable<Station> GetAllBaseStations();

        /// <summary>
        /// Displays a list of drone
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<Drone> GetAllDrones();

        /// <summary>
        /// Displays a list of customer
        /// </summary>
        /// <returns>List of customers</returns>
        public IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// Displays a list of parcel
        /// </summary>
        /// <returns>List of parcels</returns>
        public IEnumerable<Parcel> GetAllParcels();

        /// <summary>
        /// Displays a list of parcel that have not yet been assigned to the glider
        /// </summary>
        /// <returns>List of parcel that have not yet been assigned to the glider</returns>
        public IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron();

        /// <summary>
        /// Display of base stations with available charging stations
        /// </summary>
        /// <returns>List of base stations with available charging stations</returns>
        public IEnumerable<Station> GetAllStationsWithAvailableChargingStations();
        #endregion


        /// <summary>
        /// A method of requesting power consumption by a drone  
        /// </summary>
        /// <returns>array of numbers of double type</returns>
        public double[] PowerConsumptionRequest();
    }
}
