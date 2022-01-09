using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStationForList
    {
        public int BaseStationID { get; set; }
        public string NameBaseStation { get; set; }
        public int NumOfAvailableChargingPositions { get; set; }
        public int NumOfBusyChargingPositions { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }

    }
}
