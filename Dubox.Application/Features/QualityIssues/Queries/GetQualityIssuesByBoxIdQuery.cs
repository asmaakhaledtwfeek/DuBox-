using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public record GetQualityIssuesByBoxIdQuery(Guid BoxId) : IRequest<Result<List<QualityIssueDetailsDto>>>;
}
