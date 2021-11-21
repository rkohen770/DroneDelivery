using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class CustomerForList
    {
        public int Id;
        public string Name;
        public string Phone;
        public int NumParcelsSentAndDelivered;
        public int NumParcelsSentAndNotDelivered;
        public int NumParcelsReceived;
        public int SeveralParcelsOnTheWayToCustomer;

        public override string ToString()
        {
            return $"Id: {Id} \nName: {Name} \nNumParcelsSentAndDelivered {NumParcelsSentAndDelivered}" +
                $"\nNumParcelsSentNotDelivered {NumParcelsSentAndNotDelivered} \nNumParcelsReceived {NumParcelsReceived}" +
                $"\nSeveralParcelsOnTheWayToCustomer {SeveralParcelsOnTheWayToCustomer}";
        }
    }

}
