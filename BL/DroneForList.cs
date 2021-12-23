using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
            return this.ToStringProperty() + " ";
        }
    }
}
