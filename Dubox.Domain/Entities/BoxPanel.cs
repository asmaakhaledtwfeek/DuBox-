using Dubox.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities;

[Table("BoxPanels")]
public class BoxPanel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BoxPanelId { get; set; }

    [Required]
    public Guid BoxId { get; set; }
    
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    [MaxLength(200)]
    public string PanelName { get; set; } = string.Empty;

    [Required]
    public PanelStatusEnum PanelStatus { get; set; } = PanelStatusEnum.NotStarted;

    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Box Box { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}
