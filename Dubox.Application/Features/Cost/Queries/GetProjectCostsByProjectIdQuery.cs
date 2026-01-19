using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Cost.Queries;

public record GetProjectCostsByProjectIdQuery : IRequest<Result<List<ProjectCostDto>>>
{
    public Guid ProjectId { get; init; }
    public string? CostType { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}



