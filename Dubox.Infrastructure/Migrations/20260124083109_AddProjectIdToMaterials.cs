using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdToMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Materials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProjectId",
                table: "Materials",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Projects_ProjectId",
                table: "Materials",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Projects_ProjectId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProjectId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Materials");
        }
    }
}
