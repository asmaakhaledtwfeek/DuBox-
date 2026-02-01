namespace Dubox.Domain.Abstraction;

public interface ICurrentUserService
{
    string? Username { get; }
    string? UserId { get; }
    string? Role { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
    Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken cancellationToken = default);
    Task<bool> HasRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> HasAnyRoleAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
}

