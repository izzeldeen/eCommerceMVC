using Api.Dtos;
using com.sun.tools.javac.util;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using Magnum.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
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

        [System.Web.Http.HttpGet]
        public DeliveryInfoViewModel DeliveryInfo(string UserId)
        {
            DeliveryInfoViewModel model = new DeliveryInfoViewModel();
            if (UserId == null) return model;
            eCommerceContext context = new eCommerceContext();
            var UserManager = new UserManager<eCommerceUser>(new UserStore<eCommerceUser>(context));
            model.User = UserManager.FindById(UserId);
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
        [System.Web.Http.HttpGet]
        public List<Order> UserOrders(string UserId)
        {
            eCommerceContext context = new eCommerceContext();

            var res = context.Orders.Where(x => x.CustomerID == UserId).Include(x => x.Promo).Include(x => x.OrderItems.Select(a => a.Product))
                 .Include(x => x.OrderHistory).ToList();


            return res;

        
        }
        public Order GetOrderById (int id)
        {
            eCommerceContext context = new eCommerceContext();
            var res = context.Orders.Include(x => x.Promo).Include(x => x.OrderItems.Select(a=>a.Product))
                .Include(x => x.OrderHistory)
                .FirstOrDefault(x => x.ID == id);
            return res;
        }
        [System.Web.Http.HttpGet]
        public ApiPrintInvoiceViewModel PrintInvoice(int orderID)
        {
            eCommerceContext context = new eCommerceContext();
            ApiPrintInvoiceViewModel model = new ApiPrintInvoiceViewModel();
            model.OrderID = orderID;
            model.Order = context.Orders.Include(x => x.Promo).Include(x => x.OrderItems.Select(a => a.Product))
                .Include(x => x.OrderHistory)
                .FirstOrDefault(x => x.ID == orderID);

            if (model.Order == null)
            {
                return  model;
            }

               return  model;
        }


    }
}
