using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedScheduleActivitiesFromExcel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Note: The actual seeding will be done in the ApplicationDbContext startup
            // This migration is here to track that seeding should occur
            // See: DatabaseMigrationExtensions.cs for the actual seeding implementation
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove all seeded schedule activities if needed
            migrationBuilder.Sql("DELETE FROM ScheduleActivities WHERE IsCustomActivity = 1");
        }
    }
}
