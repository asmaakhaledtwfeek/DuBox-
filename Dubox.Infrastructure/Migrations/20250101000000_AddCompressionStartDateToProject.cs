using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompressionStartDateToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompressionStartDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompressionStartDate",
                table: "Projects");
        }
    }
}

