using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("Risks")]
    public class Risk
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RiskId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(Box))]
        public Guid? BoxId { get; set; }

        [MaxLength(100)]
        public string? RiskCategory { get; set; } // Schedule, Cost, Quality, Safety, Material

        public string? RiskDescription { get; set; }

        [MaxLength(20)]
        public string? Impact { get; set; } // High, Medium, Low

        [MaxLength(20)]
        public string? Probability { get; set; } // High, Medium, Low

        public string? MitigationPlan { get; set; }

        [MaxLength(200)]
        public string? Owner { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, Mitigated, Occurred, Closed

        public DateTime? IdentifiedDate { get; set; }

        public DateTime? ReviewDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual Box? Box { get; set; }

        // Calculated Risk Score (1-9)
        [NotMapped]
        public int RiskScore
        {
            get
            {
                var impactValue = Impact switch
                {
                    "High" => 3,
                    "Medium" => 2,
                    "Low" => 1,
                    _ => 1
                };

                var probabilityValue = Probability switch
                {
                    "High" => 3,
                    "Medium" => 2,
                    "Low" => 1,
                    _ => 1
                };

                return impactValue * probabilityValue;
            }
        }
    }
}
