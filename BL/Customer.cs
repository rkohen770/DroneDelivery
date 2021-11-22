using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelAtCustomer> FromCustomer { get; set; }
        public List<ParcelAtCustomer> ToCustomer { get; set; }


        public override string ToString()
        {
            return $"Id: {Id} \nName: {Name} \nPhone {Phone} \nLocation {Location}" +
                $"\nFromCustomer {FromCustomer} \nToCustomer {ToCustomer}";
        }
    }
}
