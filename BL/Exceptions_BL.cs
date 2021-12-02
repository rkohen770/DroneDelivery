using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IBL;

namespace IBL.BO
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
        public override string ToString() => base.ToString() + $", The Drone: {ID} model: {Model} is already exist in the system ";
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
        public override string ToString() => base.ToString() + $", bad Parcel id: {ID}";

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

    #region Base Station Exception
    public class BadCustomerIDException : Exception
    {
        public int ID;
        public BadCustomerIDException(int id) : base() => ID = id;
        public BadCustomerIDException(int id, string message) :
            base(message) => ID = id;
        public BadCustomerIDException(int id, string message, Exception innerException) :
              base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad Customer id: {ID}";

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
