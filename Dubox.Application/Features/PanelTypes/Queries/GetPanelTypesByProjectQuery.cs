using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.PanelTypes.Queries;

public record GetPanelTypesByProjectQuery(Guid ProjectId, bool IncludeInactive = false) : IRequest<Result<List<PanelTypeDto>>>;

