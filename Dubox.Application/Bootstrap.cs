using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Application.Features.Auth.Commands;
using Dubox.Application.Features.Groups.Commands;
using Dubox.Application.Features.Projects.Processors;
using Dubox.Application.Features.Roles.Commands;
using Dubox.Application.Features.Users.Commands;
using Dubox.Application.Services;
using Dubox.Domain.Entities;
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
            services.AddScoped<IMaterialTemplateApplicationService, MaterialTemplateApplicationService>();
            services.AddScoped<IBoxTypeTemplateAutoAssignmentService, BoxTypeTemplateAutoAssignmentService>();
            services.AddScoped<IBoxMaterialDeliveryService, BoxMaterialDeliveryService>();
            services.AddScoped<IQualityIssueMappingService, QualityIssueMappingService>();

            // Register Configuration Processors
            services.AddScoped<IConfigurationProcessor<ProjectBuilding, ProjectBuildingDto>, BuildingConfigurationProcessor>();
            services.AddScoped<IConfigurationProcessor<ProjectLevel, ProjectLevelDto>, LevelConfigurationProcessor>();
            services.AddScoped<IConfigurationProcessor<ProjectBoxType, ProjectBoxTypeDto>, BoxTypeConfigurationProcessor>();
            services.AddScoped<IConfigurationProcessor<ProjectZone, ProjectZoneDto>, ZoneConfigurationProcessor>();
            services.AddScoped<IConfigurationProcessor<ProjectBoxFunction, ProjectBoxFunctionDto>, BoxFunctionConfigurationProcessor>();

            return services;
        }
    }
}
