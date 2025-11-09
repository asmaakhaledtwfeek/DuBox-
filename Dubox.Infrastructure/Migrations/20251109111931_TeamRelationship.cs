using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TeamRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamLeaderName",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamSize",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TeamMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeamLeaderMemberId",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeName",
                table: "TeamMembers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "TeamMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DepartmentId",
                table: "Teams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamLeaderMemberId",
                table: "Teams",
                column: "TeamLeaderMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Departments_DepartmentId",
                table: "Teams",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_TeamMembers_TeamLeaderMemberId",
                table: "Teams",
                column: "TeamLeaderMemberId",
                principalTable: "TeamMembers",
                principalColumn: "TeamMemberId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Departments_DepartmentId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamMembers_TeamLeaderMemberId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_DepartmentId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamLeaderMemberId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamLeaderMemberId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Teams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamLeaderName",
                table: "Teams",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamSize",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeName",
                table: "TeamMembers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "TeamMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TeamMembers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
