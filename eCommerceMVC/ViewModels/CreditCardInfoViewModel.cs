using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerceMVC.ViewModels
{
    public class CreditCardInfoViewModel : PageViewModel
    {
        public string CustomerID { get; set; }
        public string TapId { get; set; }
        public string CustomerName { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string CountryCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerZipCode { get; set; }
        public string NewUrl { get; set; }

        public string OrderCode { get; set; }
        public int PaymentMethod { get; set; }
        public decimal TotalAmmount { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal FinalAmmount { get; set; }
        public DateTime PlacedOn { get; set; }

        public string TransactionID { get; set; }

        public int? PromoID { get; set; }
        public virtual Promo Promo { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual List<OrderHistory> OrderHistory { get; set; }

    }
}