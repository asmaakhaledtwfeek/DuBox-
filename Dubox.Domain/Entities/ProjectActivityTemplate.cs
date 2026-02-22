using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    /// <summary>
    /// Links projects to activity templates at the project level (default template)
    /// This template is used when no box-type-specific template is assigned
    /// </summary>
    [Table("ProjectActivityTemplates")]
    public class ProjectActivityTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectActivityTemplateId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }

        [Required]
        [ForeignKey(nameof(ActivityTemplate))]
        public Guid ActivityTemplateId { get; set; }

        /// <summary>
        /// Indicates whether this is the default template for the project
        /// Only one template can be default per project
        /// </summary>
        public bool IsDefault { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey(nameof(CreatedByUser))]
        public Guid? CreatedBy { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ActivityTemplate ActivityTemplate { get; set; } = null!;
        public virtual User? CreatedByUser { get; set; }
    }
}
