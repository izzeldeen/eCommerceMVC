using Api.Dtos;
using com.sun.tools.javac.util;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Services.ApiServices;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class OrderController : ApiController
    {
       
       

        private OrderApiService orderApiServices = new OrderApiService();

       

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
        public async Task<List<Order>> UserOrders(string userId) => await orderApiServices.UserOrders(userId);


        public async Task<Order> GetOrderById(int Id) => await orderApiServices.GetOrderById(Id);
       
        [System.Web.Http.HttpGet]
        public async  Task<ApiPrintInvoiceViewModel> PrintInvoice(int orderID)
        {
           
            ApiPrintInvoiceViewModel model = new ApiPrintInvoiceViewModel();
            
            model.Order = await orderApiServices.GetOrderById(orderID);
            if (model.Order == null)
                return null;
            model.OrderID = orderID;
             return  model;
        }


    }
}
