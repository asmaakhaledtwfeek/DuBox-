using Dubox.Domain.Entities;

namespace Dubox.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
