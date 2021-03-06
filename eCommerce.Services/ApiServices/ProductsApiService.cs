﻿using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Entities.Specifications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services.ApiServices
{
    public class ProductsApiService
    {
        internal DbSet<Product> dbSet;

        private static ProductsApiService _Instance;
        public ProductsApiService()
        {

        }

        eCommerceContext _context = new eCommerceContext();
        #region Define as Singleton
        public static ProductsApiService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ProductsApiService();
                }

                return (_Instance);
            }
        }

        private ProductsApiService(eCommerceContext context)
        {
            _context = context;
            this.dbSet = _context.Set<Product>();


        }




        #endregion

        public async Task<List<Product>> GetAll()
        {
            eCommerceContext context = new eCommerceContext();
            return await context.Products.Include(x => x.ProductPictures.Select(c => c.Picture)).ToListAsync();


        }

        public async Task<Product> GetProductById(int id)
        {
            eCommerceContext context = new eCommerceContext();
            return await context.Products.Include(x => x.ProductPictures.Select(c => c.Picture)).FirstOrDefaultAsync(x=>x.ID == id);
             }


        public async Task<IReadOnlyList<Product>> ListAsync(ISpecifications<Product> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        

        public async Task<List<Product>> GetALLproductWithIncude(ISpecifications<Product> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<Product> GetEntityWithSpec(ISpecifications<Product> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<Product> GetEntityWithSpec(ISpecifications<Product> spec, int id)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<Product> ApplySpecification(ISpecifications<Product> spec)
        {
           

            return SpecificationEvaluator<Product>.GetQuery(_context.Set<Product>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecifications<Product> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public IEnumerable<Product> Get(Expression<Func<Product, bool>> filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null, string includeProperties = null)
        {
            this.dbSet = _context.Set<Product>();
            IQueryable<Product> query = dbSet;

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);

                }
            }

            return query.ToList();

        }

        public Product GetProductById(Expression<Func<Product, bool>> filter = null, string includeProperties = null)
        {
            this.dbSet = _context.Set<Product>();
            IQueryable<Product> query = dbSet;

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);

                }
            }

            return query.FirstOrDefault();

        }

        public IQueryable<Product> SortByPrice(decimal? from,decimal? to , string search)
        {
            eCommerceContext context = new eCommerceContext();

            var Products =  context.Products.AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                Products = Products.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }
            if (from.HasValue && from.Value > 0.0M)
            {
                Products = Products.Include(x => x.ProductPictures.Select(a => a.Picture)).Where(x => x.Price >= from.Value);
            }

            if (to.HasValue && to.Value > 0.0M)
            {
                Products = Products.Include(x=>x.ProductPictures.Select(a=>a.Picture)).Where(x => x.Price <= to.Value);
            }

            return  Products;



        }

    }
}
