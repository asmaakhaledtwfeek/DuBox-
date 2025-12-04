using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record AddQualityIssuesCommand(
    Guid WIRId,
    List<QualityIssueItem> Issues
) : IRequest<Result<WIRCheckpointDto>>;

    public record QualityIssueItem(
    IssueTypeEnum IssueType,
    SeverityEnum Severity,
    string IssueDescription,
    string? AssignedTo = null,
    DateTime? DueDate = null,
    string? Photo = null
);
}
