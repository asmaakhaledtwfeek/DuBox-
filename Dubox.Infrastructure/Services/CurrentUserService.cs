using Dubox.Domain.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Dubox.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrWhiteSpace(id) ? null : id;
            }
        }

        public string? Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                return string.IsNullOrWhiteSpace(role) ? null : role;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
