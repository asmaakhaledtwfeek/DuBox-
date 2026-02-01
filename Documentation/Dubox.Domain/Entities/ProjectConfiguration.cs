using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

/// <summary>
/// Project-specific building configurations
/// </summary>
[Table("ProjectBuildings")]
public class ProjectBuilding
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string BuildingCode { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? BuildingName { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Project-specific level/floor configurations
/// </summary>
[Table("ProjectLevels")]
public class ProjectLevel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LevelCode { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? LevelName { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Project-specific box type configurations (extends or overrides system defaults)
/// </summary>
[Table("ProjectBoxTypes")]
public class ProjectBoxType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string TypeName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Abbreviation { get; set; }

    public bool HasSubTypes { get; set; } = false;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<ProjectBoxSubType> SubTypes { get; set; } = new List<ProjectBoxSubType>();
}

/// <summary>
/// Project-specific box sub-type configurations
/// </summary>
[Table("ProjectBoxSubTypes")]
public class ProjectBoxSubType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ProjectBoxTypeId { get; set; }

    [ForeignKey(nameof(ProjectBoxTypeId))]
    public ProjectBoxType ProjectBoxType { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string SubTypeName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? Abbreviation { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Project-specific zone configurations
/// </summary>
[Table("ProjectZones")]
public class ProjectZone
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ZoneCode { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ZoneName { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Project-specific box function configurations (new feature)
/// </summary>
[Table("ProjectBoxFunctions")]
public class ProjectBoxFunction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FunctionName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

