using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;

namespace Dubox.Application.Features.Teams.Commands
{
    public class ComplateTeamMemberProfileCommandHandler : IRequestHandler<ComplateTeamMemberProfileCommand, Result<TeamMemberDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ComplateTeamMemberProfileCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<TeamMemberDto>> Handle(ComplateTeamMemberProfileCommand request, CancellationToken cancellationToken)
        {
            var teamMember = _unitOfWork.Repository<TeamMember>()
                .GetEntityWithSpec(new GetTeamMemberWithIcludesSpecification(request.TeamMemberId));
            if (teamMember == null)
                return Result.Failure<TeamMemberDto>("Team member not found.");

            // Check if team is active
            var team = await _unitOfWork.Repository<Team>()
                .GetByIdAsync(teamMember.TeamId, cancellationToken);

            if (team == null)
                return Result.Failure<TeamMemberDto>("Team not found.");

            if (!team.IsActive)
                return Result.Failure<TeamMemberDto>("Cannot update member profile in an inactive team.");

            // Store old values for audit log
            var oldValues = new List<string>();
            var newValues = new List<string>();

            if (teamMember.EmployeeCode != request.EmployeeCode)
            {
                oldValues.Add($"EmployeeCode: {teamMember.EmployeeCode ?? "N/A"}");
                newValues.Add($"EmployeeCode: {request.EmployeeCode}");
            }

            if (teamMember.EmployeeName != request.EmployeeName)
            {
                oldValues.Add($"EmployeeName: {teamMember.EmployeeName ?? "N/A"}");
                newValues.Add($"EmployeeName: {request.EmployeeName}");
            }

            if (teamMember.MobileNumber != request.MobileNumber)
            {
                oldValues.Add($"MobileNumber: {teamMember.MobileNumber ?? "N/A"}");
                newValues.Add($"MobileNumber: {request.MobileNumber ?? "N/A"}");
            }

            teamMember.EmployeeCode = request.EmployeeCode;
            teamMember.EmployeeName = request.EmployeeName;
            teamMember.MobileNumber = request.MobileNumber;

            _unitOfWork.Repository<TeamMember>().Update(teamMember);

            // Create audit log if there are changes
            if (oldValues.Any())
            {
                var currentUserId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString());
                var auditLog = new AuditLog
                {
                    TableName = nameof(TeamMember),
                    RecordId = teamMember.TeamMemberId,
                    Action = "ProfileUpdate",
                    OldValues = string.Join(", ", oldValues),
                    NewValues = string.Join(", ", newValues),
                    ChangedBy = currentUserId,
                    ChangedDate = DateTime.UtcNow,
                    Description = $"Team member profile updated. ({oldValues.Count} properties changed)."
                };
                await _unitOfWork.Repository<AuditLog>().AddAsync(auditLog, cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = teamMember.Adapt<TeamMemberDto>();

            return Result.Success(dto);
        }
    }
}
