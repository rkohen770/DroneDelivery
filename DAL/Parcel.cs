using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities priority { get; set; }
        public int DroneId { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }


        public override string ToString()
        {
            return $"ID: {Id} \nSenderId: {SenderId} \nTargetId {TargetId} \n" +
                $"Weight {Weight} \npriority {priority} \nRequested {Requested} \n" +
                $"DroneId {DroneId} \nscheduled {Scheduled} \nPickedUp {PickedUp} \nDelivered {Delivered}\n";
        }
    }

}
