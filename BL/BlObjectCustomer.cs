using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace BL
{
    public partial class BlObject : BLApi.IBL
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
            try
            {
                //add customer fields in BL.
                BO.Customer customer = new BO.Customer()
                {
                    CustomerId = id,
                    NameOfCustomer = name,
                    PhoneOfCustomer = phone,
                    LocationOfCustomer = location
                };

                //Add customer in DAL to data source.
                dal.AddCustomer(id, name, phone, location.Longitude, location.Lattitude);
            }
            catch (DO.CustomerAlreadyExistException e)
            {
                throw new BO.CustomerAlreadyExistException(e.ID, e.Name, e.Message, e.InnerException);
            }
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
            try
            {
                if (newName == "" && newPhone == "")
                {
                    throw new Exception("No details were entered for change at the customer entity");
                }
                //update in BL
                DO.Customer customer = dal.GetCustomer(id);
                if (newName != "")
                {
                    if (newPhone != "")
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
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }

        }
        #endregion

        #region GET ITEM
        /// <summary>
        /// Customer view
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>customer</returns>
        public BO.Customer GetCustomer(int customerId)
        {
            try
            {
                DO.Customer customer = dal.GetCustomer(customerId);

                //The list of packages that the customer
                List<DO.Parcel> parcel_From_Customer = dal.GetAllParcels().
                    Where(p => p.SenderId == customerId).ToList();

                List<ParcelAtCustomer> from_customer = new();
                foreach (var parcel in parcel_From_Customer)
                {
                    //find the status of parcel.
                    var ParcelStatus = (parcel.Delivered != null) ? BO.ParcelStatus.Provided :
                        (parcel.PickedUp != null) ? BO.ParcelStatus.WasCollected :
                        (parcel.Scheduled != null) ? BO.ParcelStatus.Associated : BO.ParcelStatus.Defined;
                    ParcelAtCustomer parcelAt = new()
                    {
                        ParcelId = parcel.Id,
                        Weight = (BO.WeightCategories)parcel.Weight,
                        Priorities = (BO.Priorities)parcel.priority,
                        ParcelStatus = ParcelStatus,
                        SourceOrTarget = new()
                        {
                            CustomerId = parcel.TargetId,
                            CustomerName = dal.GetCustomer(parcel.TargetId).Name
                        }
                    };
                    from_customer.Add(parcelAt);
                };

                //The list of packages that the customer receives
                List<DO.Parcel> parcel_To_Customer = dal.GetAllParcels().
                    Where(p => p.TargetId == customerId).ToList();
                List<ParcelAtCustomer> to_customer = new();
                foreach (var parcel in parcel_To_Customer)
                {
                    //find the status of parcel.
                    var ParcelStatus = (parcel.Delivered != null) ? BO.ParcelStatus.Provided :
                        (parcel.PickedUp != null) ? BO.ParcelStatus.WasCollected :
                        (parcel.Scheduled != null) ? BO.ParcelStatus.Associated : BO.ParcelStatus.Defined;
                    ParcelAtCustomer parcelAt = new()
                    {
                        ParcelId = parcel.Id,
                        Weight = (BO.WeightCategories)parcel.Weight,
                        Priorities = (BO.Priorities)parcel.priority,
                        ParcelStatus = ParcelStatus,
                        SourceOrTarget = new()
                        {
                            CustomerId = parcel.SenderId,
                            CustomerName = dal.GetCustomer(parcel.SenderId).Name
                        }
                    };
                    to_customer.Add(parcelAt);
                };

                return new()
                {
                    CustomerId = customerId,
                    NameOfCustomer = customer.Name,
                    PhoneOfCustomer = customer.Phone,
                    LocationOfCustomer = new() { Lattitude = customer.Lattitude, Longitude = customer.Longitude },
                    FromCustomer = from_customer,
                    ToCustomer = to_customer
                };
            }
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
        }
        #endregion

        #region GET LIST
        /// <summary>
        /// View customer list
        /// </summary>
        /// <returns>customer list</returns>
        public IEnumerable<CustomerForList> GetAllCustomersBo()
        {
            try
            {
                List<CustomerForList> list = new();
                foreach (var customer in dal.GetAllCustomers())
                {
                    CustomerForList customerForList = cloneCustomer(GetCustomer(customer.Id));
                    list.Add(customerForList);
                }
                return list;
            }
            catch (DO.BadCustomerIDException e)
            {
                throw new BO.BadCustomerIDException(e.ID, e.Message, e.InnerException);
            }
        }


        public IEnumerable<CustomerForList> GetAllCustomerByPredicate(Predicate<DO.Customer> p)
        {
            List<CustomerForList> list = new();
            foreach (var customer in dal.GetAllCustomerByPredicate(p))
            {
                CustomerForList customerForList = cloneCustomer(GetCustomer(customer.Id));
                list.Add(customerForList);
            }
            return list;
        }
        #endregion

        /// <summary>
        ///  Converts from object customer to object customer for list
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>customer for list</returns>
        private CustomerForList cloneCustomer(BO.Customer customer)
        {
            return new()
            {
                IdCustomer = customer.CustomerId,
                NameOfCustomer = customer.NameOfCustomer,
                Phone = customer.PhoneOfCustomer,
                NumParcelsSentAndNotDelivered = customer.FromCustomer.Where(p => p.ParcelStatus == ParcelStatus.Provided).Count(),
                NumParcelsSentAndDelivered = customer.FromCustomer.Where(p => p.ParcelStatus != ParcelStatus.Provided).Count(),
                NumParcelsReceived = customer.ToCustomer.Where(p => p.ParcelStatus == ParcelStatus.Provided).Count(),
                SeveralParcelsOnTheWayToCustomer = customer.ToCustomer.Where(p => p.ParcelStatus != ParcelStatus.Provided).Count()
            };
        }
    
    }
}