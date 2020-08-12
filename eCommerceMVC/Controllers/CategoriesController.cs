using eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerceMVC.Controllers
{
    public class CategoriesController : Controller
    {
        //[OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult FeaturedCategories()
        {
            var model = CategoriesService.Instance.GetFeaturedCategories();

            return PartialView("_FeaturedCategoriesMenuItem", model);
        }
    }
}