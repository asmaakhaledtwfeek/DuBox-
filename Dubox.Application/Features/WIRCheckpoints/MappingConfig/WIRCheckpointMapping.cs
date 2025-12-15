namespace Dubox.Application.Features.WIRCheckpoints.MappingConfig
{
    using Dubox.Application.DTOs;
    using Dubox.Domain.Entities;
    using Mapster;
    using System;

    internal class MapsterConfig : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<WIRChecklistItem, WIRChecklistItemDto>.NewConfig()
                .Map(dest => dest.CheckpointDescription, src => src.CheckpointDescription)
                .Map(dest => dest.WIRId, src => src.WIRId)
                .Map(dest => dest.ReferenceDocument, src => src.ReferenceDocument)
                .Map(dest => dest.Remarks, src => src.Remarks)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Sequence, src => src.Sequence)
                .Map(dest => dest.PredefinedItemId, src => src.PredefinedItemId);

            TypeAdapterConfig<QualityIssue, QualityIssueDto>.NewConfig()
                .Map(dest => dest.IssueId, src => src.IssueId)
                .Map(dest => dest.IssueType, src => src.IssueType.ToString())
                .Map(dest => dest.IssueDescription, src => src.IssueDescription)
                .Map(dest => dest.Severity, src => src.Severity != null ? src.Severity.ToString() : null)
                .Map(dest => dest.DueDate, src => src.DueDate)
                .Map(dest => dest.IssueDate, src => src.IssueDate)
                .Map(dest => dest.ReportedBy, src => src.ReportedBy)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Images, src => src.Images);

            TypeAdapterConfig<WIRCheckpoint, WIRCheckpointDto>.NewConfig()
                .Map(dest => dest.WIRNumber, src => src.WIRCode)
                .Map(dest => dest.ChecklistItems, src => src.ChecklistItems)
                .Map(dest => dest.QualityIssues, src => src.QualityIssues)
                .Map(dest => dest.Images, src => src.Images)
                .Map(dest => dest.PendingDays, src => src.PendingDays)
                .Map(dest => dest.IsOverdue, src => src.IsOverdue)
                .Map(dest => dest.Box, src => src.Box)
                .Map(dest => dest.ProjectId, src => src.Box != null ? src.Box.ProjectId : (Guid?)null)
                .Map(dest => dest.ProjectCode, src => src.Box != null && src.Box.Project != null ? src.Box.Project.ProjectCode : null)
                .Map(dest => dest.BoxName, src => src.Box != null ? src.Box.BoxName : string.Empty)
                .Map(dest => dest.BoxTag, src => src.Box != null ? src.Box.BoxTag : string.Empty);

            TypeAdapterConfig<WIRCheckpointImage, WIRCheckpointImageDto>.NewConfig()
                .Map(dest => dest.WIRCheckpointImageId, src => src.WIRCheckpointImageId)
                .Map(dest => dest.WIRId, src => src.WIRId)
                .Map(dest => dest.ImageData, src => src.ImageData)
                .Map(dest => dest.ImageType, src => src.ImageType)
                .Map(dest => dest.OriginalName, src => src.OriginalName)
                .Map(dest => dest.FileSize, src => src.FileSize)
                .Map(dest => dest.Sequence, src => src.Sequence)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate);

            TypeAdapterConfig<QualityIssueImage, QualityIssueImageDto>.NewConfig()
                .Map(dest => dest.QualityIssueImageId, src => src.QualityIssueImageId)
                .Map(dest => dest.IssueId, src => src.IssueId)
                .Map(dest => dest.ImageData, src => src.ImageData)
                .Map(dest => dest.ImageType, src => src.ImageType)
                .Map(dest => dest.OriginalName, src => src.OriginalName)
                .Map(dest => dest.FileSize, src => src.FileSize)
                .Map(dest => dest.Sequence, src => src.Sequence)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate);

            TypeAdapterConfig<PredefinedChecklistItem, PredefinedChecklistItemWithChecklistDto>.NewConfig()
                .Map(dest => dest.PredefinedItemId, src => src.PredefinedItemId)
                .Map(dest => dest.CheckpointDescription, src => src.Description)
                .Map(dest => dest.Reference, src => src.Reference)
                .Map(dest => dest.Sequence, src => src.Sequence)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.ChecklistSectionId, src => src.ChecklistSectionId)
                .Map(dest => dest.SectionTitle, src => src.ChecklistSection != null ? src.ChecklistSection.Title : null)
                .Map(dest => dest.SectionOrder, src => src.ChecklistSection != null ? (int?)src.ChecklistSection.Order : null)
                .Map(dest => dest.ChecklistId, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? (Guid?)src.ChecklistSection.Checklist.ChecklistId : null)
                .Map(dest => dest.ChecklistName, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.Name : null)
                .Map(dest => dest.ChecklistCode, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.Code : null)
                .Map(dest => dest.ChecklistDiscipline, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.Discipline : null)
                .Map(dest => dest.ChecklistSubDiscipline, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.SubDiscipline : null)
                .Map(dest => dest.ChecklistPageNumber, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? (int?)src.ChecklistSection.Checklist.PageNumber : null)
                .Map(dest => dest.ChecklistWIRCode, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.WIRCode : null)
                .Map(dest => dest.ChecklistReferenceDocuments, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.ReferenceDocuments : null)
                .Map(dest => dest.ChecklistSignatureRoles, src => src.ChecklistSection != null && src.ChecklistSection.Checklist != null ? src.ChecklistSection.Checklist.SignatureRoles : null);

        }
    }

}
