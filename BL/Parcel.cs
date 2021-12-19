using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi.BO
{
    public class Parcel
    {
        public int ParcelId { get; set; }
        public CustomerInParcel SenderOfParcel { get; set; }
        public CustomerInParcel TargetToParcel { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled  { get; set; }
        public DateTime? PickedUp  { get; set; }
        public DateTime? ParcelDelivery { get; set; }


        public override string ToString()
        {
            return this.ToStringProperty() + "\n";
        }
    }
}
