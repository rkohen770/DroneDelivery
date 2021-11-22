using System;
using IBL.BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {
        #region ADD
        public void AddBaseStationBo(int id, int nameBaseStation, Location location, int numOfAvailableChargingPositions);
        public void AddDroneBo(int droneId, string model, WeightCategories maxWeight, int stationId);
        public void AddCustomer(int id, string name, string phone, Location location);
        public void AddParcel(int senderId, int targetId, WeightCategories weight, Priorities priority);
        #endregion
    }
}
