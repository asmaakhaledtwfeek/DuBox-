namespace Dubox.Domain.Services;

public interface IUserRoleService
{
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserHasRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default);
    Task<bool> UserHasAnyRoleAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
}

