
using Dubox.Api.Configurations;
using Dubox.Api.Middlewares;
using Dubox.Application.Behaviors;
using Dubox.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

// Configure Serilog for early initialization logging (catches startup errors)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
    .MinimumLevel.Override("System", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/dubox-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30,
        fileSizeLimitBytes: 10485760, // 10MB
        shared: true)
    .CreateLogger();

try
{
    Log.Information("=== Starting DuBox API application ===");
    Log.Information("Environment: {Environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production");

var builder = WebApplication.CreateBuilder(args);

    Log.Information("WebApplication builder created successfully");

// Replace default logging with Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

    Log.Information("Serilog configured from appsettings");

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // Register IHttpClientFactory
builder.Services.AddMemoryCache(); // Add Memory Cache for performance optimization
builder.Services.AddSignalR(); // Add SignalR

    Log.Information("Core services registered (HttpContext, HttpClient, Cache, SignalR)");

// Add Material Notification Background Service
builder.Services.AddHostedService<Dubox.Infrastructure.Services.MaterialNotificationBackgroundService>();
    //builder.Services.AddHostedService<Dubox.Infrastructure.Services.WeatherMonitoringWorker>();
    builder.Services.AddSingleton<WeatherMonitoringWorker>();
    builder.Services.AddHostedService(provider =>
        provider.GetRequiredService<WeatherMonitoringWorker>());

    Log.Information("Configuring application services...");
builder.Services.AddAppServicesDIConfig();
    Log.Information("App services DI configured");

builder.Services.AddMapsterConfig();
    Log.Information("Mapster mapping configured");

Dubox.Infrastructure.Bootstrap.AddInfrastructureStrapping(builder.Services);
    Log.Information("Infrastructure layer configured");

Dubox.Application.Bootstrap.AddApplicationStrapping(builder.Services);
    Log.Information("Application layer configured");

// Add Automatic Data Seeding on Startup
//builder.Services.AddHostedService<Dubox.Api.Services.DataSeederHostedService>();


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

    Log.Information("Configuring database...");
builder.Services.AddDbConfig(builder.Configuration);
    Log.Information("Database configuration completed");

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

    Log.Information("Configuring CORS policy...");
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
                .AllowCredentials() // only if using cookies or auth headers
                .WithExposedHeaders("Content-Disposition"); // Expose Content-Disposition header for filename extraction
        });
});
    Log.Information("CORS policy configured");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DuBox API", Version = "v1" });

    // Add custom operation filter for file uploads
    c.OperationFilter<FileUploadOperationFilter>();

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

    Log.Information("Configuring JWT authentication...");
builder.Services.AddJwtConfig();
    Log.Information("JWT authentication configured");

builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();
    Log.Information("Exception handling configured");

builder.Services.AddHealthChecks();
    Log.Information("Health checks configured");

    Log.Information("Building application...");
var app = builder.Build();
    Log.Information("Application built successfully");

// Auto-apply database migrations on startup
    Log.Information("Applying database migrations...");
    try
    {
        await app.ApplyDatabaseMigrationsAsync();
        Log.Information("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to apply database migrations");
        throw;
    }
    Log.Information("Configuring middleware pipeline...");
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

// CORS must be before Authentication/Authorization
app.UseCors("AllowFrontend");
    Log.Information("CORS middleware applied");

app.UseHttpsRedirection();

app.UseAuthentication();
    Log.Information("Authentication middleware applied");

app.UseAuthorization();
    Log.Information("Authorization middleware applied");

app.MapHealthChecks("/health");
    Log.Information("Health check endpoint mapped");

app.MapControllers();
    Log.Information("Controllers mapped");

// Map SignalR hub with CORS policy
app.MapHub<Dubox.Infrastructure.Hubs.NotificationHub>("/hubs/notifications")
    .RequireCors("AllowFrontend");
    Log.Information("SignalR hub mapped at /hubs/notifications");

    Log.Information("=== DuBox API application configuration completed successfully ===");
    Log.Information("Starting web server...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly during startup");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

