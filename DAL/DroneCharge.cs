﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct DroneCharge
    {
        public int DroneID { get; set; }
        public int StationID { get; set; }

        public override string ToString()
        {
            return $"DroneId: {DroneID} \nStationId: {StationID}\n";
        }
    }
}
