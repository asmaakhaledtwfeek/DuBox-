using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dubox.Application.Features.QualityIssues.Commands
{
    public record CreateQualityIssueCommand(
        Guid BoxId,
        IssueTypeEnum IssueType,
        SeverityEnum Severity,
        string IssueDescription,
        Guid? AssignedTo = null,
        Guid? AssignedToUserId = null,
        DateTime? DueDate = null,
        List<string>? ImageUrls = null,
         List<IFormFile>? Files=null,
        List<string>? FileNames = null
    ) : IRequest<Result<QualityIssueDetailsDto>>;
}

