using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel
    {
        public int DroneId { get; set; }
        public double DroneBattery { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }
}