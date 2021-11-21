using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Customer
    {
        public int Id;
        public string Name;
        public string Phone;
        public Location Location;
        public List<ParcelAtCustomer> FromCustomer;
        public List<ParcelAtCustomer> ToCustomer;


        public override string ToString()
        {
            return $"Id: {Id} \nName: {Name} \nPhone {Phone} \nLocation {Location}" +
                $"\nFromCustomer {FromCustomer} \nToCustomer {ToCustomer}";
        }
    }
}
