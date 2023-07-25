using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Common
{
    public class IdentityRoleDTO
    {
        public string? Name { get; set; }
        public string? NormalizedName{ get; set; }
        public string? ConcurrencyStamp { get; set; }
    }
}
