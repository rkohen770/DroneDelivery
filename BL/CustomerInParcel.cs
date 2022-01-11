using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public override string ToString()
        {
             return $"Customer ID: {CustomerID},\n" +
                $"Customer Name: {CustomerName}";
        }
    }
}