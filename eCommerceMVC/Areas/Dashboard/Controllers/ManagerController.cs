using eCommerce.Entities;
using eCommerce.Services;
using eCommerceMVC.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerceMVC.Areas.Dashboard.Controllers
{
    public class ManagerController : BaseController
    {
        public ActionResult Index()
        {
            ManagerViewModel model = new ManagerViewModel();
            string DataEntryOneId = "e7c33894-a133-4ba4-9df8-4b00ddb465f9"; //abyan
            string DataEntryTwoId = "e88eb4a7-52dd-4553-ac5e-f697cfd41e94"; //noor

            model.EntryOneProductsCount = ProductsService.Instance.GetDataEntryProductsCount(DataEntryOneId);
            model.EntryTwoProductsCount = ProductsService.Instance.GetDataEntryProductsCount(DataEntryTwoId);
            model.EntryOneId = DataEntryOneId;
            model.EntryTwoId = DataEntryTwoId;

            return View(model);
        }

        public ActionResult EntryDetails(string userID)
        {
            ProductsListingViewModel model = new ProductsListingViewModel();

            model.Products = ProductsService.Instance.GetDataEntryProducts(userID);

            return View(model);

        }
    }
}