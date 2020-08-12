using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerceApi.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryArName { get; set; }
        public string Category { get; set; }

        public string ArName { get; set; }
    //    public List<ProductPicture> PictuerUrl { get; set; }
       public string PictuerUrl { get; set; }
   // public List<string> pictuer { get; set;  }
        public bool isOutOfStouck { get; set; } = false;
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArDescription { get; set; }

        //   public List<ProductPicture> ProductPictures { get; set; }

        //public string ArName { get; set; }
        //public string Summary { get; set; }
        //public string ArSummary { get; set; }
        //public string Description { get; set; }
        //public string ArDescription { get; set; }

        //public decimal Price { get; set; }

        //public decimal? Cost { get; set; }

        //public string UserId { get; set; }

        //public bool isFeatured { get; set; }
        //public bool isOutOfStock { get; set; } = false;
        //public int ThumbnailPictureID { get; set; }




    }
}