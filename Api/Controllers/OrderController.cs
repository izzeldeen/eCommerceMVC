using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class OrderController : ApiController
    {
       
        private eCommerceUserManager _userManager;

       

        public eCommerceUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<eCommerceUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public OrderController()
        {
        }

      


        public CartProductsViewModel CartProducts(string productIDs)
        {
            CartProductsViewModel model = new CartProductsViewModel();

            if (!string.IsNullOrEmpty(productIDs))
            {
                model.ProductIDs = productIDs.Split('-').Select(x => int.Parse(x)).Where(x => x > 0).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                }
            }

            return model;
        }

        public DeliveryInfoViewModel DeliveryInfo(string UserId)
        {
            DeliveryInfoViewModel model = new DeliveryInfoViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model.User = UserManager.FindById(UserId);
            }
            else
            {
                model.User = new eCommerceUser();
            }

            return model;
        }
        public JsonResult PlaceOrder(Order order)
        {
            JsonResult jsonResult = new JsonResult();
            if(!ModelState.IsValid)
            {
                jsonResult.Data = new { Success = false, Message = string.Format("The Model is not vaild") };

                return jsonResult;
            }

            var res = OrdersService.Instance.SaveOrder(order);
            if(res != true)
            {
                jsonResult.Data = new { Success = false, Message = string.Format("Failed to save Order") };

                return jsonResult;
            }

            jsonResult.Data = new { Success = true, Message = string.Format("Order Saved Sucessfully") };


            return jsonResult;



        }     
        public UserOrdersViewModel UserOrders(string userEmail, int? orderID, int? orderStatus, int? pageNo)
        {
            var pageSize = ConfigurationsHelper.DashboardRecordsSizePerPage;

            UserOrdersViewModel model = new UserOrdersViewModel();

            model.UserEmail = !string.IsNullOrEmpty(userEmail) ? userEmail : User.Identity.GetUserEmail();
            model.OrderID = orderID;
            model.OrderStatus = orderStatus;

            model.UserOrders = OrdersService.Instance.SearchOrders(model.UserEmail, model.OrderID, model.OrderStatus, pageNo, pageSize);
            var totalProducts = OrdersService.Instance.SearchOrdersCount(model.UserEmail, model.OrderID, model.OrderStatus);

            model.Pager = new Pager(totalProducts, pageNo, pageSize);
            //string JsonString;

            //var Json = System.Text.Json.JsonSerializer.Serialize<UserOrdersViewModel>(model);

            return model;
        }
        public Order GetOrderById (int id)
        {
            var res = OrdersService.Instance.GetOrderByID(id);

            
            return res;
        }
        public PrintInvoiceViewModel PrintInvoice(int orderID)
        {
            PrintInvoiceViewModel model = new PrintInvoiceViewModel();
            model.OrderID = orderID;

            model.Order = OrdersService.Instance.GetOrderByID(orderID);

            if (model.Order == null)
            {
                return  model;
            }

               return  model;
        }






    }
}
