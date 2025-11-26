using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public record UpdateQualityIssueStatusCommand(
        Guid IssueId,
        QualityIssueStatusEnum Status,
        string? ResolutionDescription,
        string? PhotoPath
        ) : IRequest<Result<QualityIssueDetailsDto>>;

}
