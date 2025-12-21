using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record CreateBoxCommand(
    Guid ProjectId,
    string BoxTag,
    string? BoxName,
    int BoxTypeId,
    int? BoxSubTypeId,
    string Floor,
    string? BuildingNumber,
    string? BoxLetter,
    string? Zone,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    string? BIMModelReference,
    string? RevitElementId,
    DateTime? BoxPlannedStartDate,
    int? BoxDuration,
    Guid? FactoryId,
    List<CreateBoxAssetDto>? Assets
) : IRequest<Result<BoxDto>>;

