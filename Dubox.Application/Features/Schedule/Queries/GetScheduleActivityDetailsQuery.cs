using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Schedule.Queries;

public record GetScheduleActivityDetailsQuery(Guid ScheduleActivityId) : IRequest<Result<ScheduleActivityDto>>;




