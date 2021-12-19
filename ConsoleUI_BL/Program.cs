using System;
using BLApi.BO;
using BL;

namespace ConsoleUI_BL
{
    class Program
    {
        #region enum
        public enum Menu { EXIT, ADD, UPDATE, DISPLAY, VIEW_ITEM_LIST }
        public enum Add { ADD_BASE_STATION, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL_FOR_DELIVERY }
        public enum Update
        {
            UPDATE_DRONE_DATA, UPDATE_BASE_STATION_DATA, UPDATE_CUSTOMER_DATA,
            SENDING_DRONE_FOR_CHARGING, RELEAS_DRONE_FROME_CHARGING, ASSIGN_PARCEL_TO_DRONE,
            COLLECTION_PARCEL_BY_DRONE, DELIVERY_PARCEL_BY_DRONE
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
            BlObject bl = new BlObject();
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
                            case (int)Add.ADD_BASE_STATION:
                                addStation(bl);
                                break;

                            case (int)Add.ADD_DRONE:
                                addDrone(bl);
                                break;

                            case (int)Add.ADD_CUSTOMER:
                                addCustomer(bl);
                                break;

                            case (int)Add.ADD_PARCEL_FOR_DELIVERY:
                                addParcel(bl);
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
                            case (int)Update.UPDATE_DRONE_DATA:
                                updateDrone(bl);
                                break;

                            case (int)Update.UPDATE_BASE_STATION_DATA:
                                updateBaseStation(bl);
                                break;

                            case (int)Update.UPDATE_CUSTOMER_DATA:
                                updateCustomer(bl);
                                break;

                            case (int)Update.SENDING_DRONE_FOR_CHARGING:
                                sendingDroneForCharging(bl);
                                break;

                            case (int)Update.RELEAS_DRONE_FROME_CHARGING:
                                releasDroneFromCharging(bl);
                                break;

                            case (int)Update.ASSIGN_PARCEL_TO_DRONE:
                                assignParcelToDrone(bl);
                                break;

                            case (int)Update.COLLECTION_PARCEL_BY_DRONE:
                                collectionParcelByDrone(bl);
                                break;

                            case (int)Update.DELIVERY_PARCEL_BY_DRONE:
                                deliveryParcelByDrone(bl);
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion

                    #region display method
                    case Menu.DISPLAY:
                        MenuDisplay();
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case (int)Display.DISPLAY_STATION:
                                displayStation(bl);
                                break;

                            case (int)Display.DISPLAY_DRONE:
                                displayDrone(bl);
                                break;

                            case (int)Display.DISPLAY_CUSTOMER:
                                displayCustomer(bl);
                                break;

                            case (int)Display.DISPLAY_PARCEL:
                                displayParcel(bl);
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
                                foreach (var temp in bl.GetAllBaseStationsBo())
                                    Console.WriteLine(temp);
                                break;

                            case (int)viewItemList.LIST_OF_DRONE_VIEW:
                                foreach (var temp in bl.GetAllDronesBo())
                                    Console.WriteLine(temp);
                                break;

                            case (int)viewItemList.LIST_OF_CUSTOMER_VIEW:
                                foreach (var temp in bl.GetAllCustomersBo())
                                    Console.WriteLine(temp);
                                break;
                            case (int)viewItemList.LIST_OF_PARCEL_VIEW:
                                foreach (var temp in bl.GetAllParcelsBo())
                                    Console.WriteLine(temp);
                                break;
                            case (int)viewItemList.LIST_OF_PARCEL_WITHOUT_SPECIAL_DRONE:
                                foreach (var temp in bl.GetAllParcelsNotYetAssociatedWithDrone())
                                    Console.WriteLine(temp);
                                break;
                            case (int)viewItemList.LIST_OF_STATION_WITH_AVAILIBLE_CHARGING_STATION:
                                foreach (var temp in bl.GetAllBaseStationWhithAvailibleCharging(station => station.ChargeSlots > 0))
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
            Console.WriteLine("Enter 2 for add a new customer for an information list");
            Console.WriteLine("Enter 3 for add a parcel for delivery");
        }

