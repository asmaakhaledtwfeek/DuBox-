using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetUsersByDepartmentWithIncludesSpecification:Specification<User>
    {
        public GetUsersByDepartmentWithIncludesSpecification( Guid departmentId, List<Guid>? unActiveTeamMemberUserIds)
        {
           AddCriteria(u => u.DepartmentId == departmentId && u.IsActive == true);
            if (unActiveTeamMemberUserIds != null && unActiveTeamMemberUserIds.Count > 0)
            {
                AddCriteria(u => !unActiveTeamMemberUserIds.Contains(u.UserId));
            }
            AddInclude(nameof(User.EmployeeOfDepartment));
            AddInclude(nameof(User.UserRoles));
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}");
        }
    }
}
