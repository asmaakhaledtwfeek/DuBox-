using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTrackingAndUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformedBy",
                table: "MaterialTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "PerformedById",
                table: "MaterialTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_PerformedById",
                table: "MaterialTransactions",
                column: "PerformedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTransactions_Users_PerformedById",
                table: "MaterialTransactions",
                column: "PerformedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTransactions_Users_PerformedById",
                table: "MaterialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTransactions_PerformedById",
                table: "MaterialTransactions");

            migrationBuilder.DropColumn(
                name: "PerformedById",
                table: "MaterialTransactions");

            migrationBuilder.AddColumn<string>(
                name: "PerformedBy",
                table: "MaterialTransactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
