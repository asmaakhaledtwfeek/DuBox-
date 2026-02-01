using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Queries;

public record GetActivitiesByStageQuery(int StageNumber) : IRequest<Result<List<ActivityMasterDto>>>;

