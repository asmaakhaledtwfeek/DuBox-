using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.QualityIssues.Queries
{
    public record GetQualityIssuesByProjectIdQuery(Guid ProjectId) : IRequest<Result<List<QualityIssueDetailsDto>>>;
}


