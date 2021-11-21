using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Drone
    {
        public int Id;
        public string Model;
        public Enums.WeightCategories Weight;
        public double Battery;
        public Enums.DroneStatus DroneStatus;
        public ParcelInTransfer ParcelInTransfer;
        public Location CurrentLocation;

        public override string ToString()
        {
            return $"Id: {Id} \nModel {Model} \nWeight {Weight} \nBattery {Battery} \nDroneStatus {DroneStatus}" +
                $" \nParcelInTransfer {ParcelInTransfer} \nCurrentLocation{CurrentLocation}";
        }

    }
}
