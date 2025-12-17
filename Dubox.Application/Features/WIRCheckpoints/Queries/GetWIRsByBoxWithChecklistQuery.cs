using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.WIRCheckpoints.Queries;

/// <summary>
/// Query to get all WIRs for a box with their checklist items grouped by category
/// </summary>
public record GetWIRsByBoxWithChecklistQuery(
    Guid BoxId
) : IRequest<Result<List<WIRWithChecklistDto>>>;

public class WIRWithChecklistDto
{
    public Guid WIRId { get; set; }
    public string WIRNumber { get; set; } = string.Empty;
    public string WIRName { get; set; } = string.Empty;
    public string? WIRDescription { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? RequestedDate { get; set; }
    public DateTime? InspectionDate { get; set; }
    public string? InspectorName { get; set; }
    public string? InspectorRole { get; set; }
    public string? Comments { get; set; }
    public List<ChecklistSectionDto> Sections { get; set; } = new();
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int ProgressPercentage { get; set; }
}

public class ChecklistSectionDto
{
    public string SectionLetter { get; set; } = string.Empty; // A, B, C, D
    public string SectionName { get; set; } = string.Empty; // Category Name
    public List<ChecklistItemDetailDto> Items { get; set; } = new();
}

public class ChecklistItemDetailDto
{
    public Guid ChecklistItemId { get; set; }
    public string ItemNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ReferenceDocument { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, Pass, Fail
    public string? Remarks { get; set; }
    public int Sequence { get; set; }
}
