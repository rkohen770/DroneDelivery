using System;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude;
        public double Latitude;

        public override string ToString()
        {
            return $"Longitude: {Longitude} \nLatitude: {Latitude}";
        }
    }
}
