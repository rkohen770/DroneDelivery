using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInCharging
    {
        public int DroneId { get; set; }
        public double DroneBattery { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }
}
