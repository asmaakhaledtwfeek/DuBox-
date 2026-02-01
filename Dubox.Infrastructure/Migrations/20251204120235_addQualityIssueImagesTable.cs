using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addQualityIssueImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "QualityIssues");

            migrationBuilder.CreateTable(
                name: "QualityIssueImages",
                columns: table => new
                {
                    QualityIssueImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityIssueImages", x => x.QualityIssueImageId);
                    table.ForeignKey(
                        name: "FK_QualityIssueImages_QualityIssues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "QualityIssues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualityIssueImages_IssueId_Sequence",
                table: "QualityIssueImages",
                columns: new[] { "IssueId", "Sequence" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityIssueImages");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "QualityIssues",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
