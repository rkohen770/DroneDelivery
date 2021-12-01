using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double Battery { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nModel: {Model} \nWeight: {Weight} \nBattery: {Battery} \nDrone Status: {DroneStatus}" +
                $" \nParcel In Transfer: {ParcelInTransfer} \nCurrent Location:{CurrentLocation}\n";
        }

    }
}
