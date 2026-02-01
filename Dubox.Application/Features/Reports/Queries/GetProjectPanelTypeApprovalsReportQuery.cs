using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries
{
    public record GetProjectPanelTypeApprovalsReportQuery : IRequest<Result<List<ProjectPanelTypeApprovalReportDto>>>
    {
        public Guid? ProjectId { get; init; }
        public Guid? FactoryId { get; init; }
        public string? PanelType { get; init; }
        public string? Search { get; init; }
    }
}

