using System;
using System.Collections.Generic;
using System.Linq;
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
        string OrdinalParcelNumberisPath = @"OrdinalParcelNumberPath.xml";
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

            if (StationAdd.StationID == 0)
            {
                Station s = new Station
                {
                    StationID = id,
                    StationName = name,
                    Longitude = longitude,
                    Lattitude = lattitude,
                    ChargeSlots = chargeSlots

                };
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else
                throw new BaseStationAlreadyExistException(id, name, "The station exists");

        }

        /// <summary>
        /// Update Base Station Model
        /// </summary>
        /// <param name="id">Base Station id</param>
        /// <param name="nameBaseStation">new Base Station name</param>
        public void UpdateBaseStationData(int id, int nameBaseStation, int totalAmountOfChargingStations)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath); 
            List<DroneCharge> listCharge= (List<DroneCharge>)XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath)
                .Where(c => c.StationID == id);
            var StationAdd = (from item in ListStation
                              where item.StationID == id
                              select item).FirstOrDefault();
            if (StationAdd.StationID != 0)
            {
                Station s = StationAdd;
                s.StationName = nameBaseStation;
                s.ChargeSlots = totalAmountOfChargingStations - listCharge.Count(); 

                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new BadBaseStationIDException(id, $"The station: {id} doesn't exist");
        }

        ///// <summary>
        ///// Update Base Station Model
        ///// </summary>
        ///// <param name="id">Base Station id</param>
        ///// <param name="nameBaseStation">new Base Station name</param>
        //public void UpdateBaseStationName(int id, int nameBaseStation)
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

        //    var StationAdd = (from item in ListStation
        //                      where item.StationID == id
        //                      select item).FirstOrDefault();

        //    if (StationAdd.StationID != 0)
        //    {
        //        update(StationAdd);
        //        XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
        //    }
        //    throw new DO.BadBaseStationIDException(id, $"The station doesn't exist in the system");
        //}
        ///// <summary>
        ///// deletes station by the key number from the file
        ///// </summary>
        ///// <param name="stationKey"></param>
        //public void DeleteStation(int stationKey)
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
        //    var StationDelete = (from item in ListStation
        //                         where item.Code == stationKey && item.Available
        //                         select item).FirstOrDefault();
        //    if (StationDelete != null)
        //    {
        //        ListStation.Remove(StationDelete);
        //        StationDelete.Available = false;
        //        ListStation.Add(StationDelete);
        //        XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
        //    }
        //    else throw new DO.BadStationKeyException(stationKey, $"The station: {stationKey} doesn't exist");
        //}
        ///// <summary>
        ///// deletes all stations in file
        ///// </summary>
        //public void DeleteAllStation()
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
        //    if (ListStation != null)
        //    {
        //        foreach (var item in ListStation)
        //        {
        //            ListStation.Remove(item);
        //            item.Available = false;
        //            ListStation.Add(item);
        //        }
        //        XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
        //    }
        //}
        ///// <summary>
        ///// returns all stations in files
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Station> GetAllStations()
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
        //    return from item in ListStation
        //           where item.Available
        //           select item;
        //}
        ///// <summary>
        ///// returns all stations by the predicate from file
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public IEnumerable<Station> GetAllStationsBy(Predicate<Station> p)
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
        //    return from item in ListStation
        //           where p(item) && item.Available
        //           select item;

        //}
        ///// <summary>
        ///// returns station by the key number
        ///// </summary>
        ///// <param name="stationKey"></param>
        ///// <returns></returns>
        //public Station GetStation(int stationKey)
        //{
        //    List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
        //    var s = (from item in ListStation
        //             where item.Code == stationKey && item.Available
        //             select item).FirstOrDefault();
        //    if (s == null)
        //        throw new DO.BadStationKeyException(stationKey);
        //    return new Station
        //    {
        //        Code = stationKey,
        //        Name = s.Name,
        //        Region = s.Region,
        //        Latitude = s.Latitude,
        //        Longitude = s.Longitude,
        //        Address = s.Address,
        //        StationRoof = s.StationRoof,
        //        DigitalPanel = s.DigitalPanel,
        //        AccessForDisabled = s.AccessForDisabled,
        //        Available = s.Available
        //    };
        //}
        //#endregion

    }
}
