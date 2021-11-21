using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class DroneInParcel
    {
        public int Id;
        public double Battery;
        public Location CurrentLocation;

        public override string ToString()
        {
            return $"Id: {Id} \nBattery {Battery} \nCurrentLocation{CurrentLocation}"; 
        }
    }
}
