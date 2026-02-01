using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class boxFactory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FactoryId",
                table: "Boxes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_FactoryId",
                table: "Boxes",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_Factories_FactoryId",
                table: "Boxes",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "FactoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_Factories_FactoryId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_FactoryId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "Boxes");
        }
    }
}
