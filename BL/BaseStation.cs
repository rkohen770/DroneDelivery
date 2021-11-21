using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class BaseStation
    {
        public int Id;
        public string NameBaseStation;
        public Location Location;
        public int NumOfAvailableChargingPositions;
        public List<DroneInCharging> DroneInChargings;


        public override string ToString()
        {
            return $"Id: {Id} \nNameBaseStation {NameBaseStation} \nLocation {Location}" +
                $"\nNumOfAvailableChargingPositions {NumOfAvailableChargingPositions} \nDroneInChargings {DroneInChargings}";
        }
    }
}
