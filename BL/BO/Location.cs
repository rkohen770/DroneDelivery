using System;

namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }

        public override string ToString()
        {
            return $"{Lattitude:n3}°N, {Longitude:n3}°E";
        }
    }
}