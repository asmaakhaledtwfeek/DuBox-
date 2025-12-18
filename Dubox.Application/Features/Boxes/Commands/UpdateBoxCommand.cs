using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record UpdateBoxCommand(
    Guid BoxId,
    string? BoxTag,
    string? BoxName,
    int? BoxTypeId,
    int? BoxSubTypeId,
    string? Floor,
    string? Building,
    BoxZone? Zone,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    DateTime? PlannedStartDate,
    int? Duration,
    string? Notes
) : IRequest<Result<BoxDto>>;

