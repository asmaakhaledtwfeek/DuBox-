using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class AssignedTeamMembersCommandHandler : IRequestHandler<AssignedTeamMembersCommand, Result<TeamMembersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignedTeamMembersCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<TeamMembersDto>> Handle(AssignedTeamMembersCommand request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>()
            .GetByIdAsync(request.TeamId, cancellationToken);
            if (team == null)
                return Result.Failure<TeamMembersDto>("This Team not found.");
            if (request.UserIds == null || !request.UserIds.Any())
                return Result.Failure<TeamMembersDto>("No users provided to assign.");

            var users = await _unitOfWork.Repository<User>()
        .FindAsync(u => request.UserIds.Contains(u.UserId), cancellationToken);

            if (users == null || users.Count == 0)
                return Result.Failure<TeamMembersDto>("No valid users found.");
            var existingMembers = _unitOfWork.Repository<TeamMember>()
                    .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId))
                    .Data.ToList();
            var membersToRemove = existingMembers
                     .Where(tm => !request.UserIds.Contains(tm.UserId))
                     .ToList();

            if (membersToRemove.Any())
                _unitOfWork.Repository<TeamMember>().DeleteRange(membersToRemove);

            var existingUserIds = existingMembers.Select(tm => tm.UserId).ToHashSet();
            var newMembers = users
                .Where(u => !existingUserIds.Contains(u.UserId))
                .Select(u => new TeamMember
                {
                    TeamId = team.TeamId,
                    UserId = u.UserId,
                    IsActive = u.IsActive
                })
                .ToList();

            if (newMembers.Count > 0)
            {
                await _unitOfWork.Repository<TeamMember>().AddRangeAsync(newMembers, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            var teamMembers = _unitOfWork.Repository<TeamMember>()
                  .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId))
                       .Data.ToList();
            var dto = (team, teamMembers).Adapt<TeamMembersDto>();

            return Result.Success(dto);
        }
    }
}
