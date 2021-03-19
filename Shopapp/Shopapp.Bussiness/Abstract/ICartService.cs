using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Bussiness.Abstract
{
   public interface ICartService
    {
        public void InitializeCart(string UserID);

        Cart GetCartByUserId(string userId);
        void AddToCart(string userId, int productId, int quantity);
        void DeleteFromCart(string userId, int productId);
        void ClearCart(string cartID);
    }
}
