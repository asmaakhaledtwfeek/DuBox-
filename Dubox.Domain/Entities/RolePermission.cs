using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("RolePermissions")]
public class RolePermission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RolePermissionId { get; set; }

    [Required]
    public Guid RoleId { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; } = null!;

    [Required]
    public Guid PermissionId { get; set; }

    [ForeignKey("PermissionId")]
    public virtual Permission Permission { get; set; } = null!;

    /// <summary>
    /// When this permission was granted to the role
    /// </summary>
    public DateTime GrantedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Who granted this permission (optional audit trail)
    /// </summary>
    public Guid? GrantedByUserId { get; set; }

    [ForeignKey("GrantedByUserId")]
    public virtual User? GrantedByUser { get; set; }
}

