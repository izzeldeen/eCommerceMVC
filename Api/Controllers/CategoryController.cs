using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Services.ApiServices;

namespace Api.Controllers
{
    public class CategoryController : ApiController
    {
      
        private CategoryApiService CategoryServiceApi = new CategoryApiService();

        public async Task<List<Category>> GetAllCategories() => await CategoryServiceApi.GetAllCategrise();
        public async Task<Category> GetCategoryById(int Id) => await CategoryServiceApi.GetCategoryById(Id);
        [HttpGet]
        public async Task<List<Category>> SearchCategory(string Name) => await CategoryServiceApi.SearchCategory(Name);
  
    }
}