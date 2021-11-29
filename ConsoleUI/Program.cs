using System;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;
namespace ConsoleUI
{
    class Program
    {
        #region enum
        public enum Menu { EXIT, ADD, UPDATE, DISPLAY, VIEW_ITEM_LIST }
        public enum Add { ADD_STATION, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL }
        public enum Update
        {
            ASSIGNING_PARCEL_TO_DRONE, PARCEL_COLLECTION_BY_DRONE, DELIVERY_PARCEL_TO_CUSTOM,
            SENDING_DRONE_FOR_CHARGING, RELEAS_DRONE_FROME_CHARGING
        }
        public enum Display { DISPLAY_STATION, DISPLAY_DRONE, DISPLAY_CUSTOMER, DISPLAY_PARCEL }
        public enum viewItemList
        {
            LIST_OF_BASE_STATIONS, LIST_OF_DRONE_VIEW, LIST_OF_CUSTOMER_VIEW, LIST_OF_PARCEL_VIEW,
            LIST_OF_PARCEL_WITHOUT_SPECIAL_DRONE, LIST_OF_STATION_WITH_AVAILIBLE_CHARGING_STATION
        }
        #endregion

        static void Main(string[] args)
        {
            DalObject.DalObject dal = new DalObject.DalObject();
            MenuMessages();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
                Console.Write("ERROR, please enter a number again");
            while (choice != 0)
            {
                switch ((Menu)choice)
                {
                    #region add
                    case Menu.ADD:
                        MenuAdd();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Add.ADD_STATION:
                                addStation(dal);
                                break;

                            case (int)Add.ADD_DRONE:
                                addDrone(dal);
                                break;

                            case (int)Add.ADD_CUSTOMER:
                                addCustomer(dal);
                                break;

                            case (int)Add.ADD_PARCEL:
                                addParcel(dal);
                                break;
                            default:
                                break;
                        }

                        break;
                    #endregion

                    #region update
                    case Menu.UPDATE:
                        MenuUpdate();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Update.ASSIGNING_PARCEL_TO_DRONE:
                                assigningParcelToDrone(dal);
                                break;

                            case (int)Update.PARCEL_COLLECTION_BY_DRONE:
                                parcelCollectionByDrone(dal);
                                break;

                            case (int)Update.DELIVERY_PARCEL_TO_CUSTOM:
                                deliveryPackageToCustomer(dal);
                                break;

                            case (int)Update.SENDING_DRONE_FOR_CHARGING:
                                sendingDroneForCharging(dal);
                                break;

                            case (int)Update.RELEAS_DRONE_FROME_CHARGING:
                                releasDroneFromCharging(dal);
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion

                    #region display
                    case Menu.DISPLAY:
                        MenuDisplay();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Display.DISPLAY_STATION:
                                displayStation(dal);
                                break;

                            case (int)Display.DISPLAY_DRONE:
                                displayDrone(dal);
                                break;

                            case (int)Display.DISPLAY_CUSTOMER:

                                displayCustomer(dal);
                                break;

                            case (int)Display.DISPLAY_PARCEL:
                                displayParcel(dal);
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion

                    #region viewItemList
                    case Menu.VIEW_ITEM_LIST:
                        MenuViewItemList();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)viewItemList.LIST_OF_BASE_STATIONS:
                                foreach (var temp in dal.GetAllBaseStations())
                                    Console.WriteLine(temp);
                                break;

                            case (int)viewItemList.LIST_OF_DRONE_VIEW:
                                foreach (var temp in dal.GetAllDrones())
                                    Console.WriteLine(temp);
                                break;

                            case (int)viewItemList.LIST_OF_CUSTOMER_VIEW:
                                foreach (var temp in dal.GetAllCustomers())
                                    Console.WriteLine(temp);
                                break;
                            case (int)viewItemList.LIST_OF_PARCEL_VIEW:
                                foreach (var temp in dal.GetAllParcels())
                                    Console.WriteLine(temp);
                                break;

                            case (int)viewItemList.LIST_OF_PARCEL_WITHOUT_SPECIAL_DRONE:
                                foreach (var temp in dal.GetAllParcelsWithoutSpecialDron())
                                    Console.WriteLine(temp);
                                break;
                            case (int)viewItemList.LIST_OF_STATION_WITH_AVAILIBLE_CHARGING_STATION:
                                foreach (var temp in dal.GetAllStationsWithAvailableChargingStations())
                                    Console.WriteLine(temp);
                                break;
                            default:
                                break;
                        }
                        break;
                        #endregion

                }
                MenuMessages();
                while (!int.TryParse(Console.ReadLine(), out choice))
                    Console.Write("ERROR, please enter a number again");
            }
        }


        #region menu
        /// <summary>
        /// User messages
        /// </summary>
        private static void MenuMessages()
        {
            Console.WriteLine("Enter 1 for add");
            Console.WriteLine("Enter 2 for update ");
            Console.WriteLine("Enter 3 display");
            Console.WriteLine("Enter 4 view an item list");
            Console.WriteLine("Enter 0 for exit");
        }

        private static void MenuAdd()
        {
            Console.WriteLine("Enter 0 for add a base station to the list of stations");
            Console.WriteLine("Enter 1 for add a drone to the list of existing drone");
            Console.WriteLine("Enter 2 for receiving a new customer for an information list");
            Console.WriteLine("Enter 3 for receipt of package for shipment");
        }

        private static void MenuUpdate()
        {
            Console.WriteLine("Enter 0 for assign a package to a drone");
            Console.WriteLine("Enter 1 for collection of a package by drone");
            Console.WriteLine("Enter 2 for delivery package to customer");
            Console.WriteLine("Enter 3 for sending a drone for charging at a base station");
            Console.WriteLine("Enter 4 for release drone from charging at base station");
        }

        private static void MenuDisplay()
        {
            Console.WriteLine("Enter 0 for base Station View");
            Console.WriteLine("Enter 1 for drone view");
            Console.WriteLine("Enter 2 for customer view");
            Console.WriteLine("Enter 3 for parcel view");
        }

        private static void MenuViewItemList()
        {
            Console.WriteLine("Enter 0 for displays a list of base stations");
            Console.WriteLine("Enter 1 for displays a list of drones");
            Console.WriteLine("Enter 2 for displays a list of customers");
            Console.WriteLine("Enter 3 for displays a list of parcels");
            Console.WriteLine("Enter 4 for displays a list of parcels that have not yet been assigned to the drone");
            Console.WriteLine("Enter 5 for display of base stations with available charging stations");
        }
        #endregion

        #region add item method
        /// <summary>
        /// Receives input and activates the functions of the dal layer
        /// </summary>
        /// <param name="dal">On this standby the functions are activated</param>
        private static void addParcel(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the parcel details: senderId, targetId, maxWeight, priority, droneId)");
            int senderId = int.Parse(Console.ReadLine());
            int targetId = int.Parse(Console.ReadLine());
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            Priorities priority = (Priorities)int.Parse(Console.ReadLine());
            int droneId = int.Parse(Console.ReadLine());
            dal.AddParcel(senderId, targetId, maxWeight, priority, droneId);
        }

        private static void addCustomer(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the customer details: id, nameCustomer, phone, longitude, lattitude)");
            int id = int.Parse(Console.ReadLine());
            string nameCustomer = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            dal.AddCustomer(id, nameCustomer, phone, longitude, lattitude);
        }

        private static void addDrone(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the drone details: id, model, maxWeight, status, battery");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            //DroneStatuses status = (DroneStatuses)int.Parse(Console.ReadLine());
            //double battery = double.Parse(Console.ReadLine());
            dal.AddDrone(id, model, maxWeight/*, status,  battery*/);
        }

        private static void addStation(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the station details: id, name, longitude,lattitude, chargeSlots");
            int id = int.Parse(Console.ReadLine());
            int name = int.Parse(Console.ReadLine());
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            int chargeSlots = int.Parse(Console.ReadLine());
            dal.AddStation(id, name, longitude, lattitude, chargeSlots);
        }
        #endregion

        #region update method

        private static void releasDroneFromCharging(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the releas drone from charging: droneId, stationId");
            int droneId = int.Parse(Console.ReadLine());
            int stationId = int.Parse(Console.ReadLine());
            dal.ReleasDroneFromCharging(droneId, stationId);
        }

        private static void sendingDroneForCharging(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the sending drone for charging: droneId, stationId");
            int droneId = int.Parse(Console.ReadLine());
            int stationId = int.Parse(Console.ReadLine());
            dal.SendingDroneForCharging(droneId, stationId);
        }

        private static void deliveryPackageToCustomer(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the delivery package to customer: parcelId");
            int parceleId = int.Parse(Console.ReadLine());
            dal.DeliveryPackageToCustomer(parceleId);
        }

        private static void parcelCollectionByDrone(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the packag collection by drone: parcelId");
            int parceleId = int.Parse(Console.ReadLine());
            dal.PackagCollectionByDrone(parceleId);
        }

        private static void assigningParcelToDrone(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the parcel id for assigning to drone: parcelId , droneId");
            int parceleId = int.Parse(Console.ReadLine());
            int droneID = int.Parse(Console.ReadLine());
            dal.AssigningParcelToDrone(parceleId,droneID);
        }
        #endregion

        #region display method

        private static void displayParcel(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the parcel view: parcelId");
            int parcelId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetParcel(parcelId));
        }

        private static void displayCustomer(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the customer view: customerId");
            int customerId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetCustomer(customerId));
        }

        private static void displayDrone(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the drone view: droneId");
            int droneId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetDrone(droneId));
        }

        private static void displayStation(DalObject.DalObject dal)
        {
            Console.WriteLine("Enter the base station view: stationId");
            int stationId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetBaseStation(stationId));
        }


        #endregion

    }
}
