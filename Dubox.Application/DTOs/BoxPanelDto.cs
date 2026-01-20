using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs;

public record BoxPanelDto
{
    public Guid BoxPanelId { get; init; }
    public Guid BoxId { get; init; }
    public Guid ProjectId { get; init; }
    public string PanelName { get; init; } = string.Empty;
    public PanelStatusEnum PanelStatus { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
}
