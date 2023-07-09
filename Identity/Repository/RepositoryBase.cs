using Identity.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Identity.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        ApplicationContext ApplicationContext{ get; set; }
        public RepositoryBase(ApplicationContext _applicationDbContext)
        {
            ApplicationContext = _applicationDbContext;
        }
        public IQueryable<T> FindAll() => ApplicationContext.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            ApplicationContext.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => ApplicationContext.Set<T>().Add(entity);
        public void Update(T entity) => ApplicationContext.Set<T>().Update(entity);
        public void Delete(T entity) => ApplicationContext.Set<T>().Remove(entity);
    }
}
