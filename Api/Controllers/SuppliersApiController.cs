using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using eCommerce.Data;
using eCommerce.Entities;
using eCommerce.Services;

namespace Api.Controllers
{
    public class SuppliersApiController : ApiController
    {
        private eCommerceContext context = new eCommerceContext();

        [System.Web.Http.HttpGet]
        public List<Supplier> GetAll() => SuppliersService.Instance.GetAllSuppliers();
        [System.Web.Http.HttpGet]
        public List<Supplier> Search(int suppleierID , string searchTerm , int pageNo ,int pageSize) => SuppliersService.Instance.SearchSuppliers(suppleierID, searchTerm, pageNo, pageSize);
        [System.Web.Http.HttpGet]
        public Supplier GetSupplier(int id) => SuppliersService.Instance.GetSupplierByID(id);









        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}