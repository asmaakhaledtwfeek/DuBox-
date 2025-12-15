using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

public record GetPredefinedChecklistItemsQuery : IRequest<Result<List<PredefinedChecklistItemDto>>>
{
    public string? WIRNumber { get; init; }
}

