using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

public record GetPredefinedChecklistItemsByWIRCheckpointIdQuery(Guid WIRCheckPointId) : IRequest<Result<List<PredefinedChecklistItemWithChecklistDto>>>;


