using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DL
{
    sealed partial class DalObject : IDal
    {
        static readonly Lazy<DalObject> lazy = new Lazy<DalObject>(() => new());

        public static DalObject Instance { get { return lazy.Value; } }

        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// A function that calculates the distance between two points on the map
        /// </summary>
        /// <param name="senderId">sender ID</param>
        /// <param name="targetId">target ID</param>
        /// <returns>Returns a distance between two points</returns>
        public double GetDistanceBetweenLocationsOfParcels(int senderId, int targetId)
        {
            double minDistance = 1000000000000;
            Customer sender = GetCustomer(senderId);
            Customer target = GetCustomer(targetId);
            foreach (var station in DataSource.stations)
            {
                double dictance = Math.Sqrt(Math.Pow(sender.Lattitude - target.Lattitude, 2) + Math.Pow(sender.Longitude - target.Longitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                }
            }
            return minDistance;
        }
    }
}