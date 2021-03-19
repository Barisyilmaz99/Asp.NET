using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopapp.WebUI.Models
{
    public class ProductModel
    {
     
        public int ID { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "*Uygunsuz İsim")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Fiyat belirtiniz")]
        [Range(100,10000)]
        public decimal? Price { get; set; }
        
        [StringLength(100, MinimumLength = 10, ErrorMessage = "*Uygunsuz Açıklama")]
        public string Description { get; set; }
        public List<Category>  SelectedCategories { get; set; }
    }
}
