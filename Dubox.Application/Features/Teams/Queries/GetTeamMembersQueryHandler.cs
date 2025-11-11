using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries
{
    public class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, Result<TeamMembersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeamMembersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<TeamMembersDto>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>()
          .GetByIdAsync(request.TeamId, cancellationToken);
            if (team == null)
                return Result.Failure<TeamMembersDto>("This Team not found.");

            var teamMembers = _unitOfWork.Repository<TeamMember>()
                      .GetWithSpec(new TeamMembersByUserIdsSpecification(request.TeamId))
                      .Data.ToList();
            var dto = (team, teamMembers).Adapt<TeamMembersDto>();
            return Result.Success(dto);
        }
    }
}
