using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignRolesToUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure("User not found");

        var existingRolesCount = _unitOfWork.Repository<Role>()
       .Get().Count(r => request.RoleIds.Contains(r.RoleId));

        if (existingRolesCount != request.RoleIds.Count)
        {
            return Result.Failure("One or more roles were not found in the roles.");
        }
        var existingUserRoles = _unitOfWork.Repository<UserRole>()
            .Get()
            .Where(ur => ur.UserId == request.UserId)
            .ToList();


        _unitOfWork.Repository<UserRole>().DeleteRange(existingUserRoles);

        var userRoles = request.RoleIds.Select(roleId => new UserRole
        {
            UserId = request.UserId,
            RoleId = roleId,
            AssignedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.Repository<UserRole>().AddRangeAsync(userRoles, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}

