using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    internal partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// adding customer to the data base
        /// </summary>
        public void AddCustomer(Customer cust)
        {
            int index = DataSource.Customers.FindIndex(i => i.ID == cust.ID);
            if (index != -1)
                throw new IdExistException("the customer is already exist.");
            DataSource.Customers.Add(cust);
        }

        /// <summary>
        /// return specific customer with the same ID.
        /// </summary>
        /// <param name="id">customer's ID</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            int index = DataSource.Customers.FindIndex(i => i.ID == id);
            if (index == -1)
                throw new IdIsNotExistExeption("the id was'nt found.");
            return DataSource.Customers[index];
        }

        public IEnumerable<Customer> GetallCustomersDel(Predicate<Customer> filter = null)
        {
            return DataSource.Customers.FindAll(x => filter == null ? true : filter(x) && x.Deleted == false);

        }


        /// <summary>
        /// return a list with all Customers.
        /// </summary>
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> filter = null)
        {
            return DataSource.Customers.FindAll(x => filter == null ? true : filter(x) && x.Deleted == false);
        }

        public void UpdateCustomer(Customer c)
        {
            int index = DataSource.Customers.FindIndex(i => c.ID == i.ID);
            Customer newCust = DataSource.Customers[index];
            newCust.Name = c.Name;
            newCust.Phone = c.Phone;
            DataSource.Customers[index] = newCust;
        }

        /// <summary>
        /// delete customer from the data. (not realy delete it but marking it as a "deleted".)
        /// </summary>
        /// <param name="id"></param>
        public void RemoveCustomer(int id)
        {
            int index = DataSource.Customers.FindIndex(x => x.ID == id);
            Customer c = DataSource.Customers[index];
            c.Deleted = true;
            DataSource.Customers[index] = c;
        }
    }
}
