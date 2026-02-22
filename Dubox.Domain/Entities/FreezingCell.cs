using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("FreezingCells")]
    public class FreezingCell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FreezingCellId { get; set; }

        [ForeignKey("FactorySectionPart")]
        [Required]
        public Guid FactorySectionPartId { get; set; }

        [Required]
        public int RowNumber { get; set; }

        // Navigation property
        public virtual FactorySectionPart FactorySectionPart { get; set; } = null!;
    }
}
