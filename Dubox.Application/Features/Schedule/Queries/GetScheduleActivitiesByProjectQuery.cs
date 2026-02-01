using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Queries;

public record GetScheduleActivitiesByProjectQuery(Guid ProjectId) : IRequest<Result<List<ScheduleActivityListDto>>>;
