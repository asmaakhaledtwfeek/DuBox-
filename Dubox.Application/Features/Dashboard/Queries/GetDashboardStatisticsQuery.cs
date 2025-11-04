using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Dashboard.Queries;

public record GetDashboardStatisticsQuery : IRequest<Result<DashboardStatisticsDto>>;

