using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Mapster;

namespace Dubox.Application.Features.Users.MappingConfig
{
    public class UserMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)

        {
            config.NewConfig<User, UserDto>()
               .Map(des => des.Department, src => src.EmployeeOfDepartment != null ? src.EmployeeOfDepartment.DepartmentName : string.Empty)
                .Map(dest => dest.DirectRoles,
                    src => src.UserRoles
                        .Select(ur => ur.Role.Adapt<RoleDto>())
                        .ToList())

                .Map(dest => dest.Groups,
                    src => src.UserGroups.Select(ug => new GroupWithRolesDto
                    {
                        GroupId = ug.GroupId,
                        GroupName = ug.Group.GroupName,
                        Roles = ug.Group.GroupRoles
                            .Select(gr => gr.Role.Adapt<RoleDto>())
                            .ToList()
                    }).ToList())

                .Map(dest => dest.AllRoles,
                    src =>
                        src.UserRoles.Select(ur => ur.Role)
                        .Union(
                            src.UserGroups
                                .SelectMany(ug => ug.Group.GroupRoles)
                                .Select(gr => gr.Role)
                        )
                        .DistinctBy(r => r.RoleId)
                        .Select(role => role.Adapt<RoleDto>())
                        .ToList()
                );

            config.NewConfig<Role, RoleDto>();

        }
    }

}
