using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public record GetQualityIssueFiltersQuery() : IRequest<Result<QualityIssueFiltersDto>>;
}
