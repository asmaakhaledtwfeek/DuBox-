using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("CostCategories")]
    [Index(nameof(CategoryCode), IsUnique = true)]
    public class CostCategory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; } = string.Empty;

        [ForeignKey(nameof(ParentCategory))]
        public int? ParentCategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual CostCategory? ParentCategory { get; set; }
        public virtual ICollection<CostCategory> SubCategories { get; set; } = new List<CostCategory>();
        public virtual ICollection<BoxCost> BoxCosts { get; set; } = new List<BoxCost>();
    }

}
