﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelAtCustomer
    {
        public int Id;
        public Enums.WeightCategories Weight;
        public Enums.Priorities Priorities;
        public Enums.ParcelStatus ParcelStatus;
        public CustomerInParcel Source;
        public CustomerInParcel Target;

        public override string ToString()
        {
            return $"Id: {Id} \nWeight {Weight} \nPriorities {Priorities} \nParcelStatus {ParcelStatus}" +
                $"\nSource {Source} \nTarget {Target}";
        }

    }
}
