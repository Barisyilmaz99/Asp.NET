using Microsoft.EntityFrameworkCore;
using Shopapp.DataAccess.Abstarct;
using Shopapp.DataAccess.Concrete.EFCore;
using Shopapp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Shopapp.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderDal : EFCoreGenericRepository<Order, ShopContext>, IOrderDal
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new ShopContext())
            {
                var orders = context.Orders
                                .Include(i => i.OrderItems)
                                .ThenInclude(i => i.Product)
                                .AsQueryable();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                return orders.ToList();
            }
        }
    }
}