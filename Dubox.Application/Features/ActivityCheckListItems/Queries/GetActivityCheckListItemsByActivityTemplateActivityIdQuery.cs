using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public record GetActivityCheckListItemsByActivityTemplateActivityIdQuery(Guid ActivityTemplateActivityId) 
    : IRequest<Result<List<ActivityCheckListItemDetailsDto>>>;
