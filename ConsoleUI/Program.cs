using System;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;
namespace ConsoleUI
{
    class Program
    {

        public enum Menu { EXIT, ADD, UPDATE, DISPLAY, VIEW_ITEM_LIST }
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
                                case 0:
                                    Console.WriteLine("Enter the station details: id, name, longitude,lattitude, chargeSlots");
                                    int id = int.Parse(Console.ReadLine());
                                    int name = int.Parse(Console.ReadLine());
                                    double longitude = double.Parse(Console.ReadLine());
                                    double lattitude = double.Parse(Console.ReadLine());
                                    int chargeSlots = int.Parse(Console.ReadLine());
                                    dal.AddStation(id, name, longitude, lattitude, chargeSlots);
                                    break;

                                case 1:
                                    Console.WriteLine("Enter the drone details: id, model, maxWeight, status, battery");
                                    id = int.Parse(Console.ReadLine());
                                    string model = Console.ReadLine();
                                    WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                    DroneStatuses status = (DroneStatuses)int.Parse(Console.ReadLine());
                                    double battery = double.Parse(Console.ReadLine());
                                    dal.AddDrone(id, model, maxWeight, status, battery);
                                    break;

                                case 2:
                                    Console.WriteLine("Enter the customer details: id, nameCustomer, phone, longitude, lattitude)");
                                    id = int.Parse(Console.ReadLine());
                                    string nameCustomer = Console.ReadLine();
                                    string phone = Console.ReadLine();
                                    longitude = double.Parse(Console.ReadLine());
                                    lattitude = double.Parse(Console.ReadLine());
                                    dal.AddCustomer(id, nameCustomer, phone, longitude, lattitude);
                                    break;

                                case 3:
                                    Console.WriteLine("Enter the parcel details: senderId, targetId, maxWeight, priority, droneId)");
                                    int senderId = int.Parse(Console.ReadLine());
                                    int targetId = int.Parse(Console.ReadLine());
                                    maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                    Priorities priority = (Priorities)int.Parse(Console.ReadLine());
                                    int droneId = int.Parse(Console.ReadLine());
                                    dal.AddParcel(senderId, targetId, maxWeight, priority, droneId);
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
                            case 0:
                                Console.WriteLine("Enter the parcel id for assigning to drone: parcelId");
                                int parceleId = int.Parse(Console.ReadLine());
                                dal.AssigningParcelToDrone(parceleId);
                                break;

                            case 1:
                                Console.WriteLine("Enter the packag collection by drone: parcelId");
                                parceleId = int.Parse(Console.ReadLine());
                                dal.PackagCollectionByDrone(parceleId);
                                break;

                            case 2:
                                Console.WriteLine("Enter the delivery package to customer: parcelId");
                                parceleId = int.Parse(Console.ReadLine());
                                dal.DeliveryPackageToCustomer(parceleId);
                                break;

                            case 3:
                                Console.WriteLine("Enter the sending drone for charging: droneId, stationId");
                                int droneId = int.Parse(Console.ReadLine());
                                int stationId = int.Parse(Console.ReadLine());
                                dal.SendingDroneForCharging(droneId, stationId);
                                break;

                            case 4:
                                Console.WriteLine("Enter the releas drone from charging: droneId, stationId");
                                droneId = int.Parse(Console.ReadLine());
                                stationId = int.Parse(Console.ReadLine());
                                dal.ReleasDroneFromCharging(droneId, stationId);
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
                            case 0:
                                Console.WriteLine("Enter the base station view: stationId");
                                int stationId = int.Parse(Console.ReadLine());
                                Console.WriteLine(dal.BaseStationView(stationId));
                                break;

                            case 1:
                                Console.WriteLine("Enter the drone view: stationId");
                                stationId = int.Parse(Console.ReadLine());
                                Console.WriteLine(dal.DroneView(stationId));
                                break;

                            case 2:
                                Console.WriteLine("Enter the customer view: stationId");
                                stationId = int.Parse(Console.ReadLine());
                                Console.WriteLine(dal.CustomerView(stationId)) ;
                                break;

                            case 3:
                                Console.WriteLine("Enter the parcel view: stationId");
                                stationId = int.Parse(Console.ReadLine());
                                Console.WriteLine(dal.ParcelView(stationId));
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
                            case 0:
                                Console.WriteLine(dal.ListOfBaseStationsView());
                                break;

                            case 1:
                                Console.WriteLine(dal.ListOfDroneView());
                                break;

                            case 2:
                                Console.WriteLine(dal.ListOfCustomerView());
                                break;
                            case 3:
                                Console.WriteLine(dal.ListOfParcelView());
                                break;

                            case 4:
                                Console.WriteLine(dal.ListOfParcelWithoutSpecialDron ());
                                break;

                            case 5:
                                Console.WriteLine(dal.ListOfStationsWithAvailableChargingStations());
                                break;
                            default:
                                break;
                        }
                        break;
                        #endregion
                       
                }
                while (!int.TryParse(Console.ReadLine(), out choice))
                            Console.Write("ERROR, please enter a number again");
            }
        }

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
            Console.WriteLine("Enter 1 for drone displaye");
            Console.WriteLine("Enter 2 for customer view");
            Console.WriteLine("Enter 3 for package view");
        }

        private static void MenuViewItemList()
        {
            Console.WriteLine("Enter 0 for displays the list of drone");
            Console.WriteLine("Enter 1 for view the customer list");
            Console.WriteLine("Enter 2 for displays the list of packages");
            Console.WriteLine("Enter 3 for displays a list of packages that have not yet been assigned to a drone");
            Console.WriteLine("Enter 4 for display base stations with available charging stations");
        }



    }
}
