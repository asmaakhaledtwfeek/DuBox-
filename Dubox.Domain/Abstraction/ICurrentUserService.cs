namespace Dubox.Domain.Abstraction;

public interface ICurrentUserService
{
    string? Username { get; }
    string? UserId { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}

