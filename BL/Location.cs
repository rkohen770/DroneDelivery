﻿using System;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return $"Longitude: {Longitude} \nLatitude: {Latitude}";
        }
    }
}
