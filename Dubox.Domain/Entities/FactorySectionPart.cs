using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("FactorySectionParts")]
    [Index(nameof(SectionId), nameof(PartNumber), IsUnique = true)]
    public class FactorySectionPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PartId { get; set; }

        [ForeignKey("FactorySection")]
        [Required]
        public Guid SectionId { get; set; }

        [Required]
        public int PartNumber { get; set; } // 1, 2, 3, 4, etc.

        [Required]
        [MaxLength(100)]
        public string PartName { get; set; } = string.Empty; // e.g., "Assembly-1 Part 1", "Finishing-1 Part 2"

        [Required]
        public int MinRow { get; set; } = 1;

        [Required]
        public int MaxRow { get; set; } = 10;

        [Required]
        [MaxLength(1)]
        public string MinBay { get; set; } = "A";

        [Required]
        [MaxLength(1)]
        public string MaxBay { get; set; } = "Z";

        public int? Capacity { get; set; }

        public int CurrentOccupancy { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public virtual FactorySection FactorySection { get; set; } = null!;
        public virtual ICollection<FreezingCell> FreezingCells { get; set; } = new List<FreezingCell>();

        [NotMapped]
        public bool IsFull => Capacity.HasValue && CurrentOccupancy >= Capacity;

        [NotMapped]
        public int AvailableCapacity => Capacity.HasValue ? Capacity.Value - CurrentOccupancy : 0;

        [NotMapped]
        public int RowCount => MaxRow - MinRow + 1;

        [NotMapped]
        public int BayCount
        {
            get
            {
                if (string.IsNullOrEmpty(MinBay) || string.IsNullOrEmpty(MaxBay))
                    return 0;
                return MaxBay[0] - MinBay[0] + 1;
            }
        }
    }
}

