using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("Projects")]
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProjectId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ClientName { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }

    [Required]
    public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.Active; // Active, On Hold, Completed

    [MaxLength(500)]
    public string? Description { get; set; }

    public int TotalBoxes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<Box> Boxes { get; set; } = new List<Box>();
}
