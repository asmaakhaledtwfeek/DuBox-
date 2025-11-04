using Dubox.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dubox.Domain.Entities
{
    [Table("Projects")]
    [Index(nameof(ProjectCode), IsUnique = true)]
    public class Project : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectId { get; set; }

        [Required]
        [MaxLength(50)]
        //  [Index(IsUnique = true)]
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

        [MaxLength(50)]
        public string Status { get; set; } = "Active"; // Active, On Hold, Completed

        public bool IsActive { get; set; } = true;

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        // Navigation properties
        public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();
        public virtual ICollection<Risk> Risks { get; set; } = new List<Risk>();

        // Calculated properties (not mapped to DB)
        [NotMapped]
        public int TotalBoxes => Boxes?.Count ?? 0;

        [NotMapped]
        public int CompletedBoxes => Boxes?.Count(b => b.CurrentStatus == "Completed") ?? 0;

        [NotMapped]
        public decimal OverallProgressPercentage =>
            Boxes?.Any() == true ? Boxes.Average(b => b.ProgressPercentage) : 0;
    }
}
