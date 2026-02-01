using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class versionsCkeckpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentWIRId",
                table: "WIRCheckpoints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "WIRCheckpoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints",
                column: "ParentWIRId");

            migrationBuilder.AddForeignKey(
                name: "FK_WIRCheckpoints_WIRCheckpoints_ParentWIRId",
                table: "WIRCheckpoints",
                column: "ParentWIRId",
                principalTable: "WIRCheckpoints",
                principalColumn: "WIRId");
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
                name: "ParentWIRId",
                table: "WIRCheckpoints");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "WIRCheckpoints");
        }
    }
}
