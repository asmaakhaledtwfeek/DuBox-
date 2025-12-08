using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Reports.Queries
{
    public record GetBoxesSummaryReportQuery : IRequest<Result<PaginatedBoxSummaryReportResponseDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 25;
        public string? SortBy { get; init; }
        public string? SortDir { get; init; }
        public Guid? ProjectId { get; init; }
        public List<string>? BoxType { get; init; }
        public string? Floor { get; init; }
        public string? Building { get; init; }
        public string? Zone { get; init; }
        public List<int>? Status { get; init; }
        public decimal? ProgressMin { get; init; }
        public decimal? ProgressMax { get; init; }
        public string? Search { get; init; }
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
        public string? DateFilterType { get; init; }
    }
}
