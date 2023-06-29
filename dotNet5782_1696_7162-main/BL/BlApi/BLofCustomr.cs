using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    internal partial class BL : IBL
    {
        public void AddCustomer(Customer c)
        {
            try
            {
                data.AddCustomer(new()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Phone = c.Phone,
                    Location = new()
                    {
                        Longitude = c.Location.Longitude,
                        Latitude = c.Location.Latitude
                    },
                });
            }
            catch (DO.IdExistException ex)
            {
                throw new IdExistException("ERROR", ex);
            }
        }

        public Customer GetCustomer(int id)
        {
            try
            {
                IEnumerable<DO.Package> packages = data.GetAllPackages();
                DO.Customer customer = data.GetCustomer(id);
                return new Customer
                {
                    ID = customer.ID,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Location = convertLocationToBL(customer.Location),
                    PackageSentBy = (from i in packages
                                     where i.TargetID == id
                                     select new PackageInCust
                                     {
                                         ID = i.ID,
                                         Priority = (Priorities)i.Priority,
                                         Situation = i.Delivered != null ? Situations.Delivered : i.PickedUp != null ? Situations.PickedUp : i.assigning != null ? Situations.Assigned : Situations.Created,
                                         Weight = (WeightCategories)i.Weight,
                                         TheOtherCustomer = new CustAtPackage { ID = i.SenderID, Name = data.GetallCustomersDel(x => x.ID == i.SenderID).First().Name }
                                     }).ToList(),
                    PackageRecivedBy = (from i in packages
                                        where i.SenderID == id
                                        select new PackageInCust
                                        {
                                            ID = i.ID,
                                            Priority = (Priorities)i.Priority,
                                            Situation = i.Delivered != null ? Situations.Delivered : i.PickedUp != null ? Situations.PickedUp : i.assigning != null ? Situations.Assigned : Situations.Created,
                                            Weight = (WeightCategories)i.Weight,
                                            TheOtherCustomer = new CustAtPackage { ID = i.TargetID, Name = data.GetallCustomersDel(x => x.ID == i.TargetID).First().Name }
                                        }).ToList()
                };
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public IEnumerable<CustToList> GetAllCustomers(Predicate<CustToList> filter = null)
        {
            return convertCustomersToBL().Where(x => filter == null ? true : filter(x));
        }

        public void UpdateCustomer(Customer c)
        {
            try
            {
                DO.Customer dalCustomer = data.GetCustomer(c.ID);
                if (c.Name != null)
                    dalCustomer.Name = c.Name;
                if (c.Phone != null)
                    dalCustomer.Phone = c.Phone;
                data.UpdateCustomer(dalCustomer);
            }
            catch (DO.IdIsNotExistExeption ex)
            {
                throw new IdIsNotExistExeption("ERROR", ex);
            }
        }

        public void RemoveCustomer(int id)
        {
            if (GetAllPackages(x => GetPackage(x.ID).Target.ID == id && x.Delivered == null).Any() == true)
                throw new NotImplementedException("this customer can not be deleted cous he have package in a middle of delivery.");
            data.RemoveCustomer(id);
        }


        #region converting function

        private IEnumerable<CustToList> convertCustomersToBL()
        {
            IEnumerable<DO.Customer> dalCustomers = data.GetAllCustomers();
            List<CustToList> blCustomers = new List<CustToList>();
            IEnumerable<DO.Package> dalPackages = data.GetAllPackages();

            foreach (var i in dalCustomers)
            {
                blCustomers.Add(new CustToList
                {
                    ID = i.ID,
                    Name = i.Name,
                    Phone = i.Phone,
                    SumOfDeliveredPackages = dalPackages.Where(x => x.SenderID == i.ID && x.Delivered != null).Count(),
                    SumOfSendedPackages = dalPackages.Where(x => x.SenderID == i.ID && x.PickedUp != null && x.Delivered == null).Count(),
                    SumOfRecivedPackages = dalPackages.Where(x => x.TargetID == i.ID && x.Delivered != null).Count(),
                    SumOfOnWayPackages = dalPackages.Where(x => x.TargetID == i.ID && x.PickedUp != null && x.Delivered == null).Count()
                });
            }
            return blCustomers;
        }

        #endregion
    }
}
