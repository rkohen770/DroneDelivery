using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Parcel
    {
        public int Id;
        public CustomerInParcel Sender;
        public CustomerInParcel Getting;
        public Enums.WeightCategories Weight;
        public Enums.Priorities Priorities;
        public DroneInParcel DroneInParcel;
        public DateTime CreateParcel;
        public DateTime ParcelAssociation;
        public DateTime ParcelCollection;
        public DateTime ParcelDelivery;


        public override string ToString()
        {
            return $"Id: {Id} \nSender: {Sender} \nGetting {Getting} \nWeight {Weight} \nPriorities {Priorities}" +
                $"\nDroneInParcel {DroneInParcel} \nCreateParcel {CreateParcel} \nParcelAssociation {ParcelAssociation}" +
                $"\nParcelCollection {ParcelCollection} \nParcelDelivery {ParcelDelivery}";
        }
    }
}
