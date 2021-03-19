using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Entities
{
    public class Cart
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public List<CartItem> CartItems { get; set; }

    }
}
