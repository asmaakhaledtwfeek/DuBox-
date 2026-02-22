using Dubox.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("FactorySections")]
    public class FactorySection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SectionId { get; set; }

        [ForeignKey("Factory")]
        [Required]
        public Guid FactoryId { get; set; }

        [Required]
        public FactorySectionTypeEnum SectionType { get; set; }

        [Required]
        [MaxLength(100)]
        public string SectionName { get; set; } = string.Empty;

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
        public int DisplayOrder { get; set; }

    // Navigation properties
    public virtual Factory Factory { get; set; } = null!;
    public virtual ICollection<FactorySectionPart> Parts { get; set; } = new List<FactorySectionPart>();

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

