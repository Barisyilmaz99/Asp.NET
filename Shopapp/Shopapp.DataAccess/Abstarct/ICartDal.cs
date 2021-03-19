using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.DataAccess.Abstarct
{
    public interface ICartDal : IRepository<Cart>
    {
        Cart GetByUserID(string userId);
        void DeleteFromCart(int cartID, int productId);
        void ClearCart(string cartID);
    }
}
