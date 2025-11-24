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
            public string? Role => "System";
        public bool IsAuthenticated => false;
    }
}

