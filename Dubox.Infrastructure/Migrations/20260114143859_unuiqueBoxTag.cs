using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class unuiqueBoxTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Boxes_ProjectId_BoxTag",
                table: "Boxes");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_ProjectId_SerialNumber",
                table: "Boxes",
                columns: new[] { "ProjectId", "SerialNumber" },
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Boxes_ProjectId_SerialNumber",
                table: "Boxes");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_ProjectId_BoxTag",
                table: "Boxes",
                columns: new[] { "ProjectId", "BoxTag" },
                unique: true);
        }
    }
}
