using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using eCommerce.Entities;
using eCommerce.Services;
using eCommerce.Shared.Extensions;
using eCommerceMVC.Code.Helpers;
using eCommerceMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Xml.Linq;
using System.Xml;
using WebSupergoo.ABCpdf;
using WebSupergoo.ABCpdf3;
using System.Xml.Serialization;
using eCommerceMVC.Models;
using System.IO;
using com.sun.xml.@internal.xsom;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace eCommerceMVC.Controllers
{
    public class OrdersController : Controller
    {
        private eCommerceSignInManager _signInManager;
        private eCommerceUserManager _userManager;

        public eCommerceSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<eCommerceSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        public OrdersController()
        {
        }

        public OrdersController(eCommerceUserManager userManager, eCommerceSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
                
        public ActionResult CartProducts(string productIDs)
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

            return PartialView("_CartProducts", model);
        }

        public ActionResult Checkout(string promoCode)
        {
            CheckoutViewModel model = new CheckoutViewModel();

            model.PageTitle = "Checkout";
            model.PageDescription = "Checkout your products.";
            model.PageURL = Url.Checkout().ToSiteURL();
            model.PageImageURL = "https://www.websitedevelopers.pk/img/hero/home-website-developers-pk.jpg";

            model.PromoCode = promoCode;

            var cartItemsCookie = Request.Cookies["cartItems"];
            if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
            {
                model.CartHasProducts = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList().Count > 0;                
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Checkout", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult CartItems(string promoCode)
        {
            CartItemsViewModel model = new CartItemsViewModel();

            var cartItemsCookie = Request.Cookies["cartItems"];
            if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
            {
                model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                }

                if(!string.IsNullOrEmpty(promoCode))
                {
                    model.PromoCode = promoCode;
                    model.Promo = PromosService.Instance.GetPromoByCode(promoCode);
                }
            }
            
            return PartialView("_CartItems", model);
        }

        public ActionResult DeliveryInfo()
        {
            DeliveryInfoViewModel model = new DeliveryInfoViewModel();

            if(User.Identity.IsAuthenticated)
            {
                model.User = UserManager.FindById(User.Identity.GetUserId());
            }
            else
            {
                model.User = new eCommerceUser();
            }

            return PartialView("_DeliveryInfo", model);
        }

        public ActionResult ConfirmOrder()
        {
            ConfirmOrderViewModel model = new ConfirmOrderViewModel();

            var cartItemsCookie = Request.Cookies["cartItems"];
            if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
            {
                model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                }
            }

            return PartialView("_ConfirmOrder", model);
        }

        public JsonResult PlaceOrder(PlaceOrderCrediCardViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                var cartItemsCookie = Request.Cookies["cartItems"];
                if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
                {
                    model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                    }
                }

                if(model.ProductIDs != null && model.ProductIDs.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var newOrder = new Order();

                    newOrder.CustomerID = User.Identity.GetUserId();
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
                    if(model.PromoID > 0)
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
                    newOrder.PaymentMethod = (int)PaymentMethods.CreditCard;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;

                    #region ExecuteAuthorizeNetPayment Execution

                    var creditCardInfo = new creditCardType
                    {
                        cardNumber = model.CCCardNumber,
                        expirationDate = string.Format("{0}{1}", model.CCExpiryMonth, model.CCExpiryYear),
                        cardCode = model.CCCVC
                    };

                    var authorizeNetResponse = AuthorizeNetHelper.ExecutePayment(newOrder, creditCardInfo);

                    #endregion

                    if (authorizeNetResponse.Success)
                    {
                        newOrder.TransactionID = authorizeNetResponse.Response.transactionResponse.transId;

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
                            Success = authorizeNetResponse.Success,
                            Message = authorizeNetResponse.Message
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
        public JsonResult PlaceOrderViaCashOnDelivery(PlaceOrderCashOnDeliveryViewModel model)
        {
            JsonResult jsonResult = new JsonResult();

            var errorDetails = string.Empty;

            if (model != null && ModelState.IsValid)
            {
                var cartItemsCookie = Request.Cookies["cartItems"];
                if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
                {
                    model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                    }
                }

                if (model.ProductIDs != null && model.ProductIDs.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var newOrder = new Order();

                    newOrder.CustomerID = User.Identity.GetUserId();
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
                    newOrder.PaymentMethod = (int)PaymentMethods.CashOnDelivery;

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

        public ActionResult Tracking(int? orderID, bool orderPlaced = false)
        {
            TrackOrderViewModel model = new TrackOrderViewModel();
            model.OrderID = orderID;

            if (orderID.HasValue)
            {
                model.Order = OrdersService.Instance.GetOrderByID(orderID.Value);
            }

            if (model.Order != null)
            {
                model.PageTitle = string.Format("Track Order : {0}", model.Order.ID);
                model.PageDescription = "You can check the status of your order. Please enter the order ID and you will get information related to your order.";
                model.PageURL = Url.OrderTrack(orderID.ToString()).ToSiteURL();
            }
            else
            {
                model.PageTitle = string.Format("Track Order");
                model.PageDescription = "You can check the status of your order. Please enter the order ID and you will get information related to your order.";
                model.PageURL = Url.OrderTrack().ToSiteURL();
            }

            model.PageImageURL = PictureHelper.PageImageURL("orders.jpg");

            ViewBag.ShowOrderPlaceMessage = orderPlaced;

            return View(model);
        }

        public ActionResult UserOrders(string userEmail, int? orderID, int? orderStatus, int? pageNo)
        {
            var pageSize = ConfigurationsHelper.DashboardRecordsSizePerPage;

            UserOrdersViewModel model = new UserOrdersViewModel();

            model.UserEmail = !string.IsNullOrEmpty(userEmail) ? userEmail : User.Identity.GetUserEmail();
            model.OrderID = orderID;
            model.OrderStatus = orderStatus;

            model.UserOrders = OrdersService.Instance.SearchOrders(model.UserEmail, model.OrderID, model.OrderStatus, pageNo, pageSize);
            var totalProducts = OrdersService.Instance.SearchOrdersCount(model.UserEmail, model.OrderID, model.OrderStatus);

            model.Pager = new Pager(totalProducts, pageNo, pageSize);

            return PartialView("_UserOrders", model);
        }

        public ActionResult PrintInvoice(int orderID)
        {
            PrintInvoiceViewModel model = new PrintInvoiceViewModel();
            model.OrderID = orderID;

            model.Order = OrdersService.Instance.GetOrderByID(orderID);

            if (model.Order == null)
            {
                return HttpNotFound();
            }

            model.PageTitle = string.Format("Print Invoice Order : {0}", model.Order.ID);
            model.PageDescription = "Print invoice for your order.";
            model.PageURL = Url.PrintInvoice(orderID).ToSiteURL();
            model.PageImageURL = PictureHelper.PageImageURL("orders.jpg");

            return PartialView("_PrintInvoice", model);
        }

        public ActionResult CreditCardCheckout()
        {
            JsonResult jsonResult = new JsonResult();
            var newOrder = new CreditCardInfoViewModel();
            var model = new PlaceOrderCashOnDeliveryViewModel();

            var errorDetails = string.Empty;


                var cartItemsCookie = Request.Cookies["cartItems"];
                if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
                {
                    model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                    if (model.ProductIDs.Count > 0)
                    {
                        model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                    }
                }

                if (model.ProductIDs != null && model.ProductIDs.Count > 0 && model.Products != null && model.Products.Count > 0)
                {
                    var deliveryInfo = Request.Cookies["DeliveryInfo"];
                    if(deliveryInfo == null)
                    {
                        return View(newOrder);

                    }

                    var user = SharedService.Instance.GetUserById(deliveryInfo["CustomerID"]);
                    var name = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    var payment = PaymentsService.Instance.GetUserPayment(user.Id);


                //var client = new RestClient("https://api.tap.company/v2/customers");
                //client.Timeout = -1;
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("Authorization", "Bearer sk_test_09I6oRDbXfC7hdKBmPiGL5Ex");
                //request.AddParameter("text/plain", "", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //Console.WriteLine(response.Content);

                //string xmlString = response.Content;

                //Customer account = JsonConvert.DeserializeObject<Customer>(response.Content);


                if (payment == null)
                    {
                        var client = new RestClient("https://api.tap.company/v2/customers");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Bearer sk_live_b1IR6r3CNYhKxWaPtLi8mH5u");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/json", "{\r\n  \"first_name\": \""+user.FirstName+ "\",\r\n  \"middle_name\": \"" + user.MiddleName + "\",\r\n  \"last_name\": \"" + user.LastName + "\",\r\n  \"email\": \"" + user.Email + "\",\r\n  \"phone\": {\r\n    \"country_code\": \"" + user.CountryCode + "\",\r\n    \"number\": \"" + user.PhoneNumber + "\"\r\n  },\r\n  \"description\": \"test\",\r\n  \"metadata\": {\r\n    \"udf1\": \"test\"\r\n  },\r\n  \"currency\": \"USD\"\r\n}", ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        Customer account = JsonConvert.DeserializeObject<Customer>(response.Content);

                        Payment newPayment = new Payment() { UserId = user.Id, TapUserId = account.id};

                        var paymentToCreate = PaymentsService.Instance.AddUserPayment(newPayment);


                        newOrder.TapId = account.id;
                    }
                    else
                    {
                        newOrder.TapId = payment.TapUserId;
                    }

                    newOrder.CustomerID = user.Id;
                    newOrder.CustomerName = name;
                    newOrder.Firstname = user.FirstName;
                    newOrder.Middlename = user.MiddleName;
                    newOrder.Lastname = user.LastName;
                    newOrder.CustomerEmail = deliveryInfo["Email"];
                    newOrder.CustomerPhone = user.PhoneNumber;
                    newOrder.CountryCode = user.CountryCode;
                    newOrder.CustomerCountry = deliveryInfo["Country"];
                    newOrder.CustomerCity = deliveryInfo["City"];
                    newOrder.CustomerAddress = deliveryInfo["Address"];
                    newOrder.CustomerZipCode = deliveryInfo["ZipCode"];

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
                    if(deliveryInfo["PromoID"] != null)
                    { 
                        if (Int32.Parse(deliveryInfo["PromoID"]) > 0)
                        {
                            var promo = PromosService.Instance.GetPromoByID(Int32.Parse(deliveryInfo["PromoID"]));
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
                    }

                newOrder.FinalAmmount = newOrder.TotalAmmount + newOrder.DeliveryCharges - newOrder.Discount;
                    newOrder.PaymentMethod = (int)PaymentMethods.CashOnDelivery;

                    newOrder.OrderHistory = new List<OrderHistory>() {
                        new OrderHistory() {
                            OrderStatus = (int)OrderStatus.Placed,
                            ModifiedOn = DateTime.Now,
                            Note = "Order Placed."
                        }
                    };

                    newOrder.PlacedOn = DateTime.Now;
                }

                else
                {
                    jsonResult.Data = new
                    {
                        Success = false,
                        Message = "Invalid products in cart."
                    };
                }
            

            newOrder.NewUrl = Url.Action("CreditCardCheckout", "Orders");
            return View(newOrder);
        }


        public ActionResult CreateDeliveyCookie(PlaceOrderCashOnDeliveryViewModel model)
        {
            var newOrder = new CreditCardInfoViewModel();

            var cartItemsCookie = Request.Cookies["cartItems"];
            if (cartItemsCookie != null && !string.IsNullOrEmpty(cartItemsCookie.Value))
            {
                model.ProductIDs = cartItemsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                if (model.ProductIDs.Count > 0)
                {
                    model.Products = ProductsService.Instance.GetProductsByIDs(model.ProductIDs.Distinct().ToList());
                }
            }



                newOrder.CustomerID = User.Identity.GetUserId();
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

                newOrder.TotalAmmount = newOrder.OrderItems.Sum(x => (x.ItemPrice * x.Quantity));

            System.Web.HttpCookie DeliveryInfo = new System.Web.HttpCookie("DeliveryInfo");
            DeliveryInfo["CustomerID"] = User.Identity.GetUserId();
            DeliveryInfo["CustomerName"] = model.FullName;
            DeliveryInfo["CustomerEmail"] = model.Email;
            DeliveryInfo["CustomerPhone"] = model.PhoneNumber;
            DeliveryInfo["CustomerCountry"] = model.Country;
            DeliveryInfo["CustomerCity"] = model.City;
            DeliveryInfo["CustomerAddress"] = model.Address;
            DeliveryInfo["CustomerZipCode"] = model.ZipCode;

            if (model.PromoID > 0)
            {
                var promo = PromosService.Instance.GetPromoByID(model.PromoID);
                if (promo != null && promo.Value > 0)
                {
                    if (promo.ValidTill >= DateTime.Now)
                    {
                        DeliveryInfo["PromoID"] = promo.ID.ToString();

                        if (promo.PromoType == (int)PromoTypes.Percentage)
                        {
                            DeliveryInfo["Discount"] = Math.Round((newOrder.TotalAmmount * promo.Value) / 100).ToString();
                        }
                        else if (promo.PromoType == (int)PromoTypes.Amount)
                        {
                            DeliveryInfo["Discount"] = promo.Value.ToString();
                        }
                    }
                }
            }

            Response.Cookies.Add(DeliveryInfo);
            return Json(new { message = "Success"});
        }

        public ActionResult Test()
        {
            var client = new RestClient("https://api.tap.company/v2/customers");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer sk_live_b1IR6r3CNYhKxWaPtLi8mH5u");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n  \"first_name\": \"mostafa\",\r\n  \"middle_name\": \"basim\",\r\n  \"last_name\": \"kiswani\",\r\n  \"email\": \"mkiswaney1996@gmail.com\",\r\n  \"phone\": {\r\n    \"country_code\": \"962\",\r\n    \"number\": \"798997867\"\r\n  },\r\n  \"description\": \"test\",\r\n  \"metadata\": {\r\n    \"udf1\": \"test\"\r\n  },\r\n  \"currency\": \"USD\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);


            Customer account = JsonConvert.DeserializeObject<Customer>(response.Content);




            return View(account);
        }
    }
}