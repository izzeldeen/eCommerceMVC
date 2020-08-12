using eCommerce.Entities;
using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerceMVC.Areas.Dashboard.ViewModels
{
    public class SuppliersListingViewModel : PageViewModel
    {
        public List<Supplier> Suppliers { get; set; }

        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }

    public class SupplierActionViewModel : PageViewModel
    {
        public int ID { get; set; }

        public string SupplierName { get; set; }

        public string Address { get; set; }
        public string Phonenumber { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}