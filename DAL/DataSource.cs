using IDAL.DO;
using System;
using System.Collections.Generic;

namespace DalObject
{
    class DataSource
    {
        internal static List<Drone> drones = new List<Drone>(10);
        internal static List<Station> stations = new List<Station>(5);
        internal static List<Customer> customers = new List<Customer>(100);
        internal static List<Parcel> parcels = new List<Parcel>(1000);
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>(5);
        static Random rand = new();

        internal class Config
        {
            public static int OrdinalParcelNumber = 10000;

            public static double vacant = 0.01;
            public static double CarriesLightWeight = 0.015;
            public static double CarriesMediumWeight = 0.018;
            public static double CarriesHeavyWeight=0.02;
            public static double DroneChargingRate = 0.003;
        }

        public static void Initialize()
        {
            #region stations
            stations = new List<Station>
            {
                new Station
                {
                    Id = 11212,
                    Name = 11,
                    //Grills values that are within the borders of the State of Israel
                    Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                    Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                    ChargeSlots = 5
                },
                new Station
                {
                    Id = 22212,
                    Name = 22,
                    //Grills values that are within the borders of the State of Israel
                    Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                    Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                    ChargeSlots = 3
                }
            };
            #endregion

            #region drones
            drones = new List<Drone>
            {
                new Drone
                {
                    Id = 7486,
                    Model = "EG-574",
                    MaxWeight = WeightCategories.Liver
                    
                },
                new Drone
                {
                    Id = 7686,
                    Model = "EG-574",
                    MaxWeight = WeightCategories.Liver
                   
                },
                new Drone
                {
                    Id = 7916,
                    Model = "EG-474",
                    MaxWeight = WeightCategories.Liver
                },
                new Drone
                {
                    Id = 7216,
                    Model = "EG-474",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    //Battery = 75
                },
                new Drone
                {
                    Id = 7945,
                    Model = "EG-474",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    //Status = (DroneStatuses)rand.Next(3),
                    //Battery = 98.9
                }
            };
            #endregion

            #region customer
            customers = new List<Customer> 
            {
                new Customer
                {
                  Id = 123456789,
                  Name = "Eliya",
                  Phone = "0547689612",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },
                new Customer
                {
                  Id = 987654321,
                  Name = "Devora",
                  Phone = "0512926771",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 234567891,
                  Name = "Rachel Lea",
                  Phone = "0585848441",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 345678912,
                  Name = "Yanir",
                  Phone = "0503344684",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 456789123,
                  Name = "Reuven",
                  Phone = "0526795861",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 567891234,
                  Name = "Ilana",
                  Phone = "0548596887",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                   Id = 678912345,
                   Name = "Dalya",
                   Phone = "0558724551",
                   //Grills values that are within the borders of the State of Israel
                   Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                   Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 789123456,
                  Name = "Aviya",
                  Phone = "0518846523",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 891234567,
                  Name = "Baruch",
                  Phone = "0558742215",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                },

                new Customer
                {
                  Id = 101123556,
                  Name = "Chen",
                  Phone = "0589868554",
                  //Grills values that are within the borders of the State of Israel
                  Longitude = rand.NextDouble() * (31 + 33.3) - 31,
                  Lattitude = rand.NextDouble() * (34.3 + 35.5) - 34.3,
                }
                
            };
            #endregion

            #region parcels
            parcels = new List<Parcel>
            {
                new Parcel
                {
                   Id = Config.OrdinalParcelNumber++, //serial number
                   SenderId = 123456789,
                   TargetId = 987654321,
                   Weight = (WeightCategories)rand.Next(3),
                   priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 8,27,8,30,11),
                   DroneId = 7486,
                   Scheduled = new DateTime(2021, 8, 27, 13, 39, 53),
                   PickedUp = new DateTime(2021, 8, 27, 15, 30, 26),
                   Delivered = new DateTime(2021, 8, 27, 21, 46, 11),
                },

                new Parcel
                {
                   Id = Config.OrdinalParcelNumber++, //serial number
                   SenderId = 987654321,
                   TargetId = 234567891,
                   Weight = (WeightCategories)rand.Next(3),
                   priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 8, 25, 7, 11, 26),
                   DroneId = 7486,
                   Scheduled = new DateTime(2021, 8, 25, 8, 30, 11),
                   
                },

