using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi.BO
{
    public enum WeightCategories { Easy, Intermediate, Liver };
    public enum Priorities { Normal, Fast, Emergency };
    public enum ParcelStatus { Defined, Associated, WasCollected, Provided };
    public enum ParcelStatusInTransfer { AwaitingCollection, OnTheWayToDestination };
    public enum DroneStatus { Available, Maintenance, Delivery };
}
