using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone
    {
        public int DroneID { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories Weight { get; set; }
        public double DroneBattery { get; set; }
        public DroneStatus? DroneStatus { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return $"Drone ID: {DroneID},\n " +
                $"Drone Model: {DroneModel},\n" +
                $"Weight: {Weight},\n" +
                $"Drone Battery: {DroneBattery},\n " +
                $"Drone Status: {DroneStatus},\n " +
                $"Current Location: {CurrentLocation},\n" +
                $"Parcel In Transfer: {ParcelInTransfer}";
        }

    }
}