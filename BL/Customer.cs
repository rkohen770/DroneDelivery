using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string NameOfCustomer { get; set; }
        public string PhoneOfCustomer { get; set; }
        public Location LocationOfCustomer { get; set; }
        public List<ParcelAtCustomer> FromCustomer { get; set; }
        public List<ParcelAtCustomer> ToCustomer { get; set; }


        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }
}