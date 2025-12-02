using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<Result<PaginatedUsersResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

