using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace eCommerce.Services.ApiServices
{
    public class OrderApiService
    {
        public async Task<List<Order>> UserOrders(string userId)
        {
            if (userId == null || userId == " ") return null;
            eCommerceContext context = new eCommerceContext();

            return await context.Orders.Where(x => x.CustomerID == userId).Include(x => x.Promo).Include(x => x.OrderItems.Select(a => a.Product.ProductPictures.Select(f=> f.Picture)))
                 .Include(x => x.OrderHistory).ToListAsync();


        }
        public async Task<Order> GetOrderById(int Id)
        {
            eCommerceContext context = new eCommerceContext();
            return await context.Orders.Include(x => x.Promo).Include(x => x.OrderItems.Select(a => a.Product))
                .Include(x => x.OrderHistory).Include(x=>x.OrderItems.Select(f =>f.Product.ProductPictures.Select(a=>a.Picture)))
                .FirstOrDefaultAsync(x => x.ID == Id);


        }

        
    }
}
