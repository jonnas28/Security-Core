using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Model
{
    public class EmployeeAddress
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public Employee Employee { get; set; }
    }
}
