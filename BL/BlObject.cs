using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
using IDAL;
using IDAL.DO;
namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        Random random = new Random();
        IDal dal = new DalObject.DalObject();
        //public List<DroneForList> droneForLists=new List<DroneForList>();
        public BlObject() { }

        #region ADD

        /// <summary>
        /// Add base station
        /// </summary>
        /// <param name="id">Station number</param>
        /// <param name="nameBaseStation">Name base station</param>
        /// <param name="location">Location of the bus stop</param>
        /// <param name="numOfAvailableChargingPositions">Number of charging stations available</param>
        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions)
        {
            //add baseStation fields in BL.
            BaseStation baseStation = new BaseStation()
            {
                Id = id,
                NameBaseStation = nameBaseStation,
                Location = location,
                NumOfAvailableChargingPositions = numOfAvailableChargingPositions
            };
            //Add baseStation in DAL to data source.
            dal.AddStation(id, nameBaseStation, location.Longitude, location.Latitude, numOfAvailableChargingPositions);
        }

        /// <summary>
        /// Add drone
        /// </summary>
        /// <param name="droneId">Manufacturer's serial number</param>
        /// <param name="model">Drone model</param>
        /// <param name="maxWeight">Maximum weight</param>
        /// <param name="stationId">Number of stations to put the drone for initial charging</param>
        public void AddDroneBo(int droneId, string model, IBL.BO.WeightCategories maxWeight, int stationId)
        {
            //add drone fields in BL.
            Station s = dal.BaseStationView(stationId);
            IBL.BO.Drone drone = new IBL.BO.Drone()
            {
                Id = droneId,
                Model = model,
                Weight = maxWeight,
                Battery = random.Next(20, 41),
                DroneStatus = DroneStatus.Maintenance,
                CurrentLocation = new Location() { Latitude = s.Lattitude, Longitude = s.Longitude }
            };
            dal.SendingDroneForCharging(droneId, stationId);
            //להוסיף את הרחפן לרשימה של רחפנים בטעינה של תחנת בסיס

            //Add drone in DAL to data source.
            dal.AddDrone(droneId, model, (IDAL.DO.WeightCategories)maxWeight);
        }

        /// <summary>
        /// Absorption of a new customer
        /// </summary>
        /// <param name="id">Customer ID number</param>
        /// <param name="name">The customer's name</param>
        /// <param name="phone">Phone Number</param>
        /// <param name="location">Customer location</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            //add customer fields in BL.
            IBL.BO.Customer customer = new IBL.BO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = new Location() { Longitude = longitude, Latitude = lattitude }
            };
           
            //Add customer in DAL to data source.
            dal.AddCustomer(id, name, phone, longitude, lattitude);
        }

        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public void AddParcel(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
        {
            //Add parcel in DAL to data source and get the parcel id that was created.
            int parcelId = dal.AddParcel(senderId, targetId, (IDAL.DO.WeightCategories)weight, (IDAL.DO.Priorities)priority);

            //Find a customer sending
            IDAL.DO.Customer customerS = dal.CustomerView(senderId);
            CustomerInParcel sender = new CustomerInParcel() { Id = customerS.Id, Name = customerS.Name };
            //Find a receiving customer
            IDAL.DO.Customer customerG = dal.CustomerView(targetId);
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
                ParcelDelivery= DateTime.MinValue
            };
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
            
        }

        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {

        }

        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="newName">New name</param>
        /// <param name="newPhone">New phone</param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {

        }

        /// <summary>
        /// Sending a skimmer for charging
        /// </summary>
        /// <param name="id">Id drone</param>
        public void UpdateSendingDroneForCharging(int id)
        {

        }

        /// <summary>
        /// Release skimmer from charging
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateReleaseDroneFromCharging(int id, DateTime chargingtime)
        {

        }
        #endregion
    }
}
