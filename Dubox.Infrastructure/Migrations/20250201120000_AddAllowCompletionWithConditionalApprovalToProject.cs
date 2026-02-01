using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowCompletionWithConditionalApprovalToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowCompletionWithConditionalApproval",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Add "Less Element" box type to all existing projects that don't have it
            migrationBuilder.Sql(@"
                INSERT INTO ProjectBoxTypes (ProjectId, TypeName, Abbreviation, HasSubTypes, DisplayOrder, IsActive, CreatedDate)
                SELECT p.ProjectId, 'Less Element', 'LE', 0, 1, 1, GETUTCDATE()
                FROM Projects p
                WHERE NOT EXISTS (
                    SELECT 1 FROM ProjectBoxTypes pbt 
                    WHERE pbt.ProjectId = p.ProjectId AND pbt.TypeName = 'Less Element'
                )
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowCompletionWithConditionalApproval",
                table: "Projects");

            // Remove "Less Element" box types added by this migration
            migrationBuilder.Sql(@"
                DELETE FROM ProjectBoxTypes WHERE TypeName = 'Less Element'
            ");
        }
    }
}
