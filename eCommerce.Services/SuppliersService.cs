using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class SuppliersService
    {
        #region Define as Singleton
        private static SuppliersService _Instance;

        public static SuppliersService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SuppliersService();
                }

                return (_Instance);
            }
        }

        private SuppliersService()
        {
        }
        #endregion

        public List<Supplier> GetAllSuppliers()
        {
            eCommerceContext context = new eCommerceContext();

            return context.Suppliers.ToList();
        }

        public Supplier GetSupplierByID(int ID)
        {
            using (var context = new eCommerceContext())
            {
                return context.Suppliers.Find(ID);
            }
        }

        public List<Supplier> SearchSuppliers(int? supplierID, string searchTerm, int? pageNo, int pageSize)
        {
            eCommerceContext context = new eCommerceContext();

            var suppliers = context.Suppliers.AsQueryable();

            if (supplierID.HasValue && supplierID.Value > 0)
            {
                suppliers = suppliers.Where(x => x.ID == supplierID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                suppliers = suppliers.Where(x => x.SupplierName.ToLower().Contains(searchTerm.ToLower()));
            }

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * pageSize;

            return suppliers.ToList();
        }

        public int GetSupplierCount(int? supplierID, string searchTerm)
        {
            eCommerceContext context = new eCommerceContext();

            var suppliers = context.Suppliers.AsQueryable();

            if (supplierID.HasValue && supplierID.Value > 0)
            {
                suppliers = suppliers.Where(x => x.ID == supplierID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                suppliers = suppliers.Where(x => x.SupplierName.ToLower().Contains(searchTerm.ToLower()));
            }

            return suppliers.Count();
        }

        public bool DeleteSupplier(int ID)
        {
            using (var context = new eCommerceContext())
            {
                var supplier = context.Suppliers.Find(ID);

                context.Suppliers.Remove(supplier);

                return context.SaveChanges() > 0;
            }
        }
        public void SaveSupplier(Supplier supplier)
        {
            eCommerceContext context = new eCommerceContext();

            context.Suppliers.Add(supplier);

            context.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            eCommerceContext context = new eCommerceContext();

            context.Entry(supplier).State = System.Data.Entity.EntityState.Modified;

            context.SaveChanges();
        }

    }
}
