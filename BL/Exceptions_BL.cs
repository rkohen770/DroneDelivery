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
    class NoDataExistsException: Exception
    {
        public NoDataExistsException() { }
        public NoDataExistsException(string exe) : base(exe) { }
    }
    class DroneConditionNotConfirmException : Exception
    {
        public DroneConditionNotConfirmException() { }
        public DroneConditionNotConfirmException(string exe) : base(exe) { }
    }
    class NotEnoughFuelException : Exception
    {
        public NotEnoughFuelException() { }
        public NotEnoughFuelException(string exe) : base(exe) { }
    }
    
     class ParcelException : Exception
    {
        public ParcelException()
        {
        }

        public ParcelException(string message) : base(message)
        {
        }

        public ParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
