using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addGroupTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamGroupId",
                table: "TeamMembers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeamGroups",
                columns: table => new
                {
                    TeamGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamGroups", x => x.TeamGroupId);
                    table.ForeignKey(
                        name: "FK_TeamGroups_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WIRChecklistItems_PredefinedItemId",
                table: "WIRChecklistItems",
                column: "PredefinedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TeamGroupId",
                table: "TeamMembers",
                column: "TeamGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGroups_TeamId",
                table: "TeamGroups",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGroups_TeamId_GroupName",
                table: "TeamGroups",
                columns: new[] { "TeamId", "GroupName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_TeamGroups_TeamGroupId",
                table: "TeamMembers",
                column: "TeamGroupId",
                principalTable: "TeamGroups",
                principalColumn: "TeamGroupId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WIRChecklistItems_PredefinedChecklistItems_PredefinedItemId",
                table: "WIRChecklistItems",
                column: "PredefinedItemId",
                principalTable: "PredefinedChecklistItems",
                principalColumn: "PredefinedItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_TeamGroups_TeamGroupId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_WIRChecklistItems_PredefinedChecklistItems_PredefinedItemId",
                table: "WIRChecklistItems");

            migrationBuilder.DropTable(
                name: "TeamGroups");

            migrationBuilder.DropIndex(
                name: "IX_WIRChecklistItems_PredefinedItemId",
                table: "WIRChecklistItems");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_TeamGroupId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "TeamGroupId",
                table: "TeamMembers");
        }
    }
}
