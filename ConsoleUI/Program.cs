using System;
using DalApi;
using System.Collections.Generic;
using DO;

namespace ConsoleUI
{
    class Program
    {
        #region enum
        public enum Menu { Exit, Add, Update, Display, ViewItemList }
        public enum Add { AddStation, AddDrone, AddCustomer, AddParcel }
        public enum Update
        {
            AssigningParcelToDrone, ParcelCollectionByDrone, DeliveryParcelToCustomer,
            SendingDroneForCharging, ReleasDroneFromCharging
        }
        public enum Display { DisplayStation, DisplayDrone, DisplayCustomer, DisplayParcel }
        public enum ViewItemList
        {
            BaseStationList, DroneList, CustomerList, ParcelList,
            ListOfParcelWithoutSpecificDrone, ListOfStationWithAvailibleChargingStation
        }
        #endregion

        internal static IDal dal;
        static void Main(string[] args)
        {
            dal = DalFactory.GetDal();
            menuMessages();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
                Console.Write("ERROR, please enter a number again");
            while (choice != 0)
            {
                switch ((Menu)choice)
                {
                    #region add
                    case Menu.Add:
                        menuAdd();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Add.AddStation:
                                addStation(dal);
                                break;

                            case (int)Add.AddDrone:
                                addDrone(dal);
                                break;

                            case (int)Add.AddCustomer:
                                addCustomer(dal);
                                break;

                            case (int)Add.AddParcel:
                                addParcel(dal);
                                break;
                            default:
                                break;
                        }

                        break;
                    #endregion

                    #region update
                    case Menu.Update:
                        menuUpdate();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Update.AssigningParcelToDrone:
                                assigningParcelToDrone(dal);
                                break;

                            case (int)Update.ParcelCollectionByDrone:
                                parcelCollectionByDrone(dal);
                                break;

                            case (int)Update.DeliveryParcelToCustomer:
                                deliveryPackageToCustomer(dal);
                                break;

                            case (int)Update.SendingDroneForCharging:
                                sendingDroneForCharging(dal);
                                break;

                            case (int)Update.ReleasDroneFromCharging:
                                releasDroneFromCharging(dal);
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion

                    #region display
                    case Menu.Display:
                        menuDisplay();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Display.DisplayStation:
                                displayStation(dal);
                                break;

                            case (int)Display.DisplayDrone:
                                displayDrone(dal);
                                break;

                            case (int)Display.DisplayCustomer:

                                displayCustomer(dal);
                                break;

                            case (int)Display.DisplayParcel:
                                displayParcel(dal);
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion

                    #region viewItemList
                    case Menu.ViewItemList:
                        menuViewItemList();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)ViewItemList.BaseStationList:
                                foreach (var temp in dal.GetAllBaseStations())
                                    Console.WriteLine(temp);
                                break;

                            case (int)ViewItemList.DroneList:
                                foreach (var temp in dal.GetAllDrones())
                                    Console.WriteLine(temp);
                                break;

                            case (int)ViewItemList.CustomerList:
                                foreach (var temp in dal.GetAllCustomers())
                                    Console.WriteLine(temp);
                                break;
                            case (int)ViewItemList.ParcelList:
                                foreach (var temp in dal.GetAllParcels())
                                    Console.WriteLine(temp);
                                break;

                            case (int)ViewItemList.ListOfParcelWithoutSpecificDrone:
                                foreach (var temp in dal.GetAllParcelsWithoutSpecialDron(parcel=>parcel.ParcelID > 0 && parcel.DroneID == 0))
                                    Console.WriteLine(temp);
                                break;
                            case (int)ViewItemList.ListOfStationWithAvailibleChargingStation:
                                foreach (var temp in dal.GetAllStationsBy(station => station.ChargeSlots > 0))
                                    Console.WriteLine(temp);
                                break;
                            default:
                                break;
                        }
                        break;
                        #endregion

                }
                menuMessages();
                while (!int.TryParse(Console.ReadLine(), out choice))
                    Console.Write("ERROR, please enter a number again");
            }
        }


        #region menu
        /// <summary>
        /// User messages
        /// </summary>
        private static void menuMessages()
        {
            Console.WriteLine("Enter 1 for add");
            Console.WriteLine("Enter 2 for update ");
            Console.WriteLine("Enter 3 display");
            Console.WriteLine("Enter 4 view an item list");
            Console.WriteLine("Enter 0 for exit");
        }

        private static void menuAdd()
        {
            Console.WriteLine("Enter 0 for add a base station to the list of stations");
            Console.WriteLine("Enter 1 for add a drone to the list of existing drone");
            Console.WriteLine("Enter 2 for receiving a new customer for an information list");
            Console.WriteLine("Enter 3 for receipt of package for shipment");
        }

        private static void menuUpdate()
        {
            Console.WriteLine("Enter 0 for assign a package to a drone");
            Console.WriteLine("Enter 1 for collection of a package by drone");
            Console.WriteLine("Enter 2 for delivery package to customer");
            Console.WriteLine("Enter 3 for sending a drone for charging at a base station");
            Console.WriteLine("Enter 4 for release drone from charging at base station");
        }

        private static void menuDisplay()
        {
            Console.WriteLine("Enter 0 for base Station View");
            Console.WriteLine("Enter 1 for drone view");
            Console.WriteLine("Enter 2 for customer view");
            Console.WriteLine("Enter 3 for parcel view");
        }

        private static void menuViewItemList()
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
        private static void addParcel(DalApi.IDal dal)
        {
            Console.WriteLine("Enter the parcel details: senderId, targetId, maxWeight, Priority, droneId)");
            int senderId = int.Parse(Console.ReadLine());
            int targetId = int.Parse(Console.ReadLine());
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            Priorities priority = (Priorities)int.Parse(Console.ReadLine());
            int droneId = int.Parse(Console.ReadLine());
            dal.AddParcel(senderId, targetId, maxWeight, priority, droneId);
        }

        private static void addCustomer(IDal dal)
        {
            Console.WriteLine("Enter the customer details: id, nameCustomer, phone, longitude, lattitude)");
            int id = int.Parse(Console.ReadLine());
            string nameCustomer = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            dal.AddCustomer(id, nameCustomer, phone, longitude, lattitude);
        }

        private static void addDrone(IDal dal)
        {
            Console.WriteLine("Enter the drone details: id, model, maxWeight, status, battery");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            //DroneStatuses status = (DroneStatuses)int.Parse(Console.ReadLine());
            //double battery = double.Parse(Console.ReadLine());
            dal.AddDrone(id, model, maxWeight/*, status,  battery*/);
        }

        private static void addStation(IDal dal)
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

        private static void releasDroneFromCharging(IDal dal)
        {
            Console.WriteLine("Enter the releas drone from charging: droneId, stationId");
            int droneId = int.Parse(Console.ReadLine());
            int stationId = int.Parse(Console.ReadLine());
            dal.ReleasDroneFromCharging(droneId, stationId);
        }

        private static void sendingDroneForCharging(IDal dal)
        {
            Console.WriteLine("Enter the sending drone for charging: droneId, stationId");
            int droneId = int.Parse(Console.ReadLine());
            int stationId = int.Parse(Console.ReadLine());
            dal.SendingDroneForCharging(droneId, stationId);
        }

        private static void deliveryPackageToCustomer(IDal dal)
        {
            Console.WriteLine("Enter the delivery package to customer: parcelId");
            int parceleId = int.Parse(Console.ReadLine());
            dal.DeliveryPackageToCustomer(parceleId);
        }

        private static void parcelCollectionByDrone(IDal dal)
        {
            Console.WriteLine("Enter the packag collection by drone: parcelId");
            int parceleId = int.Parse(Console.ReadLine());
            dal.PackagCollectionByDrone(parceleId);
        }

        private static void assigningParcelToDrone(IDal dal)
        {
            Console.WriteLine("Enter the parcel id for assigning to drone: parcelId , droneId");
            int parceleId = int.Parse(Console.ReadLine());
            int droneID = int.Parse(Console.ReadLine());
            dal.AssigningParcelToDrone(parceleId, droneID);
        }
        #endregion

        #region display method

        private static void displayParcel(IDal dal)
        {
            Console.WriteLine("Enter the parcel view: parcelId");
            int parcelId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetParcel(parcelId));
        }

        private static void displayCustomer(IDal dal)
        {
            Console.WriteLine("Enter the customer view: customerId");
            int customerId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetCustomer(customerId));
        }

        private static void displayDrone(IDal dal)
        {
            Console.WriteLine("Enter the drone view: droneId");
            int droneId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetDrone(droneId));
        }

        private static void displayStation(IDal dal)
        {
            Console.WriteLine("Enter the base station view: stationId");
            int stationId = int.Parse(Console.ReadLine());
            Console.WriteLine(dal.GetBaseStation(stationId));
        }


        #endregion

    }
}
