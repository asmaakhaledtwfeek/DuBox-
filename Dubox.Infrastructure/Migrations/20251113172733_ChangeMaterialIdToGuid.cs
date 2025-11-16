using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaterialIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ==========================================
            // Step 1: Drop ALL Foreign Keys
            // ==========================================
            migrationBuilder.Sql(@"
            -- Get and drop all foreign keys that reference Materials
            DECLARE @sql NVARCHAR(MAX) = '';
            
            SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + 
                          QUOTENAME(OBJECT_NAME(parent_object_id)) + 
                          ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
            FROM sys.foreign_keys
            WHERE referenced_object_id = OBJECT_ID('Materials');
            
            EXEC sp_executesql @sql;
        ");

            // ==========================================
            // Step 2: Drop specific Indexes
            // ==========================================
            migrationBuilder.DropIndex(
                name: "IX_ActivityMaterials_MaterialId",
                table: "ActivityMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials");

            // Note: MaterialTransactions doesn't have an index based on the results

            // ==========================================
            // Step 3: Drop Primary Key of Materials
            // ==========================================
            migrationBuilder.DropPrimaryKey(
                name: "PK_Materials",
                table: "Materials");

            // ==========================================
            // Step 4: Drop old Id column in Materials
            // ==========================================
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Materials");

            // ==========================================
            // Step 5: Add new Guid Id in Materials
            // ==========================================
            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "Materials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            // ==========================================
            // Step 6: Recreate Primary Key
            // ==========================================
            migrationBuilder.AddPrimaryKey(
                name: "PK_Materials",
                table: "Materials",
                column: "MaterialId");

            // ==========================================
            // Step 7: Update MaterialTransactions
            // ==========================================
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "MaterialTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "MaterialTransactions",
                type: "uniqueidentifier",
                nullable: false);

            // ==========================================
            // Step 8: Update BoxMaterials
            // ==========================================
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "BoxMaterials");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "BoxMaterials",
                type: "uniqueidentifier",
                nullable: false);

            // ==========================================
            // Step 9: Update ActivityMaterials
            // ==========================================
            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "ActivityMaterials");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "ActivityMaterials",
                type: "uniqueidentifier",
                nullable: false);

            // ==========================================
            // Step 10: Recreate Indexes
            // ==========================================
            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransactions_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxMaterials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaterials_MaterialId",
                table: "ActivityMaterials",
                column: "MaterialId");

            // ==========================================
            // Step 11: Recreate ALL Foreign Keys
            // ==========================================
            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTransactions_Materials_MaterialId",
                table: "MaterialTransactions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxMaterials_Materials_MaterialId",
                table: "BoxMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityMaterials_Materials_MaterialId",
                table: "ActivityMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException("Cannot safely revert Guid to Int migration");
        }
    }
}
