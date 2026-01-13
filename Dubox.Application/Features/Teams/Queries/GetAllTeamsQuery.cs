using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Teams.Queries;

public record GetAllTeamsQuery : IRequest<Result<PaginatedTeamsResponseDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public string? Search { get; init; }
    public string? Department { get; init; }
    public string? Trade { get; init; }
    public bool? IsActive { get; init; }
}

