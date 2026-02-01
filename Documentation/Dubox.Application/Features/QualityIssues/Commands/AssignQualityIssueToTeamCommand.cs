using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public record AssignQualityIssueToTeamCommand(
        Guid IssueId,
        Guid? TeamId,
        Guid? TeamMemberId,
        Guid? CCUserId = null
    ) : IRequest<Result<QualityIssueDetailsDto>>;
}

