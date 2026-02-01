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
        IssueTypeEnum? IssueType = null,
        int Page = 1,
        int PageSize = 25
    ) : IRequest<Result<PaginatedQualityIssuesResponseDto>>;
}
