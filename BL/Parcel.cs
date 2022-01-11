using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Parcel
    {
        public int ParcelID { get; set; }
        public CustomerInParcel SenderOfParcel { get; set; }
        public CustomerInParcel TargetToParcel { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? ParcelDelivery { get; set; }


        public override string ToString()
        {
            return $"Parcel ID: {ParcelID},\n " +
                $"Sender Of Parcel: {SenderOfParcel}, Target To Parcel: {TargetToParcel}, \n" +
                $"Weight: {Weight},\n" +
                $"Priorities: {Priorities},\n " +
                $"Drone In Parcel: {DroneInParcel},\n" +
                $"Requested: {Requested},\n " +
                $"Scheduled: {Scheduled},\n " +
                $"PickedUp: {PickedUp},\n " +
                $"ParcelDelivery: {ParcelDelivery}";
        }
    }
}