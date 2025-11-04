using Dubox.Application.Abstractions;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Dubox.Infrastructure.ApplicationContext;
using Dubox.Infrastructure.Authentication;
using Dubox.Infrastructure.Repositories;
using Dubox.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddPersistenceStrapping(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
