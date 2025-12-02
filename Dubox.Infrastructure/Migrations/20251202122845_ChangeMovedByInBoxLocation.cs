using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMovedByInBoxLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MovedBy",
                table: "BoxLocationHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxLocationHistory_MovedBy",
                table: "BoxLocationHistory",
                column: "MovedBy");

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
            migrationBuilder.DropForeignKey(
                name: "FK_BoxLocationHistory_Users_MovedBy",
                table: "BoxLocationHistory");

            migrationBuilder.DropIndex(
                name: "IX_BoxLocationHistory_MovedBy",
                table: "BoxLocationHistory");

            migrationBuilder.AlterColumn<string>(
                name: "MovedBy",
                table: "BoxLocationHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
