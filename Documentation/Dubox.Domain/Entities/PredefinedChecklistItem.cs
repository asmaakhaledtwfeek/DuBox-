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
    public string Description { get; set; } = string.Empty;

    [Required]
    public int Sequence { get; set; }

    [MaxLength(200)]
    public string? Reference { get; set; }

    public Guid? ChecklistSectionId { get; set; }

    [ForeignKey(nameof(ChecklistSectionId))]
    public virtual ChecklistSection? ChecklistSection { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

