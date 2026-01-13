using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxDrawings.Queries;

public record GetBoxDrawingByIdQuery(Guid DrawingId) : IRequest<Result<BoxDrawingDto>>;

