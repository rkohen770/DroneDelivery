using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInTransfer
    {
        public int ParcelID { get; set; }
        public ParcelStatusInTransfer ParcelStatus { get; set; }
        public Priorities Priorities { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel SenderOfParcel { get; set; }
        public CustomerInParcel TargetToParcel { get; set; }
        public Location Collection { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty()+"\n";
        }
    }
}