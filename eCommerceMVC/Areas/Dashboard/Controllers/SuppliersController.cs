using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared;
using eCommerceMVC.Areas.Dashboard.ViewModels;
using eCommerceMVC.Code.Commons;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCommerceMVC.Areas.Dashboard.Controllers
{
    public class SuppliersController : BaseController
    {
        public ActionResult Index(int? supplierID, string searchTerm, int? pageNo)
        {
            var pageSize = ConfigurationsHelper.DashboardRecordsSizePerPage;

            SuppliersListingViewModel model = new SuppliersListingViewModel();

            model.PageTitle = "Suppliers";
            model.PageDescription = "Suppliers Listing Page";

            model.SearchTerm = searchTerm;

            model.Suppliers = SuppliersService.Instance.GetAllSuppliers();


            int? selectedSuppliersIDs = null;

            if (supplierID.HasValue && supplierID.Value > 0)
            {
                var selectedSupplier = SuppliersService.Instance.GetSupplierByID(supplierID.Value);


            }

            model.Suppliers = SuppliersService.Instance.SearchSuppliers(selectedSuppliersIDs, searchTerm, pageNo, pageSize);
            var totalProducts = SuppliersService.Instance.GetSupplierCount(selectedSuppliersIDs, searchTerm);

            model.Pager = new Pager(totalProducts, pageNo, pageSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            SupplierActionViewModel model = new SupplierActionViewModel();

            if (ID.HasValue)
            {
                var supplier = SuppliersService.Instance.GetSupplierByID(ID.Value);

                if (supplier == null) return HttpNotFound();

                model.PageTitle = "Edit Supplier";
                model.PageDescription = string.Format("Edit Supplier {0}.", supplier.SupplierName);

                model.ID = supplier.ID;
                model.SupplierName = supplier.SupplierName;
                model.Address = supplier.Address;
                model.Phonenumber = supplier.Phonenumbher;

            }
            else
            {
                model.PageTitle = "Create Supplier";
                model.PageDescription = "Create New Supplier.";
            }



            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Action(FormCollection formCollection)
        {
            SupplierActionViewModel model = GetSupplierActionViewModelFromForm(formCollection);

            if (model.ID > 0)
            {
                var supplier = SuppliersService.Instance.GetSupplierByID(model.ID);

                if (supplier == null) return HttpNotFound();

                supplier.ID = model.ID;
                supplier.SupplierName = model.SupplierName;
                supplier.Address = model.Address;
                supplier.Phonenumbher = model.Phonenumber;



                SuppliersService.Instance.UpdateSupplier(supplier);
            }
            else
            {
                Supplier supplier = new Supplier();

                supplier.ID = model.ID;
                supplier.SupplierName = model.SupplierName;
                supplier.Phonenumbher = model.Phonenumber;
                supplier.Address = model.Address;



                SuppliersService.Instance.SaveSupplier(supplier);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var operation = SuppliersService.Instance.DeleteSupplier(ID);

                result.Data = new { Success = operation, Message = operation ? string.Empty : "Can not delete supplier." };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = string.Format("{0}", ex.Message) };
            }

            return result;
        }

        private SupplierActionViewModel GetSupplierActionViewModelFromForm(FormCollection formCollection)
        {
            var model = new SupplierActionViewModel();

            model.ID = !string.IsNullOrEmpty(formCollection["ID"]) ? int.Parse(formCollection["ID"]) : 0;
            model.SupplierName = formCollection["SupplierName"];
            model.Address = formCollection["Address"];
            model.Phonenumber = formCollection["Phonenumber"];
           

            return model;
        }
    }
}