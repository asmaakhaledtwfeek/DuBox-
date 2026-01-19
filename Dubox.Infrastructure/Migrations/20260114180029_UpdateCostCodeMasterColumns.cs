using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCostCodeMasterColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level1DescriptionAbbrev",
                table: "CostCodesMaster",
                newName: "Level3DescriptionAmana");

            migrationBuilder.RenameColumn(
                name: "Level0DescriptionAmana",
                table: "CostCodesMaster",
                newName: "Level3DescriptionAbbrev");

            migrationBuilder.RenameColumn(
                name: "CostCodePerCSI",
                table: "CostCodesMaster",
                newName: "CostCodeLevel3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level3DescriptionAmana",
                table: "CostCodesMaster",
                newName: "Level1DescriptionAbbrev");

            migrationBuilder.RenameColumn(
                name: "Level3DescriptionAbbrev",
                table: "CostCodesMaster",
                newName: "Level0DescriptionAmana");

            migrationBuilder.RenameColumn(
                name: "CostCodeLevel3",
                table: "CostCodesMaster",
                newName: "CostCodePerCSI");
        }
    }
}
