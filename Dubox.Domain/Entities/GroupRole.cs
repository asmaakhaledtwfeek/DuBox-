using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("GroupRoles")]
public class GroupRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid GroupRoleId { get; set; }

    [Required]
    public Guid GroupId { get; set; }

    [Required]
    public Guid RoleId { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;
}

