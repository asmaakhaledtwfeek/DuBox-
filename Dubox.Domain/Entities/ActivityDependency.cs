using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("ActivityDependencies")]
    public class ActivityDependency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DependencyId { get; set; }

        [Required]
        [ForeignKey(nameof(BoxActivity))]
        public Guid BoxActivityId { get; set; }

        [Required]
        [ForeignKey(nameof(PredecessorActivity))]
        public Guid PredecessorActivityId { get; set; }

        [MaxLength(20)]
        public string DependencyType { get; set; } = "FS";

        public int LagDays { get; set; } = 0;

        public virtual BoxActivity BoxActivity { get; set; } = null!;
        public virtual BoxActivity PredecessorActivity { get; set; } = null!;
    }
}
