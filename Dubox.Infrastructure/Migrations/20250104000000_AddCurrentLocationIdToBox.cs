using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentLocationIdToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add CurrentLocationId column as nullable to handle existing boxes
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentLocationId",
                table: "Boxes",
                type: "uniqueidentifier",
                nullable: true);

            // Create foreign key relationship
            migrationBuilder.CreateIndex(
                name: "IX_Boxes_CurrentLocationId",
                table: "Boxes",
                column: "CurrentLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_FactoryLocations_CurrentLocationId",
                table: "Boxes",
                column: "CurrentLocationId",
                principalTable: "FactoryLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_FactoryLocations_CurrentLocationId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_CurrentLocationId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "CurrentLocationId",
                table: "Boxes");
        }
    }
}

