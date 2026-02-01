using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user with this email already exists
        var userExists = await _unitOfWork.Repository<User>()
            .IsExistAsync(u => u.Email == request.Email, cancellationToken);

        if (userExists)
            return Result.Failure<UserDto>("User with this email already exists");

        // Validate that the department exists and get it for mapping
        var department = await _unitOfWork.Repository<Department>()
            .GetByIdAsync(request.DepartmentId, cancellationToken);

        if (department == null)
            return Result.Failure<UserDto>($"Department with ID {request.DepartmentId} does not exist.");

        // Create new user
        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            FullName = request.FullName,
            DepartmentId = request.DepartmentId,
            IsActive = request.IsActive,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Repository<User>().AddAsync(user, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Load the department navigation property for proper DTO mapping
        user.EmployeeOfDepartment = department;

        return Result.Success(user.Adapt<UserDto>());
    }
}
