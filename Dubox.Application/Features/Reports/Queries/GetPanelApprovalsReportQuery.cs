using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries
{
    public record GetPanelApprovalsReportQuery : IRequest<Result<PanelApprovalReportResponseDto>>
    {
        public Guid? ProjectId { get; init; }
        public Guid? FactoryId { get; init; }
        public string? PanelType { get; init; }
        public string? ApprovalStatus { get; init; } // all, first-approval, second-approval, rejected
        public string? Search { get; init; }
    }
}

