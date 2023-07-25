using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Core.Model
{
    public class EmployeeAddress
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
