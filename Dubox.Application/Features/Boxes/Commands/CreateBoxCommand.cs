using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record CreateBoxCommand(
    Guid ProjectId,
    string BoxTag,
    string? BoxName,
    int? BoxTypeId,  // Optional - box type is now derived from BoxTag and project configuration
    int? BoxSubTypeId,
    string Floor,
    string? BuildingNumber,
    string? BoxFunction,
    string? Zone,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    string? RevitElementId,
    DateTime? BoxPlannedStartDate,
    int? BoxDuration,
    Guid? FactoryId,
    List<CreateBoxAssetDto>? Assets
    // Activity template is now auto-resolved: box type template > project template > activity master
) : IRequest<Result<BoxDto>>;

