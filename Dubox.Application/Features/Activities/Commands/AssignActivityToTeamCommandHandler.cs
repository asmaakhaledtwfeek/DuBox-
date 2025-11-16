using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Commands
{
    public class AssignActivityToTeamCommandHandler : IRequestHandler<AssignActivityToTeamCommand, Result<AssignBoxActivityTeamDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AssignActivityToTeamCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AssignBoxActivityTeamDto>> Handle(AssignActivityToTeamCommand request, CancellationToken cancellationToken)
        {
            var activity = _unitOfWork.Repository<BoxActivity>().GetEntityWithSpec(new GetBoxActivityByIdSpecification(request.BoxActivityId));

            if (activity == null)
                return Result.Failure<AssignBoxActivityTeamDto>("Box Activity not found.");

            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(request.TeamId);

            if (team == null)
                return Result.Failure<AssignBoxActivityTeamDto>($"Team with ID {request.TeamId} not found.");
            activity.TeamId = request.TeamId;

            _unitOfWork.Repository<BoxActivity>().Update(activity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var responseDto = new AssignBoxActivityTeamDto
            {
                BoxActivityId = activity.BoxActivityId,
                ActivityCode = activity.ActivityMaster.ActivityCode,
                ActivityName = activity.ActivityMaster.ActivityName,
                TeamId = team.TeamId,
                TeamCode = team.TeamCode,
                TeamName = team.TeamName,
            };

            return Result.Success(responseDto);
        }
    }
}
