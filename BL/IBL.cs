﻿using System;
using IBL.BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {
        #region ADD
        /// <summary>
        /// Add base station
        /// </summary>
        /// <param name="id">Station number</param>
        /// <param name="nameBaseStation">Name base station</param>
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
        public void AddDroneBo(int droneId, string model, WeightCategories maxWeight, int stationId);

        /// <summary>
        /// Absorption of a new customer
        /// </summary>
        /// <param name="id">Customer ID number</param>
        /// <param name="name">The customer's name</param>
        /// <param name="phone">Phone Number</param>
        /// <param name="location">Customer location</param>
        public void AddCustomer(int id, string name, string phone, Location location);

        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public void AddParcel(int senderId, int targetId, WeightCategories weight, Priorities priority);
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
        /// Sending a skimmer for charging
        /// </summary>
        /// <param name="id">Id drone</param>
        public void UpdateSendingDroneForCharging(int id);

        /// <summary>
        /// Release skimmer from charging
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateReleaseDroneFromCharging(int id, DateTime chargingtime);
        #endregion

    }


}
