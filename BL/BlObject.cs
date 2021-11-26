using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using IDAL.DO;
namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        Random random = new Random();
        IDal dal;
        List<DroneForList> droneForLists = new List<DroneForList>();
        public BlObject()
        {
            dal = new DalObject.DalObject();
            double[] power = dal.PowerConsumptionRequest();
            double vacant = power[0],
                CarriesLightWeight = power[1],
                CarriesMediumWeight = power[2],
                CarriesHeavyWeight = power[3],
                DroneChargingRate = power[4];
            List<IDAL.DO.Drone> drones = dal.GetAllDrones().ToList();

            initializeListOfDrone(drones);

        }
        /// <summary>
        /// List boot function of "drones to list "
        /// </summary>
        /// <param name="drones">list of DO of drones</param>
        private void initializeListOfDrone(List<IDAL.DO.Drone> drones)
        {
            //find A package that has not yet been delivered but the drone has already been associated
            List<IDAL.DO.Parcel> parcelsList = dal.GetAllParcels().ToList();
            DroneForList drone_BL;

            foreach (var drone_DL in drones)
            {
                drone_BL = new DroneForList()
                {
                    Id = drone_DL.Id,
                    Model = drone_DL.Model,
                    Weight = (IBL.BO.WeightCategories)drone_DL.MaxWeight
                };
                List<IDAL.DO.Parcel> parcels = parcelsList.FindAll(p => p.DroneId == drone_BL.Id);
                drone_BL.ParcelNumIsTransferred = parcels.Count();

                if (parcels != null) //If there is a package that has not yet been delivered but the drone has already been associated
                {
                    drone_BL.DroneStatus = DroneStatus.Delivery;
                    //If the package was associated but not collected
                    foreach (var p in parcels.Where(p => p.PickedUp == DateTime.MinValue))
                    {
                        // The location of the drone will be at the station closest to the sender
                        int senderId = p.SenderId;
                        double senderLattitude = dal.CustomerView(senderId).Lattitude;
                        double senderLongitude = dal.CustomerView(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Latitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        drone_BL.Battery = random.Next(0, 101);
                    }
                    //If the package has been collected but has not yet been delivered
                    foreach (var p in parcels.Where(p => p.PickedUp != DateTime.MinValue && p.Delivered == DateTime.MinValue))
                    {
                        //The location of the drone will be at the location of the sender
                        int senderId = p.SenderId;
                        double senderLattitude = dal.CustomerView(senderId).Lattitude;
                        double senderLongitude = dal.CustomerView(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Latitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        double distance = dal.GetDistanceBetweenLocations(p.SenderId, p.TargetId)
                            + dal.GetDistanceBetweenLocationAndClosestBaseStation(p.TargetId);
                        switch (p.Weight)
                        {
                            case IDAL.DO.WeightCategories.Easy:
                                drone_BL.Battery = random.Next((int)(distance * dal.PowerConsumptionRequest()[1] + 1), 101);
                                break;
                            case IDAL.DO.WeightCategories.Intermediate:
                                drone_BL.Battery = random.Next((int)(distance * dal.PowerConsumptionRequest()[2] + 1), 101);
                                break;
                            case IDAL.DO.WeightCategories.Liver:
                                drone_BL.Battery = random.Next((int)(distance * dal.PowerConsumptionRequest()[3] + 1), 101);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else //the drone is not in delivery
                {
                    drone_BL.DroneStatus = (DroneStatus)random.Next(2); //Maintenance or Available
                    if (drone_BL.DroneStatus == DroneStatus.Maintenance)
                    {
                        //Its location will be drawn between the purchasing stations
                        List<Station> baseStations = dal.GetAllBaseStations().ToList();
                        int index=random.Next(0, baseStations.Count());
                        drone_BL.CurrentLocation = new()
                        {
                            Latitude = baseStations[index].Lattitude,
                            Longitude = baseStations[index].Longitude
                        };

                        drone_BL.Battery = random.Next(0, 21);
                    }
                    else if (drone_BL.DroneStatus == DroneStatus.Available)
                    {
                        //Its location will be raffled off among customers who have packages provided to them
                        List<IDAL.DO.Parcel> parcelsDelivered = parcels.FindAll(p => p.Delivered != DateTime.MinValue);
                        int index = random.Next(0, parcelsDelivered.Count());
                        drone_BL.CurrentLocation = new()
                        {
                            Latitude = dal.CustomerView(parcelsDelivered[index].TargetId).Lattitude,
                            Longitude = dal.CustomerView(parcelsDelivered[index].TargetId).Longitude
                        };
                        // Battery mode will be recharged between a minimal charge that will allow it to reach the station closest to charging and a full charge
                        double distance = dal.GetDistanceBetweenLocationAndClosestBaseStation(parcelsDelivered[index].TargetId);
                        drone_BL.Battery = random.Next((int)(distance * dal.PowerConsumptionRequest()[0] + 1), 101);
                    }
                }
                droneForLists.Add(drone_BL);
            }
        }


        #region ADD



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
        public void AddCustomerBo(int id, string name, string phone, Location location)
        {
            //add customer fields in BL.
            IBL.BO.Customer customer = new IBL.BO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = location
            };

            //Add customer in DAL to data source.
            dal.AddCustomer(id, name, phone, location.Longitude, location.Latitude);
        }

        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public void AddParcelBo(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
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
                ParcelDelivery = DateTime.MinValue
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
            //update in dal
            dal.UpdateDroneModle(id, model);

            //update in BL
            for (int i = 0; i < drones.Count; i++)
            {
                if (drones[i].Id == id)//Obtain an index for the location where the package ID is located
                {
                    if (drones[i].Model != model)
                    {
                        IDAL.DO.Drone drone = drones[i];
                        drone.Model = model;
                        drones[i] = drone;
                        return;
                    }
                }
            }
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
        public void UpdateReleaseDroneFromCharging(int id, TimeSpan chargingtime)
        {

        }

        /// <summary>
        /// Assign parcel to drone
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateAssignParcelToDrone(int id)
        {

        }

        /// <summary>
        /// Collection of a parcek by drone
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateCollectionParcelByDrone(int id)
        {

        }

        /// <summary>
        /// Delivery parcel by drone
        /// </summary>
        /// <param name="id">Id drone</param>
        /// <param name="chargingtime">Charging time</param>
        public void UpdateDeliveryParcelByDrone(int id)
        {

        }


        #endregion


        #region GET ITEM
        /// <summary>
        /// Base station view
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns>Base station show</returns>
        public BaseStation BaseStationViewBl(int baseStationId)
        {
            Station station = dal.BaseStationView(baseStationId);
            List<int> dronesId = dal.GetDronesInChargingsAtStation(baseStationId).ToList();
            IDAL.DO.Drone drone = drones.Find(d => d.Id == dronesId[0]);
            List<DroneInCharging> dronesInCarging = new()
            {
                new DroneInCharging
                {
                    Id = dronesId[0]
                },
                new DroneInCharging
                {
                    Id = dronesId[1]
                }
            };
            BaseStation baseStation = new BaseStation()
            {
                Id = baseStationId,
                NameBaseStation = station.Name,
                Location = new() { Latitude = station.Lattitude, Longitude = station.Longitude },
                DroneInChargings = dronesInCarging,
                NumOfAvailableChargingPositions = station.ChargeSlots + drones.Count()
            };

            //find the station in the array of stations and return it.
            return baseStation;
        }

        public IBL.BO.Drone DroneViewBl(int droneId)
        {
            throw new NotImplementedException();
        }

        public IBL.BO.Customer CustomerViewBl(int customerId)
        {
            throw new NotImplementedException();
        }

        public IBL.BO.Parcel ParcelViewBl(int parcelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStationForList> GetAllBaseStationsBo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneForList> GetAllDronesBo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerForList> GetAllCustomersBo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParcelForList> GetAllParcelsBo()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParcelForList> GetAllParcelsNotYetAssociatedWithGlider()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStationForList> GetAllBaseStationWhithAvailibleCharging()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
