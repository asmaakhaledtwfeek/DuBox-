using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("TeamGroups")]
    public class TeamGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamGroupId { get; set; }

        [Required]
        [ForeignKey(nameof(Team))]
        public Guid TeamId { get; set; }

        [Required]
        [MaxLength(50)]
        public string GroupTag { get; set; } = string.Empty; // required (G1, G2, A, B)

        [Required]
        [MaxLength(100)]
        public string GroupType { get; set; } = string.Empty;

        public Guid? GroupLeaderId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }

        // Navigation
        public virtual Team Team { get; set; } = null!;
        [ForeignKey(nameof(GroupLeaderId))]
        public virtual TeamMember GroupLeader { get; set; } = null!;
        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    }
}

