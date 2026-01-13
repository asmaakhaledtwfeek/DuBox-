using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Navigation.Queries;

public record GetNavigationMenuItemsQuery : IRequest<Result<List<NavigationMenuItemDto>>>;

