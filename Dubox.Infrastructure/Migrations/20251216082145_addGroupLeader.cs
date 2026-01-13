using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addGroupLeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupCode",
                table: "TeamGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupLeaderId",
                table: "TeamGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupTag",
                table: "TeamGroups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupType",
                table: "TeamGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGroups_GroupLeaderId",
                table: "TeamGroups",
                column: "GroupLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamGroups_TeamMembers_GroupLeaderId",
                table: "TeamGroups",
                column: "GroupLeaderId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamGroups_TeamMembers_GroupLeaderId",
                table: "TeamGroups");

            migrationBuilder.DropIndex(
                name: "IX_TeamGroups_GroupLeaderId",
                table: "TeamGroups");

            migrationBuilder.DropColumn(
                name: "GroupLeaderId",
                table: "TeamGroups");

            migrationBuilder.DropColumn(
                name: "GroupTag",
                table: "TeamGroups");

            migrationBuilder.DropColumn(
                name: "GroupType",
                table: "TeamGroups");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "TeamGroups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
