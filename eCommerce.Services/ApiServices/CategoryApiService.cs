using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services.ApiServices
{
     public  class CategoryApiService
    {

        Methods Method = new Methods();

        private readonly eCommerceContext _context;

        public CategoryApiService()
        {
        }

        private CategoryApiService(eCommerceContext context)
        {
            _context = context;
         }

        public async Task<List<Category>> GetAllCategrise() 
        { 
            eCommerceContext context = new eCommerceContext();
          return  await context.Categories
                .Include(x => x.ParentCategory)
                .Include(a=>a.Products.Select(f=>f.ProductPictures.Select(t=>t.Picture))).
                ToListAsync();
        }

       public async Task<List<Category>> GetAllCategoriesWithoutInclude()
        {
            eCommerceContext context = new eCommerceContext();
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int Id)
        {
            eCommerceContext context = new eCommerceContext();
            Category res =  await context.Categories.Include(x => x.ParentCategory)
                        .Include(a => a.Products.Select(f => f.ProductPictures.Select(t => t.Picture))).
                         FirstOrDefaultAsync(x => x.ID == Id);

            res.Description = Method.RemoveRegx(res.Description);
            foreach (var item in res.Products)
            {
                item.Description = Method.RemoveRegx(item.Description);
                item.ArDescription = Method.RemoveRegx(item.ArDescription);
            }

            return res;
        }

        public async Task<List<Category>> SearchCategory(string Name)
        {
            if (Name == null) return null;
            eCommerceContext context = new eCommerceContext();
            List<Category> res = await context.Categories.Include(x => x.ParentCategory)
                        .Include(a => a.Products.Select(f => f.ProductPictures.Select(t => t.Picture)))
                        .Where(x => x.Name == Name || Name == x.ArName).ToListAsync();
            
           // int i = 0;
           for(int i=0 ; i< res.Count();i++)
            {
                res[i].Description = Method.RemoveRegx(res[i].Description);
                foreach (var item in res[i].Products)
                {
                    item.Description = Method.RemoveRegx(item.Description);
                    item.ArDescription = Method.RemoveRegx(item.ArDescription);
                }
            }
           

            return res;

        }


        //internal DbSet<Category> dbSet;

        //private static CategoryApiService _Instance;
        //public CategoryApiService()
        //{

        //}

        //eCommerceContext _context = new eCommerceContext();
        //#region Define as Singleton
        //public static CategoryApiService Instance
        //{
        //    get
        //    {
        //        if (_Instance == null)
        //        {
        //            _Instance = new CategoryApiService();
        //        }

        //        return (_Instance);
        //    }
        //}
    }
}
