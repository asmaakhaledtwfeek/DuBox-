using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries
{
    public record GetBoxPanelApprovalsReportQuery : IRequest<Result<List<BoxPanelApprovalReportDto>>>
    {
        public Guid? ProjectId { get; init; }
        public Guid? FactoryId { get; init; }
        public string? ApprovalStatus { get; init; } // all, approved, pending
        public string? Search { get; init; }
    }
}

