using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities.Specifications
{
    public class ProductForFiltersForCount : BaseSpecification<Product>
    {
        public ProductForFiltersForCount(string sort, int? CateogyID, int? SuplierId, int PageSize, int PageIndex , string Search) : base(x =>
           (string.IsNullOrEmpty(Search) ||
        x.Name.ToLower().Contains(Search)) &&
   (!CateogyID.HasValue || x.CategoryID == CateogyID)
   && (!SuplierId.HasValue || x.SupplierID == SuplierId)
        )
        {

        }
    }
}
