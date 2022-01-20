using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
    {
        #region ADD
        /// <summary>
        /// adds customer to the file
        /// </summary>
        /// <param name="id">Unique ID number</param>
        /// <param name="name">The customer name</param>
        /// <param name="phone">The customer phone number</param>
        /// <param name="longitude">Longitude within the borders of the Land of Israel</param>
        /// <param name="lattitude">Lattitude within the borders of the Land of Israel</param>
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id
                               select item).FirstOrDefault();
            if (CustomerAdd.CustomerID != 0 && CustomerAdd.Available)
                throw new CustomerAlreadyExistException(id, name, "The customer exists");
            if (CustomerAdd.CustomerID != 0 && !CustomerAdd.Available)
            {
                DeleteCustomer(id);
            }

            Customer c = new Customer
            {
                CustomerID = id,
                Name = name,
                Phone = phone,
                Longitude = longitude,
                Lattitude = lattitude,
            };
            ListCustomer.Add(c);
            XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Update Customer data
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {
            List<Customer> listCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in listCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();
            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Name = newName;
                c.Phone = newPhone;

                listCustomer.Remove(CustomerAdd);
                listCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(listCustomer, CustomersPath);
            }
            else throw new BadCustomerIDException(id, $"The customer: {id} doesn't exist");
        }

        /// <summary>
        /// update customer name
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        public void UpdateCustomerName(int id, string newName)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();

            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Name = newName;

                ListCustomer.Remove(CustomerAdd);
                ListCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
        }

        /// <summary>
        /// customer phone
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newPhone">customer name</param>
        public void UpdateCustomerPhone(int id, string newPhone)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            var CustomerAdd = (from item in ListCustomer
                               where item.CustomerID == id && item.Available
                               select item).FirstOrDefault();

            if (CustomerAdd.CustomerID != 0)
            {
                Customer c = CustomerAdd;
                c.Phone = newPhone;

                ListCustomer.Remove(CustomerAdd);
                ListCustomer.Add(c);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
        }
        #endregion

        #region Get item
        /// <summary>
        /// return customer by customer id 
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>get customer</returns>
        public Customer GetCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var c = (from item in ListCustomer
                     where item.CustomerID == customerId && item.Available
                     select item).FirstOrDefault();
            if (c.CustomerID == 0)
                throw new BadCustomerIDException(customerId, "the customer not exists in the list of customers");
            return c;
        }
        #endregion

        #region Get lists
        /// <summary>
        /// return a list of actual customer
        /// </summary>
        /// <returns>list of customers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            return from item in ListCustomer
                   where item.Available
                   select item;
        }

        /// <summary>
        /// return customers by predicat
        /// </summary>
        /// <param name="p">predicat</param>
        /// <returns>customers by predicat</returns>
        public IEnumerable<Customer> GetAllCustomerByPredicate(Predicate<Customer> p)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

            return from customer in ListCustomer
                   where p(customer)
                   select customer;
        }
        #endregion

        #region Delete
        /// <summary>
        /// deletes customer by the id number from the file
        /// </summary>
        /// <param name="customerId">customer id</param>
        public void DeleteCustomer(int customerId)
        {
            List<Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
            var CustomerDelete = (from item in ListCustomer
                                  where item.CustomerID == customerId && item.Available
                                  select item).FirstOrDefault();
            if (CustomerDelete.CustomerID != 0)
            {
                ListCustomer.Remove(CustomerDelete);
                CustomerDelete.Available = false;
                ListCustomer.Add(CustomerDelete);
                XMLTools.SaveListToXMLSerializer(ListCustomer, CustomersPath);
            }
            else throw new BadCustomerIDException(customerId, $"The customer: {customerId} doesn't exist");
        }
        #endregion

    }
}
