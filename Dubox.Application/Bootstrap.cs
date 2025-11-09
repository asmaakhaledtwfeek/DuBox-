using Dubox.Application.Features.Auth.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Application
{
    public static class Bootstrap
    {
        public static IServiceCollection AddApplicationStrapping(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
            return services;
        }
    }
}
