using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;

namespace Api.Controllers
{
    public class CategoryController : ApiController
    {
        private eCommerceContext db = new eCommerceContext();

     
        
        
        [HttpGet]
         public List<Category> GetAll() => db.Categories.Include(x=>x.ParentCategory).ToList();
        [HttpGet]
        public Category GetCategoryById(int id) => CategoriesService.Instance.GetCategoryByID(id);
        [HttpGet]
        public Category SearchCategory(string Search) => CategoriesService.Instance.GetCategoryByName(Search);








    }
}