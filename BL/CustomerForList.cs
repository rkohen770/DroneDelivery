using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class CustomerForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int NumParcelsSentAndDelivered { get; set; }
        public int NumParcelsSentAndNotDelivered { get; set; }
        public int NumParcelsReceived { get; set; }
        public int SeveralParcelsOnTheWayToCustomer { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nName: {Name} \nNumParcelsSentAndDelivered {NumParcelsSentAndDelivered}" +
                $"\nNumParcelsSentNotDelivered {NumParcelsSentAndNotDelivered} \nNumParcelsReceived {NumParcelsReceived}" +
                $"\nSeveralParcelsOnTheWayToCustomer {SeveralParcelsOnTheWayToCustomer}";
        }
    }

}
