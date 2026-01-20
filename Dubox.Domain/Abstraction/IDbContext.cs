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
    
    // Permissions
    DbSet<Permission> Permissions { get; }
    DbSet<RolePermission> RolePermissions { get; }
    
    // Navigation
    DbSet<NavigationMenuItem> NavigationMenuItems { get; }

    // DuBox tracking system - Core entities
    DbSet<Project> Projects { get; }
    DbSet<Box> Boxes { get; }
    DbSet<BoxAsset> BoxAssets { get; }
    DbSet<BoxPanel> BoxPanels { get; }
    DbSet<ActivityMaster> ActivityMasters { get; }
    DbSet<BoxActivity> BoxActivities { get; }
    DbSet<ActivityDependency> ActivityDependencies { get; }
    DbSet<ProgressUpdate> ProgressUpdates { get; }
    DbSet<ProgressUpdateImage> ProgressUpdateImages { get; }
    DbSet<WIRRecord> WIRRecords { get; }
    DbSet<WIRCheckpoint> WIRCheckpoints { get; }
    DbSet<WIRChecklistItem> WIRChecklistItems { get; }
    DbSet<WIRCheckpointImage> WIRCheckpointImages { get; }
    DbSet<PredefinedChecklistItem> PredefinedChecklistItems { get; }
    
    // Production tracking
    DbSet<DailyProductionLog> DailyProductionLogs { get; }
    DbSet<QualityIssue> QualityIssues { get; }
    DbSet<QualityIssueImage> QualityIssueImages { get; }
    
    // Team management
    DbSet<Team> Teams { get; }
    DbSet<TeamMember> TeamMembers { get; }
    
    // Material management
    DbSet<Material> Materials { get; }
    DbSet<BoxMaterial> BoxMaterials { get; }
    DbSet<MaterialTransaction> MaterialTransactions { get; }
    
    // Factory layout
    DbSet<Factory> Factories { get; }
    DbSet<FactoryLocation> FactoryLocations { get; }
    DbSet<BoxLocationHistory> BoxLocationHistory { get; }
    
    // Cost tracking
    DbSet<CostCategory> CostCategories { get; }
    DbSet<BoxCost> BoxCosts { get; }
    
    // Cost Management
    DbSet<CostCodeMaster> CostCodes { get; }
    DbSet<ProjectCostItem> ProjectCostItems { get; }
    DbSet<HRCostRecord> HRCostRecords { get; }
    DbSet<ProjectCost> ProjectCosts { get; }
    
    // Schedule Activities (New Module)
    DbSet<ScheduleActivity> ScheduleActivities { get; }
    DbSet<ScheduleActivityTeam> ScheduleActivityTeams { get; }
    DbSet<ScheduleActivityMaterial> ScheduleActivityMaterials { get; }
    
    // BIM Models (New Module)
    DbSet<BIMModel> BIMModels { get; }
    
    // Risk & notifications
    DbSet<Risk> Risks { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<AuditLog> AuditLogs { get; }
    
    // Checklist Sections
    DbSet<ChecklistSection> ChecklistSections { get; }
    DbSet<Checklist> Checklists { get; }
    
    // Project Configuration
    DbSet<ProjectBuilding> ProjectBuildings { get; }
    DbSet<ProjectLevel> ProjectLevels { get; }
    DbSet<ProjectBoxType> ProjectBoxTypes { get; }
    DbSet<ProjectBoxSubType> ProjectBoxSubTypes { get; }
    DbSet<ProjectZone> ProjectZones { get; }
    DbSet<ProjectBoxFunction> ProjectBoxFunctions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    // Allow generic access to DbSet for repository pattern
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

