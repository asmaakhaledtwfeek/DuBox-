using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Navigation.Queries;

public class GetNavigationMenuItemsQueryHandler : IRequestHandler<GetNavigationMenuItemsQuery, Result<List<NavigationMenuItemDto>>>
{
    private readonly IDbContext _context;

    public GetNavigationMenuItemsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<NavigationMenuItemDto>>> Handle(GetNavigationMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var menuItems = await _context.NavigationMenuItems
            .Where(m => m.IsActive && m.IsVisible && m.ParentMenuItemId == null)
            .OrderBy(m => m.DisplayOrder)
            .Include(m => m.Children.Where(c => c.IsActive && c.IsVisible))
            .ToListAsync(cancellationToken);

        var result = menuItems.Select(m => new NavigationMenuItemDto(
            m.MenuItemId,
            m.Label,
            m.Icon,
            m.Route,
            string.IsNullOrEmpty(m.Aliases) ? null : m.Aliases.Split(',', StringSplitOptions.RemoveEmptyEntries),
            m.PermissionModule,
            m.PermissionAction,
            m.DisplayOrder,
            m.IsActive,
            m.Children.Any() ? m.Children
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new NavigationMenuItemDto(
                    c.MenuItemId,
                    c.Label,
                    c.Icon,
                    c.Route,
                    string.IsNullOrEmpty(c.Aliases) ? null : c.Aliases.Split(',', StringSplitOptions.RemoveEmptyEntries),
                    c.PermissionModule,
                    c.PermissionAction,
                    c.DisplayOrder,
                    c.IsActive,
                    null
                )).ToList() : null
        )).ToList();

        return Result.Success(result);
    }
}

