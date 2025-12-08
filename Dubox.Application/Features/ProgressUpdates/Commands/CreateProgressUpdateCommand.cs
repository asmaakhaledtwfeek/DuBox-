using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ProgressUpdates.Commands;

public record CreateProgressUpdateCommand(
    Guid BoxId,
    Guid BoxActivityId,
    decimal ProgressPercentage,
    string? WorkDescription,
    string? IssuesEncountered,
    double? Latitude,
    double? Longitude,
    string? LocationDescription,
    List<byte[]>? Files,
    List<string>? ImageUrls,
    string UpdateMethod,
    string? DeviceInfo
) : IRequest<Result<ProgressUpdateDto>>;

