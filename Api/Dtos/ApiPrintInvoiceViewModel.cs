using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Dtos
{
    public class ApiPrintInvoiceViewModel
    {
        public Order Order { get; set; }
        public int? OrderID { get; set; }
    }
}