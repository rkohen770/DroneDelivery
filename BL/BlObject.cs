﻿using System;
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
        static Random random = new Random();
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
                    MaxWeight = (IBL.BO.WeightCategories)drone_DL.MaxWeight
                };
                //If there is a package that has not yet been delivered but the drone has already been associated
                var parcel = parcelsList.Find(p => p.DroneId == drone_BL.Id && p.Delivered == DateTime.MinValue);
                if (parcel.Id != 0)
                {
                    double minValueBattery = 0;
                    drone_BL.DroneStatus = DroneStatus.Delivery;
                    //If the parcel was associated but not collected
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        // The location of the drone will be at the station closest to the sender
                        int senderId = parcel.SenderId;
                        double senderLattitude = dal.GetCustomer(senderId).Lattitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Lattitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        minValueBattery = getDistanceBetweenTwoPoints(senderLattitude, senderLongitude, st.Lattitude, st.Longitude)
                           + dal.GetDistanceBetweenLocationsOfParcels(parcel.SenderId, parcel.TargetId)
                           + dal.GetDistanceBetweenLocationAndClosestBaseStation(parcel.TargetId) * dal.PowerConsumptionRequest()[0] + 1;
                    }
                    else if (parcel.PickedUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                    {
                        //The location of the drone will be at the location of the sender
                        int senderId = parcel.SenderId;
                        double senderLattitude = dal.GetCustomer(senderId).Lattitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Lattitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        double distance = dal.GetDistanceBetweenLocationsOfParcels(parcel.SenderId, parcel.TargetId)
                            + dal.GetDistanceBetweenLocationAndClosestBaseStation(parcel.TargetId);
                        switch (parcel.Weight)
                        {
                            case IDAL.DO.WeightCategories.Easy:
                                minValueBattery = distance * dal.PowerConsumptionRequest()[1] + 1;
                                break;
                            case IDAL.DO.WeightCategories.Intermediate:
                                minValueBattery = distance * dal.PowerConsumptionRequest()[2] + 1;
                                break;
                            case IDAL.DO.WeightCategories.Liver:
                                minValueBattery = distance * dal.PowerConsumptionRequest()[3] + 1;
                                break;
                            default:
                                break;
                        }
                    }
                    drone_BL.Battery = random.Next((int)minValueBattery, 101);
                }
                else //the drone is not in delivery
                {
                    drone_BL.DroneStatus = (DroneStatus)random.Next(2); //Maintenance or Available
                    if (drone_BL.DroneStatus == DroneStatus.Maintenance)
                    {
                        //Its location will be drawn between the purchasing stations
                        List<Station> baseStations = dal.GetAllBaseStations().ToList();
                        int index = random.Next(0, baseStations.Count());
                        drone_BL.CurrentLocation = new()
                        {
                            Lattitude = baseStations[index].Lattitude,
                            Longitude = baseStations[index].Longitude
                        };
                        drone_BL.Battery = random.Next(0, 21);
                    }
                    else if (drone_BL.DroneStatus == DroneStatus.Available)
                    {
                        //Its location will be raffled off among customers who have packages provided to them
                        List<IDAL.DO.Parcel> parcelsDelivered = parcelsList.FindAll(p => p.Delivered != DateTime.MinValue);
                        int index = random.Next(0, parcelsDelivered.Count());
                        drone_BL.CurrentLocation = new()
                        {
                            Lattitude = dal.GetCustomer(parcelsDelivered[index].TargetId).Lattitude,
                            Longitude = dal.GetCustomer(parcelsDelivered[index].TargetId).Longitude
                        };
                        // Battery mode will be recharged between a minimal charge that will allow it to reach the station closest to charging and a full charge
                        double distance = dal.GetDistanceBetweenLocationAndClosestBaseStation(parcelsDelivered[index].TargetId);
                        drone_BL.Battery = random.Next((int)(distance * dal.PowerConsumptionRequest()[0] + 1), 101);
                    }
                }
                droneForLists.Add(drone_BL);
            }

        }

        /// <summary>
        /// --BONUS--: another option that recives coordinates and print the distance from it to a station or a customer
        /// A function that calculates the distance between the location
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private static double getDistanceBetweenTwoPoints(double lattitude_1, double longitude_1, double lattitude_2, double longitude_2)
        {
            return Math.Sqrt(Math.Pow(lattitude_1 -lattitude_2, 2) +
                                    Math.Pow(longitude_1 - longitude_2, 2));
        }
    }
}
