using Dubox.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dubox.Domain.Entities
{
    [Table("Factories")]
    [Index(nameof(FactoryCode), IsUnique = true)]
    public class Factory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FactoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FactoryCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FactoryName { get; set; } = string.Empty;
        [Required]
        public ProjectLocationEnum Location { get; set; } = ProjectLocationEnum.UAE;
        public int? Capacity { get; set; }

        public int CurrentOccupancy { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<FactoryLocation> FactoryLocations { get; set; } = new List<FactoryLocation>();
        public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();


        [NotMapped]
        public bool IsFull => Capacity.HasValue && CurrentOccupancy >= Capacity;

        [NotMapped]
        public int AvailableCapacity => Capacity.HasValue ? Capacity.Value - CurrentOccupancy : 0;
    }
}

