using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBoxTypeForeignKeyConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints from Boxes table
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxSubTypes_BoxSubTypeId",
                table: "Boxes");

            // Drop indexes on foreign keys
            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxTypeId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxSubTypeId",
                table: "Boxes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate indexes (if rolling back)
            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxSubTypeId",
                table: "Boxes",
                column: "BoxSubTypeId");

            // Recreate foreign key constraints (if rolling back)
            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxTypes_BoxTypeId",
                table: "Boxes",
                column: "BoxTypeId",
                principalTable: "BoxTypes",
                principalColumn: "BoxTypeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxSubTypes_BoxSubTypeId",
                table: "Boxes",
                column: "BoxSubTypeId",
                principalTable: "BoxSubTypes",
                principalColumn: "BoxSubTypeId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
