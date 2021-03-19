using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopapp.WebUI.Models
{
    public class CartModel
    {
        public int CartID { get; set; }
        public List<CartItemModel> CartItems { get; set; }

        public decimal TotalPrice()
        {
            return CartItems.Sum(i => i.Price * i.Quantity);
        }
    }



    public class CartItemModel
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public string  Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }


    }
}
