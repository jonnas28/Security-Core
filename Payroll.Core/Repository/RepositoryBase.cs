using Microsoft.EntityFrameworkCore;
using Payroll.Core.Context;
using System.Linq.Expressions;

namespace Payroll.Core.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected PayrollContext _context;
        public RepositoryBase(PayrollContext context)
        {
            _context = context;
        }
        public void Create(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public IQueryable<T> FindAll()=> _context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)=>
            _context.Set<T>().Where(expression).AsNoTracking();

        public int GetNextId()
        {
            int nextId = 0;
            string tableName = _context.Model.FindEntityType(typeof(T)).GetTableName();
            string query = $"SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{_context.Database.GetDbConnection().Database}' AND TABLE_NAME = '{tableName}';";
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                _context.Database.OpenConnection();
                nextId = Convert.ToInt32(command.ExecuteScalar() ?? 0);
                _context.Database.CloseConnection();
            }
            return nextId;
        }
    }
}
