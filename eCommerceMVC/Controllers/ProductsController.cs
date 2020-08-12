using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerceMVC.Controllers
{
    public class ProductsController : Controller
    {
        //[OutputCache(Duration = 1000, VaryByParam = "productID;pageSize")]
        public ActionResult FeaturedProducts(int? productID, int pageSize = 0, bool isForHomePage = false)
        {
            if(pageSize == 0)
            {
                pageSize = ConfigurationsHelper.FeaturedRecordsSizePerPage;
            }

            HttpCookie LangCookie = Request.Cookies["LangCookie"];
            FeaturedProductsViewModel model = new FeaturedProductsViewModel();
            model.Products = ProductsService.Instance.SearchFeaturedProducts(pageSize, new List<int>() { productID.HasValue ? productID.Value : 0 });



            if (LangCookie != null)
            {
                if (LangCookie.Value == "ar") { 

                    foreach (var elem in model.Products.ToList())
                    {

                            elem.Name = elem.ArName;
                            elem.Description = elem.ArDescription;
                            elem.Summary = elem.ArSummary;
                        elem.Category.Name = elem.Category.ArName;



                    }
                }
            }



            if (isForHomePage)
            {
                return PartialView("_FeaturedProductsHomePage", model);
            }
            else
            {
                return PartialView("_FeaturedProducts", model);
            }
        }

        public ActionResult RecentProducts(int? productID, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                pageSize = ConfigurationsHelper.FrontendRecordsSizePerPage;
            }

            HttpCookie LangCookie = Request.Cookies["LangCookie"];
            FeaturedProductsViewModel model = new FeaturedProductsViewModel();
            model.Products = ProductsService.Instance.SearchProducts(null, null, null, null, null, 1, pageSize);

            if (LangCookie != null)
            {
                if (LangCookie.Value == "ar")
                {

                    foreach (var elem in model.Products.ToList())
                    {

                        elem.Name = elem.ArName;
                        elem.Description = elem.ArDescription;
                        elem.Summary = elem.ArSummary;
                        elem.Category.Name = elem.Category.ArName;



                    }
                }
            }





            return PartialView("_RecentProducts", model);
        }

        public ActionResult RelatedProducts(int categoryID, int productID, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                pageSize = ConfigurationsHelper.RelatedProductsRecordsSizePerPage;
            }

            HttpCookie LangCookie = Request.Cookies["LangCookie"];

            RelatedProductsViewModel model = new RelatedProductsViewModel();
            model.Products = ProductsService.Instance.SearchProducts(new List<int>() { categoryID }, null, null, null, null, 1, pageSize);


            if(model.Products != null && model.Products.Count >= ConfigurationsHelper.RelatedProductsRecordsSizePerPage)
            {
                model.PageTitle = model.Products.FirstOrDefault().Category.Name;
            }
            else
            {
                //the realted products are less than the specfified RelatedProductsRecordsSizePerPage, so instead show featured products
                model.PageTitle = string.Format("{0}'s", ConfigurationsHelper.ApplicationName);
                model.Products = ProductsService.Instance.SearchFeaturedProducts(pageSize);
            }

            if (LangCookie != null)
            {
                if (LangCookie.Value == "ar")
                {

                    foreach (var elem in model.Products.ToList())
                    {

                        elem.Name = elem.ArName;
                        elem.Description = elem.ArDescription;
                        elem.Summary = elem.ArSummary;
                        elem.Category.Name = elem.Category.ArName;


                    }
                }
            }

            return PartialView("_RelatedProducts", model);
        }


        [HttpGet]
        public ActionResult Details(int ID, string category)
        {


            ProductDetailsViewModel model = new ProductDetailsViewModel();
            
            model.Product = ProductsService.Instance.GetProductByID(ID);

            HttpCookie LangCookie = Request.Cookies["LangCookie"];
            if (LangCookie != null)
            {
                if (LangCookie.Value == "ar")
                {



                    model.Product.Name = model.Product.ArName;
                    model.Product.Description = model.Product.ArDescription;
                    model.Product.Summary = model.Product.ArSummary;
                    model.Product.Category.Name = model.Product.Category.ArName;




                }
            }

            if (model.Product == null || !model.Product.Category.SanitizedName.ToLower().Equals(category))
                return HttpNotFound();            

            model.EntityID = (int)EntityEnums.Product;
            model.RecordID = model.Product.ID;
            model.Comments = CommentsService.Instance.GetComments(model.EntityID, model.RecordID);

            model.PageTitle = model.Product.Name;
            model.PageDescription = model.Product.Summary;
            model.PageURL = Url.ProductDetails(category, model.Product.ID).ToSiteURL();

            var thumbnail = model.Product.ProductPictures.Where(x => x.PictureID == model.Product.ThumbnailPictureID).FirstOrDefault();
            
            if(thumbnail != null)
            {
                model.PageImageURL = PictureHelper.PageImageURL(thumbnail.Picture.URL, isCompletePath:true);
            }
            else
            {
                model.PageImageURL = PictureHelper.PageImageURL("products.jpg");
            }

            return View(model);
        }
    }
}