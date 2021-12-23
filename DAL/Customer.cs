using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }

            public override string ToString()
            {
               return$"ID: {Id} \nName: {Name} \nPhone {Phone} \n" +
                    $"Longitude {Longitude} \nLattitude {Lattitude}\n";
            }
        }
    }
}
