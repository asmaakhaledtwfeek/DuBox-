using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public record GetAllProjectsQuery : IRequest<Result<List<ProjectDto>>>;

