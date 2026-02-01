using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToCostTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Chapter",
                table: "HRCostRecords",
                column: "Chapter");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Chapter_SubChapter",
                table: "HRCostRecords",
                columns: new[] { "Chapter", "SubChapter" });

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Chapter_SubChapter_Classification",
                table: "HRCostRecords",
                columns: new[] { "Chapter", "SubChapter", "Classification" });

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Classification",
                table: "HRCostRecords",
                column: "Classification");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Code",
                table: "HRCostRecords",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Status",
                table: "HRCostRecords",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_SubChapter",
                table: "HRCostRecords",
                column: "SubChapter");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_SubClassification",
                table: "HRCostRecords",
                column: "SubClassification");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Type",
                table: "HRCostRecords",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_HRCostRecords_Units",
                table: "HRCostRecords",
                column: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_Code",
                table: "CostCodesMaster",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1",
                table: "CostCodesMaster",
                column: "CostCodeLevel1");

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1_CostCodeLevel2",
                table: "CostCodesMaster",
                columns: new[] { "CostCodeLevel1", "CostCodeLevel2" });

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1_CostCodeLevel2_CostCodeLevel3",
                table: "CostCodesMaster",
                columns: new[] { "CostCodeLevel1", "CostCodeLevel2", "CostCodeLevel3" });

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_CostCodeLevel2",
                table: "CostCodesMaster",
                column: "CostCodeLevel2");

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_CostCodeLevel3",
                table: "CostCodesMaster",
                column: "CostCodeLevel3");

            migrationBuilder.CreateIndex(
                name: "IX_CostCodesMaster_IsActive",
                table: "CostCodesMaster",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Chapter",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Chapter_SubChapter",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Chapter_SubChapter_Classification",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Classification",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Code",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Status",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_SubChapter",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_SubClassification",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Type",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_HRCostRecords_Units",
                table: "HRCostRecords");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_Code",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1_CostCodeLevel2",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_CostCodeLevel1_CostCodeLevel2_CostCodeLevel3",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_CostCodeLevel2",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_CostCodeLevel3",
                table: "CostCodesMaster");

            migrationBuilder.DropIndex(
                name: "IX_CostCodesMaster_IsActive",
                table: "CostCodesMaster");
        }
    }
}
