using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public record GetQualityIssueByIdQuery(Guid IssueId) : IRequest<Result<QualityIssueDetailsDto>>;
}
