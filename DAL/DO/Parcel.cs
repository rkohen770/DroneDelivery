using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Parcel
    {
        public int ParcelID { get; set; }
        public int SenderID { get; set; }
        public int TargetID { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities priority { get; set; }
        public int DroneID { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }


        public override string ToString()
        {
            return $"ID: {ParcelID} \nSenderId: {SenderID} \nTargetId {TargetID} \n" +
                $"Weight {Weight} \npriority {priority} \nRequested {Requested} \n" +
                $"DroneId {DroneID} \nscheduled {Scheduled} \nPickedUp {PickedUp} \nDelivered {Delivered}\n";
        }
    }

}
