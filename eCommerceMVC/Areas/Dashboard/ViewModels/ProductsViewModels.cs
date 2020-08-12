using eCommerce.Entities;
using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerceMVC.Areas.Dashboard.ViewModels
{
    public class ProductsListingViewModel : PageViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public string SearchTerm { get; set; }
        
        public Pager Pager { get; set; }
    }
    
    public class ProductActionViewModel : PageViewModel
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string ArName { get; set; }
        public string Summary { get; set; }
        public string ArSummary { get; set; }
        public string Description { get; set; }
        public string ArDescription { get; set; }
        public decimal Price { get; set; }
        public bool isFeatured { get; set; }
        public bool isOutOfStock { get; set; }
        public string UserId { get; set; }

        public decimal? Cost { get; set; }

        public string ProductPictures { get; set; }
        public int ThumbnailPicture { get; set; }
        public List<ProductPicture> ProductPicturesList { get; set; }

        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<ProductSpecification> ProductSpecifications { get; set; }
        public List<ProductCost> ProductCosts { get; set; }
    }
}