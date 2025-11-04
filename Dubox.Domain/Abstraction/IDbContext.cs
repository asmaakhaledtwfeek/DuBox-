using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Domain.Abstraction;

public interface IDbContext
{
    // User management
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Group> Groups { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserGroup> UserGroups { get; }
    DbSet<GroupRole> GroupRoles { get; }

    // DuBox tracking system - Core entities
    DbSet<Project> Projects { get; }
    DbSet<Box> Boxes { get; }
    DbSet<BoxAsset> BoxAssets { get; }
    DbSet<ActivityMaster> ActivityMasters { get; }
    DbSet<BoxActivity> BoxActivities { get; }
    DbSet<ActivityDependency> ActivityDependencies { get; }
    DbSet<ProgressUpdate> ProgressUpdates { get; }
    DbSet<WIRRecord> WIRRecords { get; }
    DbSet<WIRCheckpoint> WIRCheckpoints { get; }
    DbSet<WIRChecklistItem> WIRChecklistItems { get; }
    
    // Production tracking
    DbSet<DailyProductionLog> DailyProductionLogs { get; }
    DbSet<QualityIssue> QualityIssues { get; }
    
    // Team management
    DbSet<Team> Teams { get; }
    DbSet<TeamMember> TeamMembers { get; }
    
    // Material management
    DbSet<Material> Materials { get; }
    DbSet<BoxMaterial> BoxMaterials { get; }
    DbSet<MaterialTransaction> MaterialTransactions { get; }
    
    // Factory layout
    DbSet<FactoryLocation> FactoryLocations { get; }
    DbSet<BoxLocationHistory> BoxLocationHistory { get; }
    
    // Cost tracking
    DbSet<CostCategory> CostCategories { get; }
    DbSet<BoxCost> BoxCosts { get; }
    
    // Risk & notifications
    DbSet<Risk> Risks { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<AuditLog> AuditLogs { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Allow generic access to DbSet for repository pattern
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

