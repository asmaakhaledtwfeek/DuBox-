using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Commands;

public class AssignUserToGroupsCommandHandler : IRequestHandler<AssignUserToGroupsCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignUserToGroupsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignUserToGroupsCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure("User not found");

        var existingGroupsCount = _unitOfWork.Repository<Group>()
       .Get().Count(r => request.GroupIds.Contains(r.GroupId));

        if (existingGroupsCount != request.GroupIds.Count)
        {
            return Result.Failure("One or more Group were not found in the groups.");
        }
        var existingUserGroups = _unitOfWork.Repository<UserGroup>()
            .Get()
            .Where(ug => ug.UserId == request.UserId)
            .ToList();

        _unitOfWork.Repository<UserGroup>().DeleteRange(existingUserGroups);

        var userGroups = request.GroupIds.Select(groupId => new UserGroup
        {
            UserId = request.UserId,
            GroupId = groupId,
            JoinedDate = DateTime.UtcNow
        }).ToList();

        await _unitOfWork.Repository<UserGroup>().AddRangeAsync(userGroups, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}

