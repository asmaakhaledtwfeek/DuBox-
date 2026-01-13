using Dubox.Domain.Specification;
using Dubox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetUserPermissionSpecification :Specification<User>
    {
        public GetUserPermissionSpecification(Guid userId)
        {
            AddCriteria(u => u.UserId == userId);
            AddInclude(nameof(User.UserRoles));
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}.{nameof(Role.RolePermissions)}");
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}.{nameof(Role.RolePermissions)}.{nameof(RolePermission.Permission)}");

            AddInclude(nameof(User.UserGroups));
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}.{nameof(Role.RolePermissions)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}.{nameof(Role.RolePermissions)}.{nameof(RolePermission.Permission)}");
        }
    }
}
