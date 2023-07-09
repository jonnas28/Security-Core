using Helper.Common;
using Payroll.Common.Paramaters;
using Payroll.Core.Model;

namespace Payroll.Core.Repository.Masterfile.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetAllAsync(EmployeeParamater parameter);
        Task<Employee> GetByIdAsync(int Id);
    }
}
