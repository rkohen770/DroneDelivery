using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerForList
    {
        public int IdCustomer { get; set; }
        public string NameOfCustomer { get; set; }
        public string Phone { get; set; }
        public int NumParcelsSentAndDelivered { get; set; }
        public int NumParcelsSentAndNotDelivered { get; set; }
        public int NumParcelsReceived { get; set; }
        public int SeveralParcelsOnTheWayToCustomer { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }

}
