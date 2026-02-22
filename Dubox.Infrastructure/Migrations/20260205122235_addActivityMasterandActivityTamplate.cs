using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addActivityMasterandActivityTamplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomActivity",
                table: "ActivityTemplateActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceActivityMasterId",
                table: "ActivityTemplateActivities",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomActivity",
                table: "ActivityTemplateActivities");

            migrationBuilder.DropColumn(
                name: "SourceActivityMasterId",
                table: "ActivityTemplateActivities");
        }
    }
}
