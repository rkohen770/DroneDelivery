using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DO.DataSource.Initialize();
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
            DO.DataSource.stations[DO.DataSource.Config.IndexStation] = s;//Adding the new station to the array
            Station[] result = new Station[DO.DataSource.Config.IndexStation++];//Create a new array of the desired size
            DO.DataSource.stations.CopyTo(result, 0);//Copy the data from the previous array
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
            DO.DataSource.drones[DO.DataSource.Config.IndexDrone] = d; //Adding the new drone to the array
            Drone[] result = new Drone[DO.DataSource.Config.IndexDrone++];//Create a new array of the desired size
            DO.DataSource.drones.CopyTo(result, 0);//Copy the data from the previous array
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
            DO.DataSource.customers[DO.DataSource.Config.Indexcustomer] = c;//Adding the new customer to the array
            Customer[] result = new Customer[DO.DataSource.Config.Indexcustomer++];//Create a new array of the desired size
            DO.DataSource.customers.CopyTo(result, 0);//Copy the data from the previous array
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
        public int AddParcel( int senderId, int targetId, WeightCategories weight, 
            Priorities priority)
        {
            Parcel p = new Parcel
            {
                Id = DO.DataSource.Config.OrdinalNumber++,
                SenderId = senderId,
                TargetId = targetId,
                Weight = weight,
                priority = priority,
                Requested = DateTime.Now
            };
            DO.DataSource.parcels[DO.DataSource.Config.Indexparcel] = p;//Adding the new parcel to the array
            Parcel[] result = new Parcel[DO.DataSource.Config.Indexparcel++];//Create a new array of the desired size
            DO.DataSource.parcels.CopyTo(result, 0);//Copy the data from the previous array
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
            for (int i=0; i<DO.DataSource.Config.IndexDrone; i++)
            {
                if( DO.DataSource.drones[i].Status == DroneStatuses.Available)
                {
                    DO.DataSource.drones[i].Status = DroneStatuses.Shipping;//Update the status field in the drone found
                    int pIndex = Array.FindIndex<Parcel>(DO.DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
                    DO.DataSource.parcels[pIndex].DroneId = DO.DataSource.drones[i].Id;//Update the droneid field in the drone package found
                    DO.DataSource.parcels[pIndex].scheduled = DateTime.Now;//Update packet time association field to now.
                    return;
                }
            }
        }

        /// <summary>
        /// Package collection by drone
        /// </summary>
        /// <param name="parcelId">Package ID for collection</param>
        public void PackagzCollectionByDrone(int parcelId)
        {
            int pIndex = Array.FindIndex<Parcel>(DO.DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
            DO.DataSource.parcels[pIndex].PickedUp = DateTime.Now;//Update packet time pickeup field to now.
        }

        /// <summary>
        /// Delivery package to customer
        /// </summary>
        /// <param name="parcelId">Package ID for delivery</param>
        public void DeliveryPackageToCustomer(int parcelId)
        {
            int pIndex = Array.FindIndex<Parcel>(DO.DataSource.parcels, p => p.Id == parcelId);//Obtain an index for the location where the package ID is located
            DO.DataSource.parcels[pIndex].Delivered = DateTime.Now;//Update packet time delivered field to now.
        }

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void SendingDroneForCharging(int droneId, int stationId)
        {
            int sIndex = Array.FindIndex<Station>(DO.DataSource.stations, s => s.Id == stationId);//We found the place of the station in the array of stations
            DO.DataSource.stations[stationId].ChargeSlots--;//We will update the number of loading locations
            int dIndex = Array.FindIndex<Drone>(DO.DataSource.drones, d => d.Id == droneId);//We found the place of the drone in the array of drones
            DO.DataSource.drones[dIndex].Status=DroneStatuses.Maintenance;//Changing the glider position
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId };//Add a instance of an instance loading entity
            DO.DataSource.droneCharges[DO.DataSource.Config.IndexDroneCharge++] = droneCharge;//Add a load of drones to the array
        }
            
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            int sIndex = Array.FindIndex<Station>(DO.DataSource.stations, s => s.Id == stationId);//We found the place of the station in the array of stations
            DO.DataSource.stations[stationId].ChargeSlots++;//We will update the number of loading locations
            int dIndex = Array.FindIndex<Drone>(DO.DataSource.drones, d => d.Id == droneId);//We found the place of the drone in the array of drones
            DO.DataSource.drones[dIndex].Status = DroneStatuses.Available;//Changing the status drone
            int dcIndex = Array.FindIndex(DO.DataSource.droneCharges, dc => dc.DroneId == droneId && dc.StationId == stationId);
            DO.DataSource.droneCharges[dcIndex] = null;//remove a load of drones to the array

        }

        #endregion
    }
}
