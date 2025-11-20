using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("BoxLocationHistory")]
    [Index(nameof(BoxId))]
    public class BoxLocationHistory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid HistoryId { get; set; }

        [Required]
        [ForeignKey(nameof(Box))]
        public Guid BoxId { get; set; }

        [Required]
        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }

        [ForeignKey(nameof(MovedFromLocation))]
        public Guid? MovedFromLocationId { get; set; }

        public DateTime MovedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? MovedBy { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime? RFIDReadTime { get; set; }

        // Navigation properties
        public virtual Box Box { get; set; } = null!;
        public virtual FactoryLocation Location { get; set; } = null!;
        public virtual FactoryLocation? MovedFromLocation { get; set; }
    }
}
