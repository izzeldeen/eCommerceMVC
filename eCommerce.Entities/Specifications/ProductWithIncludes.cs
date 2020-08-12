using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.Specifications
{
    public class ProductWithIncludes : BaseSpecification<Product>
    {
        public ProductWithIncludes(string sort , int? CateogyID , int? SuplierId , int PageSize , int PageIndex, string Search) : base (x=>
        (string.IsNullOrEmpty(Search) ||
         x.Name.ToLower().Contains(Search)) &&
        (string.IsNullOrEmpty(Search) || x.Name.ToLower().Contains(Search))
         &&
        (!CateogyID.HasValue || x.CategoryID == CateogyID) 
        && (!SuplierId.HasValue || x.SupplierID == SuplierId)
        )
        {
            //AddInclude(x => x.Category.Select(x=>x.ParentCategory));
           AddInclude(x=>x.Category);
            AddInclude(x => x.Supplier);            
            AddInclude(x => x.ProductCosts);
            AddInclude(x => x.ProductPictures.Select(c=>c.Picture));
            AddInclude(x => x.ProductSpecifications);
            AddOrderBy(x => x.Name);
            ApplyPaging(PageSize * ( PageIndex - 1) , PageSize);

            if(!string.IsNullOrEmpty(sort))
            {
                switch(sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDes":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;

                }
            }

         }

        public ProductWithIncludes(int id)
            :base(x=> x.ID == id)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.Supplier);
            AddInclude(x => x.ProductCosts);
            AddInclude(x => x.ProductPictures.Select(c => c.Picture));
            AddInclude(x => x.ProductSpecifications);
        }
    }
}
