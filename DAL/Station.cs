﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} \nName: {Name} \nLongitude: {Longitude} \n" + 
                    $"Lattitude: {Lattitude} \nChargeSlots: {ChargeSlots}";
            }
        }


    }
}
