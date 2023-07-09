using Helper.Common;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Payroll.Common.Paramaters;
using Payroll.Core.Context;
using Payroll.Core.Model;
using Payroll.Core.Repository.Masterfile.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Repository.Masterfile.Business
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<PagedList<Employee>> GetAllAsync(EmployeeParamater parameter)
        {
            IQueryable<Employee> query = FindAll();
            Search(ref query, parameter.Search);

            return await PagedList<Employee>.ToPagedListAsync(query, parameter.PageNumber, parameter.PageSize);
        }

        public async Task<Employee> GetByIdAsync(int Id)
        {
            return await FindByCondition(x => x.Id.Equals(Id))
            .FirstOrDefaultAsync();
        }

        private void Search(ref IQueryable<Employee> query, string? search)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(search))
                return;
            var predicate = PredicateBuilder.New<Employee>(true);
            predicate.Or(x => x.FirstName.ToLower().Contains(search.Trim().ToLower()));
            query = query.Where(predicate);
        }
    }
}
