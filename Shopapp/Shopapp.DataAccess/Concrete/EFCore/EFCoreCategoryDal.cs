using Microsoft.EntityFrameworkCore;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shopapp.DataAccess.Concrete.EFCore
{
    public class EFCoreCategoryDal : EFCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryID, int productID)
        {
            using(var context = new ShopContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1";
                context.Database.ExecuteSqlCommand(cmd, productID, categoryID);
            }
        }

        public Category GetByIdWithProducts(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Categories
                .Where(i => i.ID == id)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();
            }
        }
    }
}
