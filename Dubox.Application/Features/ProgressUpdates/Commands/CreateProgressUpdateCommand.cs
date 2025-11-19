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
    List<string>? PhotoUrls,
    string UpdateMethod,
    string? DeviceInfo
) : IRequest<Result<ProgressUpdateDto>>;

