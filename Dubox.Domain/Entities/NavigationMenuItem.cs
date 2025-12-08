using Dubox.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("NavigationMenuItems")]
public class NavigationMenuItem : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MenuItemId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Route { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Aliases { get; set; } // Comma-separated aliases like "/quality,/qc"

    [Required]
    [MaxLength(100)]
    public string PermissionModule { get; set; } = string.Empty; // e.g., "projects"

    [Required]
    [MaxLength(50)]
    public string PermissionAction { get; set; } = "view"; // e.g., "view"

    public Guid? ParentMenuItemId { get; set; }
    [ForeignKey("ParentMenuItemId")]
    public virtual NavigationMenuItem? ParentMenuItem { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public bool IsVisible { get; set; } = true;

    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }

    public virtual ICollection<NavigationMenuItem> Children { get; set; } = new List<NavigationMenuItem>();
}

