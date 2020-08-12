using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eCommerceMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { area = "", controller = "Users", action = "Register" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { area = "", controller = "Users", action = "Login" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "SocialLogin",
                url: "social-login",
                defaults: new { area = "", controller = "Users", action = "SocialLogin" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "SocialLoginCallback",
                url: "social-login-callback",
                defaults: new { area = "", controller = "Users", action = "SocialLoginCallback" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "forgot-password",
                defaults: new { area = "", controller = "Users", action = "ForgotPassword" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "reset-password",
                defaults: new { area = "", controller = "Users", action = "ResetPassword" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "WebLogin",
                url: "account",
                defaults: new { area = "", controller = "Users", action = "WebLogin" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Logoff",
                url: "logoff",
                defaults: new { area = "", controller = "Users", action = "LogOff" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "AboutUs",
                url: "about-us",
                defaults: new { area = "", controller = "Contents", action = "AboutUs" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ContactUs",
                url: "contact-us",
                defaults: new { area = "", controller = "Contents", action = "ContactUs" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Blog",
                url: "blog",
                defaults: new { area = "", controller = "Contents", action = "Blog" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "PrivacyPolicy",
                url: "privacy-policy",
                defaults: new { area = "", controller = "Contents", action = "PrivacyPolicy" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "RefundPolicy",
                url: "refund-policy",
                defaults: new { area = "", controller = "Contents", action = "RefundPolicy" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "CancellationPolicy",
                url: "cancellation-policy",
                defaults: new { area = "", controller = "Contents", action = "CancellationPolicy" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "TermsConditions",
                url: "terms-conditions",
                defaults: new { area = "", controller = "Contents", action = "TermsConditions" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "SearchProducts",
                url: "search/{category}",
                defaults: new { area = "", controller = "Home", action = "Search", category = UrlParameter.Optional },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ProductDetails",
                url: "{category}/product/{ID}",
                defaults: new { area = "", controller = "Products", action = "Details" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UserProfile",
                url: "user/profile",
                defaults: new { area = "", controller = "Users", action = "UserProfile" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UpdateProfile",
                url: "user/update-profile",
                defaults: new { area = "", controller = "Users", action = "UpdateProfile" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ChangePassword",
                url: "user/change-password",
                defaults: new { area = "", controller = "Users", action = "ChangePassword" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UpdatePassword",
                url: "user/update-password",
                defaults: new { area = "", controller = "Users", action = "UpdatePassword" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ChangeAvatar",
                url: "user/change-avatar",
                defaults: new { area = "", controller = "Users", action = "ChangeAvatar" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UpdateAvatar",
                url: "user/update-avatar",
                defaults: new { area = "", controller = "Users", action = "UpdateAvatar" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UserOrders",
                url: "user/orders",
                defaults: new { controller = "Orders", action = "UserOrders" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );


            routes.MapRoute(
                name: "CartProducts",
                url: "cart-products",
                defaults: new { area = "", controller = "Orders", action = "CartProducts" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "CartItems",
                url: "cart-items",
                defaults: new { area = "", controller = "Orders", action = "CartItems" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "DeliveryInfo",
                url: "delivery-info",
                defaults: new { area = "", controller = "Orders", action = "DeliveryInfo" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "ConfirmOrder",
                url: "confirm-order",
                defaults: new { area = "", controller = "Orders", action = "ConfirmOrder" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "checkout",
                defaults: new { area = "", controller = "Orders", action = "Checkout" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "PlaceOrder",
                url: "place-order",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrder" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "PlaceOrderViaCashOnDelivery",
                url: "place-order-cod",
                defaults: new { area = "", controller = "Orders", action = "PlaceOrderViaCashOnDelivery" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "OrderTrack",
                url: "tracking",
                defaults: new { area = "", controller = "Orders", action = "Tracking" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "PrintInvoice",
                url: "print-ivoice",
                defaults: new { area = "", controller = "Orders", action = "PrintInvoice" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "FeaturedProducts",
                url: "featured-products",
                defaults: new { area = "", controller = "Products", action = "FeaturedProducts" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "RecentProducts",
                url: "recent-products",
                defaults: new { area = "", controller = "Products", action = "RecentProducts" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "RelatedProducts",
                url: "related-products",
                defaults: new { area = "", controller = "Products", action = "RelatedProducts" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "FeaturedCategories",
                url: "featured-categories",
                defaults: new { area = "", controller = "Categories", action = "FeaturedCategories" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "UploadPictures",
                url: "pictures/upload",
                defaults: new { area = "", controller = "Shared", action = "UploadPictures" },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "eCommerceMVC.Controllers" }
            );
        }
    }
}
