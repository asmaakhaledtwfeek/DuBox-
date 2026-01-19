using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Users.Queries;

public record GetProjectManagersQuery : IRequest<Result<List<ProjectManagerDto>>>;

public record ProjectManagerDto
{
    public Guid UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

