using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

/// <summary>
/// Query to get all checklist items for a specific BoxActivity with their review status
/// </summary>
public record GetActivityChecklistByBoxActivityIdQuery(Guid BoxActivityId) 
    : IRequest<Result<GetActivityChecklistByBoxActivityDto>>;





