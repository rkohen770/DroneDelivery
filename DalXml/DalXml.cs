using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
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
        string dir = @"xml/";

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
        /// Station XMLSerializer
        /// </summary>
        string StationsPath = @"StationsXml.xml";
        /// <summary>
        /// Ordinal Parcel Number XMLSerializer
        /// </summary>
        string ConfigPath = @"ConfigXml.xml";
        #endregion

      
        /// <summary>
        /// A function that calculates the distance between two points on the map
        /// </summary>
        /// <param name="senderId">sender ID</param>
        /// <param name="targetId">target ID</param>
        /// <returns>Returns a distance between two points</returns>
        public double GetDistanceBetweenLocationsOfParcels(int senderId, int targetId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>( StationsPath);
            double minDistance = 1000000000000;
            Customer sender = GetCustomer(senderId);
            Customer target = GetCustomer(targetId);
            foreach (var station in ListStation)
            {
                double dictance = Math.Sqrt(Math.Pow(sender.Lattitude - target.Lattitude, 2) + Math.Pow(sender.Longitude - target.Longitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                }
            }
            return minDistance;
        }

        public IEnumerable<DroneCharge> GetAllDroneCharge(Predicate<DroneCharge> predicate)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>( DroneChargePath);
            return droneChargeList.FindAll(predicate);
        }
    }
}