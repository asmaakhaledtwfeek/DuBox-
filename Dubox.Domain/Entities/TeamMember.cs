using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    public class TeamMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamMemberId { get; set; }

        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        [MaxLength(50)]
        public string? EmployeeCode { get; set; }

        [MaxLength(200)]
        public string? EmployeeName { get; set; }

        [MaxLength(100)]
        public string? Role { get; set; }

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Team Team { get; set; } = null!;
    }
}
