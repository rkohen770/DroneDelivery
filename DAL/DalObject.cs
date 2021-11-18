using IDAL;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        #region Add

      
        /// <summary>
        /// Add a drone to the list of existing drones
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">The drone model</param>
        /// <param name="maxWeight">Weight category (light, medium, heavy)</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight)
        {
            if (DataSource.drones.Exists(drone => drone.Id == id))
            {
                throw new ExistingFigureException("the drone exists allready");
            }
            else
            {
                Drone d = new Drone
                {
                    Id = id,
                    Model = model,
                    MaxWeight = maxWeight
                };
                DataSource.drones.Add(d); //Adding the new drone to the array
            }
        }

        /// <summary>
        /// Absorption of a new customer to the customer list
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">the customer's name</param>
        /// <param name="phone">Customer phone number</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            if (DataSource.customers.Exists(customer => customer.Id == id))
            {
                throw new ExistingFigureException("the customer exists allready");
            }
            else
            {
                Customer c = new Customer
                {
                    Id = id,
                    Name = name,
                    Phone = phone,
                    Longitude = longitude,
                    Lattitude = lattitude
                };
                DataSource.customers.Add(c);//Adding the new customer to the array
            }
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
            if (DataSource.customers.Exists(customer => customer.Id != senderId))
            {
                throw new ExistingFigureException("the sender not exists in the list of customers");
            }
            if (DataSource.customers.Exists(customer => customer.Id != targetId))
            {
                throw new ExistingFigureException("the target not exists in the list of customers");
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
                throw new NoDataExistsException("the percel not exists in the list of parcels");
            }
            if (!DataSource.drones.Exists(drone => drone.Id == droneId))
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
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
                        parcel.scheduled = DateTime.Now;//Update packet time association field to now.
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
                throw new NoDataExistsException("the percel not exists in the list of parcels");
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
                throw new NoDataExistsException("the percel not exists in the list of parcels");
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

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void SendingDroneForCharging(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(drone => drone.Id == droneId))
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
            }
            if (!DataSource.stations.Exists(station => station.Id == stationId))
            {
                throw new NoDataExistsException("the base station not exists in the list of station");
            }
            for (int sIndex = 0; sIndex < DataSource.stations.Count; sIndex++)
            {
                if (DataSource.stations[sIndex].Id == stationId)//We found the place of the station in the array of stations
                {
                    Station station = DataSource.stations[sIndex];
                    station.ChargeSlots--;//We will update the number of loading locations
                    DataSource.stations[sIndex] = station;
                    break;
                }
            }
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId };//Add a instance of an instance loading entity
            DataSource.droneCharges.Add(droneCharge);//Add a load of drones to the array
        }

        /// <summary>
        /// Release the UAV from a charge at the base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(drone => drone.Id == droneId))
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
            }
            if (!DataSource.stations.Exists(station => station.Id == stationId))
            {
                throw new NoDataExistsException("the base station not exists in the list of station");
            }
            for (int sIndex = 0; sIndex < DataSource.stations.Count; sIndex++)
            {
                if (DataSource.stations[sIndex].Id == stationId)//We found the place of the station in the array of stations
                {
                    Station station = DataSource.stations[sIndex];
                    station.ChargeSlots++;//We will update the number of loading locations
                    DataSource.stations[sIndex] = station;
                    break;
                }
            }
            for (int dCIndex = 0; dCIndex < DataSource.stations.Count; dCIndex++)
            {
                if (DataSource.droneCharges[dCIndex].StationId == stationId && DataSource.droneCharges[dCIndex].DroneId == droneId)//We found the place of the station in the array of stations
                {
                    DataSource.droneCharges.RemoveAt(dCIndex);//remove a load of drones to the array
                    break;
                }
            }
        }

        #endregion

        #region Get item


        /// <summary>
        /// return base station by station ID.
        /// </summary>
        /// <param name="stationId">station ID</param>
        /// <returns>statoin</returns>
        public Station BaseStationView(int stationId)
        {
            if (!DataSource.stations.Exists(station => station.Id == stationId))
            {
                throw new NoDataExistsException("the base station not exists in the list of station");
            }
            //find the station in the array of stations and return it.
            return DataSource.stations.Find(s => s.Id == stationId);
        }

        /// <summary>
        /// return drone by drone ID to print
        /// </summary>
        /// <param name="droneId">drone ID to print</param>
        /// <returns>drone to show</returns>
        public Drone DroneView(int droneId)
        {
            if (!DataSource.drones.Exists(drone => drone.Id == droneId))
            {
                throw new NoDataExistsException("the drone not exists in the list of drones");
            }
            //find the place of the drone in the array of drones
            return DataSource.drones.Find(d => d.Id == droneId);
        }

        /// <summary>
        /// return customer by customer ID to print.
        /// </summary>
        /// <param name="customerId">customer ID to print</param>
        /// <returns>customer to show</returns>
        public Customer CustomerView(int customerId)
        {
            if (!DataSource.customers.Exists(customer => customer.Id == customerId))
            {
                throw new NoDataExistsException("the customer not exists in the list of customers");
            }
            //find the place of the customer in the array of customers
            return DataSource.customers.Find(c => c.Id == customerId);
        }

        /// <summary>
        /// return parcel by parcel ID to print.
        /// </summary>
        /// <param name="parcelId">parcel ID to print</param>
        /// <returns>parcel to show</returns>
        public Parcel ParcelView(int parcelId)
        {
            if (!DataSource.parcels.Exists(parcel => parcel.Id == parcelId))
            {
                throw new NoDataExistsException("the parcel not exists in the list of parcels");
            }
            //find the place of the parcel in the array of parcels
            return DataSource.parcels.Find(p => p.Id == parcelId);
        }

        #endregion

        #region Get lists

        /// <summary>
        /// return a list of actual base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public IEnumerable<Station> GetAllBaseStations()
        {
            return from station in DataSource.stations
                   select station.Clone();
        }

        /// <summary>
        /// return a list of actual drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            return from drone in DataSource.drones
                   select drone.Clone();
        }

        /// <summary>
        /// return a list of actual custpmer
        /// </summary>
        /// <returns>list of castomers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return from customer in DataSource.customers
                   select customer.Clone();
        }

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

        /// <summary>
        /// return base stations with available charging stations
        /// </summary>
        /// <returns>list of station with availible charge station</returns>
        public IEnumerable<Station> GetAllStationsWithAvailableChargingStations()
        {
            return from station in DataSource.stations
                   where station.ChargeSlots > 0
                   select station.Clone();
        }

        #endregion

        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns>An array of the amount of power consumption of a drone for each situation</returns>
        public double[] PowerConsumptionRequest()
        {
            double[] result = {DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
                DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,
                DataSource.Config.DroneChargingRate };
            return result;
        }
    }
}
