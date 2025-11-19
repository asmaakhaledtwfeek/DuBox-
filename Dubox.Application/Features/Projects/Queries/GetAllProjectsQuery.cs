using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public record GetAllProjectsQuery(
    string? SearchTerm,
    int? StatusFilter
) : IRequest<Result<List<ProjectDto>>>;

