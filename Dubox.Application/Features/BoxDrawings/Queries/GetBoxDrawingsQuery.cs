using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxDrawings.Queries;

public record GetBoxDrawingsQuery(Guid BoxId) : IRequest<Result<List<BoxDrawingDto>>>;

