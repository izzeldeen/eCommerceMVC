using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared;
using eCommerce.Shared.Extensions;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eCommerceMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PageViewModel model = new PageViewModel();
            
            model.PageTitle = "Jomlah Jo";
            model.PageDescription = "";
            model.PageURL = Url.Home().ToSiteURL();
            model.PageImageURL = PictureHelper.PageImageURL("products.jpg");

            return View(model);
        }

        public ActionResult Language()
        {
            return PartialView("_Language");
        }

        public ActionResult SelectLanguage(string SelectedLanguage)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SelectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SelectedLanguage.ToLower());

            HttpCookie LangCookie = new HttpCookie("LangCookie");
            LangCookie.Value = SelectedLanguage;
            Response.Cookies.Add(LangCookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult HomeSliders()
        {
            HomeSlidersViewModel model = new HomeSlidersViewModel();
            
            model.SlidersConfigurations = ConfigurationsService.Instance.GetConfigurationsByType((int)ConfigurationType.HomeSliders);

            return PartialView("_BannerSlider", model);
        }

        public ActionResult Search(string category, string q, decimal? from, decimal? to, string sortby, int? pageNo, int? pageSize, bool isPartial = false)
        {
            pageSize = pageSize ?? ConfigurationsHelper.FrontendRecordsSizePerPage;

            ProductsViewModel model = new ProductsViewModel();
            model.Categories = CategoriesService.Instance.GetAllCategories();

            HttpCookie LangCookie = Request.Cookies["LangCookie"];

            if (!string.IsNullOrEmpty(category))
            {
                var selectedCategory = CategoriesService.Instance.GetCategoryByName(category);

                if (selectedCategory == null) return HttpNotFound();
                else
                {
                    model.CategoryID = selectedCategory.ID;
                    model.CategoryName = selectedCategory.SanitizedName;

                    model.SearchedCategories = Methods.GetAllCategoryChildrens(selectedCategory, model.Categories);

                    model.PageTitle = string.Format("{0} Products", selectedCategory.Name);
                    model.PageDescription = selectedCategory.Description;

                    model.PageURL = Url.SearchProducts(selectedCategory.SanitizedName, q).ToSiteURL();
                }
            }
            else
            {
                model.PageTitle = "Search Products";
                model.PageDescription = "Search Latest Products on Jomlah Jo";

                model.PageURL = Url.SearchProducts().ToSiteURL();
            }

            model.PageImageURL = PictureHelper.PageImageURL("products.jpg");

            model.SearchTerm = q;
            model.PriceFrom = from;
            model.PriceTo = to;
            model.SortBy = sortby;
            model.PageSize = pageSize;
            model.isPartial = isPartial;
            
            var selectedCategoryIDs = model.SearchedCategories != null ? model.SearchedCategories.Select(x => x.ID).ToList() : null;

            model.Products = ProductsService.Instance.SearchProducts(selectedCategoryIDs, model.SearchTerm, model.PriceFrom, model.PriceTo, model.SortBy, pageNo, pageSize.Value);
            if (LangCookie != null)
            {
                if (LangCookie.Value == "ar")
                {

                    foreach (var elem in model.Products.ToList())
                    {

                        elem.Name = elem.ArName;
                        elem.Description = elem.ArDescription;
                        elem.Summary = elem.ArSummary;
                    //    elem.Category.Name = elem.Category.ArName;


                    }
                }
            }
            var totalProducts = ProductsService.Instance.GetProductCount(selectedCategoryIDs, q, model.PriceFrom, model.PriceTo);

            model.Pager = new Pager(totalProducts, pageNo, pageSize.Value);

            if (model.isPartial)
            {
                return PartialView(model);
            }
            else
            {
                return View(model);
            }
        }
    }
}