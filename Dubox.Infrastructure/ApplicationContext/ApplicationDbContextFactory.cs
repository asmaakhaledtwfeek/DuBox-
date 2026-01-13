using Dubox.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dubox.Infrastructure.ApplicationContext;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var apiPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Dubox.Api");
        
        if (Directory.Exists(apiPath))
        {
            basePath = apiPath;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        var connectionString = configuration.GetConnectionString("Database") 
            ?? "Server=.;Database=Dubox;Trusted_Connection=True;TrustServerCertificate=True;";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(
            optionsBuilder.Options,
            new DesignTimeDateTime(),
            new DesignTimeCurrentUserService());
    }

    private class DesignTimeDateTime : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }

    private class DesignTimeCurrentUserService : ICurrentUserService
    {
        public string? Username => "System";
        public string? UserId => "00000000-0000-0000-0000-000000000000";
        public string? Role => "SystemAdmin";
        public IEnumerable<string> Roles => new[] { "SystemAdmin" };
        public bool IsAuthenticated => false;

        public Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<string>>(new[] { "SystemAdmin" });
        }

        public Task<bool> HasRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(roleName == "SystemAdmin");
        }

        public Task<bool> HasAnyRoleAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(roleNames.Contains("SystemAdmin"));
        }
    }
}

