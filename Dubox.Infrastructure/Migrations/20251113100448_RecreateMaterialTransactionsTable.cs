using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMaterialTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "MaterialTransactions",
    columns: table => new
    {
        TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        MaterialId = table.Column<int>(type: "int", nullable: false),
        BoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
        BoxActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
        TransactionType = table.Column<int>(type: "int", maxLength: 50, nullable: true),
        Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
        TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
        PerformedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
        Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_MaterialTransactions", x => x.TransactionId);
        table.ForeignKey(
            name: "FK_MaterialTransactions_BoxActivities_BoxActivityId",
            column: x => x.BoxActivityId,
            principalTable: "BoxActivities",
            principalColumn: "BoxActivityId",
            onDelete: ReferentialAction.SetNull);
        table.ForeignKey(
            name: "FK_MaterialTransactions_Boxes_BoxId",
            column: x => x.BoxId,
            principalTable: "Boxes",
            principalColumn: "BoxId");
        table.ForeignKey(
            name: "FK_MaterialTransactions_Materials_MaterialId",
            column: x => x.MaterialId,
            principalTable: "Materials",
            principalColumn: "MaterialId",
            onDelete: ReferentialAction.Restrict);
        table.ForeignKey(
            name: "FK_MaterialTransactions_Users_PerformedById",
            column: x => x.PerformedById,
            principalTable: "Users",
            principalColumn: "UserId",
            onDelete: ReferentialAction.SetNull);
    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
    name: "ActivityMaterials");
        }
    }
}
