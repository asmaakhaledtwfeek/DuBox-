using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressUpdateImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgressUpdateImages",
                columns: table => new
                {
                    ProgressUpdateImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgressUpdateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressUpdateImages", x => x.ProgressUpdateImageId);
                    table.ForeignKey(
                        name: "FK_ProgressUpdateImages_ProgressUpdates_ProgressUpdateId",
                        column: x => x.ProgressUpdateId,
                        principalTable: "ProgressUpdates",
                        principalColumn: "ProgressUpdateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressUpdateImages_ProgressUpdateId_Sequence",
                table: "ProgressUpdateImages",
                columns: new[] { "ProgressUpdateId", "Sequence" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgressUpdateImages");
        }
    }
}
