using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi.BO
{
    public class ParcelForList
    {
        public int ParcelId { get; set; }
        public string CustomerNameSend { get; set; }
        public string CustomerNameTarget { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public ParcelStatus ParcelStatus { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }

    }
}
