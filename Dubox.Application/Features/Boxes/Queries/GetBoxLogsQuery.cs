using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GetBoxLogsQuery(
    Guid BoxId,
    int PageNumber = 1,
    int PageSize = 25,
    string? SearchTerm = null,
    string? Action = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IRequest<Result<PaginatedBoxLogsResponseDto>>;

public record PaginatedBoxLogsResponseDto
{
    public List<BoxLogDto> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

