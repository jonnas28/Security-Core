using AutoMapper;
using Payroll.Common;
using Payroll.Core.Model;

namespace WebAPI.Mapper
{
    public class MasterfileMappingProfile : Profile
    {
        public MasterfileMappingProfile()
        {
            CreateMap<string, byte[]>().ConvertUsing(new StringToByte());
            CreateMap<byte[], string>().ConvertUsing(new ByteToString());

            CreateMap<Employee, EmployeeDTO>()
                .ReverseMap()
                .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

            CreateMap<EmployeeJobDescription, EmployeeJobDescriptionDTO>()
                .ReverseMap()
                .ForAllMembers(o => o.Condition((src, dest, value) => value != null));
        }

        public class StringToByte : ITypeConverter<string, byte[]>
        {
            byte[] ITypeConverter<string, byte[]>.Convert(string source, byte[] destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source)) return null;
                return System.Convert.FromBase64String(source);
            }
        }
        public class ByteToString : ITypeConverter<byte[], string>
        {
            public string Convert(byte[] source, string destination, ResolutionContext context)
            {
                if (source == null) return null;
                return System.Convert.ToBase64String(source);
            }
        }
    }
}
