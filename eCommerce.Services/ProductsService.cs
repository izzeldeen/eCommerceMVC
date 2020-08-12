﻿using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class ProductsService
    {
        #region Define as Singleton
        private static ProductsService _Instance;

        public static ProductsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ProductsService();
                }

                return (_Instance);
            }
        }

        private ProductsService()
        {
        }
        #endregion

        public List<Product> GetAllProducts()
        {
            eCommerceContext context = new eCommerceContext();

            return context.Products.ToList();
        }

        public List<Product> SearchFeaturedProducts(int pageSize, List<int> excludeProductIDs = null)
        {
            excludeProductIDs = excludeProductIDs ?? new List<int>();

            eCommerceContext context = new eCommerceContext();

            return context.Products.Where(a => a.isFeatured && !excludeProductIDs.Contains(a.ID)).OrderByDescending(x => x.ID).Take(pageSize).ToList();
        }

        public List<Product> SearchProducts(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to, string sortby, int? pageNo, int pageSize)
        {
            eCommerceContext context = new eCommerceContext();

            var Products = context.Products.AsQueryable();

            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                Products = Products.Where(x => categoryIDs.Contains(x.CategoryID));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Products = Products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                Products = Products.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                Products = Products.Where(x => x.Price <= to.Value);
            }

            if (!string.IsNullOrEmpty(sortby) && string.Equals(sortby, "names", StringComparison.OrdinalIgnoreCase))
            {
                Products = Products.OrderBy(x => x.Name);
            }
            else //sortBy Product Date
            {
                Products = Products.OrderByDescending(x => x.ModifiedOn);
            }

            pageNo = pageNo ?? 1;

            var skipCount = (pageNo.Value - 1) * pageSize;

            return Products.Skip(skipCount).Take(pageSize).ToList();
        }

        public int GetProductCount(List<int> categoryIDs, string searchTerm, decimal? from, decimal? to)
        {
            eCommerceContext context = new eCommerceContext();

            var Products = context.Products.AsQueryable();

            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                Products = Products.Where(x => categoryIDs.Contains(x.CategoryID));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Products = Products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (from.HasValue && from.Value > 0.0M)
            {
                Products = Products.Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                Products = Products.Where(x => x.Price <= to.Value);
            }

            return Products.Count();
        }

        public int GetDataEntryProductsCount(string sentUserId)
        {
            eCommerceContext context = new eCommerceContext();

            var Products = context.Products.Where(x => x.UserId == sentUserId).ToList();

            return Products.Count();
        }

        public List<Product> GetDataEntryProducts(string sentUserId)
        {
            eCommerceContext context = new eCommerceContext();

            var Products = context.Products.Where(x => x.UserId == sentUserId).ToList();

            return Products;
        }

        public Product GetProductByID(int ID)
        {
            eCommerceContext context = new eCommerceContext();

            return context.Products.Find(ID);
        }

        public List<Product> GetProductsByIDs(List<int> IDs)
        {
            eCommerceContext context = new eCommerceContext();

            return IDs.Select(id => context.Products.Find(id)).OrderBy(x=>x.ID).ToList();
        }

        public void SaveProduct(Product Product)
        {
            eCommerceContext context = new eCommerceContext();

            context.Products.Add(Product);

            context.SaveChanges();
        }


        public void UpdateProduct(Product Product)
        {
            eCommerceContext context = new eCommerceContext();

            var exitingProduct = context.Products.Find(Product.ID);

            context.ProductPictures.RemoveRange(exitingProduct.ProductPictures);
            context.ProductSpecifications.RemoveRange(exitingProduct.ProductSpecifications);
            context.ProductCosts.RemoveRange(exitingProduct.ProductCosts);

            context.Entry(exitingProduct).CurrentValues.SetValues(Product);

            context.ProductPictures.AddRange(Product.ProductPictures);
            context.ProductSpecifications.AddRange(Product.ProductSpecifications);
            context.ProductCosts.AddRange(Product.ProductCosts);

            context.SaveChanges();
        }
        
        public bool DeleteProduct(int ID)
        {
            using (var context = new eCommerceContext())
            {
                var product = context.Products.Find(ID);

                context.Products.Remove(product);

                return context.SaveChanges() > 0;
            }
        }
    }
}
