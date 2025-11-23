using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTeamMemberCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
        {
            // Verify team exists
            var team = await _unitOfWork.Repository<Team>()
                .GetByIdAsync(request.TeamId, cancellationToken);
            
            if (team == null)
                return Result.Failure<bool>("Team not found.");

            // Get the team member
            var teamMember = await _unitOfWork.Repository<TeamMember>()
                .GetByIdAsync(request.TeamMemberId, cancellationToken);

            if (teamMember == null)
                return Result.Failure<bool>("Team member not found.");

            // Verify the member belongs to the team
            if (teamMember.TeamId != request.TeamId)
                return Result.Failure<bool>("Team member does not belong to this team.");

            // Check if this member is the team leader
            if (team.TeamLeaderMemberId == request.TeamMemberId)
            {
                // Remove team leader assignment
                team.TeamLeaderMemberId = null;
                _unitOfWork.Repository<Team>().Update(team);
            }

            // Delete the team member
            _unitOfWork.Repository<TeamMember>().Delete(teamMember);

            // Save changes
            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            if (!saveResult)
                return Result.Failure<bool>("Failed to remove team member.");

            return Result.Success(true);
        }
    }
}


