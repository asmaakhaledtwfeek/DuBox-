using Dubox.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Api.Configurations
{
    public static class DbConfig
    {
        public static IServiceCollection AddDbConfig(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Database")!;


            services.AddDbContext<ApplicationDbContext>(
                (sp, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlServer(connectionString);
                });

            return services;
        }

    }
}