        private static void MenuUpdate()
        {
            Console.WriteLine("Enter 0 for update the name of the drone");
            Console.WriteLine("Enter 1 for update the base station data");
            Console.WriteLine("Enter 2 for update the customer data");
            Console.WriteLine("Enter 3 for sending a drone for charging at a base station");
            Console.WriteLine("Enter 4 for release the drone from charging");
            Console.WriteLine("Enter 5 for assign a parcel to a drone");
            Console.WriteLine("Enter 6 for Collection of a parcel by drone");
            Console.WriteLine("Enter 7 for delivery of a parcel by drone");
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
        /// Receives input and activates the functions of the bl layer
        /// </summary>
        /// <param name="bl">On this standby the functions are activated</param>
        private static void addStation(BlObject bl)
        {
            Console.WriteLine("Enter the station details: id, name, location, charging ");
            int id = int.Parse(Console.ReadLine());
            int name = int.Parse(Console.ReadLine());
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            Location location = new Location() { Lattitude = lattitude, Longitude = longitude };
            int chargeSlots = int.Parse(Console.ReadLine());
            bl.AddBaseStationBo(id, name, location, chargeSlots);
        }

        private static void addDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone details: id, model, maxWeight, stationId");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            int droneId = int.Parse(Console.ReadLine());
            bl.AddDroneBo(id, model, maxWeight, droneId);
        }

        private static void addCustomer(BlObject bl)
        {
            Console.WriteLine("Enter the customer details: id, nameCustomer, phone, location)");
            int id = int.Parse(Console.ReadLine());
            string nameCustomer = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            Location location = new Location() { Longitude = longitude, Lattitude = lattitude };
            bl.AddCustomerBo(id, nameCustomer, phone, location);
        }

        private static void addParcel(BlObject bl)
        {
            Console.WriteLine("Enter the parcel details: senderId, targetId, maxWeight, priority)");
            int senderId = int.Parse(Console.ReadLine());
            int targetId = int.Parse(Console.ReadLine());
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            Priorities priority = (Priorities)int.Parse(Console.ReadLine());
            Console.WriteLine(bl.AddParcelBo(senderId, targetId, maxWeight, priority)); 
        }


        #endregion

        #region update item method
        private static void updateDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone details: id,model");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            bl.UpdateNameOfDrone(id, model);
        }

        public static void updateBaseStation(BlObject bl)
        {
            Console.WriteLine("Enter the base station details: id,name, num charging");
            int stationId = int.Parse(Console.ReadLine());
            int nameBaseStation = int.Parse(Console.ReadLine());
            int totalAmountOfChargingStations = int.Parse(Console.ReadLine());
            bl.UpdateBaseStationData(id: stationId, nameBaseStation: nameBaseStation, totalAmountOfChargingStations: totalAmountOfChargingStations);

        }

        public static void updateCustomer(BlObject bl)
        {
            Console.WriteLine("Enter the customer details: id,name, phone");
            int id = int.Parse(Console.ReadLine());
            string newName = Console.ReadLine();
            string newPhone = Console.ReadLine();
            bl.UpdateCustomerData(id, newName, newPhone);
        }

        public static void sendingDroneForCharging(BlObject bl)
        {
            Console.WriteLine("Enter the drone id: droneId");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine(bl.SendingDroneForCharging(id)); 
        }

        private static void releasDroneFromCharging(BlObject bl)
        {
            Console.WriteLine("Enter the drone details: droneId, stationId");
            int id = int.Parse(Console.ReadLine());
            TimeSpan chargingTime = TimeSpan.Parse(Console.ReadLine());
            bl.UpdateReleaseDroneFromCharging(id, chargingTime);
        }

        private static void assignParcelToDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone id: id");
            int id = int.Parse(Console.ReadLine());
            bl.UpdateAssignParcelToDrone(id);
        }

        private static void collectionParcelByDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone id: id");
            int id = int.Parse(Console.ReadLine());
            bl.UpdateCollectionParcelByDrone(id);
        }

        private static void deliveryParcelByDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone id: id");
            int id = int.Parse(Console.ReadLine());
            bl.UpdateDeliveryParcelByDrone(id);
        }


        #endregion

        #region display method
        private static void displayStation(BlObject bl)
        {
            Console.WriteLine("Enter the station view: stationId");
            int stationId = int.Parse(Console.ReadLine());
            Console.WriteLine(bl.GetBaseStation(stationId));
        }

        private static void displayDrone(BlObject bl)
        {
            Console.WriteLine("Enter the drone view: droneId");
            int droneId = int.Parse(Console.ReadLine());
            Console.WriteLine(bl.GetDrone(droneId));
        }

        private static void displayCustomer(BlObject bl)
        {
            Console.WriteLine("Enter the customer view: customerId");
            int customerId = int.Parse(Console.ReadLine());
            Console.WriteLine(bl.GetCustomer(customerId));
        }

        private static void displayParcel(BlObject bl)
        {
            Console.WriteLine("Enter the parcel view: parcelId");
            int parcelId = int.Parse(Console.ReadLine());
            Console.WriteLine(bl.GetParcel(parcelId));
        }
        #endregion

    }
}
