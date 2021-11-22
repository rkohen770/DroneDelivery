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
    public partial class BlObject :IBL.IBL
    {
        IDal dal = new DalObject.DalObject();
        //public List<DroneForList> droneForLists=new List<DroneForList>();
        public BlObject()
        {
            
        }

        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions)
        {
            BaseStation baseStation = new BaseStation();
            dal.AddStation(id, nameBaseStation, location.Longitude, location.Latitude, numOfAvailableChargingPositions);
            baseStation.Id = id;
            baseStation.NameBaseStation = nameBaseStation;
            baseStation.Location = location;
            baseStation.NumOfAvailableChargingPositions = numOfAvailableChargingPositions;
        }


        public void AddDroneBo(int droneId, string model, IBL.BO.WeightCategories maxWeight, int stationId)
        {
            IBL.BO.Drone drone = new IBL.BO.Drone();
            dal.AddDrone(droneId, model, (IDAL.DO.WeightCategories)maxWeight);


        }


        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            throw new NotImplementedException();
        }

        

        public int AddParcel(int senderId, int targetId, WeightCategories weight, Enums.Priorities priority, int droneId = 0)
        {
            throw new NotImplementedException();
        }
    }
}
