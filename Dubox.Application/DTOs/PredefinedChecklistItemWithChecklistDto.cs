namespace Dubox.Application.DTOs;

public class PredefinedChecklistItemWithChecklistDto
{
    public Guid PredefinedItemId { get; set; }
    public string CheckpointDescription { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public int Sequence { get; set; }
    public bool IsActive { get; set; }
    
    // ChecklistSection information
    public Guid? ChecklistSectionId { get; set; }
    public string? SectionTitle { get; set; }
    public int? SectionOrder { get; set; }
    
    // Checklist information
    public Guid? ChecklistId { get; set; }
    public string? ChecklistName { get; set; }
    public string? ChecklistCode { get; set; }
    public string? ChecklistDiscipline { get; set; }
    public string? ChecklistSubDiscipline { get; set; }
    public int? ChecklistPageNumber { get; set; }
    public string? ChecklistWIRCode { get; set; }
    public List<string>? ChecklistReferenceDocuments { get; set; }
    public List<string>? ChecklistSignatureRoles { get; set; }
}

