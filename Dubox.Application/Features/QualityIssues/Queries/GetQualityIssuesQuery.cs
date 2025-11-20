using Dubox.Application.DTOs;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public record GetQualityIssuesQuery(
    string? SearchTerm = null,
    QualityIssueStatusEnum? Status = null,
    SeverityEnum? Severity = null,
    IssueTypeEnum? IssueType = null
) : IRequest<Result<List<QualityIssueDetailsDto>>>;
}
