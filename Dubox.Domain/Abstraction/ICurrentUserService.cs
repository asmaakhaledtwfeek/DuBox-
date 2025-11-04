namespace Dubox.Domain.Abstraction;

public interface ICurrentUserService
{
    string? Username { get; }
    string? UserId { get; }
    bool IsAuthenticated { get; }
}

