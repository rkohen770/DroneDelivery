using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        #region ADD
        /// <summary>
        /// Absorption of a new customer to the customer list
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">the customer's name</param>
        /// <param name="phone">Customer phone number</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            if (DataSource.customers.Exists(customer => customer.Id == id))
            {
                throw new ExistingFigureException("the customer exists allready");
            }
            else
            {
                Customer c = new Customer
                {
                    Id = id,
                    Name = name,
                    Phone = phone,
                    Longitude = longitude,
                    Lattitude = lattitude
                };
                DataSource.customers.Add(c);//Adding the new customer to the array
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update Customer Data
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        /// <param name="newPhone">customer phone</param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {
            if (!DataSource.customers.Exists(c => c.Id == id))
            {
                throw new NoDataExistsException("the customer not exists in the list of customers");
            }
            else
            {
                int cIndex = DataSource.customers.FindIndex(c => c.Id == id);
                Customer customer = DataSource.customers[cIndex];
                customer.Name = newName;
                customer.Phone = newPhone;
                DataSource.customers[cIndex] = customer;
            }
        }

        /// <summary>
        /// Update Customer name
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        public void UpdateCustomerName(int id, string newName)
        {
            if (!DataSource.customers.Exists(c => c.Id == id))
            {
                throw new NoDataExistsException("the customer not exists in the list of customers");
            }
            else
            {
                int cIndex = DataSource.customers.FindIndex(c => c.Id == id);
                Customer customer = DataSource.customers[cIndex];
                customer.Name = newName;
                DataSource.customers[cIndex] = customer;
            }
        }

        /// <summary>
        /// Update Customer phone
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newPhone">customer phone</param>
        public void UpdateCustomerPhone(int id, string newPhone)
        {
            if (!DataSource.customers.Exists(c => c.Id == id))
            {
                throw new NoDataExistsException("the customer not exists in the list of customers");
            }
            else
            {
                int cIndex = DataSource.customers.FindIndex(c => c.Id == id);
                Customer customer = DataSource.customers[cIndex];
                customer.Phone = newPhone;
                DataSource.customers[cIndex] = customer;
            }
        }
        #endregion

        #region Get item
        /// <summary>
        /// return customer by customer ID to print.
        /// </summary>
        /// <param name="customerId">customer ID to print</param>
        /// <returns>customer to show</returns>
        public Customer GetCustomer(int customerId)
        {
            if (!DataSource.customers.Exists(customer => customer.Id == customerId))
            {
                throw new NoDataExistsException("the customer not exists in the list of customers");
            }
            //find the place of the customer in the array of customers
            return DataSource.customers.Find(c => c.Id == customerId);
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual custpmer
        /// </summary>
        /// <returns>list of castomers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return from customer in DataSource.customers
                   select customer.Clone();
        }
        #endregion
    }
}
