namespace Dubox.Application.DTOs;

public record JwtTokenResult
{
    public string Token { get; init; } = string.Empty;
    public int ExpiresInSeconds { get; init; }
}





