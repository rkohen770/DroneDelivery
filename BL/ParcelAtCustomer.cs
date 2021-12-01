using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel SourceOrTarget { get; set; }
        public override string ToString()
        {
            return $"Id: {Id} \nWeight: {Weight} \nPrioritie: {Priorities} \nParcel Status: {ParcelStatus}" +
                $"\nSource: {SourceOrTarget} \n";
        }

    }
}
