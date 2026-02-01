using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCurrentLocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_Boxes_BoxId",
                table: "BoxLocationHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_FactoryLocations_MovedFromLocationId",
                table: "BoxLocationHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentLocationId",
                table: "Boxes",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_BoxLocationHistory_Boxes_BoxId",
                table: "BoxLocationHistory",
                column: "BoxId",
                principalTable: "Boxes",
                principalColumn: "BoxId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxLocationHistory_FactoryLocations_MovedFromLocationId",
                table: "BoxLocationHistory",
                column: "MovedFromLocationId",
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

            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_Boxes_BoxId",
                table: "BoxLocationHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_FactoryLocations_MovedFromLocationId",
                table: "BoxLocationHistory");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_CurrentLocationId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "CurrentLocationId",
                table: "Boxes");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxLocationHistory_Boxes_BoxId",
                table: "BoxLocationHistory",
                column: "BoxId",
                principalTable: "Boxes",
                principalColumn: "BoxId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxLocationHistory_FactoryLocations_MovedFromLocationId",
                table: "BoxLocationHistory",
                column: "MovedFromLocationId",
                principalTable: "FactoryLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
