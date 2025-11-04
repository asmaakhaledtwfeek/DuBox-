using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IDbContext context, 
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        if (!user.IsActive)
            return Result.Failure<LoginResponseDto>("User account is inactive");

        if (string.IsNullOrEmpty(user.PasswordHash) || 
            !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtProvider.GenerateToken(user);

        var response = new LoginResponseDto
        {
            Token = token,
            User = user.Adapt<UserDto>()
        };

        return Result.Success(response);
    }
}

