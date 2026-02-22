using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForChecklistItemSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add unique constraint on ChecklistSectionId and Sequence combination
            // Only for active items
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IX_PredefinedChecklistItems_ChecklistSectionId_Sequence_Active
                ON PredefinedChecklistItems (ChecklistSectionId, Sequence)
                WHERE IsActive = 1;
            ");

            // Add unique constraint for ActivityCheckListItems by ActivityMasterId and Sequence
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IX_ActivityCheckListItems_ActivityMasterId_Sequence_Active
                ON ActivityCheckListItems (ActivityMasterId, Sequence)
                WHERE IsActive = 1 AND ActivityMasterId IS NOT NULL;
            ");

            // Add unique constraint for ActivityCheckListItems by ActivityTemplateActivityId and Sequence
            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX IX_ActivityCheckListItems_ActivityTemplateActivityId_Sequence_Active
                ON ActivityCheckListItems (ActivityTemplateActivityId, Sequence)
                WHERE IsActive = 1 AND ActivityTemplateActivityId IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS IX_PredefinedChecklistItems_ChecklistSectionId_Sequence_Active
                ON PredefinedChecklistItems;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS IX_ActivityCheckListItems_ActivityMasterId_Sequence_Active
                ON ActivityCheckListItems;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS IX_ActivityCheckListItems_ActivityTemplateActivityId_Sequence_Active
                ON ActivityCheckListItems;
            ");
        }
    }
}
