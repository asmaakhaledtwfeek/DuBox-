using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record DuplicateBoxCommand(
    Guid BoxId
) : IRequest<Result<BoxDto>>;



