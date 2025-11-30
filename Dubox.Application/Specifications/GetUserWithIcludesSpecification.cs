using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetUserWithIcludesSpecification : Specification<User>
    {
        public GetUserWithIcludesSpecification()
        {
            AddInclude(nameof(User.EmployeeOfDepartment));
            AddInclude(nameof(User.UserGroups));
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}");
            AddInclude(nameof(User.UserRoles));
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
        }
        public GetUserWithIcludesSpecification(Guid userId)
        {
            AddCriteria(u => u.UserId == userId);
            AddInclude(nameof(User.EmployeeOfDepartment));
            AddInclude(nameof(User.UserGroups));
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}");
            AddInclude($"{nameof(User.UserGroups)}.{nameof(UserGroup.Group)}.{nameof(Group.GroupRoles)}.{nameof(GroupRole.Role)}");
            AddInclude(nameof(User.UserRoles));
            AddInclude($"{nameof(User.UserRoles)}.{nameof(UserRole.Role)}");
        }
    }
}
