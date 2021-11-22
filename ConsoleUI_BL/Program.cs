using System;
using IBL.BO;
using BL;

namespace ConsoleUI_BL
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
        public enum Display { PRINT_STATION, PRINT_DRONE, PRINT_CUSTOMER, PRINT_PARCEL }
        public enum viewItemList
        {
            GEt_ALL_BASE_STATIONS, LIST_OF_DRONE_VIEW, LIST_OF_CUSTOMER_VIEW, LIST_OF_PARCEL_VIEW,
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
                            case (int)Add.ADD_STATION:
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

        private static void addStation(BlObject bl)
        {
            Console.WriteLine("Enter the station details: id, name, longitude,lattitude, chargeSlots");
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
            Console.WriteLine("Enter the drone details: id, model, maxWeight, status, battery");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            WeightCategories maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            DroneStatus status = (DroneStatus)int.Parse(Console.ReadLine());
            double battery = double.Parse(Console.ReadLine());
            bl.AddDrone(id, model, maxWeight);
        }


        #endregion
    }
}
