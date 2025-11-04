using Scrutor;

namespace Dubox.Api.Configurations
{
    public static class AppServicesDIConfig
    {
        public static IServiceCollection AddAppServicesDIConfig(this IServiceCollection services)
        {
            services
                .Scan(
                    selector => selector
                        .FromAssemblies(
                            Infrastructure.AssemblyReference.Assembly,
                            Application.AssemblyReference.Assembly)
                        .AddClasses(false)
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            return services;
        }
    }
}