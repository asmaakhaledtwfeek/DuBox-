using Dubox.Domain.Entities;
using Dubox.Domain.Interfaces;
using Dubox.Infrastructure.Abstraction;
using Dubox.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.ApplicationContext
{
    public sealed class ApplicationDbContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Apply all configurations from assembly
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Additional custom configurations
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
            ConfigureDefaultValues(modelBuilder);
            ActivityMasterSeedData.SeedActivityMaster(modelBuilder);
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
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Additional composite indexes for performance
            modelBuilder.Entity<BoxActivity>()
                .HasIndex(ba => new { ba.BoxId, ba.Status });

            modelBuilder.Entity<ProgressUpdate>()
                .HasIndex(pu => new { pu.BoxId, pu.UpdateDate });

            modelBuilder.Entity<DailyProductionLog>()
                .HasIndex(dpl => new { dpl.LogDate, dpl.TeamId });
        }

        private void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            // Set default values using HasDefaultValue
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasDefaultValue("Active");

            modelBuilder.Entity<Box>()
                .Property(b => b.CurrentStatus)
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
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Auto-populate audit fields
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

            // Optional: Create audit log entries
            await CreateAuditLog(cancellationToken);

            return result;
        }

        private async Task CreateAuditLog(CancellationToken cancellationToken)
        {
            // Get all modified entries
            var auditEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                           e.State == EntityState.Modified ||
                           e.State == EntityState.Deleted)
                .Select(e => new AuditLog
                {
                    TableName = e.Entity.GetType().Name,
                    Action = e.State.ToString(),
                    ChangedBy = _currentUserService.Username,
                    ChangedDate = _dateTime.Now,
                    // Serialize old and new values as needed
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
