using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BLApi;

namespace BO
{
    [Serializable]
    #region Base Station Exception
    public class BadBaseStationIDException : Exception
    {
        public int Code;
        public BadBaseStationIDException(int code) : base() => Code = code;
        public BadBaseStationIDException(int code, string message) :
            base(message) => Code = code;
        public BadBaseStationIDException(int code, string message, Exception innerException) :
              base(message, innerException) => Code = code;
        public override string ToString() => base.ToString() + $", bad base station code: {Code}";
    }

    public class BaseStationAlreadyExistException : Exception
    {
        public int ID;
        public int? Name;
        public BaseStationAlreadyExistException(int id, int name) : base()
        {
            Name = name;
            ID = id;
        }
        public BaseStationAlreadyExistException(int id, int name, string message) : base(message)
        {
            Name = name;
            ID = id;
        }

        public BaseStationAlreadyExistException(int id, int name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
            ID = id;
        }
        public override string ToString() => base.ToString() + $", The base station: {ID} is already exist in the system ";
    }
    #endregion

    #region Drone Exception
    public class BadDroneIDException : Exception
    {
        public int ID;
        public BadDroneIDException(int id) : base() => ID = id;
        public BadDroneIDException(int id, string message) :
            base(message) => ID = id;
        public BadDroneIDException(int id, string message, Exception innerException) :
              base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad Drone id: {ID}";

    }
    public class StatusDroneNotAllowException : Exception
    {
        public int ID;
        public DroneStatus? Status;
        public string Action;
        public StatusDroneNotAllowException(int id, DroneStatus? status, string action) : base()
        {
            Action = action;
            Status = status;
            ID = id;
        }
        public StatusDroneNotAllowException(int id, DroneStatus? status, string action, string message) : base(message)
        {
            Action = action;
            Status = status;
            ID = id;
        }

        public StatusDroneNotAllowException(int id, DroneStatus? status, string action, string message, Exception innerException) : base(message, innerException)
        {
            Action = action;
            Status = status;
            ID = id;
        }
        public override string ToString() => base.ToString() + $", The Drone: {ID} Can't perform {Action} because his status: {Status} does not allow him ";

    }
    public class BatteryOfDroneNotAllowException : Exception
    {
        public int ID;
        public double? Battery;
        public string Action;
        public BatteryOfDroneNotAllowException(int id, double battery, string action) : base()
        {
            Action = action;
            Battery = battery;
            ID = id;
        }
        public BatteryOfDroneNotAllowException(int id, double battery, string action, string message) : base(message)
        {
            Action = action;
            Battery = battery;
            ID = id;
        }

        public BatteryOfDroneNotAllowException(int id, double battery, string action, string message, Exception innerException) : base(message, innerException)
        {
            Action = action;
            Battery = battery;
            ID = id;
        }
        public override string ToString() => base.ToString() + $", The Drone: {ID} Can't perform {Action} because his battery: {Battery} does not allow him ";

    }

    public class DroneAlreadyExistException : Exception
    {
        public int ID;
        public string? Model;
        public DroneAlreadyExistException(int id, string model) : base()
        {
            Model = model;
            ID = id;
        }
        public DroneAlreadyExistException(int id, string model, string message) : base(message)
        {
            Model = model;
            ID = id;
        }

        public DroneAlreadyExistException(int id, string model, string message, Exception innerException) : base(message, innerException)
        {
            Model = model;
            ID = id;
        }
        public override string ToString() => base.ToString() + $", The Drone: {ID} status: {Model} is already exist in the system ";
    }
    #endregion

    #region Parcel Exception
    public class BadParcelIDException : Exception
    {
        public int ID;
        public BadParcelIDException(int id) : base() => ID = id;
        public BadParcelIDException(int id, string message) :
            base(message) => ID = id;
        public BadParcelIDException(int id, string message, Exception innerException) :
              base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The Parcel: {ID }not found";

    }

    public class ParcelAlreadyExistException : Exception
    {
        public int ID;
        public ParcelAlreadyExistException(int id) : base() => ID = id;
        public ParcelAlreadyExistException(int id, string message) :
            base(message) => ID = id;
        public ParcelAlreadyExistException(int id, string message, Exception innerException) :
            base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The Parcel: {ID} is already exist in the system ";
    }
    #endregion

    #region Customer Exception
    public class BadCustomerIDException : Exception
    {
        public int ID;
        public BadCustomerIDException(int id) : base() => ID = id;
        public BadCustomerIDException(int id, string message) :
            base(message) => ID = id;
        public BadCustomerIDException(int id, string message, Exception innerException) :
              base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", Customer not found. ID: {ID}";

    }

    public class CustomerAlreadyExistException : Exception
    {
        public int ID;
        public string? Name;
        public CustomerAlreadyExistException(int id, string name) : base()
        {
            Name = name;
            ID = id;
        }
        public CustomerAlreadyExistException(int id, string name, string message) : base(message)
        {
            Name = name;
            ID = id;
        }

        public CustomerAlreadyExistException(int id, string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
            ID = id;
        }
        public override string ToString() => base.ToString() + $", The Customer:{Name} \nID: {ID} is already exist in the system ";
    }
    #endregion
}