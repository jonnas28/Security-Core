using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Core.Model
{
    public class EmployeeJobDescription
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
