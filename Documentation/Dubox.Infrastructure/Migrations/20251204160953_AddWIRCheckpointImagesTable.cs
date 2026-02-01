using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWIRCheckpointImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WIRCheckpointImages",
                columns: table => new
                {
                    WIRCheckpointImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WIRId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIRCheckpointImages", x => x.WIRCheckpointImageId);
                    table.ForeignKey(
                        name: "FK_WIRCheckpointImages_WIRCheckpoints_WIRId",
                        column: x => x.WIRId,
                        principalTable: "WIRCheckpoints",
                        principalColumn: "WIRId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WIRCheckpointImages_WIRId_Sequence",
                table: "WIRCheckpointImages",
                columns: new[] { "WIRId", "Sequence" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WIRCheckpointImages");
        }
    }
}
