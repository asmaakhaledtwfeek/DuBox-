using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
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

        if (request.Email != null)
        {
            var emailExists = await _unitOfWork.Repository<User>()
                .IsExistAsync(u => u.Email == request.Email && u.UserId != request.UserId, cancellationToken);
            if (emailExists)
                return Result.Failure<UserDto>("Email already in use by another user");
            user.Email = request.Email;
        }
        if (request.FullName != null)
            user.FullName = request.FullName;

        if (request.DepartmentId.HasValue)
        {
            var departmentExists = await _unitOfWork.Repository<Department>()
                .IsExistAsync(d => d.DepartmentId == request.DepartmentId.Value, cancellationToken);

            if (!departmentExists)
                return Result.Failure<UserDto>($"Department does not exist.");
            if (user.DepartmentId != request.DepartmentId)
            {
                var teamMember = await _unitOfWork.Repository<TeamMember>().FindAsync(x => x.UserId == user.UserId, cancellationToken);
                var teamMemberExist = teamMember.FirstOrDefault();
                if (teamMemberExist != null)
                    _unitOfWork.Repository<TeamMember>().Delete(teamMemberExist);
            }
            user.DepartmentId = request.DepartmentId.Value;
        }
        if (request.IsActive.HasValue)
            user.IsActive = request.IsActive.Value;

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(user.Adapt<UserDto>());
    }
}

