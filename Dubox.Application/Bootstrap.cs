using Dubox.Application.Features.Auth.Commands;
using Dubox.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dubox.Application
{
    public static class Bootstrap
    {
        public static IServiceCollection AddApplicationStrapping(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddScoped<IProjectProgressService, ProjectProgressService>();
            return services;
        }
    }
}
