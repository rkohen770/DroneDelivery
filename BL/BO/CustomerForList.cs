using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerForList
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int SentAndDelivered { get; set; }
        public int SentAndNotDelivered { get; set; }
        public int Received { get; set; }
        public int OnTheWayToCustomer { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }

}