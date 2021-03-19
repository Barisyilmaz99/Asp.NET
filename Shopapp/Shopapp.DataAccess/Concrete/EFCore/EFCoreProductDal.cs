using Microsoft.EntityFrameworkCore;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Shopapp.DataAccess.Concrete.EFCore
{
    public class EFCoreProductDal : EFCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public Product GetByIdWithCategries(int id)
        {
            using (var context =new ShopContext())
            {
                return context.Products
                    .Where(i => i.ID == id)
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Category)
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var Products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    Products = Products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }


                return Products.Count();
            }
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(i => i.ID == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
                
            }
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var Products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    Products = Products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a=>a.Category.Name.ToLower() ==category.ToLower()));
                }


                return Products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext()) {
                var product = context.Products
                        .Include(i => i.ProductCategories)
                        .FirstOrDefault(i => i.ID == entity.ID);
                if (product!=null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.ImageURL = entity.ImageURL;
                    product.ProductCategories = categoryIds.Select(i => new ProductCategory()
                    {
                        CategoryID = i,
                        ProductID=entity.ID

                    }).ToList();
                    context.SaveChanges();
                }
            }
        }
    }
}