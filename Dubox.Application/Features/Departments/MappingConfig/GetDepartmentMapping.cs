using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Departments.MappingConfig
{
    internal class GetDepartmentMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Department, DepartmentDto>()
            .Map(dest => dest.ManagerName, src => src.Manager != null ? src.Manager.FullName : null)
            .Map(dest => dest.EmployeeCount, src => src.Employees != null ? src.Employees.Count : 0);
        }
    }
}
