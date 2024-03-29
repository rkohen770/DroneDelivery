﻿using DO;
using System;
using System.Collections.Generic;

namespace DL
{
    class DataSource
    {
        internal static List<Drone> drones = new List<Drone>(10);
        internal static List<Station> stations = new List<Station>(5);
        internal static List<Customer> customers = new List<Customer>(100);
        internal static List<Parcel> parcels = new List<Parcel>(1000);
        internal static List<User> users = new List<User>(10000);
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>(5);
        static Random rand = new();


        internal class Config
        {
            internal static double Light = 3.4;
            internal static double Intermediate = 3.7;
            internal static double Heavy = 4.3;
            internal static double Available = 3.2;
            internal static double DroneChargingRate = 10;

            internal static int SerialNumber = 10000;

            internal static Random r = new Random();
            internal static DateTime currentDate = DateTime.Now;

        }
        public static void Initialize()
        {
            #region stations
            stations = new List<Station>
            {
                new Station
                {
                    StationID = 11212,
                    StationName = 11,
                    //Grills values that are within the borders of the State of Israel
                    Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                    Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                    ChargeSlots = 5,
                    Available=true,

                },
                new Station
                {
                    StationID = 22212,
                    StationName = 22,
                    //Grills values that are within the borders of the State of Israel
                    Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                    Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                    ChargeSlots = 3,
                    Available=true,
                }
            };
            #endregion

            #region drones
            drones = new List<Drone>
            {
                new Drone
                {
                    DroneID = 7486,
                    DroneModel = "EG-574",
                    MaxWeight = WeightCategories.Intermediate,
                    Available=true,


                },
                new Drone
                {
                    DroneID = 7686,
                    DroneModel = "EG-574",
                    MaxWeight = WeightCategories.Easy,
                    Available=true,


                },
                new Drone
                {
                    DroneID = 7916,
                    DroneModel = "EG-474",
                    MaxWeight = WeightCategories.Liver,
                    Available=true,

                },
                new Drone
                {
                    DroneID = 7216,
                    DroneModel = "EG-474",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    //Battery = 75
                    Available=true,
                },
                new Drone
                {
                    DroneID = 7945,
                    DroneModel = "EG-474",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    //Battery = 98.9
                    Available=true,

                }
            };
            #endregion

            #region customer
            customers = new List<Customer>
            {
                new Customer
                {
                  CustomerID = 123456789,
                  Name = "Eliya",
                  Phone = "0547689612",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                   Available=true,

                },
                new Customer
                {
                  CustomerID = 987654321,
                  Name = "Devora",
                  Phone = "0512926771",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                   Available=true,

                },

                new Customer
                {
                  CustomerID = 234567891,
                  Name = "Rachel Lea",
                  Phone = "0585848441",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                  Available=true,

                },

                new Customer
                {
                  CustomerID = 345678912,
                  Name = "Yanir",
                  Phone = "0503344684",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,   
                  Available=true,

                },

                new Customer
                {
                  CustomerID = 456789123,
                  Name = "Reuven",
                  Phone = "0526795861",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3, 
                  Available=true,

                },

                new Customer
                {
                  CustomerID = 567891234,
                  Name = "Ilana",
                  Phone = "0548596887",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                  Available=true,

                },

                new Customer
                {
                   CustomerID = 678912345,
                   Name = "Dalya",
                   Phone = "0558724551",
                   //Grills values that are within the borders of the State of Israel
                   Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                   Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,  
                   Available=true,

                },

                new Customer
                {
                  CustomerID = 789123456,
                  Name = "Aviya",
                  Phone = "0518846523",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,   
                  Available=true,

                },

                new Customer
                {
                  CustomerID = 891234567,
                  Name = "Baruch",
                  Phone = "0558742215",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,   
                  Available=true,
                },

                new Customer
                {
                  CustomerID = 101123556,
                  Name = "Chen",
                  Phone = "0589868554",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3, 
                  Available=true,

                }

            };
            #endregion

            #region parcels
            parcels = new List<Parcel>
            {
                new Parcel
                {
                   ParcelID =Config.SerialNumber++, //serial number
                   SenderID = 123456789,
                   TargetID = 987654321,
                   Weight = (WeightCategories)rand.Next(3),
                   Priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 8,27,8,30,11),
                   DroneID = 7486,
                   Scheduled = new DateTime(2021, 8, 27, 13, 39, 53),
                   PickedUp = new DateTime(2021, 8, 27, 15, 30, 26),
                   Delivered = new DateTime(2021, 8, 27, 21, 46, 11),
                   Available=true,
                },

                new Parcel
                {
                   ParcelID = Config.SerialNumber++, //serial number
                   SenderID = 987654321,
                   TargetID = 234567891,
                   Weight = (WeightCategories)rand.Next(3),
                   Priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 8, 25, 7, 11, 26),
                   DroneID = 7486,
                   Scheduled = new DateTime(2021, 8, 25, 8, 30, 11), 
                   Available=true,


                },

                new Parcel
                {
                  ParcelID = Config.SerialNumber++, //serial number
                  SenderID = 345678912,
                  TargetID = 456789123,
                  Weight = (WeightCategories)rand.Next(3),
                  Priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 8, 8, 5, 30, 11),
                  DroneID = 7686,
                  Scheduled = new DateTime(2021, 8, 8, 5, 56, 54),
                  PickedUp = new DateTime(2021, 8, 8, 7, 45, 18),
                  Delivered = new DateTime(2021, 8, 8, 15, 25, 48),
                  Available=true,

                },

