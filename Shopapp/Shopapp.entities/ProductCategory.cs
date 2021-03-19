using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Entities
{
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public Category Category { get; set; }
        public Product Product { get; set; }


    }
}
