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
    public static IServiceCollection AddInfrastructureStrapping(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped(typeof(IDateTime), typeof(DateTimeService));
        services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));
        services.AddScoped<IProjectTeamVisibilityService, ProjectTeamVisibilityService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IQRCodeService, QRCodeService>();
        services.AddScoped<IBoxActivityService, BoxActivityService>();
        services.AddScoped<ISerialNumberService, SerialNumberService>();
        services.AddScoped<IImageProcessingService, ImageProcessingService>();


        return services;
    }
}
