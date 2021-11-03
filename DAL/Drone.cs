﻿using DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} \nModel: {Model} \nMaxWeight: {MaxWeight} \n" +
                    $"Status: {Status} \nBattery: {Battery}\n";
            }
        }
    }
}
