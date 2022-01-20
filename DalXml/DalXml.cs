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
        string dir = Directory.GetCurrentDirectory();

        /// <summary>
        /// users XElement
        /// </summary>
        string UseresPath = @"\Data\UseresXml.xml";
        /// <summary>
        /// Customers XMLSerializer
        /// </summary>
        string CustomersPath = @"\Data\CustomersXml.xml";
        /// <summary>
        /// Drones XMLSerializer
        /// </summary>
        string DronesPath = @"\Data\DronesXml.xml";
        /// <summary>
        /// Drone Charge XMLSerializer
        /// </summary>
        string DroneChargePath = @"\Data\DroneChargeXml.xml";
        /// <summary>
        /// Parcel XMLSerializer
        /// </summary>
        string ParcelsPath = @"\Data\ParcelsXml.xml";
        /// <summary>
        /// Station XMLSerializer
        /// </summary>
        string StationsPath = @"\Data\StationsXml.xml";
        /// <summary>
        /// Ordinal Parcel Number XMLSerializer
        /// </summary>
        string ConfigPath = @"\Data\ConfigXml.xml";
        #endregion

        //#region Config
        ///// <summary>
        ///// returns line station ID from file
        ///// </summary>
        ///// <returns></returns>
        //public int GetConfigNumber(string name)
        //{
        //    List<Config> ListConfig = XMLTools.LoadListFromXMLSerializer<Config>(ConfigPath);
        //    int id = ListConfig.Find(i => i.Name==name).Id;
        //    return id;
        //}
        ///// <summary>
        ///// updates the line station in file
        ///// </summary>
        //public void UpdateConfigNumber(string name)
        //{
        //    List<Config> ListID = XMLTools.LoadListFromXMLSerializer<Config>(ConfigPath);
        //    Config i = ListID.Find(x => x.Name == name);
        //    ListID.Remove(i);
        //    i.Id++;
        //    ListID.Add(i); //no need to Clone()
        //    XMLTools.SaveListToXMLSerializer(ListID, ConfigPath);
        //}
        //#endregion

        /// <summary>
        /// A function that calculates the distance between two points on the map
        /// </summary>
        /// <param name="senderId">sender ID</param>
        /// <param name="targetId">target ID</param>
        /// <returns>Returns a distance between two points</returns>
        public double GetDistanceBetweenLocationsOfParcels(int senderId, int targetId)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(dir + StationsPath);
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
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dir + DroneChargePath);
            return droneChargeList.FindAll(predicate);
        }
    }
}