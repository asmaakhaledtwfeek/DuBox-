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

    [MaxLength(50)]
    public string? ItemNumber { get; set; }

    [MaxLength(50)]
    public string? WIRNumber { get; set; }

    public Guid? ReferenceId { get; set; }

    [ForeignKey(nameof(ReferenceId))]
    public virtual Reference? Reference { get; set; }

    [Required]
    public int Sequence { get; set; }

    public Guid? CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

