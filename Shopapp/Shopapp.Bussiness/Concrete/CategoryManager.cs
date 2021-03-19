using Shopapp.Bussiness.Abstract;
using Shopapp.DataAccess.Abstarct;
using Shopapp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopapp.Bussiness.Concrete
{
    public class CategoryManager : ICategoryService
        
    {
        private ICategoryDal _categoryDal;
        public CategoryManager( ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public void Create(Category entity)
        {
            _categoryDal.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryDal.Delete(entity);
        }

        public void DeleteFromCategory(int categoryID, int productID)
        {
            _categoryDal.DeleteFromCategory(categoryID,productID);
        }

        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryDal.GetByID(id);
        }

        public Category GetByIdWithProducts(int id)
        {
            return _categoryDal.GetByIdWithProducts(id);
        }

        public void Update(Category entity)
        {
            _categoryDal.Update(entity);
        }
    }
}
