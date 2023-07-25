using AutoMapper;
using Identity.Common;
using Microsoft.AspNetCore.Identity;
using Payroll.Common;
using Payroll.Core.Model;

namespace WebAPI.Mapper
{
    public class IdentityMappingProfile:Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<IdentityRole, IdentityRoleDTO>()
                .ReverseMap()
                .ForAllMembers(o => o.Condition((src, dest, value) => value != null));
        }
    }
}
