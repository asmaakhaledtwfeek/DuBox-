using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Dubox.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        public string? Username
        {
            get
            {
                var name = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                return string.IsNullOrWhiteSpace(name) ? null : name;
            }
        }

        public string? UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrWhiteSpace(id) ? null : id;
            }
        }

        public string? Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.Role)?.Value;
                return string.IsNullOrWhiteSpace(role) ? null : role;
            }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindAll(ClaimTypes.Role)
                    .Select(c => c.Value) ?? Enumerable.Empty<string>();
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public async Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken cancellationToken = default)
        {
            if (!IsAuthenticated || string.IsNullOrEmpty(UserId) || !Guid.TryParse(UserId, out var userId))
            {
                return Enumerable.Empty<string>();
            }

            // Lazy-load the UserRoleService to avoid circular dependencies during authentication
            using var scope = _serviceProvider.CreateScope();
            var userRoleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();
            return await userRoleService.GetUserRolesAsync(userId, cancellationToken);
        }

        public async Task<bool> HasRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            if (!IsAuthenticated || string.IsNullOrEmpty(UserId) || !Guid.TryParse(UserId, out var userId))
            {
                return false;
            }

            // Lazy-load the UserRoleService to avoid circular dependencies during authentication
            using var scope = _serviceProvider.CreateScope();
            var userRoleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();
            return await userRoleService.UserHasRoleAsync(userId, roleName, cancellationToken);
        }

        public async Task<bool> HasAnyRoleAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
        {
            if (!IsAuthenticated || string.IsNullOrEmpty(UserId) || !Guid.TryParse(UserId, out var userId))
            {
                return false;
            }

            // Lazy-load the UserRoleService to avoid circular dependencies during authentication
            using var scope = _serviceProvider.CreateScope();
            var userRoleService = scope.ServiceProvider.GetRequiredService<IUserRoleService>();
            return await userRoleService.UserHasAnyRoleAsync(userId, roleNames, cancellationToken);
        }
    }
}
