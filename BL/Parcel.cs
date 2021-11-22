using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Getting { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime CreateParcel { get; set; }
        public DateTime ParcelAssociation { get; set; }
        public DateTime ParcelCollection { get; set; }
        public DateTime ParcelDelivery { get; set; }


        public override string ToString()
        {
            return $"Id: {Id} \nSender: {Sender} \nGetting {Getting} \nWeight {Weight} \nPriorities {Priorities}" +
                $"\nDroneInParcel {DroneInParcel} \nCreateParcel {CreateParcel} \nParcelAssociation {ParcelAssociation}" +
                $"\nParcelCollection {ParcelCollection} \nParcelDelivery {ParcelDelivery}";
        }
    }
}
