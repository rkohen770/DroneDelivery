﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using DO;
namespace BL
{
    public sealed partial class BlObject : BLApi.IBL
    {
        static readonly Lazy<BlObject> lazy = new Lazy<BlObject>(() => new());

        public static BlObject Instance { get { return lazy.Value; } }

        static Random random = new();

        List<DroneForList> droneForLists = new();

        internal static IDal dal = DalFactory.GetDal();

        public BlObject()
        {
            double[] power = dal.PowerConsumptionRequest();
            List<DO.Drone> drones = dal.GetAllDrones().ToList();

            initializeListOfDrone(drones, power);

        }
        /// <summary>
        /// List boot function of "drones to list "
        /// </summary>
        /// <param name="drones">list of DO of drones</param>
        private void initializeListOfDrone(List<DO.Drone> drones, double[] power)
        {
            //find A package that has not yet been delivered but the drone has already been associated
            List<DO.Parcel> parcelsList = dal.GetAllParcels().ToList();
            DroneForList drone_BL;

            foreach (var drone_DL in drones)
            {
                drone_BL = new DroneForList()
                {
                    DroneID = drone_DL.DroneID,
                    DroneModel = drone_DL.DroneModel,
                    MaxWeight = (BO.WeightCategories)drone_DL.MaxWeight
                };
                //If there is a package that has not yet been delivered but the drone has already been associated
                var parcel = parcelsList.Find(p => p.DroneID == drone_BL.DroneID && p.Delivered == null);
                if (parcel.ParcelID != 0)
                {
                    double minValueBattery = 0;
                    drone_BL.DroneStatus = DroneStatus.Delivery;
                    //If the parcel was associated but not collected
                    if (parcel.PickedUp == null)
                    {
                        // The location of the drone will be at the station closest to the sender
                        int senderId = parcel.SenderID;
                        double senderLattitude = dal.GetCustomer(senderId).Lattitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Lattitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        minValueBattery = drone_BL.DroneBattery - getDistanceBetweenTwoPoints(senderLattitude, senderLongitude, st.Lattitude, st.Longitude)
                           + dal.GetDistanceBetweenLocationsOfParcels(parcel.SenderID, parcel.TargetID)
                           + dal.GetDistanceBetweenLocationAndClosestBaseStation(parcel.TargetID) * power[0];
                    }
                    else if (parcel.PickedUp != null && parcel.Delivered == null)
                    {
                        //The location of the drone will be at the location of the sender
                        int senderId = parcel.SenderID;
                        double senderLattitude = dal.GetCustomer(senderId).Lattitude;
                        double senderLongitude = dal.GetCustomer(senderId).Longitude;
                        Station st = dal.GetClosestStation(senderLattitude, senderLongitude);
                        drone_BL.CurrentLocation = new Location
                        {
                            Lattitude = st.Lattitude,
                            Longitude = st.Longitude
                        };
                        double distance = dal.GetDistanceBetweenLocationsOfParcels(parcel.SenderID, parcel.TargetID)
                            + dal.GetDistanceBetweenLocationAndClosestBaseStation(parcel.TargetID);
                        switch (parcel.Weight)
                        {
                            case DO.WeightCategories.Easy:
                                minValueBattery = drone_BL.DroneBattery - power[1];
                                break;
                            case DO.WeightCategories.Intermediate:
                                minValueBattery = drone_BL.DroneBattery - power[2];
                                break;
                            case DO.WeightCategories.Liver:
                                minValueBattery = drone_BL.DroneBattery - power[3];
                                break;
                            default:
                                break;
                        }
                    }
                    if (minValueBattery < 100)
                        drone_BL.DroneBattery = random.Next((int)minValueBattery, 101);
                    else
                        drone_BL.DroneBattery = 100;
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
                        drone_BL.DroneBattery = random.Next(0, 21);
                    }
                    else if (drone_BL.DroneStatus == DroneStatus.Available)
                    {
                        //Its location will be raffled off among customers who have packages provided to them
                        List<DO.Parcel> parcelsDelivered = parcelsList.FindAll(p => p.Delivered != null);
                        int index = random.Next(0, parcelsDelivered.Count());
                        drone_BL.CurrentLocation = new()
                        {
                            Lattitude = dal.GetCustomer(parcelsDelivered[index].TargetID).Lattitude,
                            Longitude = dal.GetCustomer(parcelsDelivered[index].TargetID).Longitude
                        };
                        // Battery mode will be recharged between a minimal charge that will allow it to reach the station closest to charging and a full charge
                        double distance = dal.GetDistanceBetweenLocationAndClosestBaseStation(parcelsDelivered[index].TargetID);
                        if (distance * power[0] < 100)
                            drone_BL.DroneBattery = random.Next((int)(distance * power[0]), 101);
                        else
                            drone_BL.DroneBattery = 100;
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
            return Math.Sqrt(Math.Pow(lattitude_1 - lattitude_2, 2) +
                                    Math.Pow(longitude_1 - longitude_2, 2));
        }
    }
}