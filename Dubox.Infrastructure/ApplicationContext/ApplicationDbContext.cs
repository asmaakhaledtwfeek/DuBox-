using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.ApplicationContext;

public sealed class ApplicationDbContext : DbContext, IDbContext
{
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTime dateTime,
        ICurrentUserService currentUserService) : base(options)
    {
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Box> Boxes { get; set; } = null!;
    public DbSet<BoxAsset> BoxAssets { get; set; } = null!;
    public DbSet<ActivityMaster> ActivityMasters { get; set; } = null!;
    public DbSet<BoxActivity> BoxActivities { get; set; } = null!;
    public DbSet<ActivityDependency> ActivityDependencies { get; set; } = null!;
    public DbSet<ProgressUpdate> ProgressUpdates { get; set; } = null!;
    public DbSet<WIRRecord> WIRRecords { get; set; } = null!;
    public DbSet<DailyProductionLog> DailyProductionLogs { get; set; } = null!;
    public DbSet<WIRCheckpoint> WIRCheckpoints { get; set; } = null!;
    public DbSet<WIRChecklistItem> WIRChecklistItems { get; set; } = null!;
    public DbSet<QualityIssue> QualityIssues { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TeamMember> TeamMembers { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<BoxMaterial> BoxMaterials { get; set; } = null!;
    public DbSet<MaterialTransaction> MaterialTransactions { get; set; } = null!;
    public DbSet<FactoryLocation> FactoryLocations { get; set; } = null!;
    public DbSet<BoxLocationHistory> BoxLocationHistory { get; set; } = null!;
    public DbSet<CostCategory> CostCategories { get; set; } = null!;
    public DbSet<BoxCost> BoxCosts { get; set; } = null!;
    public DbSet<Risk> Risks { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    public DbSet<GroupRole> GroupRoles { get; set; } = null!;
    public DbSet<Department> Departments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureRelationships(modelBuilder);
        ConfigureIndexes(modelBuilder);
        ConfigureDefaultValues(modelBuilder);
        ActivityMasterSeedData.SeedActivityMaster(modelBuilder);
        RoleAndUserSeedData.SeedRolesGroupsAndUsers(modelBuilder);
        DepartmentSeesData.SeedDepartmnts(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Activity Dependencies - Configure self-referencing relationship
        modelBuilder.Entity<ActivityDependency>()
            .HasOne(d => d.BoxActivity)
            .WithMany(a => a.Dependencies)
            .HasForeignKey(d => d.BoxActivityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ActivityDependency>()
            .HasOne(d => d.PredecessorActivity)
            .WithMany(a => a.DependentActivities)
            .HasForeignKey(d => d.PredecessorActivityId)
            .OnDelete(DeleteBehavior.Restrict);
        {
            // ActivityDependency - Self-referencing BoxActivity
            // Must use Restrict on both to avoid multiple cascade paths
            modelBuilder.Entity<ActivityDependency>()
                .HasOne(ad => ad.BoxActivity)
                .WithMany()
                .HasForeignKey(ad => ad.BoxActivityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityDependency>()
                .HasOne(ad => ad.PredecessorActivity)
                .WithMany()
                .HasForeignKey(ad => ad.PredecessorActivityId)
                .OnDelete(DeleteBehavior.Restrict);

        // Cost Categories - Self-referencing for parent-child
        modelBuilder.Entity<CostCategory>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Box Location History - Multiple FKs to FactoryLocation
        modelBuilder.Entity<BoxLocationHistory>()
            .HasOne(h => h.Location)
            .WithMany(l => l.BoxLocationHistory)
            .HasForeignKey(h => h.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BoxLocationHistory>()
            .HasOne(h => h.MovedFromLocation)
            .WithMany()
            .HasForeignKey(h => h.MovedFromLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupRole>()
            .HasOne(gr => gr.Group)
            .WithMany(g => g.GroupRoles)
            .HasForeignKey(gr => gr.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupRole>()
            .HasOne(gr => gr.Role)
            .WithMany(r => r.GroupRoles)
            .HasForeignKey(gr => gr.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
        .HasOne(u => u.EmployeeOfDepartment)
        .WithMany(d => d.Employees)
        .HasForeignKey(u => u.DepartmentId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Department>()
            .HasOne(d => d.Manager)
            .WithOne(u => u.ManagedDepartment)
            .HasForeignKey<Department>(d => d.ManagerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<GroupRole>()
                .HasOne(gr => gr.Role)
                .WithMany(r => r.GroupRoles)
                .HasForeignKey(gr => gr.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WIRRecord>()
                .HasOne(w => w.BoxActivity)
                .WithMany(ba => ba.WIRRecords)
                .HasForeignKey(w => w.BoxActivityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WIRRecord>()
                .HasOne(w => w.RequestedByUser)
                .WithMany()
                .HasForeignKey(w => w.RequestedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WIRRecord>()
                .HasOne(w => w.InspectedByUser)
                .WithMany()
                .HasForeignKey(w => w.InspectedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ProgressUpdate relationships
            modelBuilder.Entity<ProgressUpdate>()
                .HasOne(p => p.Box)
                .WithMany(b => b.ProgressUpdates)
                .HasForeignKey(p => p.BoxId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProgressUpdate>()
                .HasOne(p => p.BoxActivity)
                .WithMany(ba => ba.ProgressUpdates)
                .HasForeignKey(p => p.BoxActivityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProgressUpdate>()
                .HasOne(p => p.UpdatedByUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // BoxActivity relationships
            modelBuilder.Entity<BoxActivity>()
                .HasOne(ba => ba.Box)
                .WithMany(b => b.BoxActivities)
                .HasForeignKey(ba => ba.BoxId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoxActivity>()
                .HasOne(ba => ba.ActivityMaster)
                .WithMany(am => am.BoxActivities)
                .HasForeignKey(ba => ba.ActivityMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoxActivity>()
                .HasOne(ba => ba.AssignedUser)
                .WithMany()
                .HasForeignKey(ba => ba.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Box relationships
            modelBuilder.Entity<Box>()
                .HasOne(b => b.Project)
                .WithMany(p => p.Boxes)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // BoxAsset relationships
            modelBuilder.Entity<BoxAsset>()
                .HasOne(ba => ba.Box)
                .WithMany(b => b.BoxAssets)
                .HasForeignKey(ba => ba.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // Project indexes
        modelBuilder.Entity<Project>()
                .HasIndex(p => p.ProjectCode)
                .IsUnique();

            // Box indexes
            modelBuilder.Entity<Box>()
                .HasIndex(b => new { b.ProjectId, b.BoxTag })
                .IsUnique();

            modelBuilder.Entity<Box>()
                .HasIndex(b => b.QRCodeString)
                .IsUnique();

            modelBuilder.Entity<Box>()
                .HasIndex(b => new { b.Status, b.ProjectId });

            // BoxActivity indexes
            modelBuilder.Entity<BoxActivity>()
                .HasIndex(ba => new { ba.BoxId, ba.Status });

        modelBuilder.Entity<ProgressUpdate>()
            .HasIndex(pu => new { pu.BoxId, pu.UpdateDate });
            modelBuilder.Entity<BoxActivity>()
                .HasIndex(ba => new { ba.BoxId, ba.Sequence })
                .IsUnique();

            // ActivityMaster indexes
            modelBuilder.Entity<ActivityMaster>()
                .HasIndex(am => am.ActivityCode)
                .IsUnique();

            modelBuilder.Entity<ActivityMaster>()
                .HasIndex(am => new { am.StageNumber, am.SequenceInStage });

            // WIRRecord indexes
            modelBuilder.Entity<WIRRecord>()
                .HasIndex(w => new { w.BoxActivityId, w.WIRCode });

            modelBuilder.Entity<WIRRecord>()
                .HasIndex(w => new { w.Status, w.RequestedDate });

            modelBuilder.Entity<ProgressUpdate>()
                .HasIndex(pu => new { pu.BoxId, pu.UpdateDate });

        modelBuilder.Entity<DailyProductionLog>()
            .HasIndex(dpl => new { dpl.LogDate, dpl.TeamId });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<Group>()
            .HasIndex(g => g.GroupName)
            .IsUnique();

        modelBuilder.Entity<UserRole>()
            .HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        modelBuilder.Entity<UserGroup>()
            .HasIndex(ug => new { ug.UserId, ug.GroupId })
            .IsUnique();

        modelBuilder.Entity<GroupRole>()
            .HasIndex(gr => new { gr.GroupId, gr.RoleId })
            .IsUnique();
    }

    private void ConfigureDefaultValues(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasDefaultValue("Active");

            modelBuilder.Entity<Box>()
                .Property(b => b.Status)
                .HasDefaultValue("Not Started");

        modelBuilder.Entity<Box>()
            .Property(b => b.ProgressPercentage)
            .HasDefaultValue(0);

        modelBuilder.Entity<BoxActivity>()
            .Property(ba => ba.Status)
            .HasDefaultValue("Not Started");

        modelBuilder.Entity<BoxActivity>()
            .Property(ba => ba.ProgressPercentage)
            .HasDefaultValue(0);
        modelBuilder.Entity<ActivityMaster>()
            .Property(a => a.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<Department>()
            .Property(a => a.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<Department>()
            .Property(a => a.UpdatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.Username;
                    entry.Entity.CreatedDate = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _currentUserService.Username;
                    entry.Entity.ModifiedDate = _dateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        await CreateAuditLog(cancellationToken);
        return result;
    }

    private async Task CreateAuditLog(CancellationToken cancellationToken)
    {
        var auditEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                           e.State == EntityState.Modified ||
                           e.State == EntityState.Deleted)
                .Select(e => new AuditLog
                {
                    TableName = e.Entity.GetType().Name,
                    Action = e.State.ToString(),
                    ChangedBy = _currentUserService.Username,
                    ChangedDate = _dateTime.Now
                })
                .ToList();

        if (auditEntries.Any())
        {
            await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
            await base.SaveChangesAsync(cancellationToken);
        }
    }
}
}

