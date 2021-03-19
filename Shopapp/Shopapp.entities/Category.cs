using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Entities
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

    }
}
 