using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class DroneForList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public double Battery { get; set; }
        public Enums.DroneStatus DroneStatus { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelNumIsTransferred { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nModel {Model} \nWeight {Weight} \nBattery {Battery} \nDroneStatus {DroneStatus}" +
                $"\nCurrentLocation{CurrentLocation} \nParcelNumIsTransferred {ParcelNumIsTransferred} ";
        }
    }
}
