using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Infrastructure.Services
{
    public class PermissionService:IPermissionService
    {
        private readonly ApplicationDbContext _dbContext;

        public PermissionService( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<bool> UserHasPermissionAsync(Guid userId, PermissionModuleEnum module, PermissionActionEnum action,CancellationToken cancellationToken)
        {
           
            return await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.Module == module.ToString() && rp.Permission.Action == action.ToString(), cancellationToken);
        }

    }
}
