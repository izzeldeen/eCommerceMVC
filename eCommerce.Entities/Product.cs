using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Entities
{
    public class Product : BaseEntity
    {
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }


        public string Name { get; set; }
        public string ArName { get; set; }
        public string Summary { get; set; }
        public string ArSummary { get; set; }
        public string Description { get; set; }
        public string ArDescription { get; set; }
        public decimal Price { get; set; }

        public decimal? Cost { get; set; }

        public string UserId { get; set; }

        public bool isFeatured { get; set; }
        public bool isOutOfStock { get; set; } = false;
        public int ThumbnailPictureID { get; set; }


        public virtual List<ProductPicture> ProductPictures { get; set; }
        public virtual List<ProductSpecification> ProductSpecifications { get; set; }
        public virtual List<ProductCost> ProductCosts { get; set; }
    }
}