                new Parcel
                {
                  ParcelID = Config.SerialNumber++, //serial number
                  SenderID = 456789123,
                  TargetID = 567891234,
                  Weight = (WeightCategories)rand.Next(3),
                  Priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 10, 15, 12, 28, 36),
                  DroneID = 7686,
                  Scheduled = new DateTime(2021, 10, 15, 13, 47, 16),
                  PickedUp = new DateTime(2021, 10, 15, 16, 25, 5),
                  Delivered = null,   
                  Available=true,

                },

                new Parcel
                {
                  ParcelID = Config.SerialNumber++, //serial number
                  SenderID = 567891234,
                  TargetID = 678912345,
                  Weight = (WeightCategories)rand.Next(3),
                  Priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 10, 2, 15, 26, 16),
                  DroneID = 7686,
                  Scheduled = new DateTime(2021, 10, 2, 16, 47, 45),
                  PickedUp = new DateTime(2021, 10, 2, 21, 54, 13),
                  Delivered = new DateTime(2021, 10, 2, 22, 47, 25), 
                  Available=true,

                },

                new Parcel
                {
                   ParcelID = Config.SerialNumber++, //serial number
                   SenderID = 678912345,
                   TargetID = 789123456,
                   Weight = (WeightCategories)rand.Next(3),
                   Priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 5, 4, 3, 36, 35),
                   DroneID = 7916,
                   Scheduled = new DateTime(2021, 5, 4, 6, 45, 12),
                   PickedUp = new DateTime(2021, 5, 4, 8, 52, 15),
                   Delivered = new DateTime(2021, 5, 4, 3, 36, 35),    
                   Available=true,

                },

                new Parcel
                {
                   ParcelID = Config.SerialNumber++, //serial number
                   SenderID = 789123456,
                   TargetID = 123456789,
                   Weight = (WeightCategories)rand.Next(3),
                   Priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 12, 21, 12, 25, 35),
                   DroneID = 7916,
                   Scheduled = new DateTime(2021, 12, 21, 14, 25, 32),
                   PickedUp = new DateTime(2021, 12, 21, 16, 31, 35),
                   Delivered = new DateTime(2021, 12, 21, 14, 56, 23),    
                   Available=true,

                },

                new Parcel
                {
                   ParcelID = Config.SerialNumber++, //serial number
                   SenderID = 456789123,
                   TargetID = 123456789,
                   Weight = (WeightCategories)rand.Next(3),
                   Priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 6, 29, 16, 25, 35),
                   DroneID = 7216,
                   Scheduled = new DateTime(2021, 6, 29, 18, 36, 35),
                   PickedUp = new DateTime(2021, 6, 29, 19, 25, 54),
                   Delivered = new DateTime(2021, 6, 29, 20, 12, 20),      
                   Available=true,

                },

                new Parcel
                {
                  ParcelID = Config.SerialNumber++, //serial number
                  SenderID = 234567891,
                  TargetID = 891234567,
                  Weight = (WeightCategories)rand.Next(3),
                  Priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 4, 21, 11, 25, 45),
                  DroneID = 7216,
                  Scheduled = new DateTime(2021, 4, 21, 11, 58, 45),
                  PickedUp = new DateTime(2021, 4, 21, 14, 12, 11),
                  Delivered = new DateTime(2021, 4, 21, 16, 10, 17),  
                  Available=true,

                },

                new Parcel
                {
                  ParcelID = Config.SerialNumber++, //serial number
                  SenderID = 101123556,
                  TargetID = 567891234,
                  Weight = (WeightCategories)rand.Next(3),
                  Priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 2, 2, 19, 52, 21),
                  DroneID = 0,
                  Scheduled = null,
                  PickedUp = null,
                  Delivered =null,    
                  Available=true,

                }
            };

            #endregion

            #region user
            users = new List<User>
            {
                new User()
                {
                    UserName="Eliya",
                    Admin = Permission.Client,
                    Password ="1111",
                    Available =true
                },
                new User()
                {
                    UserName="Devora",
                    Admin = Permission.Managment,
                    Password ="315",
                    Available =true,
                },
                new User()
                {
                    UserName="Rachel Lea",
                    Admin = Permission.Managment,
                    Password ="770770",
                    Available =true,
                },
                new User()
                {
                    UserName="Yanir",
                    Admin = Permission.Client,
                    Password ="2222",
                    Available =true,
                },
                new User()
                {
                    UserName="Reuven",
                    Admin = Permission.Client,
                    Password ="3333",
                    Available =true,
                },
                new User()
                {
                    UserName="Ilana",
                    Admin = Permission.Client,
                    Password ="4444",
                    Available =true,
                },
                new User()
                {
                    UserName="Dalya",
                    Admin = Permission.Client,
                    Password ="5555",
                    Available =true,
                },
                new User()
                {
                    UserName="Aviya",
                    Admin = Permission.Client,
                    Password ="6666",
                    Available =true,
                },
                new User()
                {
                    UserName="Baruch",
                    Admin = Permission.Client,
                    Password ="7777",
                    Available =true,
                },
                new User()
                {
                    UserName="Chen",
                    Admin = Permission.Client,
                    Password ="8888",
                    Available =true,
                },
            };
            #endregion

            #region DroneCharge
            droneCharges = new List<DroneCharge>
            {
                new DroneCharge()
                {
                    DroneID=7486,
                    StationID=11212,
                    Available =false
                },
                new DroneCharge()
                {
                    DroneID=7686,
                    StationID=11212,
                    Available =false
                },
            };
            #endregion
        }
    }
}