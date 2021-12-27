using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneForList
    {
        public int DroneId { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double DroneBattery { get; set; }
        public DroneStatus? DroneStatus { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelNumIsTransferred { get; set; }

        public override string ToString()
        {
            return $"Drone ID: {DroneId},\n " +
                $"Drone Model: {DroneModel},\n" +
                $" Max Weight: {MaxWeight},\n" +
                $" Drone Battery: {DroneBattery},\n " +
                $"Drone Status: {DroneStatus},\n " +
                $"Current Location: {CurrentLocation},\n" +
                $"Parcel Num Is Transferred: {ParcelNumIsTransferred}";
        }
    }
}