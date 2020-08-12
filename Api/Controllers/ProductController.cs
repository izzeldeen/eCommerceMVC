

using com.sun.org.apache.xpath.@internal.functions;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Entities.Specifications;
using eCommerce.Services.ApiServices;
using eCommerceApi.Dtos;
using javax.swing.text.html;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Razor.Parser;

namespace Api.Controllers
{


    public class ProductController : ApiController
    {
        eCommerceContext _context = new eCommerceContext();


        public List<ProductToReturnDto> GetAll()
        {
            var product = _context.Products.Include(x => x.ProductPictures.Select(c => c.Picture)).ToList();           
            List<ProductToReturnDto> response = new List<ProductToReturnDto>();
            var categories = _context.Categories;
            foreach (var elem in product)
            {
                var category = categories.Find(elem.CategoryID).Name;
                var categoryArName = categories.Find(elem.CategoryID).ArName;
              //  System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");              
                elem.Description =  RemoveRegx(elem.Description);
                elem.ArDescription = RemoveRegx(elem.ArDescription);
                ProductToReturnDto products = new ProductToReturnDto
                {
                    Category = category,
                    CategoryId = elem.CategoryID,
                    CategoryArName = categoryArName,
                    Price = elem.Price,
                    Name = elem.Name,
                    PictuerUrl = elem.ProductPictures[0].Picture.URL.ToString(),
                    ArName = elem.ArName,
                    isOutOfStouck = elem.isOutOfStock,
                    Id = elem.ID,
                    Description = elem.Description,
                    ArDescription = elem.ArDescription
                    
                
                };

                response.Add(products);
            }

            return response;

        }
    

        public string RemoveRegx(string elem)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            elem = rx.Replace(elem, "");
            elem = Regex.Replace(elem, @"&lt;.+?&gt;|&nbsp;|\t|\r|\n", "").Trim();

            return elem;
        }

        [System.Web.Http.HttpGet]

        public ProductToReturnDto GetProductsById(int id)

        {
            var product = _context.Products.Include( c => c.ProductPictures.Select(x => x.Picture)).FirstOrDefault(x=>x.ID == id);
            var categories = _context.Categories;

            var category = categories.Find(product.CategoryID).Name;
            var categoryArName = categories.Find(product.CategoryID).ArName;
            if (product != null)
            {
                return new ProductToReturnDto (){
                  
                    Category =category,
                    CategoryArName = categoryArName,
                    Price = product.Price,
                    Name = product.Name,
                    PictuerUrl = product.ProductPictures[0].Picture.URL.ToString(),
                    ArName = product.ArName,
                    isOutOfStouck = product.isOutOfStock,
                    Id = product.ID

                };
            }


            return null;


        }

        [System.Web.Http.HttpGet]
        //api/Product?sort=&Search=&CategoryId=&SuplierId=&PageSize=5&PageIndex=1
        public async Task<List<Product>> GetALLProduct(string sort, string Search, int? CategoryId, int? SuplierId, int PageSize = 5, int PageIndex = 1)
        {
            var spec = new ProductWithIncludes(sort, CategoryId, SuplierId, PageSize, PageIndex, Search);
           
            
            var products = await ProductsApiService.Instance.GetALLproductWithIncude(spec);

            return products;


        }

        

    }
}
