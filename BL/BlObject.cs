using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
using IDAL;
using IDAL.DO;
namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        Random random = new Random();
        IDal dal = new DalObject.DalObject();
        //public List<DroneForList> droneForLists=new List<DroneForList>();
        public BlObject() { }

        #region ADD

        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions)
        {
            //add baseStation fields in BL.
            BaseStation baseStation = new BaseStation()
            {
                Id = id,
                NameBaseStation = nameBaseStation,
                Location = location,
                NumOfAvailableChargingPositions = numOfAvailableChargingPositions
            };
            //Add baseStation in DAL to data source.
            dal.AddStation(id, nameBaseStation, location.Longitude, location.Latitude, numOfAvailableChargingPositions);
        }


        public void AddDroneBo(int droneId, string model, IBL.BO.WeightCategories maxWeight, int stationId)
        {
            //add drone fields in BL.
            Station s = dal.BaseStationView(stationId);
            IBL.BO.Drone drone = new IBL.BO.Drone()
            {
                Id = droneId,
                Model = model,
                Weight = maxWeight,
                Battery = random.Next(20, 41),
                DroneStatus = DroneStatus.Maintenance,
                CurrentLocation = new Location() { Latitude = s.Lattitude, Longitude = s.Longitude }
            };
            dal.SendingDroneForCharging(droneId, stationId);
            //להוסיף את הרחפן לרשימה של רחפנים בטעינה של תחנת בסיס

            //Add drone in DAL to data source.
            dal.AddDrone(droneId, model, (IDAL.DO.WeightCategories)maxWeight);
        }


        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            //add customer fields in BL.
            IBL.BO.Customer customer = new IBL.BO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = new Location() { Longitude = longitude, Latitude = lattitude }
            };
           
            //Add customer in DAL to data source.
            dal.AddCustomer(id, name, phone, longitude, lattitude);
        }



        public void AddParcel(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
        {
            //Add parcel in DAL to data source and get the parcel id that was created.
            int parcelId = dal.AddParcel(senderId, targetId, (IDAL.DO.WeightCategories)weight, (IDAL.DO.Priorities)priority);

            //Find a customer sending
            IDAL.DO.Customer customerS = dal.CustomerView(senderId);
            CustomerInParcel sender = new CustomerInParcel() { Id = customerS.Id, Name = customerS.Name };
            //Find a receiving customer
            IDAL.DO.Customer customerG = dal.CustomerView(targetId);
            CustomerInParcel getting = new CustomerInParcel() { Id = customerG.Id, Name = customerG.Name };

            //add per fields in BL.
            IBL.BO.Parcel parcel = new IBL.BO.Parcel()
            {
                Id = parcelId,
                Sender = sender,
                Getting = getting,
                Weight = weight,
                Priorities = priority,
                DroneInParcel = null,
                CreateParcel = DateTime.Now,
                ParcelAssociation = DateTime.MinValue,
                ParcelCollection = DateTime.MinValue,
                ParcelDelivery= DateTime.MinValue
            };
        }

        #endregion

    }
}
