﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelAtCustomer
    {
        public int Id { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public Enums.Priorities Priorities { get; set; }
        public Enums.ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel Source { get; set; }
        public CustomerInParcel Target { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nWeight {Weight} \nPriorities {Priorities} \nParcelStatus {ParcelStatus}" +
                $"\nSource {Source} \nTarget {Target}";
        }

    }
}
