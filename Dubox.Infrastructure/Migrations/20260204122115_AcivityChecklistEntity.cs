using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AcivityChecklistEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityCheckListItems",
                columns: table => new
                {
                    ActivityCheckListItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActivityTemplateActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PredefinedChecklistItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCheckListItems", x => x.ActivityCheckListItemId);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItems_ActivityMaster_ActivityMasterId",
                        column: x => x.ActivityMasterId,
                        principalTable: "ActivityMaster",
                        principalColumn: "ActivityMasterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItems_ActivityTemplateActivities_ActivityTemplateActivityId",
                        column: x => x.ActivityTemplateActivityId,
                        principalTable: "ActivityTemplateActivities",
                        principalColumn: "ActivityTemplateActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCheckListItems_PredefinedChecklistItems_PredefinedChecklistItemId",
                        column: x => x.PredefinedChecklistItemId,
                        principalTable: "PredefinedChecklistItems",
                        principalColumn: "PredefinedItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItems_ActivityMasterId",
                table: "ActivityCheckListItems",
                column: "ActivityMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItems_ActivityMasterId_Sequence",
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityMasterId", "Sequence" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItems_ActivityTemplateActivityId",
                table: "ActivityCheckListItems",
                column: "ActivityTemplateActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItems_ActivityTemplateActivityId_Sequence",
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityTemplateActivityId", "Sequence" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCheckListItems_PredefinedChecklistItemId",
                table: "ActivityCheckListItems",
                column: "PredefinedChecklistItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityCheckListItems");
        }
    }
}
