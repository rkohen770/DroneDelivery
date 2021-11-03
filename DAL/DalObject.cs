using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        #region Add

        /// <summary>
        /// Add a base station to the list of stations
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The station name</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <param name="chargeSlots">Several arguments</param>
        /// <returns>An array that contains all the stations</returns>
        public Station[] AddStation(int id, int name, double longitude, double lattitude, int chargeSlots)
        {
            Station s = new Station
            {
                Id = id,
                Name = name,
                Longitude = longitude,
                Lattitude = lattitude,
                ChargeSlots = chargeSlots
            };
            DataSource.Stations[DataSource.Config.IndexStation] = s;//Adding the new station to the array
            Station[] result = new Station[DataSource.Config.IndexStation++];//Create a new array of the desired size
            result.CopyTo(DataSource.Stations, 0);//Copy the data from the previous array
            return result;
        }

        /// <summary>
        /// Add a drone to the list of existing drones
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">The drone model</param>
        /// <param name="maxWeight">Weight category (light, medium, heavy)</param>
        /// <param name="status">Drone condition (Available, maintenance, delivery)</param>
        /// <param name="battery">Battery status (charge level)</param>
        /// <returns>An array that contains all the dorones</returns>
        public Drone[] AddDrone(int id, string model, WeightCategories maxWeight, DroneStatuses status, double battery)
        {
            Drone d = new Drone
            {
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                Status = status,
                Battery = battery
            };
            DataSource.drones[DataSource.Config.IndexDrone] = d; //Adding the new drone to the array
            Drone[] result = new Drone[DataSource.Config.IndexDrone++];//Create a new array of the desired size
            DataSource.drones.CopyTo(result, 0);//Copy the data from the previous array
            return result;
        }

        /// <summary>
        /// Absorption of a new customer to the customer list
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">the customer's name</param>
        /// <param name="phone">Customer phone number</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <returns>An array that contains all the customer</returns>
        public Customer[] AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            Customer c = new Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = longitude,
                Lattitude = lattitude
            };
            DataSource.customers[DataSource.Config.IndexCustomer] = c;//Adding the new customer to the array
            Customer[] result = new Customer[DataSource.Config.IndexCustomer++];//Create a new array of the desired size
            DataSource.customers.CopyTo(result, 0);//Copy the data from the previous array
            return result;
        }

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
            DataSource.parcels[DataSource.Config.IndexParcel] = p;//Adding the new parcel to the array
            Parcel[] result = new Parcel[DataSource.Config.IndexParcel++];//Create a new array of the desired size
            DataSource.parcels.CopyTo(result, 0);//Copy the data from the previous array
            return p.Id;
        }

        #endregion

        #region Uppdate

        /// <summary>
        /// Assigning a package to a drone
        /// </summary>
        /// <param name="parcelId">Package ID for association</param>
        public void AssigningParcelToDrone(int parcelId)
        {
            //We will go through the entire array of the drone, to find a available drone
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                if (DataSource.drones[i].Status == DroneStatuses.Available)
                {
                    DataSource.drones[i].Status = DroneStatuses.Shipping;//Update the status field in the drone found
                    int pIndex = Array.FindIndex<Parcel>(DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
                    DataSource.parcels[pIndex].DroneId = DataSource.drones[i].Id;//Update the droneid field in the drone package found
                    DataSource.parcels[pIndex].scheduled = DateTime.Now;//Update packet time association field to now.
                    return;
                }
            }
        }

        /// <summary>
        /// Package collection by drone
        /// </summary>
        /// <param name="parcelId">Package ID for collection</param>
        public void PackagCollectionByDrone(int parcelId)
        {
            int pIndex = Array.FindIndex<Parcel>(DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
            DataSource.parcels[pIndex].PickedUp = DateTime.Now;//Update packet time pickeup field to now.
        }

        /// <summary>
        /// Delivery package to customer
        /// </summary>
        /// <param name="parcelId">Package ID for delivery</param>
        public void DeliveryPackageToCustomer(int parcelId)
        {
            int pIndex = Array.FindIndex<Parcel>(DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
            DataSource.parcels[pIndex].Delivered = DateTime.Now;//Update packet time delivered field to now.
        }

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void SendingDroneForCharging(int droneId, int stationId)
        {
            int sIndex = Array.FindIndex<Station>(DataSource.Stations, s => s.Id == stationId);//We found the place of the station in the array of stations
            DataSource.Stations[stationId].ChargeSlots--;//We will update the number of loading locations
            int dIndex = Array.FindIndex<Drone>(DataSource.drones, d => d.Id == droneId);//We found the place of the drone in the array of drones
            DataSource.drones[dIndex].Status = DroneStatuses.Maintenance;//Changing the glider position
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId };//Add a instance of an instance loading entity
            DataSource.droneCharges[DataSource.Config.IndexDroneCharge++] = droneCharge;//Add a load of drones to the array
        }

        /// <summary>
        /// Release the UAV from a charge at the base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            int sIndex = Array.FindIndex<Station>(DataSource.Stations, s => s.Id == stationId);//We found the place of the station in the array of stations
            DataSource.Stations[stationId].ChargeSlots++;//We will update the number of loading locations
            int dIndex = Array.FindIndex<Drone>(DataSource.drones, d => d.Id == droneId);//We found the place of the drone in the array of drones
            DataSource.drones[dIndex].Status = DroneStatuses.Available;//Changing the status drone
            int dcIndex = Array.FindIndex(DataSource.droneCharges, dc => dc.DroneId == droneId && dc.StationId == stationId);
            //remove a load of drones to the array
            DataSource.droneCharges[dcIndex].DroneId = 0;
            DataSource.droneCharges[dcIndex].StationId = 0;
        }

        #endregion

        #region view item


        /// <summary>
        /// return base station by station ID to print.
        /// </summary>
        /// <param name="stationId">station ID to print</param>
        /// <returns>statoin to show</returns>
        public Station BaseStationView(int stationId)
        {
            //find the station in the array of stations and return it.
            return Array.Find(DataSource.Stations, s => s.Id == stationId);
        }

        /// <summary>
        /// return drone by drone ID to print
        /// </summary>
        /// <param name="droneId">drone ID to print</param>
        /// <returns>drone to show</returns>
        public Drone DroneView(int droneId)
        {
            //find the place of the drone in the array of drones
            return Array.Find(DataSource.drones, d => d.Id == droneId);
        }

        /// <summary>
        /// return customer by customer ID to print.
        /// </summary>
        /// <param name="customerId">customer ID to print</param>
        /// <returns>customer to show</returns>
        public Customer CustomerView(int customerId)
        {
            //find the place of the customer in the array of customers
            return Array.Find(DataSource.customers, c => c.Id == customerId);
        }

        /// <summary>
        /// return parcel by parcel ID to print.
        /// </summary>
        /// <param name="parcelId">parcel ID to print</param>
        /// <returns>parcel to show</returns>
        public Parcel ParcelView(int parcelId)
        {
            //find the place of the parcel in the array of parcels
            return Array.Find(DataSource.parcels, p => p.Id == parcelId);
        }

        #endregion

        #region Get lists

        /// <summary>
        /// return a list of actual base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public Station[] GetAllBaseStations()
        {
            Station[] stations = new Station[DataSource.Config.IndexStation];
            Array.Copy(DataSource.Stations, stations, DataSource.Config.IndexStation);
            return stations;
        }

        /// <summary>
        /// return a list of drones to print
        /// </summary>
        /// <returns>list of drone to show</returns>
        public Drone[] ListOfDroneView()
        {
            //return all the list of drones
            return DataSource.drones;
        }

        /// <summary>
        /// return a list of custpmer to print
        /// </summary>
        /// <returns>list of castomer to show</returns>
        public Customer[] ListOfCustomerView()
        {
            //return all the list of drones
            return DataSource.customers;
        }

        /// <summary>
        /// return a list of parcel to print
        /// </summary>
        /// <returns>list of parcel to show</returns>
        public Parcel[] ListOfParcelView()
        {
            //return all the list of drones
            return DataSource.parcels;
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcel without special dron</returns>
        public Parcel[] ListOfParcelWithoutSpecialDron()
        {
            //return all the parcels without special dron
            return Array.FindAll(DataSource.parcels, p => p.DroneId == 0);
        }

        /// <summary>
        /// View base stations with available charging stations
        /// </summary>
        /// <returns>list of station with availible charge station to print</returns>
        public Station[] ListOfStationsWithAvailableChargingStations()
        {
            return Array.FindAll(DataSource.Stations, s => s.ChargeSlots > 0);
        }

        #endregion

    }
}
