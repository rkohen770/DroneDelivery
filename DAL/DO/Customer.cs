using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }

        public override string ToString()
        {
            return $"ID: {CustomerID} \nName: {Name} \nPhone {Phone} \n" +
                $"{Lattitude:n3}°N, {Longitude:n3}°E\n";
        }
    }
}
