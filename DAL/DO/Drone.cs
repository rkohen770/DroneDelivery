using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Drone
    {
        public int DroneID { get; set; }
        public string DroneModel { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public bool Available { get; set; }


        public override string ToString()
        {
            return $"ID: {DroneID} \nModel: {DroneModel} \nMaxWeight: {MaxWeight} \n";
        }
    }
}
