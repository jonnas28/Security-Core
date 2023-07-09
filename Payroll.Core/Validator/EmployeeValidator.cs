using FluentValidation;
using Payroll.Common;

namespace Payroll.Core.Validator
{
    public class EmployeeValidator : AbstractValidator<EmployeeDTO>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .Length(0, 250);

            RuleFor(x => x.LastName)
                .NotNull()
                .Length(0, 250);
        }
    }
}
