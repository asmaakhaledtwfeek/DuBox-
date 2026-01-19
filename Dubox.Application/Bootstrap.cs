using Dubox.Application.Features.Auth.Commands;
using Dubox.Application.Features.Groups.Commands;
using Dubox.Application.Features.Roles.Commands;
using Dubox.Application.Features.Users.Commands;
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
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddScoped<IValidator<UpdateRoleCommand>, UpdateRoleCommandValidator>();
            services.AddScoped<IValidator<UpdateGroupCommand>, UpdateGroupCommandValidator>();
            services.AddScoped<IProjectProgressService, ProjectProgressService>();
            services.AddScoped<IBoxMapper, BoxMapper>();
            services.AddScoped<IBoxCreationService, BoxCreationService>();

            return services;
        }
    }
}
