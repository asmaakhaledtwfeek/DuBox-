using MediatR;
using Scrutor;

namespace Dubox.Api.Configurations;

public static class AppServicesDIConfig
{
    public static IServiceCollection AddAppServicesDIConfig(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly)
            .AddClasses(classes => classes
                .Where(type => !type.Name.EndsWith("Command") 
                            && !type.Name.EndsWith("Query")
                            && !type.Name.EndsWith("CommandHandler")
                            && !type.Name.EndsWith("QueryHandler")
                            && !type.IsAssignableTo(typeof(IBaseRequest))))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}