using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure<UserDto>("User not found");

        var emailExists = await _unitOfWork.Repository<User>()
            .IsExistAsync(u => u.Email == request.Email && u.UserId != request.UserId, cancellationToken);

        if (emailExists)
            return Result.Failure<UserDto>("Email already in use by another user");

        user.Email = request.Email;
        user.FullName = request.FullName;
        user.Department = request.Department;
        user.IsActive = request.IsActive;

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(user.Adapt<UserDto>());
    }
}

