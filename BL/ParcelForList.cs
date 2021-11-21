using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelForList
    {
        public int Id;
        public string CustomerNameSend;
        public string CustomerNameGetting;
        public Enums.WeightCategories Weight;
        public Enums.Priorities Priorities;
        public Enums.ParcelStatus ParcelStatus;

        public override string ToString()
        {
            return $"Id: {Id} \nCustomerNameSend: {CustomerNameSend} \nCustomerNameGetting {CustomerNameGetting}" +
                $" \nWeight {Weight} \nPriorities {Priorities} \nParcelStatus {ParcelStatus}";
        }

    }
}
