using Microsoft.EntityFrameworkCore;
using Shopapp.DataAccess.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shopapp.DataAccess.Concrete.EFCore
{
    public class EFCoreGenericRepository<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext, new()
    {
        public virtual void Create(T entity)
        {
            var context = new TContext();
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public virtual void Delete(T entity)
        {
            var context = new TContext();
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            var context = new TContext();
            {
                return context.Set<T>().ToList();     
            }
        }

        public T GetByID(int ID)
        {
            var context = new TContext();
            {
                return context.Set<T>().Find(ID); 
            }
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            var context = new TContext();
            {
                return context.Set<T>().Where(filter).SingleOrDefault();
            }
        }

        public virtual void Update(T entity)
        {
            var context = new TContext();
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
