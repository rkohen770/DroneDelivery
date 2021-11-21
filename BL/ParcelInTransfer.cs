using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelInTransfer
    {
        public int Id;
        public Enums.ParcelStatusInTransfer ParcelStatusInTransfer;
        public Enums.Priorities Priorities;
        public Enums.WeightCategories Weight;
        public CustomerInParcel Sender;
        public CustomerInParcel Getting;
        public Location Collection;
        public Location DeliveryDestination;
        public double TransportDistance;

        public override string ToString()
        {
            return $"Id: {Id} \nParcelStatusInTransfer {ParcelStatusInTransfer} \nPriorities {Priorities} \nWeight {Weight}" +
                $"\nSender {Sender} \nGetting {Getting} \nCollection {Collection} \nDeliveryDestination {DeliveryDestination} " +
                $"\nTransportDistance {TransportDistance}";
        }
    }
}
