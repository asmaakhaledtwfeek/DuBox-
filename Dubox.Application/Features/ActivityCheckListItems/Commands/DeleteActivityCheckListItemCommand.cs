using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.ActivityCheckListItems.Commands;

public record DeleteActivityCheckListItemCommand(Guid ActivityCheckListItemId) : IRequest<Result>;
