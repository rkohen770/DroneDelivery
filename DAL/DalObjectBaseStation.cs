using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IDAL.DO;

namespace DAL
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// Add a base station to the list of stations
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The station name</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        /// <param name="chargeSlots">Several arguments</param>
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots)
        {
            if (DataSource.stations.Exists(station => station.Id == id))
            {
                throw new ExistingFigureException("the station exists allready");
            }
            else
            {
                Station s = new Station
                {
                    Id = id,
                    Name = name,
                    Longitude = longitude,
                    Lattitude = lattitude,
                    ChargeSlots = chargeSlots
                };
                DataSource.stations.Add(s);//Adding the new station to the array;
            }
        }


    }
