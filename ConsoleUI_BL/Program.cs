using System;
using IBL.BO;
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

                            case (int)Add.ADD_PARCEL:
                                addParcel(bl);
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
            Console.WriteLine("Enter 2 for receiving a new customer for an information list");
            Console.WriteLine("Enter 3 for receipt of package for shipment");
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
            Location location = new Location() { Latitude = lattitude, Longitude = longitude };
            int chargeSlots = int.Parse(Console.ReadLine());
            bl.AddBaseStationBo(id, name,location, chargeSlots);
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
            Console.WriteLine("Enter the customer details: id, nameCustomer, phone, longitude, lattitude)");
            int id = int.Parse(Console.ReadLine());
            string nameCustomer = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            bl.AddCustomer(id, nameCustomer, phone, longitude, lattitude);
        }


        #endregion
    }
}
