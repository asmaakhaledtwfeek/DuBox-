
using Dubox.Api.Configurations;
using Dubox.Api.Middlewares;
using Dubox.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // Register IHttpClientFactory
builder.Services.AddSignalR(); // Add SignalR
builder.Services.AddAppServicesDIConfig();
builder.Services.AddMapsterConfig();
Dubox.Infrastructure.Bootstrap.AddInfrastructureStrapping(builder.Services);
Dubox.Application.Bootstrap.AddApplicationStrapping(builder.Services);

// Add Automatic Data Seeding on Startup
builder.Services.AddHostedService<Dubox.Api.Services.DataSeederHostedService>();


builder.Services.AddMediatR(cfg =>
{

    cfg.RegisterServicesFromAssembly(Dubox.Application.AssemblyReference.Assembly);
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});



builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(Dubox.Application.AssemblyReference.Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddValidatorsFromAssembly(
    Dubox.Application.AssemblyReference.Assembly,
    includeInternalTypes: true);

builder.Services.AddDbConfig(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use camelCase for JSON property names
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        // Preserve property names for reference types
        options.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        // Write indented JSON for easier debugging
        options.JsonSerializerOptions.WriteIndented = true;
        // Serialize and deserialize enums using their string names
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
              "http://localhost:4200",
              "http://localhost:51091",
              "https://localhost:7158",
              "https://dubox-frontend-gjgbcgbrb8d3fra2.uaenorth-01.azurewebsites.net"
            )
              .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // only if using cookies or auth headers
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DuBox API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddJwtConfig();

builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Auto-apply database migrations on startup
await app.ApplyDatabaseMigrationsAsync();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

// CORS must be before Authentication/Authorization
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

// Map SignalR hub
app.MapHub<Dubox.Infrastructure.Hubs.NotificationHub>("/hubs/notifications");

app.Run();

