using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public record CreateQualityIssueCommand(
        Guid BoxId,
        IssueTypeEnum IssueType,
        SeverityEnum Severity,
        string IssueDescription,
        string? AssignedTo = null,
        DateTime? DueDate = null,
        List<string>? ImageUrls = null,
        List<byte[]>? Files = null
    ) : IRequest<Result<QualityIssueDetailsDto>>;
}

