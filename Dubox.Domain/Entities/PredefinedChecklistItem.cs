using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("PredefinedChecklistItems")]
public class PredefinedChecklistItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PredefinedItemId { get; set; }

    [Required]
    [MaxLength(500)]
    public string CheckpointDescription { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ReferenceDocument { get; set; }

    [Required]
    public int Sequence { get; set; }

    [MaxLength(50)]
    public string? Category { get; set; } // e.g., "General", "Setting Out", "Installation Activity"

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

