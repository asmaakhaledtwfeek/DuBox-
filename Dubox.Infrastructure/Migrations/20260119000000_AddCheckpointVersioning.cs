using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckpointVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Version column with default value of 1
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "WIRCheckpoints",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Add ParentWIRId column for tracking checkpoint lineage
            migrationBuilder.AddColumn<Guid>(
                name: "ParentWIRId",
                table: "WIRCheckpoints",
                type: "uniqueidentifier",
                nullable: true);

            // Create foreign key relationship for ParentWIRId (self-referencing)
            migrationBuilder.CreateIndex(
                name: "IX_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints",
                column: "ParentWIRId");

            migrationBuilder.AddForeignKey(
                name: "FK_WIRCheckpoints_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints",
                column: "ParentWIRId",
                principalTable: "WIRCheckpoints",
                principalColumn: "WIRId",
                onDelete: ReferentialAction.NoAction); // Prevent cascade delete issues
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WIRCheckpoints_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints");

            migrationBuilder.DropIndex(
                name: "IX_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "WIRCheckpoints");

            migrationBuilder.DropColumn(
                name: "ParentWIRId",
                table: "WIRCheckpoints");
        }
    }
}
