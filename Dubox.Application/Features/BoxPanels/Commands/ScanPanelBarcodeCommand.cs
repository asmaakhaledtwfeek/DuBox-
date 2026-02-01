using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxPanels.Commands;

public record ScanPanelBarcodeCommand(
    string Barcode,
    string ScanType, // Dispatch, SiteArrival, Installation, Inspection
    string? ScanLocation,
    decimal? Latitude,
    decimal? Longitude,
    string? Notes
) : IRequest<Result<BoxPanelDto>>;

