using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Common
{
    public class EmployeeJobDescriptionDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
    }
}
