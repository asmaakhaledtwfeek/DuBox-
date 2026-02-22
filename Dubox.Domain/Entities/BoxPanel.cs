using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxPanels")]
public class BoxPanel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxPanelId { get; set; }

    [Required]
    public Guid BoxId { get; set; }
    
    [Required]
    public Guid ProjectId { get; set; }
    public Guid? PanelTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string PanelName { get; set; } = string.Empty;

    [Required]
    public PanelStatusEnum PanelStatus { get; set; } = PanelStatusEnum.NotStarted;

    // Identification & Tracking
    [MaxLength(100)]
    public string? QRCode { get; set; }

    [MaxLength(500)]
    public string? QRCodeUrl { get; set; }

    // Manufacturing Information
    [MaxLength(200)]
    public string? ManufacturerName { get; set; }

    public DateTime? ManufacturedDate { get; set; }

    // Dispatch & Delivery
    public DateTime? DispatchedDate { get; set; }
    public DateTime? EstimatedArrivalDate { get; set; }
    public DateTime? ActualArrivalDate { get; set; }

    [MaxLength(100)]
    public string? DeliveryNoteNumber { get; set; }

    [MaxLength(500)]
    public string? DeliveryNoteUrl { get; set; }

    // Pre-cast Location Stage Progress (7 stages)
    public PanelStageEnum CurrentStage { get; set; } = PanelStageEnum.NotStarted;

    // Stage completion tracking (7 stages)
    // Stage 1: Mold Preparation
    public bool MoldPreparationComplete { get; set; } = false;
    public DateTime? MoldPreparationDate { get; set; }

    // Stage 2: Initial
    public bool InitialComplete { get; set; } = false;
    public DateTime? InitialDate { get; set; }

    // Stage 3: MEP Inserts / Embedded Items Installation
    public bool MEPInsertsInstallationComplete { get; set; } = false;
    public DateTime? MEPInsertsInstallationDate { get; set; }

    // Stage 4: Reinforcement Setup
    public bool ReinforcementSetupComplete { get; set; } = false;
    public DateTime? ReinforcementSetupDate { get; set; }

    // Stage 5: Concrete Casting
    public bool ConcreteCastingComplete { get; set; } = false;
    public DateTime? ConcreteCastingDate { get; set; }

    // Stage 6: Surface Finishing
    public bool SurfaceFinishingComplete { get; set; } = false;
    public DateTime? SurfaceFinishingDate { get; set; }

    // Stage 7: Curing & Demolding
    public bool CuringAndDemoldingComplete { get; set; } = false;
    public DateTime? CuringAndDemoldingDate { get; set; }

    [MaxLength(50)]
    public string? WorkflowStatus { get; set; } // InProgress, OnHold, Completed

    public DateTime? WorkflowStatusDate { get; set; }
    public Guid? WorkflowStatusBy { get; set; }
    public string? WorkflowStatusNotes { get; set; }

    // Quality Issue reference (when rejected or on hold)
    public Guid? QualityIssueId { get; set; }

    // First Approval (Quality Check)
    [MaxLength(50)]
    public string? FirstApprovalStatus { get; set; } // Pending, Approved, Rejected

    public Guid? FirstApprovalBy { get; set; }
    public DateTime? FirstApprovalDate { get; set; }

    public string? FirstApprovalNotes { get; set; }

    // Second Approval (Installation Ready)
    [MaxLength(50)]
    public string? SecondApprovalStatus { get; set; } // Pending, Approved, Rejected

    public Guid? SecondApprovalBy { get; set; }
    public DateTime? SecondApprovalDate { get; set; }

    public string? SecondApprovalNotes { get; set; }

    // Location Tracking
    [MaxLength(50)]
    public string? CurrentLocationStatus { get; set; } // InTransit, ArrivedFactory, Installed, Rejected

    public DateTime? ScannedAtFactory { get; set; }
    public DateTime? InstalledDate { get; set; }

    // Physical Information
    [Column(TypeName = "decimal(10,2)")]
    public decimal? Weight { get; set; }

    [MaxLength(100)]
    public string? Dimensions { get; set; }

    public string? Notes { get; set; }

    // Audit fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Box Box { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
    public virtual PanelType? PanelType { get; set; }
    public virtual ICollection<PanelScanLog> ScanLogs { get; set; } = new List<PanelScanLog>();
}
