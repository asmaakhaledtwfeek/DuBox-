using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMovedByToGuidInBoxLocationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old column if it exists
            migrationBuilder.DropColumn(
                name: "MovedBy",
                table: "BoxLocationHistory");

            // Add the new Guid column
            migrationBuilder.AddColumn<Guid>(
                name: "MovedBy",
                table: "BoxLocationHistory",
                type: "uniqueidentifier",
                nullable: true);

            // Create index for better query performance
            migrationBuilder.CreateIndex(
                name: "IX_BoxLocationHistory_MovedBy",
                table: "BoxLocationHistory",
                column: "MovedBy");

            // Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_BoxLocationHistory_Users_MovedBy",
                table: "BoxLocationHistory",
                column: "MovedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_Users_MovedBy",
                table: "BoxLocationHistory");

            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_BoxLocationHistory_MovedBy",
                table: "BoxLocationHistory");

            // Drop the Guid column
            migrationBuilder.DropColumn(
                name: "MovedBy",
                table: "BoxLocationHistory");

            // Restore the old string column
            migrationBuilder.AddColumn<string>(
                name: "MovedBy",
                table: "BoxLocationHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}

