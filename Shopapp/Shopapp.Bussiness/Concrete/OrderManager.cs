using Shopapp.Bussiness.Abstract;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System.Collections.Generic;

namespace Shopapp.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;

        public OrderManager()
        {
        }

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }
        public void Create(Order entity)
        {
            _orderDal.Create(entity);
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderDal.GetOrders(userId);
        }
    }
}