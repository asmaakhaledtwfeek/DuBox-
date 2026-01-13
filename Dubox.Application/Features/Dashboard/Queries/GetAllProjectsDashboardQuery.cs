using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Dashboard.Queries;

public record GetAllProjectsDashboardQuery : IRequest<Result<List<ProjectDashboardDto>>>;

