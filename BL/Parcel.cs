﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priorities { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Scheduled  { get; set; }
        public DateTime PickedUp  { get; set; }
        public DateTime ParcelDelivery { get; set; }


        public override string ToString()
        {
            return $"Id: {Id} \nSender: {Sender} \nTarget: {Target} \nWeight: {Weight} \nPrioritie: {Priorities}" +
                $"\nDrone In Parcel: {DroneInParcel} \nCreate Parcel: {Requested} \nParce Scheduled: {Scheduled }" +
                $"\nParcel Collection: {PickedUp } \nParcel Delivery:  {ParcelDelivery}\n";
        }
    }
}
