using Dubox.Domain.Entities;

namespace Dubox.Domain.Services;

public interface IUserRolePermissionService
{
    List<string> GetAllUserRoles(User user);
    List<string> GetDirectUserRoles(User user);
    List<string> GetAllUserPermissionKeys(User user);
}
