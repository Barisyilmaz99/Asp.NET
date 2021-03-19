using Shopapp.Bussiness.Abstract;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Bussiness.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i => i.ProductID == productId);
                if (index<0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductID = productId,
                        Quantity = quantity,
                        CartID = cart.ID
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartDal.Update(cart);
            }
        }

        public void ClearCart(string cartID)
        {
            _cartDal.ClearCart(cartID);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {

                _cartDal.DeleteFromCart(cart.ID, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartDal.GetByUserID(userId);
        }

        public void InitializeCart(string userID)
        {
            _cartDal.Create(new Cart() { UserID = userID });
        }
    }
}
