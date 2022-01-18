using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Station
    {
        public int StationID { get; set; }
        public int StationName { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int ChargeSlots { get; set; }
        public bool Available { get; set; }

        public override string ToString()
        {
            return $"ID: {StationID} \nName: {StationName} \nLongitude: {Longitude} \n" +
                $"Lattitude: {Lattitude} \nChargeSlots: {ChargeSlots}\n";
        }
    }


}
