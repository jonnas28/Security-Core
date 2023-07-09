using Payroll.Core.Context;
using Payroll.Core.Repository.Masterfile.Contracts;

namespace Payroll.Core.Repository
{
    public interface IRepositoryWrapper
    {
        IEmployeeRepository Employee { get; }
        PayrollContext GetContext();
        Task SaveAsync();
    }
}
