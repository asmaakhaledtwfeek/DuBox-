using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Activities.Queries;

public record GetBoxActivitiesByBoxQuery(Guid BoxId) : IRequest<Result<List<BoxActivityDto>>>;

