using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared;
using eCommerceMVC.Areas.Dashboard.ViewModels;
using eCommerceMVC.Code.Commons;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCommerceMVC.Areas.Dashboard.Controllers
{
    public class ProductsController : BaseController
    {
        private eCommerceUserManager _userManager;


        public eCommerceUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<eCommerceUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(int? categoryID, string searchTerm, int? pageNo)
        {
            var pageSize = ConfigurationsHelper.DashboardRecordsSizePerPage;

            ProductsListingViewModel model = new ProductsListingViewModel();
            
            model.PageTitle = "Products";
            model.PageDescription = "Products Listing Page";
            
            model.SearchTerm = searchTerm;

            model.Categories = CategoriesService.Instance.GetAllCategories();

            model.Suppliers = SuppliersService.Instance.GetAllSuppliers();

            List<int> selectedCategoryIDs = null;

            if (categoryID.HasValue && categoryID.Value > 0)
            {
                var selectedCategory = CategoriesService.Instance.GetCategoryByID(categoryID.Value);

                if (selectedCategory == null) return HttpNotFound();
                else
                {
                    model.CategoryID = selectedCategory.ID;

                    var searchedCategories = Methods.GetAllCategoryChildrens(selectedCategory, model.Categories);

                    selectedCategoryIDs = searchedCategories != null ? searchedCategories.Select(x => x.ID).ToList() : null;
                }
            }

            model.Products = ProductsService.Instance.SearchProducts(selectedCategoryIDs, searchTerm, null, null, null, pageNo, pageSize);
            var totalProducts = ProductsService.Instance.GetProductCount(selectedCategoryIDs, searchTerm, null, null);

            model.Pager = new Pager(totalProducts, pageNo, pageSize);

            return View(model);
        }
        
        [HttpGet]
        public ActionResult Action(int? ID)
        {
            ProductActionViewModel model = new ProductActionViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
            }

            if (ID.HasValue)
            {
                var product = ProductsService.Instance.GetProductByID(ID.Value);

                if (product == null) return HttpNotFound();

                model.PageTitle = "Edit Product";
                model.PageDescription = string.Format("Edit Product {0}.", product.Name);

                model.ID = product.ID;
                model.CategoryID = product.CategoryID;
                model.Name = product.Name;
                model.ArName = product.ArName;
                model.Summary = product.Summary;
                model.ArSummary = product.ArSummary;
                model.Description = product.Description;
                model.ArDescription = product.ArDescription;
                model.Price = product.Price;
                model.Cost = product.Cost;
                model.isFeatured = product.isFeatured;
                model.isOutOfStock = product.isOutOfStock;
                model.ProductPicturesList = product.ProductPictures;
                model.ThumbnailPicture = product.ThumbnailPictureID;
                model.ProductSpecifications = product.ProductSpecifications;
                model.SupplierID = product.SupplierID;
                model.ProductCosts = product.ProductCosts;
                model.UserId = product.UserId;
            }
            else
            {
                model.PageTitle = "Create Product";
                model.PageDescription = "Create New Product.";
            }

            model.Categories = CategoriesService.Instance.GetAllCategories();
            model.Suppliers = SuppliersService.Instance.GetAllSuppliers();

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Action(FormCollection formCollection)
        {
            ProductActionViewModel model = GetProductActionViewModelFromForm(formCollection);

                var user = UserManager.FindById(User.Identity.GetUserId());
            

            if (model.ID > 0)
            {
                var product = ProductsService.Instance.GetProductByID(model.ID);

                if (product == null) return HttpNotFound();

                product.ID = model.ID;
                product.Name = model.Name;
                product.ArName = model.ArName;
                product.CategoryID = model.CategoryID;
                product.ArSummary = model.ArSummary;
                product.Summary = model.Summary;
                product.Description = model.Description;
                product.ArDescription = model.ArDescription;
                product.Price = model.Price;
                product.isFeatured = model.isFeatured;
                product.isOutOfStock = model.isOutOfStock;
                product.ModifiedOn = DateTime.Now;
                product.Cost = model.Cost;
                product.SupplierID = model.SupplierID;
                product.UserId = user.Id;


                if (model.ProductSpecifications != null)
                {
                    product.ProductSpecifications.Clear();
                    product.ProductSpecifications.AddRange(model.ProductSpecifications.Select(x=> new ProductSpecification() { ProductID = product.ID, Title = x.Title, Value = x.Value }));
                }

                if (model.ProductCosts != null)
                {
                    product.ProductCosts.Clear();
                    product.ProductCosts.AddRange(model.ProductCosts.Select(x=> new ProductCost() { ProductID = product.ID, Title = x.Title, Value = x.Value }));
                }

                if (!string.IsNullOrEmpty(model.ProductPictures))
                {
                    var pictureIDs = model.ProductPictures
                                                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(ID => int.Parse(ID)).ToList();

                    if (pictureIDs.Count > 0)
                    {
                        product.ProductPictures.Clear();
                        product.ProductPictures.AddRange(pictureIDs.Select(x => new ProductPicture() { ProductID = product.ID, PictureID = x }).ToList());

                        product.ThumbnailPictureID = model.ThumbnailPicture != 0 ? model.ThumbnailPicture : pictureIDs.First();
                    }
                }

                ProductsService.Instance.UpdateProduct(product);
            }
            else
            {
                Product product = new Product();

                product.ID = model.ID;
                product.Name = model.Name;
                product.ArName = model.ArName;
                product.CategoryID = model.CategoryID;
                product.Summary = model.Summary;
                product.ArSummary = model.ArSummary;
                product.Description = model.Description;
                product.ArDescription = model.ArDescription;
                product.Price = model.Price;
                product.isFeatured = model.isFeatured;
                product.isOutOfStock = model.isOutOfStock;
                product.ModifiedOn = DateTime.Now;
                product.Cost = model.Cost;
                product.SupplierID = model.SupplierID;
                product.UserId = user.Id;

                if (model.ProductSpecifications != null)
                {
                    product.ProductSpecifications = new List<ProductSpecification>();
                    product.ProductSpecifications.AddRange(model.ProductSpecifications.Select(x => new ProductSpecification() { ProductID = product.ID, Title = x.Title, Value = x.Value }));
                }

                if (model.ProductCosts != null)
                {
                    product.ProductCosts = new List<ProductCost>();
                    product.ProductCosts.AddRange(model.ProductCosts.Select(x => new ProductCost() { ProductID = product.ID, Title = x.Title, Value = x.Value }));
                }

                if (!string.IsNullOrEmpty(model.ProductPictures))
                {
                    var pictureIDs = model.ProductPictures
                                                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(ID => int.Parse(ID)).ToList();

                    if (pictureIDs.Count > 0)
                    {
                        product.ProductPictures = new List<ProductPicture>();
                        product.ProductPictures.AddRange(pictureIDs.Select(x => new ProductPicture() { ProductID = product.ID, PictureID = x }).ToList());

                        product.ThumbnailPictureID = model.ThumbnailPicture != 0 ? model.ThumbnailPicture : pictureIDs.First();
                    }
                }

                ProductsService.Instance.SaveProduct(product);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            JsonResult result = new JsonResult();

            try
            {
                var operation = ProductsService.Instance.DeleteProduct(ID);

                result.Data = new { Success = operation, Message = operation ? string.Empty : "Can not delete product." };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = string.Format("{0}", ex.Message) };
            }

            return result;
        }

        private ProductActionViewModel GetProductActionViewModelFromForm(FormCollection formCollection)
        {
            var model = new ProductActionViewModel();

            model.ID = !string.IsNullOrEmpty(formCollection["ID"]) ? int.Parse(formCollection["ID"]) : 0;
            model.CategoryID = int.Parse(formCollection["CategoryID"]);
            model.Name = formCollection["Name"];
            model.ArName = formCollection["ArName"];
            model.Summary = formCollection["Summary"];
            model.ArSummary = formCollection["ArSummary"];
            model.Description = formCollection["Description"];
            model.ArDescription = formCollection["ArDescription"];
            model.Price = decimal.Parse(formCollection["Price"]);
            model.Cost = decimal.Parse(formCollection["Cost"]);
            model.isFeatured = formCollection["isFeatured"].Contains("true");
            model.isOutOfStock = formCollection["isOutOfStock"].Contains("true");
            model.ProductPictures = formCollection["ProductPictures"];
            model.ThumbnailPicture = !string.IsNullOrEmpty(formCollection["ThumbnailPicture"]) ? int.Parse(formCollection["ThumbnailPicture"]) : 0;
            model.SupplierID = int.Parse(formCollection["SupplierID"]);

            model.ProductSpecifications = new List<ProductSpecification>();
            model.ProductCosts = new List<ProductCost>();

            foreach (string key in formCollection)
            {
                if (key.Contains("specification"))
                {
                    var value = formCollection[key];

                    if(!string.IsNullOrEmpty(value))
                    {
                        var specificationTitle = value.GetSubstringText("", "~");
                        var specificationValue = value.GetSubstringText("~", "");

                        if (!string.IsNullOrEmpty(specificationTitle) && !string.IsNullOrEmpty(specificationValue))
                        {
                            model.ProductSpecifications.Add(new ProductSpecification() { Title = specificationTitle, Value = specificationValue });
                        }
                    }
                }
            }

            foreach (string key in formCollection)
            {
                if (key.Contains("cost"))
                {
                    var value = formCollection[key];

                    if(!string.IsNullOrEmpty(value))
                    {
                        var costTitle = value.GetSubstringText("", "~");
                        var costValue = value.GetSubstringText("~", "");

                        if (!string.IsNullOrEmpty(costTitle) && !string.IsNullOrEmpty(costValue))
                        {
                            model.ProductCosts.Add(new ProductCost() { Title = costTitle, Value = costValue });
                        }
                    }
                }
            }
            
            return model;
        }
    }
}