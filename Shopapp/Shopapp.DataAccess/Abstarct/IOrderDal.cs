using Shopapp.Entities;
using System.Collections.Generic;

namespace Shopapp.DataAccess.Abstarct
{
    public interface IOrderDal:IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
