using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Queries;

public record GetActivityCheckListItemsByActivityMasterIdQuery(Guid ActivityMasterId) : IRequest<Result<List<ActivityCheckListItemDetailsDto>>>;
