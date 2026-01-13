using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("FactoryLocations")]
    [Index(nameof(LocationCode), IsUnique = true)]
    public class FactoryLocation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string LocationName { get; set; } = string.Empty;
        [ForeignKey("Factory")]
        public Guid? FactoryId { get; set; }

        [MaxLength(50)]
        public string? LocationType { get; set; } // Assembly Bay, Finishing Bay, Storage, Dispatch Area

        [MaxLength(50)]
        public string? Bay { get; set; }

        [MaxLength(50)]
        public string? Row { get; set; }

        [MaxLength(50)]
        public string? Position { get; set; }

        public int? Capacity { get; set; }

        public int CurrentOccupancy { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<BoxLocationHistory> BoxLocationHistory { get; set; } = new List<BoxLocationHistory>();
        public virtual Factory Factory { get; set; } = null!;
        [NotMapped]
        public bool IsFull => Capacity.HasValue && CurrentOccupancy >= Capacity;

        [NotMapped]
        public int AvailableCapacity => Capacity.HasValue ? Capacity.Value - CurrentOccupancy : 0;
    }
}
