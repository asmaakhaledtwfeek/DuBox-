using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GetBoxByIdQuery(Guid BoxId) : IRequest<Result<BoxDto>>;

