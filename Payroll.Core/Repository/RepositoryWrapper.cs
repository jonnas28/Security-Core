using Helper.Common;
using Payroll.Core.Context;
using Payroll.Core.Repository.Masterfile.Business;
using Payroll.Core.Repository.Masterfile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        #region 
        IEmployeeRepository _employeeRepository;
        #endregion

        PayrollContext _context;
        public RepositoryWrapper(PayrollContext context)
        {

            _context = context;

        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new EmployeeRepository(_context);
                }
                return _employeeRepository;
            }
        }

        public PayrollContext GetContext()
        {
            return _context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
