using Microsoft.EntityFrameworkCore;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopapp.DataAccess.Concrete.EFCore
{
    public class EFCoreCartDal : EFCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
        public Cart GetByUserID(string userId)
        {
            using (var context = new ShopContext())
            {
                return context
                    .Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserID == userId);
            }
        }

        public void DeleteFromCart(int cartID, int productId)
        {
             using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItem where CartID=@p0 And ProductID =@p1 ";
                context.Database.ExecuteSqlCommand(cmd, cartID, productId);
            }
        }

        public void ClearCart(string cartID)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItem where CartID=@p0 ";
                context.Database.ExecuteSqlCommand(cmd, cartID);
            }
        }
    }
}
