using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class ComplateTeamMemberProfileCommandHandler : IRequestHandler<ComplateTeamMemberProfileCommand, Result<TeamMemberDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComplateTeamMemberProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<TeamMemberDto>> Handle(ComplateTeamMemberProfileCommand request, CancellationToken cancellationToken)
        {
            var teamMember = _unitOfWork.Repository<TeamMember>()
                .GetEntityWithSpec(new GetTeamMemberWithIcludesSpecification(request.TeamMemberId));
            if (teamMember == null)
                return Result.Failure<TeamMemberDto>("Team member not found.");

            teamMember.EmployeeCode = request.EmployeeCode;
            teamMember.EmployeeName = request.EmployeeName;
            teamMember.MobileNumber = request.MobileNumber;

            _unitOfWork.Repository<TeamMember>().Update(teamMember);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = teamMember.Adapt<TeamMemberDto>();

            return Result.Success(dto);
        }
    }
}
