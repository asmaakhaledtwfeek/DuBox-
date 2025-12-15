using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("ChecklistSections")]
public class ChecklistSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ChecklistSectionId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int Order { get; set; }

    public Guid? ChecklistId { get; set; }

    [ForeignKey(nameof(ChecklistId))]
    public virtual Checklist? Checklist { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<PredefinedChecklistItem> Items { get; set; } = new List<PredefinedChecklistItem>();
}
