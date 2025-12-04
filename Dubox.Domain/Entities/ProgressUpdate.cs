using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ProgressUpdates")]
public class ProgressUpdate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProgressUpdateId { get; set; }

    public Guid BoxId { get; set; }
    public Box Box { get; set; } = null!;

    public Guid BoxActivityId { get; set; }
    public BoxActivity BoxActivity { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public Guid UpdatedBy { get; set; }
    public User UpdatedByUser { get; set; } = null!;

    public decimal ProgressPercentage { get; set; } // Progress at time of update
    public decimal BoxProgressSnapshot { get; set; } = 0;

    [Required]
    [MaxLength(50)]
    public BoxStatusEnum Status { get; set; } = BoxStatusEnum.NotStarted;

    [MaxLength(1000)]
    public string? WorkDescription { get; set; }

    [MaxLength(1000)]
    public string? IssuesEncountered { get; set; }

    // Location tracking
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(200)]
    public string? LocationDescription { get; set; }

    public string? Photo { get; set; }

    // Update method
    [MaxLength(50)]
    public string UpdateMethod { get; set; } = "Mobile"; // Mobile, Web

    [MaxLength(100)]
    public string? DeviceInfo { get; set; }

    public DateTime CreatedDate { get; set; }
}
