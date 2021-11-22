using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationForList
    {
        public int Id { get; set; }
        public string NameBaseStation { get; set; }
        public int NumOfAvailableChargingPositions { get; set; }
        public int NumOfBusyChargingPositions { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nNameBaseStation {NameBaseStation} " +
                $"\nNumOfAvailableChargingPositions {NumOfAvailableChargingPositions}" +
                $"\nNumOfBusyChargingPositions {NumOfBusyChargingPositions}";
        }

    }
}
