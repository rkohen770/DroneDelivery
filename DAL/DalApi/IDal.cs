using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region ADD
        /// <summary>
        /// Add station
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">Model of station</param>
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
        void AssigningParcelToDrone(int parcelId, int droneId);

        /// <summary>
        /// Parcel collection by drone
        /// </summary>
        /// <param name="parcelId">Parcel id</param>
        void PackagCollectionByDrone(int parcelId);

        /// <summary>
        /// Delivery parcel to customer
        /// </summary>
        /// <param name="parcelId">parcel id</param>
        void DeliveryPackageToCustomer(int parcelId);

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone id</param>
        /// <param name="stationId">station id</param>
        int SendingDroneForCharging(int droneId, int stationId);

        /// <summary>
        /// Release drone from charging at base station
        /// </summary>
        /// <param name="droneId">Drone id</param>
        /// <param name="stationId">Station id</param>
        void ReleasDroneFromCharging(int droneId, int stationId);

        /// <summary>
        /// Update Drone Modle at a base station
        /// </summary>
        /// <param name="droneId">drone id to update</param>
        /// <param name="model">new model</param>
        void UpdateDroneModle(int droneId, string model);

        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations);

        /// <summary>
        /// Update Base Station Model
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name</param>
        void UpdateBaseStationName(int id, int nameBaseStation);

        /// <summary>
        /// Update base station total number of charge slots
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        void UpdateBaseStationCharging(int id, int totalAmountOfChargingStations);

        /// <summary>
        /// Update Customer Data
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        /// <param name="newPhone">customer phone</param>
        void UpdateCustomerData(int id, string newName, string newPhone);

        /// <summary>
        /// Update Customer name
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        void UpdateCustomerName(int id, string newName);

        /// <summary>
        /// Update Customer phone
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newPhone">customer phone</param>
        void UpdateCustomerPhone(int id, string newPhone);
        /// <summary>
        /// Update assign drone id to parcel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="droneID"></param>
        void UpdateParcelData(int id, int droneID);

        #endregion

        #region GET ITEM
        /// <summary>
        /// Base Station View
        /// </summary>
        /// <param name="stationId">ID station</param>
        /// <returns>Base station to show</returns>
        Station GetBaseStation(int stationId);

        /// <summary>
        /// A function that returns a minimum distance between a point and a base station
        /// </summary>
        /// <param name="senderLattitude">Lattitude of sender</param>
        /// <param name="senderLongitude">longitude of sender</param>
        /// <param name="flag">Optional field for selecting a nearby base station flag = false or available nearby base station flag = true</param>
        /// <returns>Base station closest to the point</returns>
        Station GetClosestStation(double senderLattitude, double senderLongitude, bool flag = false);

        /// <summary>
        /// A function that calculates the distance between two points on the map
        /// </summary>
        /// <param name="senderId">sender ID</param>
        /// <param name="targetId">target ID</param>
        /// <returns>Returns a distance between two points</returns>
        double GetDistanceBetweenLocationsOfParcels(int senderId, int targetId);

        /// <summary>
        /// A function that calculates the distance between a customer's location and a base station for charging
        /// </summary>
        /// <param name="targetId">target ID</param>
        /// <returns>Minimum distance to the nearest base station</returns>
        double GetDistanceBetweenLocationAndClosestBaseStation(int targetId);

        /// <summary>
        /// Drone display
        /// </summary>
        /// <param name="droneId">ID drone</param>
        /// <returns>Dron to show</returns>
        Drone GetDrone(int droneId);

        /// <summary>
        /// Customer view
        /// </summary>
        /// <param name="customerId">ID customer</param>
        /// <returns>Customer to show</returns>
        Customer GetCustomer(int customerId);

        /// <summary>
        /// Parcel view
        /// </summary>
        /// <param name="parcelId">ID parcel</param>
        /// <returns>parcel to show</returns>
        Parcel GetParcel(int parcelId);
        #endregion

        #region GET LIST
        /// <summary>
        /// Displays a list of base stations
        /// </summary>
        /// <returns>List of base stations</returns>
        IEnumerable<Station> GetAllBaseStations();

        /// <summary>
        /// Displays a list of drone
        /// </summary>
        /// <returns>List of drones</returns>
        IEnumerable<Drone> GetAllDrones();

        /// <summary>
        /// Displays a list of customer
        /// </summary>
        /// <returns>List of customers</returns>
        IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// Displays a list of parcel
        /// </summary>
        /// <returns>List of parcels</returns>
        IEnumerable<Parcel> GetAllParcels();

        /// <summary>
        /// Displays a list of parcel that have not yet been assigned to the glider
        /// </summary>
        /// <returns>List of parcel that have not yet been assigned to the glider</returns>
        IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron(Predicate<Parcel> p);

        /// <summary>
        /// Display of base stations with available charging stations
        /// </summary>
        /// <returns>List of base stations with available charging stations</returns>
        IEnumerable<Station> GetAllStationsBy(Predicate<Station> p);

        /// <summary>
        /// List of drones loaded at a specific station
        /// </summary>
        /// <param name="stationId">base station ID</param>
        /// <returns>List of drones loaded at a specific station</returns>
        IEnumerable<int> GetDronesInChargingsAtStation(int stationId, Predicate<DroneCharge> p);

        IEnumerable<Drone> GetDronesByPredicat(Predicate<Drone> p);

        IEnumerable<Customer> GetAllCustomerByPredicate(Predicate<Customer> p);

        #endregion

        #region User
        /// <summary>
        ///  adds a new DO.user instance
        /// </summary>
        /// <param name="user"></param>
        void AddUser(DO.User user);
        /// <summary>
        /// updates an exist DO.user instance
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(DO.User user);
        /// <summary>
        /// deletes an exist DO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        void DeleteUser(string userName);
        /// <summary>
        ///  returns an exist DO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DO.User GetUser(string userName, string password = null);
        #endregion
        /// <summary>
        /// A method of requesting power consumption by a drone  
        /// </summary>
        /// <returns>array of numbers of double type</returns>
       double[] PowerConsumptionRequest();

       
    }
}
