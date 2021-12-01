using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public ParcelStatusInTransfer ParcelStatusInTransfer { get; set; }
        public Priorities Priorities { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location Collection { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
