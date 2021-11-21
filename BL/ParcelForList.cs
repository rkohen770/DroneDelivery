using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelForList
    {
        public int Id { get; set; }
        public string CustomerNameSend { get; set; }
        public string CustomerNameGetting { get; set; }
        public Enums.WeightCategories Weight { get; set; }
        public Enums.Priorities Priorities { get; set; }
        public Enums.ParcelStatus ParcelStatus { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} \nCustomerNameSend: {CustomerNameSend} \nCustomerNameGetting {CustomerNameGetting}" +
                $" \nWeight {Weight} \nPriorities {Priorities} \nParcelStatus {ParcelStatus}";
        }

    }
}
