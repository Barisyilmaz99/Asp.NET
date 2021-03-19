using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Entities

{
    public class Product
    {
        public int ID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
