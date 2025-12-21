using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFactoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FactoryId",
                table: "FactoryLocations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    FactoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FactoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    CurrentOccupancy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.FactoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryLocations_FactoryId",
                table: "FactoryLocations",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_FactoryCode",
                table: "Factories",
                column: "FactoryCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FactoryLocations_Factories_FactoryId",
                table: "FactoryLocations",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "FactoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactoryLocations_Factories_FactoryId",
                table: "FactoryLocations");

            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_FactoryLocations_FactoryId",
                table: "FactoryLocations");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "FactoryLocations");
        }
    }
}
