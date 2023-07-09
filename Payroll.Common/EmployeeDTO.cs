using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Common
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public ICollection<EmployeeAddress> EmployeeAddress { get; set; }
    }
}