                new Parcel
                {
                  Id = Config.OrdinalParcelNumber++, //serial number
                  SenderId = 345678912,
                  TargetId = 456789123,
                  Weight = (WeightCategories)rand.Next(3),
                  priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 8, 8, 5, 30, 11),
                  DroneId = 7686,
                  Scheduled = new DateTime(2021, 8, 8, 5, 56, 54),
                  PickedUp = new DateTime(2021, 8, 8, 7, 45, 18),
                  Delivered = new DateTime(2021, 8, 8, 15, 25, 48),
                },

                new Parcel
                {
                  Id = Config.OrdinalParcelNumber++, //serial number
                  SenderId = 456789123,
                  TargetId = 567891234,
                  Weight = (WeightCategories)rand.Next(3),
                  priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 10, 15, 12, 28, 36),
                  DroneId = 7686,
                  Scheduled = new DateTime(2021, 10, 15, 13, 47, 16),
                  PickedUp = new DateTime(2021, 10, 15, 16, 25, 5),
                  Delivered = null,
                },

                new Parcel
                {
                  Id = Config.OrdinalParcelNumber++, //serial number
                  SenderId = 567891234,
                  TargetId = 678912345,
                  Weight = (WeightCategories)rand.Next(3),
                  priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 10, 2, 15, 26, 16),
                  DroneId = 7686,
                  Scheduled = new DateTime(2021, 10, 2, 16, 47, 45),
                  PickedUp = new DateTime(2021, 10, 2, 21, 54, 13),
                  Delivered = new DateTime(2021, 10, 2, 22, 47, 25),
                },

                new Parcel
                {
                   Id = Config.OrdinalParcelNumber++, //serial number
                   SenderId = 678912345,
                   TargetId = 789123456,
                   Weight = (WeightCategories)rand.Next(3),
                   priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 5, 4, 3, 36, 35),
                   DroneId = 7916,
                   Scheduled = new DateTime(2021, 5, 4, 6, 45, 12),
                   PickedUp = new DateTime(2021, 5, 4, 8, 52, 15),
                   Delivered = new DateTime(2021, 5, 4, 3, 36, 35),
                },

                new Parcel
                {
                   Id = Config.OrdinalParcelNumber++, //serial number
                   SenderId = 789123456,
                   TargetId = 123456789,
                   Weight = (WeightCategories)rand.Next(3),
                   priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 12, 21, 12, 25, 35),
                   DroneId = 7916,
                   Scheduled = new DateTime(2021, 12, 21, 14, 25, 32),
                   PickedUp = new DateTime(2021, 12, 21, 16, 31, 35),
                   Delivered = new DateTime(2021, 12, 21, 14, 56, 23),
                },

                new Parcel
                {
                   Id = Config.OrdinalParcelNumber++, //serial number
                   SenderId = 456789123,
                   TargetId = 123456789,
                   Weight = (WeightCategories)rand.Next(3),
                   priority = (Priorities)rand.Next(3),
                   Requested = new DateTime(2021, 6, 29, 16, 25, 35),
                   DroneId = 7216,
                   Scheduled = new DateTime(2021, 6, 29, 18, 36, 35),
                   PickedUp = new DateTime(2021, 6, 29, 19, 25, 54),
                   Delivered = new DateTime(2021, 6, 29, 20, 12, 20),
                },

                new Parcel
                {
                  Id = Config.OrdinalParcelNumber++, //serial number
                  SenderId = 234567891,
                  TargetId = 891234567,
                  Weight = (WeightCategories)rand.Next(3),
                  priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 4, 21, 11, 25, 45),
                  DroneId = 7216,
                  Scheduled = new DateTime(2021, 4, 21, 11, 58, 45),
                  PickedUp = new DateTime(2021, 4, 21, 14, 12, 11),
                  Delivered = new DateTime(2021, 4, 21, 16, 10, 17),
                },

                new Parcel
                {
                  Id = Config.OrdinalParcelNumber++, //serial number
                  SenderId = 101123556,
                  TargetId = 567891234,
                  Weight = (WeightCategories)rand.Next(3),
                  priority = (Priorities)rand.Next(3),
                  Requested = new DateTime(2021, 2, 2, 19, 52, 21),
                  DroneId = 0,
                  Scheduled = null,
                  PickedUp = null,
                  Delivered =null,
                }
            };

            #endregion

        }
    }
}
