using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.PanelTypes.Queries;

public record GetPanelTypeByIdQuery(Guid PanelTypeId) : IRequest<Result<PanelTypeDto>>;

