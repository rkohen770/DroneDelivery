using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;
namespace BL
{
    public partial class BlObject : IBL.IBL
    {
        #region ADD
        /// <summary>
        /// Receipt of parcel for delivery
        /// </summary>
        /// <param name="senderId">ID of sending customer</param>
        /// <param name="targetId">Customer ID card</param>
        /// <param name="weight">Parcel weight</param>
        /// <param name="priority">Priority(Normal, Fast, Emergency)</param>
        public void AddParcelBo(int senderId, int targetId, IBL.BO.WeightCategories weight, IBL.BO.Priorities priority)
        {
            //Add parcel in DAL to data source and get the parcel id that was created.
            int parcelId = dal.AddParcel(senderId, targetId, (IDAL.DO.WeightCategories)weight, (IDAL.DO.Priorities)priority);

            //Find a customer sending
            IDAL.DO.Customer customerS = dal.GetCustomer(senderId);
            CustomerInParcel sender = new CustomerInParcel() { Id = customerS.Id, Name = customerS.Name };
            //Find a receiving customer
            IDAL.DO.Customer customerG = dal.GetCustomer(targetId);
            CustomerInParcel getting = new CustomerInParcel() { Id = customerG.Id, Name = customerG.Name };

            //add per fields in BL.
            IBL.BO.Parcel parcel = new IBL.BO.Parcel()
            {
                Id = parcelId,
                Sender = sender,
                Getting = getting,
                Weight = weight,
                Priorities = priority,
                DroneInParcel = null,
                CreateParcel = DateTime.Now,
                ParcelAssociation = DateTime.MinValue,
                ParcelCollection = DateTime.MinValue,
                ParcelDelivery = DateTime.MinValue
            };
        }

#endregion
    }
}
