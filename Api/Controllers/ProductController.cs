

using Castle.Core.Smtp;
using com.sun.org.apache.xpath.@internal.functions;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Entities.Specifications;
using eCommerce.Services;
using eCommerce.Services.ApiServices;
using eCommerce.Shared;
using eCommerceApi.Dtos;
using javax.swing.text.html;
using Magnum.Extensions;
using System;
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
        ProductsApiService productsApiService = new ProductsApiService();
        Methods Method = new Methods();
        CategoryApiService categoryApiService = new CategoryApiService();
        public async Task<List<ProductToReturnDto>> GetAll()
        {
            
            var product = await productsApiService.GetAll();          
            List<ProductToReturnDto> response = new List<ProductToReturnDto>();
            var categories = _context.Categories;
            foreach (var elem in product)
            {
                var category = categories.Find(elem.CategoryID).Name;
                var categoryArName = categories.Find(elem.CategoryID).ArName;            
                elem.Description =  Method.RemoveRegx(elem.Description);
                elem.ArDescription = Method.RemoveRegx(elem.ArDescription);
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
    
        [System.Web.Http.HttpGet]

        public async Task<ProductToReturnDto> GetProductsById(int id)

        {
            Product product = await productsApiService.GetProductById(id);
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
                    Description = Method.RemoveRegx(product.Description),
                    ArDescription = Method.RemoveRegx(product.ArDescription),
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


        public IQueryable<Product> GetProductSortByPrice( decimal from  , decimal to, string search)
        {
           var ListProduct =    productsApiService.SortByPrice(from , to , search);

            foreach(var item in ListProduct)
            {
                item.Description = Method.RemoveRegx(item.Description);
                item.ArDescription = Method.RemoveRegx(item.ArDescription);
                
            }    

            return ListProduct;

        }
        

    }
}
