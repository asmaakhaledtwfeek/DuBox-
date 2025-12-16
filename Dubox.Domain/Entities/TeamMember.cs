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
        public Guid TeamId { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(TeamGroup))]
        public Guid? TeamGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual TeamGroup? TeamGroup { get; set; }
        public virtual ICollection<BoxActivity> BoxActivities { get; set; } = new List<BoxActivity>();

    }
}
