using System;

namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }
}