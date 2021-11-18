using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        public void AddStation(int id, int name, double longitude, double lattitude, int chargeSlots);
        public void AddDrone(int id, string model, WeightCategories maxWeight);
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude);
        public int AddParcel(int senderId, int targetId, WeightCategories weight, Priorities priority, int droneId = 0);

        public void AssigningParcelToDrone(int parcelId, int droneId);
        public void PackagCollectionByDrone(int parcelId);
        public void DeliveryPackageToCustomer(int parcelId);
        public void SendingDroneForCharging(int droneId, int stationId);
        public void ReleasDroneFromCharging(int droneId, int stationId);

        public Station BaseStationView(int stationId);
        public Drone DroneView(int droneId);
        public Customer CustomerView(int customerId);
        public Parcel ParcelView(int parcelId);

        public IEnumerable<Station> GetAllBaseStations();
        public IEnumerable<Drone> GetAllDrones();
        public IEnumerable<Customer> GetAllCustomers();
        public IEnumerable<Parcel> GetAllParcels();
        public IEnumerable<Parcel> GetAllParcelsWithoutSpecialDron();
        public IEnumerable<Station> GetAllStationsWithAvailableChargingStations();

        public double[] PowerConsumptionRequest();
    }
}
