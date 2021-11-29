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
        /// Absorption of a new customer
        /// </summary>
        /// <param name="id">Customer ID number</param>
        /// <param name="name">The customer's name</param>
        /// <param name="phone">Phone Number</param>
        /// <param name="location">Customer location</param>
        public void AddCustomerBo(int id, string name, string phone, Location location)
        {
            //add customer fields in BL.
            IBL.BO.Customer customer = new IBL.BO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = location
            };

            //Add customer in DAL to data source.
            dal.AddCustomer(id, name, phone, location.Longitude, location.Lattitude);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="newName">New name</param>
        /// <param name="newPhone">New phone</param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {
            //update in BL
            IDAL.DO.Customer customer = dal.GetCustomer(id);
            if (newName != null)
            {
                if (newPhone != null)
                {
                    dal.UpdateCustomerData(id, newName, newPhone);
                }
                else
                {
                    dal.UpdateCustomerName(id, newName);
                }
            }
            else
            {
                dal.UpdateCustomerPhone(id, newPhone);
            }

        }
        #endregion

        #region GET ITEM

        public IBL.BO.Customer GetCustomer(int customerId)
        {
            IDAL.DO.Customer customer = dal.GetCustomer(customerId);

            //The list of packages that the customer
            List<IDAL.DO.Parcel> parcel_From_Customer = dal.GetAllParcels().
                Where(p => p.SenderId == customerId).ToList();

            List<ParcelAtCustomer> from_customer = new();
            foreach (var parcel in parcel_From_Customer)
            {
                //find the status of parcel.
                var ParcelStatus = (parcel.Delivered != DateTime.MinValue) ? IBL.BO.ParcelStatus.Provided :
                    (parcel.PickedUp != DateTime.MinValue) ? IBL.BO.ParcelStatus.WasCollected :
                    (parcel.scheduled != DateTime.MinValue) ? IBL.BO.ParcelStatus.Associated : IBL.BO.ParcelStatus.Defined;
                ParcelAtCustomer parcelAt = new()
                {
                    Id = parcel.Id,
                    Weight = (IBL.BO.WeightCategories)parcel.Weight,
                    Priorities = (IBL.BO.Priorities)parcel.priority,
                    ParcelStatus = ParcelStatus,
                    SourceOrTarget = new()
                    {
                        Id = parcel.TargetId,
                        Name = dal.GetCustomer(parcel.TargetId).Name
                    }
                };
                from_customer.Add(parcelAt);
            };

            //The list of packages that the customer receives
            List<IDAL.DO.Parcel> parcel_To_Customer = dal.GetAllParcels().
                Where(p => p.TargetId == customerId).ToList();
            List<ParcelAtCustomer> to_customer = new();
            foreach (var parcel in parcel_From_Customer)
            {
                //find the status of parcel.
                var ParcelStatus = (parcel.Delivered != DateTime.MinValue) ? IBL.BO.ParcelStatus.Provided :
                    (parcel.PickedUp != DateTime.MinValue) ? IBL.BO.ParcelStatus.WasCollected :
                    (parcel.scheduled != DateTime.MinValue) ? IBL.BO.ParcelStatus.Associated : IBL.BO.ParcelStatus.Defined;
                ParcelAtCustomer parcelAt = new()
                {
                    Id = parcel.Id,
                    Weight = (IBL.BO.WeightCategories)parcel.Weight,
                    Priorities = (IBL.BO.Priorities)parcel.priority,
                    ParcelStatus = ParcelStatus,
                    SourceOrTarget = new()
                    {
                        Id = parcel.SenderId,
                        Name = dal.GetCustomer(parcel.SenderId).Name
                    }
                };
                from_customer.Add(parcelAt);
            };

            return new()
            {
                Id = customerId,
                Name = customer.Name,
                Phone = customer.Phone,
                Location = new() { Lattitude = customer.Lattitude, Longitude = customer.Longitude },
                FromCustomer = from_customer,
                ToCustomer = to_customer
            };

        }
        #endregion

        #region GET LIST
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerForList> GetAllCustomersBo()
        {
            List<CustomerForList> list = new();
            foreach (var customer in dal.GetAllCustomers())
            {
                CustomerForList customerForList = clonCustomer(GetCustomer(customer.Id));
                list.Add(customerForList);
            }
            return list;
        }
        #endregion
        private CustomerForList clonCustomer(IBL.BO.Customer customer)
        {
            return new CustomerForList()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                NumParcelsSentAndNotDelivered = customer.FromCustomer.Where(p => p.ParcelStatus == ParcelStatus.Provided).Count(),
                NumParcelsSentAndDelivered = customer.FromCustomer.Where(p => p.ParcelStatus != ParcelStatus.Provided).Count(),
                NumParcelsReceived = customer.ToCustomer.Where(p => p.ParcelStatus == ParcelStatus.Provided).Count(),
                SeveralParcelsOnTheWayToCustomer = customer.ToCustomer.Where(p => p.ParcelStatus != ParcelStatus.Provided).Count()
            };
        }
        
    }
}
