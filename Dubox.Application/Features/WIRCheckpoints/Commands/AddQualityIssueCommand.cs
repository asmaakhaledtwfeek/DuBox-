using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Commands
{
    public record AddQualityIssueCommand(
        Guid WIRId,
        IssueTypeEnum IssueType,
        SeverityEnum Severity,
        string IssueDescription,
        Guid? AssignedTo = null,
        DateTime? DueDate = null,
        List<string>? ImageUrls = null,
        List<byte[]>? Files = null
    ) : IRequest<Result<WIRCheckpointDto>>;
}

