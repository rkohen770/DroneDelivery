using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DO;
namespace DL
{
    public sealed partial class DalXml : DalApi.IDal
    {
        #region Add
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
          //  XElement dalCustomersRoot = XElement.Load(CustomersPath);
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            XElement customer = new XElement("Customer",
                                new XElement("CustomerID", id),
                                new XElement("Name", name),
                                new XElement("Phone", phone),
                                new XElement("Longitude", longitude),
                                new XElement("Lattitude", lattitude),
                                new XElement("Available", true));

            dalCustomersRoot.Add(customer);
            XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);

          //  dalCustomersRoot.Save(CustomersPath);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update customer data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        public void UpdateCustomerData(int id, string newName, string newPhone)
        {
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            //XElement dalCustomersRoot = XElement.Load(CustomersPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("CustomerID").Value == id.ToString()
                                    select customer).FirstOrDefault();
            if (dalCustomer == null) throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
            dalCustomer.Element("Name").SetValue(newName);
            dalCustomer.Element("Phone").SetValue(newPhone);
            XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);

           // dalCustomersRoot.Save(CustomersPath);
        }

        /// <summary>
        /// update customer name
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newName">customer name</param>
        public void UpdateCustomerName(int id, string newName)
        {
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            //XElement dalCustomersRoot = XElement.Load(CustomersPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("CustomerID").Value == id.ToString()
                                    select customer).FirstOrDefault();
            if (dalCustomer == null) throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
            dalCustomer.Element("Name").SetValue(newName);
            XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);

           // dalCustomersRoot.Save(CustomersPath);
        }
        /// <summary>
        /// customer phone
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="newPhone">customer name</param>
        public void UpdateCustomerPhone(int id, string newPhone)
        {
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            //XElement dalCustomersRoot = XElement.Load(CustomersPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("CustomerID").Value == id.ToString()
                                    select customer).FirstOrDefault();
            if (dalCustomer == null) throw new BadCustomerIDException(id, $"The customer doesn't exist in the system");
            dalCustomer.Element("Phone").SetValue(newPhone);
            XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);

          //  dalCustomersRoot.Save(CustomersPath);
        }
        #endregion

        #region GEt
        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Customer object </returns>
        public Customer GetCustomer(int customerId)
        {
            //XElement dalCustomersRoot = XElement.Load(CustomersPath);
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            Customer dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("CustomerID").Value == customerId.ToString()
                                    select new Customer
                                    {
                                        CustomerID = customerId,
                                        Name = customer.Element("Name").Value,
                                        Phone = customer.Element("Phone").Value,
                                        Longitude = double.Parse(customer.Element("Longitude").Value),
                                        Lattitude = double.Parse(customer.Element("Lattitude").Value),
                                        Available = bool.Parse(customer.Element("Available").Value)
                                    }).FirstOrDefault();

            //dalCustomersRoot.Save(CustomersPath);
            XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);
            return dalCustomer.CustomerID != customerId || dalCustomer.Available == false ? throw new BadCustomerIDException(customerId, $"The customer doesn't exist in the system") : dalCustomer;
        }
        #endregion

        #region Get lists
        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            //XElement dalCustomersRoot = XElement.Load(CustomersPath);

            return from customer in dalCustomersRoot.Elements()
                   where XmlConvert.ToBoolean(customer.Element("Available").Value)
                   select new Customer
                   {
                       CustomerID = XmlConvert.ToInt32(customer.Element("CustomerID").Value),
                       Name = customer.Element("Name").Value,
                       Phone = customer.Element("Phone").Value,
                       Longitude = XmlConvert.ToDouble(customer.Element("Longitude").Value),
                       Lattitude = XmlConvert.ToDouble(customer.Element("Lattitude").Value),
                       Available = XmlConvert.ToBoolean(customer.Element("Available").Value)
                   };
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
            XElement dalCustomersRoot = XMLTools.LoadListFromXMLElement(CustomersPath);

            //XElement dalCustomersRoot = XElement.Load(CustomersPath);

            XElement dalCustomer = (from customer in dalCustomersRoot.Elements()
                                    where customer.Element("CustomerID").Value == customerId.ToString()
                                    select customer).FirstOrDefault();
            dalCustomer.Element("Available").SetValue(false);
           // dalCustomersRoot.Save(CustomersPath);
             XMLTools.SaveListToXMLElement(dalCustomersRoot, CustomersPath);

        }
        #endregion
    }
}
