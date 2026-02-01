using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.BoxPanels.Commands;

/// <summary>
/// Public command for approving panels via QR code scan (no authentication required)
/// </summary>
public record PublicApprovePanelCommand(
    Guid BoxPanelId,
    string ApprovalToken, // Security token embedded in QR code
    string ApprovalType, // "First" or "Second"
    double? Latitude,
    double? Longitude,
    string? DeviceInfo,
    string? Notes
) : IRequest<Result<PublicPanelApprovalDto>>;

/// <summary>
/// DTO for public panel approval response
/// </summary>
public record PublicPanelApprovalDto
{
    public Guid BoxPanelId { get; init; }
    public string PanelName { get; init; } = string.Empty;
    public string ApprovalType { get; init; } = string.Empty;
    public string ApprovalStatus { get; init; } = string.Empty;
    public DateTime ApprovalDate { get; init; }
    public string Message { get; init; } = string.Empty;
}





