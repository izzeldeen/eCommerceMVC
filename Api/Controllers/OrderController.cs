using Api.Dtos;
using Api.ViewModels;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Services.ApiServices;
using eCommerceMVC.Code.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //[System.Web.Http.HttpPost]
        //public JsonResult PlaceOrder(Order order)
        //{
        //    JsonResult jsonResult = new JsonResult();
        //    if(!ModelState.IsValid)
        //    {
        //        jsonResult.Data = new { Success = false, Message = string.Format("The Model is not vaild") };

        //        return jsonResult;
        //    }

        //    var res = OrdersService.Instance.SaveOrder(order);
        //    if(res != true)
        //    {
        //        jsonResult.Data = new { Success = false, Message = string.Format("Failed to save Order") };

        //        return jsonResult;
        //    }
        //    jsonResult.Data = new { Success = true, Message = string.Format("Order Saved Sucessfully") };
        //    return jsonResult;
        //   }


        [System.Web.Http.HttpPost]
        public JsonResult PlaceOrder(PlaceOrderCrediCardViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {


                if (model.ProductIDs == null)
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "There is no items in model"
                    };

                    return jsonResult;
                }

                model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                if (model.ProductIDs != null && model.ProductIDs.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var newOrder = new Order();

                    newOrder.CustomerID = model.UserId;
                    newOrder.CustomerName = model.FullName;
                    newOrder.CustomerEmail = model.Email;
                    newOrder.CustomerPhone = model.PhoneNumber;
                    newOrder.CustomerCountry = model.Country;
                    newOrder.CustomerCity = model.City;
                    newOrder.CustomerAddress = model.Address;
                    newOrder.CustomerZipCode = model.ZipCode;

                    newOrder.OrderItems = new List<OrderItem>();
                    foreach (var product in model.Products)
                    {
                        var orderItem = new OrderItem();
                        orderItem.ProductID = product.ID;
                        orderItem.ProductName = product.Name;
                        orderItem.ItemPrice = product.Price;
                        orderItem.Quantity = model.ProductIDs.Where(x => x == product.ID).Count();

                        newOrder.OrderItems.Add(orderItem);
                    }

                    newOrder.OrderCode = Guid.NewGuid().ToString();
                    newOrder.TotalAmmount = newOrder.OrderItems.Sum(x => (x.ItemPrice * x.Quantity));
                    newOrder.DeliveryCharges = ConfigurationsHelper.FlatDeliveryCharges;

                    //Applying Promo/voucher 
                    if (model.PromoID > 0)
                    {
                        var promo = PromosService.Instance.GetPromoByID(model.PromoID);
                        if (promo != null && promo.Value > 0)
                        {
                            if (promo.ValidTill >= DateTime.Now)
                            {
                                newOrder.PromoID = promo.ID;

                                if (promo.PromoType == (int)PromoTypes.Percentage)
                                {
                                    newOrder.Discount = Math.Round((newOrder.TotalAmmount * promo.Value) / 100);
                                }
                                else if (promo.PromoType == (int)PromoTypes.Amount)
                                {
                                    newOrder.Discount = promo.Value;
                                }
                            }
                        }
                    }

                    newOrder.FinalAmmount = newOrder.TotalAmmount + newOrder.DeliveryCharges - newOrder.Discount;
                    newOrder.PaymentMethod = model.PaymentMethod;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;

                    if (OrdersService.Instance.SaveOrder(newOrder))
                    {
                        jsonResult.Data = new
                        {
                            Success = true,
                            Order = newOrder
                        };
                    }
                    else
                    {
                        jsonResult.Data = new
                        {
                            Success = false,
                            Message = "System can not take any order."
                        };
                    }
                }
                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = string.Join("\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                };
            }

            return jsonResult;
        }


        [System.Web.Http.HttpGet]
        public async Task<List<Order>> UserOrders(string userId) => await orderApiServices.UserOrders(userId);

        [System.Web.Http.HttpGet]
        public async Task<Order> GetOrderById(int Id) => await orderApiServices.GetOrderById(Id);

        [System.Web.Http.HttpGet]
        public async Task<ApiPrintInvoiceViewModel> PrintInvoice(int orderID)
        {
           
            ApiPrintInvoiceViewModel model = new ApiPrintInvoiceViewModel { Order = await orderApiServices.GetOrderById(orderID) };
           // model.Order = await orderApiServices.GetOrderById(orderID);
            if (model.Order == null)
                return null;
            model.OrderID = orderID;
             return  model;
        }


        public JsonResult PlaceOrderViaCashOnDelivery(PlaceOrderCashOnDeliveryViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
            

                if(model.ProductIDs == null)
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "There is no items in model"
                    };

                    return jsonResult;
                }
               
                  model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                if (model.ProductIDs != null && model.ProductIDs.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var newOrder = new Order();

                    newOrder.CustomerID = model.UserId;
                    newOrder.CustomerName = model.FullName;
                    newOrder.CustomerEmail = model.Email;
                    newOrder.CustomerPhone = model.PhoneNumber;
                    newOrder.CustomerCountry = model.Country;
                    newOrder.CustomerCity = model.City;
                    newOrder.CustomerAddress = model.Address;
                    newOrder.CustomerZipCode = model.ZipCode;

                    newOrder.OrderItems = new List<OrderItem>();
                    foreach (var product in model.Products)
                    {
                        var orderItem = new OrderItem();
                        orderItem.ProductID = product.ID;
                        orderItem.ProductName = product.Name;
                        orderItem.ItemPrice = product.Price;
                        orderItem.Quantity = model.ProductIDs.Where(x => x == product.ID).Count();

                        newOrder.OrderItems.Add(orderItem);
                    }

                    newOrder.OrderCode = Guid.NewGuid().ToString();
                    newOrder.TotalAmmount = newOrder.OrderItems.Sum(x => (x.ItemPrice * x.Quantity));
                    newOrder.DeliveryCharges = ConfigurationsHelper.FlatDeliveryCharges;

                    //Applying Promo/voucher 
                    if (model.PromoID > 0)
                    {
                        var promo = PromosService.Instance.GetPromoByID(model.PromoID);
                        if (promo != null && promo.Value > 0)
                        {
                            if (promo.ValidTill >= DateTime.Now)
                            {
                                newOrder.PromoID = promo.ID;

                                if (promo.PromoType == (int)PromoTypes.Percentage)
                                {
                                    newOrder.Discount = Math.Round((newOrder.TotalAmmount * promo.Value) / 100);
                                }
                                else if (promo.PromoType == (int)PromoTypes.Amount)
                                {
                                    newOrder.Discount = promo.Value;
                                }
                            }
                        }
                    }

                    newOrder.FinalAmmount = newOrder.TotalAmmount + newOrder.DeliveryCharges - newOrder.Discount;
                    if(model.PaymentMethod == 3)
                    {
                        newOrder.PaymentMethod = (int)PaymentMethods.CashOnDelivery;
                    }
                    else
                    {
                        newOrder.PaymentMethod = (int)model.PaymentMethod;
                    }
                   

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;

                    if (OrdersService.Instance.SaveOrder(newOrder))
                    {
                        jsonResult.Data = new
                        {
                            Success = true,
                            Order = newOrder
                        };
                    }
                    else
                    {
                        jsonResult.Data = new
                        {
                            Success = false,
                            Message = "System can not take any order."
                        };
                    }
                }
                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            }
            else
            {
                jsonResult.Data = new
                {
                    Success = false,
                    Message = string.Join("\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                };
            }

            return jsonResult;
        }

    }
}
