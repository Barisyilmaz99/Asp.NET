using Shopapp.Entities;

namespace Shopapp.DataAccess.Abstarct
{
    public interface ICategoryDal : IRepository<Category>
    {
        Category GetByIdWithProducts(int id);
        void DeleteFromCategory(int categoryID, int productID);
    }
}
