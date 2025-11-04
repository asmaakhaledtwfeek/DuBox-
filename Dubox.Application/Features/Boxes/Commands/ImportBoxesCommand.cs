using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record ImportBoxesCommand(
    Guid ProjectId,
    List<CreateBoxDto> Boxes
) : IRequest<Result<List<BoxDto>>>;

