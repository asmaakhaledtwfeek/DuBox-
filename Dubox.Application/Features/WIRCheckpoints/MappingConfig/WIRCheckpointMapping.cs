namespace Dubox.Application.Features.WIRCheckpoints.MappingConfig
{
    using Dubox.Application.DTOs;
    using Dubox.Domain.Entities;
    using Dubox.Domain.Enums;
    using Mapster;

    internal class MapsterConfig : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<WIRChecklistItem, WIRChecklistItemDto>.NewConfig()
                .Map(dest => dest.ItemName, src => src.CheckpointDescription)
                .Map(dest => dest.IsChecked, src => src.Status != CheckListItemStatusEnum.Pending && src.Status == CheckListItemStatusEnum.Pass)
                .Map(dest => dest.Comments, src => src.Remarks);

            TypeAdapterConfig<QualityIssue, QualityIssueDto>.NewConfig()
                .Map(dest => dest.IssueTitle, src => src.IssueType.ToString())
                .Map(dest => dest.IssueDescription, src => src.IssueDescription)
                .Map(dest => dest.Severity, src => src.Severity)
                .Map(dest => dest.CreatedDate, src => src.IssueDate);

            TypeAdapterConfig<WIRCheckpoint, WIRCheckpointDto>.NewConfig()
                .Map(dest => dest.ChecklistItems, src => src.ChecklistItems)
                .Map(dest => dest.QualityIssues, src => src.QualityIssues)
                .Map(dest => dest.PendingDays, src => src.PendingDays)
                .Map(dest => dest.IsOverdue, src => src.IsOverdue)
                .Map(dest => dest.Box, src => src.Box)
                .Map(dest => dest.BoxName, src => src.Box != null ? src.Box.BoxName : string.Empty)
                .Map(dest => dest.BoxTag, src => src.Box != null ? src.Box.BoxTag : string.Empty);
        }
    }

}
