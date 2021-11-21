using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class DroneForList
    {
        public int Id;
        public string Model;
        public Enums.WeightCategories Weight;
        public double Battery;
        public Enums.DroneStatus DroneStatus;
        public Location CurrentLocation;
        public int ParcelNumIsTransferred;

        public override string ToString()
        {
            return $"Id: {Id} \nModel {Model} \nWeight {Weight} \nBattery {Battery} \nDroneStatus {DroneStatus}" +
                $"\nCurrentLocation{CurrentLocation} \nParcelNumIsTransferred {ParcelNumIsTransferred} ";
        }
    }
}
