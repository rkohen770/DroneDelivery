﻿using IDAL.DO;
using System;

namespace DalObject
{
    class DataSource
    {
        internal static Drone[] drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
        internal static DroneCharge[] droneCharges = new DroneCharge[5];
        static Random rand = new();

        internal class Config
        {
            internal static int IndexDrone = 0;
            internal static int IndexStation = 0;
            internal static int IndexCustomer = 0;
            internal static int IndexParcel = 0;
            internal static int IndexDroneCharge = 0;
            public static int OrdinalParcelNumber = 10000;
        }

        public static void Initialize()
        {
            #region stations
            Stations[Config.IndexStation++] = new Station
            {
                Id = 11212,
                Name = 11,
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                ChargeSlots = 5,
            };

            Stations[Config.IndexStation++] = new Station
            {
                Id = 22212,
                Name = 22,
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                ChargeSlots = 8,
            };
            #endregion

            #region drones
            drones[Config.IndexDrone++] = new Drone
            {
                Id = 7486,
                Model = "EG-574",
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = (DroneStatuses)rand.Next(3),
                Battery = 94.3,
            };

            drones[Config.IndexDrone++] = new Drone
            {
                Id = 7686,
                Model = "EG-574",
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = (DroneStatuses)rand.Next(3),
                Battery = 36,
            };

            drones[Config.IndexDrone++] = new Drone
            {
                Id = 7916,
                Model = "EG-474",
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = (DroneStatuses)rand.Next(3),
                Battery = 84.8,
            };

            drones[Config.IndexDrone++] = new Drone
            {
                Id = 7216,
                Model = "EG-474",
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = (DroneStatuses)rand.Next(3),
                Battery = 75,
            };

            drones[Config.IndexDrone++] = new Drone
            {
                Id = 7945,
                Model = "EG-474",
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = (DroneStatuses)rand.Next(3),
                Battery = 98.9,
            };

            #endregion

            #region customer
            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 123456789,
                Name = "Eliya",
                Phone = "0547689612",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 987654321,
                Name = "Devora",
                Phone = "0512926771",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 234567891,
                Name = "Rachel Lea",
                Phone = "0585848441",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 345678912,
                Name = "Yanir",
                Phone = "0503344684",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 456789123,
                Name = "Reuven",
                Phone = "0526795861",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 567891234,
                Name = "Ilana",
                Phone = "0548596887",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 678912345,
                Name = "Dalya",
                Phone = "0558724551",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 789123456,
                Name = "Aviya",
                Phone = "0518846523",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 891234567,
                Name = "Baruch",
                Phone = "0558742215",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };

            customers[Config.IndexCustomer++] = new Customer
            {
                Id = 101123556,
                Name = "Chen",
                Phone = "0589868554",
                //Grills values that are within the borders of the State of Israel
                Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
            };
            #endregion

            #region parcels
            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 123456789,
                TargetId = 987654321,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7486,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 987654321,
                TargetId = 234567891,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7486,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 345678912,
                TargetId = 456789123,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7686,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 456789123,
                TargetId = 567891234,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7686,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 567891234,
                TargetId = 678912345,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7686,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 678912345,
                TargetId = 789123456,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7916,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 789123456,
                TargetId = 123456789,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7916,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 456789123,
                TargetId = 123456789,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7216,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 234567891,
                TargetId = 891234567,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 7216,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            parcels[Config.IndexParcel] = new Parcel
            {
                Id = Config.OrdinalParcelNumber++, //serial number
                SenderId = 101123556,
                TargetId = 567891234,
                Weight = (WeightCategories)rand.Next(3),
                priority = (Priorities)rand.Next(3),
                Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29)),
                DroneId = 0,
                scheduled = parcels[Config.IndexParcel].Requested + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                PickedUp = parcels[Config.IndexParcel].scheduled + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
                Delivered = parcels[Config.IndexParcel++].PickedUp + new TimeSpan(rand.Next(24), rand.Next(60), rand.Next(60)),
            };

            #endregion

        }
    }
}
