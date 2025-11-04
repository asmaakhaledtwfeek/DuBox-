using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using MediatR;

namespace Dubox.Application.Features.Groups.Commands;

public class AssignRolesToGroupCommandHandler : IRequestHandler<AssignRolesToGroupCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignRolesToGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignRolesToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Repository<Group>()
            .GetByIdAsync(request.GroupId, cancellationToken);

        if (group == null)
            return Result.Failure("Group not found");

        var existingGroupRoles = _unitOfWork.Repository<GroupRole>()
            .Get()
            .Where(gr => gr.GroupId == request.GroupId)
            .ToList();

        _unitOfWork.Repository<GroupRole>().DeleteRange(existingGroupRoles);

        var groupRoles = request.RoleIds.Select(roleId => new GroupRole
        {
            GroupId = request.GroupId,
            RoleId = roleId,
            AssignedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.Repository<GroupRole>().AddRangeAsync(groupRoles, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}

