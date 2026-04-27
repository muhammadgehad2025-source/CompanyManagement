using AutoMapper;
using Company.Core.DTOs;
using Company.Core.Entities;

namespace Company.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    d => d.Department,
                    o => o.MapFrom(s => s.Department.Name)
                );
        }
    }
}