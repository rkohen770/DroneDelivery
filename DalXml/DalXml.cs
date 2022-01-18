using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed class DalXml : IDal
    {
        //Using a design pattern of singelton
        #region singelton
        static readonly DalXml instance = new DalXml();
        /// <summary>
        /// static constructor to ensure instance init is done just before first use
        /// </summary>
        static DalXml() { }
        /// <summary>
        ///  constructor. default => private
        /// </summary>
        DalXml() { }
        /// <summary>
        /// the public Instance property for use. returns the instance
        /// </summary>
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files Path
        /// <summary>
        /// users XElement
        /// </summary>
        string UseresPath = @"UseresXml.xml";
        /// <summary>
        /// Customers XMLSerializer
        /// </summary>
        string CustomersPath = @"CustomersXml.xml";
        /// <summary>
        /// Drones XMLSerializer
        /// </summary>
        string DronesPath = @"DronesXml.xml";
        /// <summary>
        /// Drone Charge XMLSerializer
        /// </summary>
        string DroneChargePath = @"DroneChargeXml.xml";
        /// <summary>
        /// Parcel XMLSerializer
        /// </summary>
        string ParcelsPath = @"ParcelsXml.xml";
        /// <summary>
        /// Sexagesimal Angle XMLSerializer
        /// </summary>
        string SexagesimalAnglePath = @"SexagesimalAngleXml.xml";
        /// <summary>
        /// Station XMLSerializer
        /// </summary>
        string StationsPath = @"StationsXml.xml";
        /// <summary>
        /// Ordinal Parcel Number XMLSerializer
        /// </summary>
        string ConfigPath = @"ConfigXml.xml";
        #endregion

        #region User
        /// <summary>
        /// returns user by the user name from the file
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);

            User user = users.Find(u => u.UserName == userName && u.Available == true);
            if (user.UserName != null)
                return user; //no need to Clone()
            else
                throw new BadUserNameException(userName, $"bad User Name: {userName}");

        }

        ///// <summary>
        ///// returns user by the user name from the file
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public User GetUser(string userName)
        //{
        //    XElement dalUserId= XElement.Load(CustomersPath);

        //    User dalUser = (from User in dalUserId.Elements()
        //                 where User.Element("id").Value==
        //    if (user.UserName != null)
        //        return user; //no need to Clone()
        //    else
        //        throw new BadUserNameException(userName, $"bad User Name: {userName}");

        //}
        /// <summary>
        /// returns all users from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUseres()
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            return users.AsEnumerable();
        }
        /// <summary>
        /// returns all users by predicate from the file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        //public IEnumerable<User> GetAllUseresBy(Predicate<DO.User> predicate)
        //{
        //    List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
        //    return from u1 in users
        //           where predicate(u1) && u1.Available == true
        //           select u1;
        //}

        public IEnumerable<User> GetAllUseresBy(Predicate<DO.User> predicate)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            return from u1 in users
                   where predicate(u1) && u1.Available == true
                   select u1;
        }


        /// <summary>
        /// adds new user to the file
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            DO.User user1 = (from u in users
                             where u.UserName == user.UserName
                             select u).FirstOrDefault();

            if (user1.UserName != null)
                throw new BadUserNameException(user.UserName, "Duplicate user name");
            users.Add(user);
            XMLTools.SaveListToXMLSerializer(users, UseresPath);

        }
        /// <summary>
        /// deletes user from the file
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);

            User user = (from u in users
                         where u.UserName == userName
                         select u).FirstOrDefault();

            if (user.UserName != null)
            {
                user.Available = false;
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new BadUserNameException(userName, $"bad user name: {userName}");
        }
        /// <summary>
        /// updates user in the file
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);

            User user1 = (from u in users
                          where u.UserName == user.UserName
                          select u).FirstOrDefault();

            if (user1.UserName != null)
            {
                users.Remove(user1);
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new BadUserNameException(user.UserName, $"bad user name: {user.UserName}");
        }
        #endregion

        #region Stations 
        /// <summary>
        /// adds station to the file
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The station name</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <param name="chargeSlots">Several arguments</param>
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationAdd = (from item in ListStation
                              where item.StationID == id
                              select item).FirstOrDefault();

            if (StationAdd.StationID != 0 && StationAdd.Available)
                throw new BaseStationAlreadyExistException(id, name, "The station exists");

            if (StationAdd.StationID != 0 && !StationAdd.Available)
            {
                DeleteStation(id);
            }
            Station s = new Station
            {
                StationID = id,
                StationName = name,
                Longitude = longitude,
                Lattitude = lattitude,
                ChargeSlots = chargeSlots,
            };
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);


        }

        /// <summary>
        /// Update Base Station name and total Amount Of Charging Stations
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name and total Amount Of Charging Stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id && item.Available
                              select item).FirstOrDefault();
            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;
                s.StationName = nameBaseStation;

                var listCharge = from item in XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath)
                                 where item.StationID == id && item.Available
                                 select item;

                s.ChargeSlots = totalAmountOfChargingStations - listCharge.Count();

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(id, $"The station: {id} doesn't exist");
        }

        /// <summary>
        /// Update Base Station name
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name</param>
        public void UpdateBaseStationName(int id, int nameBaseStation)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id && item.Available
                              select item).FirstOrDefault();

            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;
                s.StationName = nameBaseStation;

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            throw new BadBaseStationIDException(id, $"The station doesn't exist in the system");
        }

        /// <summary>
        /// Update base station total number of charge slots
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationCharging(int id, int totalAmountOfChargingStations)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.StationID == id
                              select item).FirstOrDefault();
            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;

                var listCharge =
                    from item in XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath)
                    where item.StationID == id
                    select item;

                s.ChargeSlots = totalAmountOfChargingStations - listCharge.Count();

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(id, $"The station: {id} doesn't exist");
        }

        /// <summary>
        /// deletes station by the ID from the file
        /// </summary>
        /// <param name="stationID"></param>
        public void DeleteStation(int stationID)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationDelete = (from item in ListStation
                                 where item.StationID == stationID && item.Available
                                 select item).FirstOrDefault();
            if (StationDelete.StationID != 0)
            {
                ListStation.Remove(StationDelete);
                StationDelete.Available = false;
                ListStation.Add(StationDelete);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(stationID, $"The station: {stationID} doesn't exist");
        }

        /// <summary>
        /// deletes all stations in file
        /// </summary>
        public void DeleteAllStation()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (ListStation != null)
            {
                foreach (var item in ListStation)
                {
                    DeleteStation(item.StationID);
                }
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
        }

        /// <summary>
        /// return a list of actual base stations
        /// </summary>
        /// <returns>list of base stations</returns>
        public IEnumerable<Station> GetAllBaseStations()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation
                   where item.Available
                   select item;
        }

        /// <summary>
        /// return base stations with 
        /// </summary>
        /// <returns>list of station with </returns>
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> p)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation
                   where p(item) && item.Available
                   select item;

        }

        /// <summary>
        /// return base station by station ID.
        /// </summary>
        /// <param name="stationId">station ID</param>
        /// <returns>statoin</returns>
        public Station GetBaseStation(int stationId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            Station s = (from item in ListStation
                         where item.StationID == stationId && item.Available
                         select item).FirstOrDefault();
            if (s.StationID == 0)
                throw new BadBaseStationIDException(stationId, "the base station not exists in the system");
            return s;
        }

        /// <summary>
        /// A function that returns a minimum distance between a point and a base station
        /// </summary>
        /// <param name="senderLattitude">Lattitude of sender</param>
        /// <param name="senderLongitude">longitude of sender</param>
        /// <param name="flag">Optional field for selecting a nearby base station flag = false or available nearby base station flag = true</param>
        /// <returns>Base station closest to the point</returns>
        public Station GetClosestStation(double senderLattitude, double senderLongitude, bool flag = false)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            double minDistance = 1000000000000;
            Station station = new();
            if (!flag)
            {
                foreach (var stations in ListStation)
                {
                    double dictance = Math.Sqrt(Math.Pow(stations.Lattitude - senderLattitude, 2) + Math.Pow(stations.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = stations;
                    }
                }
            }
            else
            {
                foreach (var stations in ListStation.Where(s => s.ChargeSlots > 0))
                {
                    double dictance = Math.Sqrt(Math.Pow(stations.Lattitude - senderLattitude, 2) + Math.Pow(stations.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = stations;
                    }
                }
            }
            return station;
        }

        /// <summary>
        /// A function that calculates the distance between a customer's location and a base station for charging
        /// </summary>
        /// <param name="targetId">target ID</param>
        /// <returns>Minimum distance to the nearest base station</returns>
        public double GetDistanceBetweenLocationAndClosestBaseStation(int targetId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            double minDistance = 1000000000000;
            Customer target = GetCustomer(targetId);
            foreach (var s in ListStation)
            {
                double dictance = Math.Sqrt(Math.Pow(s.Lattitude - target.Lattitude, 2) + Math.Pow(s.Longitude - target.Longitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                }
            }
            return minDistance;
        }
        #endregion

        #region customer
        /// <summary>
        /// adds customer to the file
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The customer name</param>
        /// <param name="phone">The customer phone number</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id
                               select item).FirstOrDefault();
            if (CustomerAdd.CustomerID != 0 && CustomerAdd.Available)
                throw new CustomerAlreadyExistException(id, name, "The customer exists");
            if (CustomerAdd.CustomerID != 0 && !CustomerAdd.Available)
            {
                DeleteCustomer(id);
            }

            Customer c = new Customer
            {
                CustomerID = id,
                Name = name,
                Phone = phone,
                Longitude = longitude,
                Lattitude = lattitude,
            };
            ListCustomer.Add(c);
            XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
        }
        /// <summary>
        /// Update Customer data
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {
            List<Customer> listCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in listCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();
            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Name = newName;
                c.Phone = newPhone;

                listCustomer.Remove(CustomerAdd);
                listCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(listCustomer, CustomersPath);
            }
            else throw new BadCustomerIDException(id, $"The customer: {id} doesn't exist");
        }
        /// <summary>
        /// update customer name
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        public void UpdateCustomerName(int id, string newName)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();

            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Name = newName;

                ListCustomer.Remove(CustomerAdd);
                ListCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
        }

        /// <summary>
        /// customer phone
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newPhone">customer name</param>
        public void UpdateCustomerPhone(int id, string newPhone)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();

            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Phone = newPhone;

                ListCustomer.Remove(CustomerAdd);
                ListCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
        }
        /// <summary>
        /// deletes customer by the id number from the file
        /// </summary>
        /// <param name="customerId">customer id</param>
        public void DeleteCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var CustomerDelete = (from item in ListCustomer
                                  where item.CustomerID == customerId && item.Available
                                  select item).FirstOrDefault();
            if (CustomerDelete.CustomerID != 0)
            {
                ListCustomer.Remove(CustomerDelete);
                CustomerDelete.Available = false;
                ListCustomer.Add(CustomerDelete);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            else throw new BadCustomerIDException(customerId, $"The customer: {customerId} doesn't exist");
        }
        /// <summary>
        /// return customer by customer id 
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>get customer</returns>
        public Customer GetCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var c = (from item in ListCustomer
                     where item.CustomerID == customerId && item.Available
                     select item).FirstOrDefault();
            if (c.CustomerID == 0)
                throw new BadCustomerIDException(customerId, "the customer not exists in the list of customers");
            return c;
        }

        /// <summary>
        /// return a list of actual customer
        /// </summary>
        /// <returns>list of customers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            return from item in ListCustomer
                   where item.Available
                   select item;
        }

        /// <summary>
        /// return customers by predicat
        /// </summary>
        /// <param name="p">predicat</param>
        /// <returns>customers by predicat</returns>
        public IEnumerable<Customer> GetAllCustomerByPredicate(Predicate<Customer> p)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            return from customer in ListCustomer
                   where p(customer)
                   select customer;
        }

        #endregion

        #region Drone
        /// <summary>
        /// Add a drone to the list of existing drones
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="model">The drone model</param>
        /// <param name="maxWeight">Weight category (light, medium, heavy)</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var DroneAdd = (from item in ListDrone
                            where item.DroneID == id
                            select item).FirstOrDefault();

            if (DroneAdd.DroneID != 0 && DroneAdd.Available)
                throw new DroneAlreadyExistException(id, model, "The drone alredy exists");

            if (DroneAdd.DroneID != 0 && !DroneAdd.Available)
            {
                DeleteDrone(id);
            }
            Drone s = new Drone
            {
                DroneID = id,
                DroneModel = model,
                MaxWeight = maxWeight,
                Available = true,
            };
            ListDrone.Add(s);
            XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);


        }

        /// <summary>
        /// Sending a drone for charging at a base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public int SendingDroneForCharging(int droneId, int stationId)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }

            Station s = station;
            s.ChargeSlots--;//We will update the number of loading locations
            ListStation.Remove(station);
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);

            DroneCharge droneCharge = new DroneCharge//Add a instance of an instance loading entity
            {
                DroneID = droneId,
                StationID = stationId
            };
            ListDroneCharges.Add(droneCharge);//Add a load of drones to file
            XMLTools.SaveListToXMLSerializer(ListDroneCharges, DroneChargePath);

            return stationId;
        }

        /// <summary>
        /// Release the UAV from a charge at the base station
        /// </summary>
        /// <param name="droneId">Drone ID for charging</param>
        /// <param name="stationId">Charging station ID</param>
        public void ReleasDroneFromCharging(int droneId, int stationId)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            var charge = (from item in ListDroneCharges
                          where item.StationID == stationId && item.DroneID == droneId && item.Available
                          select item).FirstOrDefault();

            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }

            Station s = station;
            s.ChargeSlots++;//We will update the number of loading locations
            ListStation.Remove(station);
            ListStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);

            DeleteDroneGharge(charge);
        }

        /// <summary>
        /// Update Drone Modle at a base station
        /// </summary>
        /// <param name="droneId">drone id to update</param>
        /// <param name="model">new model</param>
        public void UpdateDroneModle(int droneId, string model)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            var drone = (from item in ListDrone
                         where item.DroneID == droneId && item.Available
                         select item).FirstOrDefault();
            if (drone.DroneID != droneId)
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }

            Drone d = drone;
            d.DroneModel = model;
            ListDrone.Remove(drone);
            ListDrone.Add(d);
            XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);
        }

        /// <summary>
        /// return drone by drone ID 
        /// </summary>
        /// <param name="droneId">drone ID</param>
        /// <returns>drone to show</returns>
        public Drone GetDrone(int droneId)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var d = (from item in ListDrone
                     where item.DroneID == droneId && item.Available
                     select item).FirstOrDefault();
            if (d.DroneID == 0)
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            return d;
        }

        /// <summary>
        /// return a list of actual drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            return from item in ListDrone
                   where item.Available
                   select item;
        }

        /// <summary>
        /// List of drones loaded at a specific station
        /// </summary>
        /// <param name="stationId">base station ID</param>
        /// <returns>List of drones loaded at a specific station</returns>
        public IEnumerable<int> GetDronesInChargingsAtStation(int stationId, Predicate<DroneCharge> p)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            List<DroneCharge> ListDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            var station = (from item in ListStation
                           where item.StationID == stationId && item.Available
                           select item).FirstOrDefault();

            if (station.StationID != stationId)
            {
                throw new BadBaseStationIDException(stationId, "the base station not exists in the list of station");
            }
            return from droneCharge in ListDroneCharges
                   where p(droneCharge) && droneCharge.Available
                   select droneCharge.DroneID;
        }

        /// <summary>
        /// return drones by predicat
        /// </summary>
        /// <param name="p">predicat</param>
        /// <returns>drones by predicat</returns>
        public IEnumerable<Drone> GetDronesByPredicat(Predicate<Drone> p)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            return from drone in ListDrone
                   where p(drone)
                   select drone;
        }

        /// <summary>
        /// deletes drone by the ID from the file
        /// </summary>
        /// <param name="droneID"></param>
        public void DeleteDrone(int droneID)
        {
            List<Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            var DroneDelete = (from item in ListDrone
                               where item.DroneID == droneID && item.Available
                               select item).FirstOrDefault();
            if (DroneDelete.DroneID != 0)
            {
                ListDrone.Remove(DroneDelete);
                DroneDelete.Available = false;
                ListDrone.Add(DroneDelete);
                XMLTools.SaveListToXMLSerializer(ListDrone, DronesPath);
            }
            else throw new BadBaseStationIDException(droneID, $"The drone: {droneID} doesn't exist in the system");
        }

        /// <summary>
        /// delete drone charge from the file
        /// </summary>
        /// <param name="charge"></param>
        private void DeleteDroneGharge(DroneCharge charge)
        {
            List<DroneCharge> ListDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);
            ListDroneCharge.Remove(charge);
            charge.Available = false;
            ListDroneCharge.Add(charge);
            XMLTools.SaveListToXMLSerializer(ListDroneCharge, DroneChargePath);
        }

        //}
        ///// <summary>
        ///// Method of applying drone power
        ///// </summary>
        ///// <returns>An array of the amount of power consumption of a drone for each situation</returns>
        //public double[] PowerConsumptionRequest()
        //{
        //    double[] result = {DataSource.Config.vacant, DataSource.Config.CarriesLightWeight,
        //        DataSource.Config.CarriesMediumWeight, DataSource.Config.CarriesHeavyWeight,
        //        DataSource.Config.DroneChargingRate };
        //    return result;
        //}
        #endregion

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

            var castomer= (from item in ListCustomer
                           where item.CustomerID == senderId
                           select item).FirstOrDefault();
            if(castomer.CustomerID==0||!castomer.Available)
                throw new BadCustomerIDException(senderId, "The sender not exists in the file of customers");

            castomer = (from item in ListCustomer
                        where item.CustomerID == targetId
                        select item).FirstOrDefault();
            if (castomer.CustomerID == 0 || !castomer.Available)
                throw new BadCustomerIDException(targetId, "The target not exists in the file of customers");

            else
            {
                Parcel p = new()
                {
                    ParcelID = DataSource.Config.OrdinalParcelNumber++,
                    SenderID = senderId,
                    TargetID = targetId,
                    Weight = weight,
                    priority = priority,
                    Requested = DateTime.Now,
                    DroneID = droneId
                };
                ListParcel.Add(p);
                XMLTools.SaveListToXMLSerializer(ListParcel, CustomersPath);//Adding the new parcel to the file
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
                            where item.ParcelID == parcelId
                            select item).FirstOrDefault();
            if (parcel.ParcelID == 0 || !parcel.Available)
                throw new BadCustomerIDException(parcelId, "The parcel not exists in the file of parcels");

            castomer = (from item in ListCustomer
                        where item.CustomerID == targetId
                        select item).FirstOrDefault();
            if (castomer.CustomerID == 0 || !castomer.Available)
                throw new BadCustomerIDException(targetId, "The target not exists in the file of customers");

            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            if (!DataSource.drones.Exists(drone => drone.DroneID == droneId))
            {
                throw new BadDroneIDException(droneId, "the drone not exists in the list of drones");
            }
            else
            {
                //We will go through the entire list of the drone, to find a available drone
                for (int pIndex = 0; pIndex < DataSource.parcels.Count; pIndex++)
                {
                    if (DataSource.parcels[pIndex].ParcelID == parcelId)
                    {
                        Parcel parcel = DataSource.parcels[pIndex];//Obtain an index for the location where the package ID is located
                        parcel.DroneID = droneId;//Update the droneid field in the drone package found
                        parcel.Scheduled = DateTime.Now;//Update packet time association field to now.
                        DataSource.parcels[pIndex] = parcel;
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ParcelID == parcelId)//Obtain an index for the location where the package ID is located
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the percel not exists in the list of parcels");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ParcelID == parcelId)//Obtain an index for the location where the package ID is located
                {
                    Parcel parcel = DataSource.parcels[i];
                    parcel.Delivered = DateTime.Now;//Update packet time delivered field to now.
                    DataSource.parcels[i] = parcel;
                    return;
                }
            }
        }


        public void UpdateParcelData(int id, int droneID)
        {
            if (!DataSource.parcels.Exists(p => p.ParcelID == id))
            {
                throw new BadParcelIDException(id, "the parcel not exists in the system");
            }
            else
            {
                int pIndex = DataSource.parcels.FindIndex(p => p.ParcelID == id);
                Parcel parcel = DataSource.parcels[pIndex];
                parcel.DroneID = droneID;
            }
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
            if (!DataSource.parcels.Exists(parcel => parcel.ParcelID == parcelId))
            {
                throw new BadParcelIDException(parcelId, "the parcel not exists in the list of parcels");
            }
            //find the place of the parcel in the array of parcels
            return DataSource.parcels.Find(p => p.ParcelID == parcelId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual parcel
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            return (IEnumerable<Parcel>)DataSource.parcels;
        }

        /// <summary>
        /// Displays a list of parcels that have not yet been assigned to the drone
        /// </summary>
        /// <returns>list of parcel without special dron</returns>
        public IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron(Predicate<Parcel> p)
        {
            //return all the parcels without special drone
            return from parcel in DataSource.parcels
                   where p(parcel)
                   select parcel.Clone();
        }
       
        /// <summary>
        /// return a list of actual customer
        /// </summary>
        /// <returns>list of customers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            return from item in ListCustomer
                   where item.Available
                   select item;
        }

        /// <summary>
        /// return customers by predicat
        /// </summary>
        /// <param name="p">predicat</param>
        /// <returns>customers by predicat</returns>
        public IEnumerable<Customer> GetAllCustomerByPredicate(Predicate<Customer> p)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            return from customer in ListCustomer
                   where p(customer)
                   select customer;
        }

        #endregion

        #endregion
    }
}

