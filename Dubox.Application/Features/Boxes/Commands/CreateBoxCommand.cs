using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Commands;

public record CreateBoxCommand(
    Guid ProjectId,
    string BoxTag,
    string? BoxName,
    string BoxType,
    string Floor,
    string? Building,
    string? Zone,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    string? BIMModelReference,
    string? RevitElementId,
    List<CreateBoxAssetDto>? Assets
) : IRequest<Result<BoxDto>>;

