using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopapp.WebUI.Models
{
    public class CategoryModel { 
        public int ID { get; set; }

        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
