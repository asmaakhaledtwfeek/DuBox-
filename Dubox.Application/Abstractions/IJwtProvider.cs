using Dubox.Application.DTOs;
using Dubox.Domain.Entities;

namespace Dubox.Application.Abstractions;

public interface IJwtProvider
{
    JwtTokenResult GenerateToken(User user);
}
