using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelInTransfer
    {
        public int Id { get; set; }
        public Enums.ParcelStatusInTransfer ParcelStatusInTransfer { get; set; }
        public Enums.Priorities Priorities { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Getting { get; set; }
        public Location Collection { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nParcelStatusInTransfer {ParcelStatusInTransfer} \nPriorities {Priorities} \nWeight {Weight}" +
                $"\nSender {Sender} \nGetting {Getting} \nCollection {Collection} \nDeliveryDestination {DeliveryDestination} " +
                $"\nTransportDistance {TransportDistance}";
        }
    }
}
