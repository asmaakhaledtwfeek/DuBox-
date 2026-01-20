using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record BoxPanelDto
{
    public Guid BoxPanelId { get; init; }
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid? PanelTypeId { get; init; }
    public string? TypeName { get; init; } // From PanelType navigation
    public string? TypeCode { get; init; } // From PanelType navigation
    public string PanelName { get; init; } = string.Empty;
    public PanelStatusEnum PanelStatus { get; init; }
    
    // Barcode & QR
    public string? Barcode { get; init; }
    public string? QRCodeUrl { get; init; }
    
    // Manufacturing
    public string? ManufacturerName { get; init; }
    public DateTime? ManufacturedDate { get; init; }
    
    // Delivery & Tracking
    public DateTime? DispatchedDate { get; init; }
    public DateTime? EstimatedArrivalDate { get; init; }
    public DateTime? ActualArrivalDate { get; init; }
    public string? DeliveryNoteNumber { get; init; }
    public string? DeliveryNoteUrl { get; init; }
    
    // First Approval
    public string? FirstApprovalStatus { get; init; }
    public Guid? FirstApprovalBy { get; init; }
    public DateTime? FirstApprovalDate { get; init; }
    public string? FirstApprovalNotes { get; init; }
    
    // Second Approval
    public string? SecondApprovalStatus { get; init; }
    public Guid? SecondApprovalBy { get; init; }
    public DateTime? SecondApprovalDate { get; init; }
    public string? SecondApprovalNotes { get; init; }
    
    // Location
    public string? CurrentLocationStatus { get; init; }
    public DateTime? ScannedAtFactory { get; init; }
    public DateTime? InstalledDate { get; init; }
    
    // Physical Info
    public decimal? Weight { get; init; }
    public string? Dimensions { get; init; }
    public string? Notes { get; init; }
    
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
}
