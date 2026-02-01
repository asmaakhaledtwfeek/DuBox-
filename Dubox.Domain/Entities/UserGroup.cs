using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("UserGroups")]
public class UserGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserGroupId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid GroupId { get; set; }

    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; } = null!;
}

