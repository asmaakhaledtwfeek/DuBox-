using Dubox.Infrastructure.Abstraction;
using Dubox.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Infrastructure
{
    public static class Bootstrap
    {
        public static IServiceCollection AddPersistenceStrapping(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
