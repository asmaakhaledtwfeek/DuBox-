using Dubox.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Services
{
    public interface IPermissionService
    {
         Task<bool> UserHasPermissionAsync(Guid userId, PermissionModuleEnum module, PermissionActionEnum action,CancellationToken cancellationToken);
    }
}
