using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? PasswordHash { get; set; }

    [MaxLength(200)]
    public string? FullName { get; set; }

    public Guid DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}

