using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public int NameBaseStation { get; set; }
        public Location Location { get; set; }
        public int NumOfAvailableChargingPositions { get; set; }
        public List<DroneInCharging> DroneInChargings = new List<DroneInCharging>();//list of drones in charging


        public override string ToString()
        {
            return $"Id: {Id} \nName Base Station:  {NameBaseStation} \nLocation: {Location} " +
                $"\nNum Of Available Charging Positions: {NumOfAvailableChargingPositions} \nDrone In Chargings: {DroneInChargings}\n ";
        }
    }
}
