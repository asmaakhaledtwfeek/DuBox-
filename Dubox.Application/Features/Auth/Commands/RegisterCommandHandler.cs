using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _unitOfWork.Repository<User>()
            .IsExistAsync(u => u.Email == request.Email, cancellationToken);

        if (userExists)
            return Result.Failure<UserDto>("User with this email already exists");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            FullName = request.FullName,
            Department = request.Department,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<User>().AddAsync(user, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(user.Adapt<UserDto>());
    }
}

