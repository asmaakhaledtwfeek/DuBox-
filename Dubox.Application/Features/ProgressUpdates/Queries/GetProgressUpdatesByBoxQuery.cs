using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ProgressUpdates.Queries;

public record GetProgressUpdatesByBoxQuery(Guid BoxId) : IRequest<Result<PaginatedProgressUpdatesResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? ActivityName { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}

