using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        /// <summary>
        /// Add base station
        /// </summary>
        /// <param name="id">Station number</param>
        /// <param name="nameBaseStation">Name base station</param>
        /// <param name="location">Location of the bus stop</param>
        /// <param name="numOfAvailableChargingPositions">Number of charging stations available</param>
        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions)
        {
            //add baseStation fields in BL.
            BaseStation baseStation = new BaseStation()
            {
                Id = id,
                NameBaseStation = nameBaseStation,
                Location = location,
                NumOfAvailableChargingPositions = numOfAvailableChargingPositions
            };
            //Add baseStation in DAL to data source.
            dal.AddStation(id, nameBaseStation, location.Longitude, location.Latitude, numOfAvailableChargingPositions);
        }

        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="id">Base station id</param>
        /// <param name="nameBaseStation">Base station name</param>
        /// <param name="totalAmountOfChargingStations">Total amount of charging stations</param>
        public void UpdateBaseStationData(int id, int nameBaseStation , int totalAmountOfChargingStations)
        {
            //update in BL
            Station station= dal.BaseStationView(id);
            if (nameBaseStation!=0)
            {
                if (totalAmountOfChargingStations!=0)
                {
                    dal.UpdateBaseStationData(id, nameBaseStation, totalAmountOfChargingStations);
                }
                else
                {
                    dal.UpdateBaseStationName(id, nameBaseStation);
                }
            }
            else
            {
                dal.UpdateBaseStationCharging(id, totalAmountOfChargingStations);
            }



            //    for (int i = 0; i < drones.Count; i++)
            //    {
            //        if (drones[i].Id == id)//Obtain an index for the location where the package ID is located
            //        {
            //            if (drones[i].Model != model)
            //            {
            //                IDAL.DO.Drone drone = drones[i];
            //                drone.Model = model;
            //                drones[i] = drone;
            //                return;
            //            }
            //        }
            //    }
            // }
        }
    }
    
}
