using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class BaseStationForList
    {
        public int Id;
        public string NameBaseStation;
        public int NumOfAvailableChargingPositions;
        public int NumOfBusyChargingPositions;

        public override string ToString()
        {
            return $"Id: {Id} \nNameBaseStation {NameBaseStation} " +
                $"\nNumOfAvailableChargingPositions {NumOfAvailableChargingPositions}" +
                $"\nNumOfBusyChargingPositions {NumOfBusyChargingPositions}";
        }

    }
}
