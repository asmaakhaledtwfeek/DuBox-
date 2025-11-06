using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record UpdateBoxCommand(
    Guid BoxId,
    string BoxTag,
    string? BoxName,
    string BoxType,
    string Floor,
    string? Building,
    string? Zone,
    int Status,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    DateTime? PlannedEndDate,
    string? Notes
) : IRequest<Result<BoxDto>>;

