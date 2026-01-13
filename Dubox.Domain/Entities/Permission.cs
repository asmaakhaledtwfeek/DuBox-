using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("Permissions")]
public class Permission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PermissionId { get; set; }

    /// <summary>
    /// The module/controller this permission belongs to (e.g., "Projects", "Boxes", "Users")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// The action this permission grants (e.g., "View", "Create", "Edit", "Delete")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// A unique key for this permission (e.g., "projects.create", "boxes.delete")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PermissionKey { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable name for the permission
    /// </summary>
    [MaxLength(200)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Description of what this permission allows
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Group permissions for better organization in UI
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Order for display in UI
    /// </summary>
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

