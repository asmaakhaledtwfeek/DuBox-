using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Mapster;
namespace Dubox.Application.Features.QualityIssues.MappingConfig
{

    public class QualityIssueMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QualityIssue, QualityIssueDetailsDto>()
                //.Map(dest => dest.IssueId, src => src.IssueId)
                //.Map(dest => dest.IssueDate, src => src.IssueDate)
                //.Map(dest => dest.IssueType, src => src.IssueType)
                //.Map(dest => dest.Severity, src => src.Severity)
                //.Map(dest => dest.IssueDescription, src => src.IssueDescription)
                //.Map(dest => dest.ReportedBy, src => src.ReportedBy)
                //.Map(dest => dest.AssignedTo, src => src.AssignedTo)
                //.Map(dest => dest.DueDate, src => src.DueDate)
                //.Map(dest => dest.Status, src => src.Status)
                //.Map(dest => dest.ResolutionDate, src => src.ResolutionDate)
                //.Map(dest => dest.ResolutionDescription, src => src.ResolutionDescription)
                //.Map(dest => dest.PhotoPath, src => src.PhotoPath)
                //.Map(dest => dest.IsOverdue, src => src.IsOverdue)
                //.Map(dest => dest.OverdueDays, src => src.OverdueDays)

                .Map(dest => dest.BoxId, src => src.Box.BoxId)
                .Map(dest => dest.BoxName, src => src.Box.BoxName)
                .Map(dest => dest.BoxTag, src => src.Box.BoxTag)
                .Map(dest => dest.ProjectName, src => src.Box.Project.ProjectName)
.Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                .Map(dest => dest.ProjectName, src => src.Box.Project.ProjectName)
                .Map(dest => dest.WIRId, src => src.WIRCheckpoint != null ? (Guid?)src.WIRCheckpoint.WIRId : null)
                .Map(dest => dest.WIRNumber, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRCode : null)
                .Map(dest => dest.WIRName, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.WIRName : null)
                .Map(dest => dest.WIRStatus, src => src.WIRCheckpoint != null ? (WIRCheckpointStatusEnum?)src.WIRCheckpoint.Status : null)
                .Map(dest => dest.WIRRequestedDate, src => src.WIRCheckpoint != null ? src.WIRCheckpoint.RequestedDate : null);
        }
    }

}
