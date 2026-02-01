using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetUserWithRolesSpecification : Specification<User>
    {
        public GetUserWithRolesSpecification(string email)
        {
            AddCriteria(u => u.Email == email);
            AddInclude(nameof(User.UserRoles));
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
            AddInclude(nameof(User.UserGroups));
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}");
        }
    }
}
