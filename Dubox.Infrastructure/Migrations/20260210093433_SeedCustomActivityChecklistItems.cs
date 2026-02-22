using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCustomActivityChecklistItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("00d61436-fbac-44c9-97b2-96b3b1123b97"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("19ae9148-c2d3-439e-add2-7363c038d1a9"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("1cca29b8-b6cc-484b-906b-7f16d248a703"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("331fed1d-0bc5-4e4b-98a8-3612efce6c8a"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("34636736-6a73-49e9-9b8b-ff6207305763"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("42088937-edfd-451d-b2c6-fe80736b1398"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("51fb42b0-1109-4f50-a413-21195f243a0a"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("58ad8182-bb42-4749-9954-a88fe76532c2"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("5ca99248-d740-494b-bb6a-cf2472ff8aef"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("68e04da5-d4ba-4371-888d-b7b76cd22914"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("80f3d8b8-5370-41fc-bb4e-cc8438491985"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("81641724-dd69-4375-be7d-88a2174a9920"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("81ff8cad-2be0-49cd-af16-99c37025f6bf"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("9f211fa2-2873-43b0-9f7b-77db177f2eaf"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("a2ea8ad1-b8ab-4837-a9c1-524eeea52034"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("a886964b-8457-43a8-8913-6cc9f59a37f3"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("c8b8f1ac-25fe-492a-96c3-44ba33ae89dd"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("cae979fc-3ab3-4c51-8bd9-e6d70adc1eb8"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("dbc097d2-517f-4bb9-88f6-8d42574d0115"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("e97e8ac4-383f-4385-be55-983e71ee949f"));

            migrationBuilder.InsertData(
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityCheckListItemId", "ActivityMasterId", "ActivityTemplateActivityId", "CreatedBy", "CreatedDate", "IsActive", "IsMandatory", "ModifiedBy", "ModifiedDate", "PredefinedChecklistItemId", "Sequence" },
                values: new object[,]
                {
                    { new Guid("f1a00000-0000-0000-0000-000000000001"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("bfaebe11-4b44-4046-8353-05b4aa658c74"), 1 },
                    { new Guid("f1a00000-0000-0000-0000-000000000002"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("b5a597a9-1cbf-49d8-af83-0cf43698e8e8"), 2 },
                    { new Guid("f1a00000-0000-0000-0000-000000000003"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f134eeef-7a92-4274-9115-125cf9d5373f"), 3 },
                    { new Guid("f1a00000-0000-0000-0000-000000000004"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("441afc7f-a0cb-4cb2-b386-312c14f24160"), 4 },
                    { new Guid("f1a00000-0000-0000-0000-000000000005"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("66cdd933-7c82-466d-a3c0-43c1651064ec"), 5 },
                    { new Guid("f1a00000-0000-0000-0000-000000000006"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("2b6382a0-29a5-485b-918c-4b04719ac277"), 6 },
                    { new Guid("f1a00000-0000-0000-0000-000000000007"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("955140b4-00aa-4ab2-a1f1-5ab51fc3f08d"), 7 },
                    { new Guid("f1a00000-0000-0000-0000-000000000008"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("2c65ce6c-4aeb-4dcf-adf5-79fd96a74fef"), 8 },
                    { new Guid("f1a00000-0000-0000-0000-000000000009"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f24a7845-057a-4b89-b83b-817822f8e4c6"), 9 },
                    { new Guid("f1a00000-0000-0000-0000-000000000010"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7bfda768-0861-456f-bb67-81e44547f005"), 10 },
                    { new Guid("f1a00000-0000-0000-0000-000000000011"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("09b3e941-a3fc-426f-ba68-980e3a7a6a27"), 11 },
                    { new Guid("f1a00000-0000-0000-0000-000000000012"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7e4c3f32-9931-4cca-9672-9edbd662a8d9"), 12 },
                    { new Guid("f1a00000-0000-0000-0000-000000000013"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("26f83daf-f89d-4ece-a1b3-a9d6c00307a5"), 13 },
                    { new Guid("f1a00000-0000-0000-0000-000000000014"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("ab057b3a-5b18-46d3-8af1-c608cbd564e0"), 14 },
                    { new Guid("f1a00000-0000-0000-0000-000000000015"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("40cc3cb6-1bc2-4c4c-83e8-c79b360f0e4a"), 15 },
                    { new Guid("f1a00000-0000-0000-0000-000000000016"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("e05bc064-2c78-4dce-bb83-caa05e894ca0"), 16 },
                    { new Guid("f1a00000-0000-0000-0000-000000000017"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("db840b56-f8a1-42bc-b984-df7e134ad5a5"), 17 },
                    { new Guid("f1a00000-0000-0000-0000-000000000018"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("93edeff9-3c3f-4090-bf19-edce625c26c8"), 18 },
                    { new Guid("f1a00000-0000-0000-0000-000000000019"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("16df93a1-6764-46e4-ae7c-f7c5e9cc094f"), 19 },
                    { new Guid("f1a00000-0000-0000-0000-000000000020"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("a372b33c-04f9-4d86-8316-f9264775ccda"), 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f1a00000-0000-0000-0000-000000000020"));

            migrationBuilder.InsertData(
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityCheckListItemId", "ActivityMasterId", "ActivityTemplateActivityId", "CreatedBy", "CreatedDate", "IsActive", "IsMandatory", "ModifiedBy", "ModifiedDate", "PredefinedChecklistItemId", "Sequence" },
                values: new object[,]
                {
                    { new Guid("00d61436-fbac-44c9-97b2-96b3b1123b97"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("16df93a1-6764-46e4-ae7c-f7c5e9cc094f"), 19 },
                    { new Guid("19ae9148-c2d3-439e-add2-7363c038d1a9"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("93edeff9-3c3f-4090-bf19-edce625c26c8"), 18 },
                    { new Guid("1cca29b8-b6cc-484b-906b-7f16d248a703"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("b5a597a9-1cbf-49d8-af83-0cf43698e8e8"), 2 },
                    { new Guid("331fed1d-0bc5-4e4b-98a8-3612efce6c8a"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("2c65ce6c-4aeb-4dcf-adf5-79fd96a74fef"), 8 },
                    { new Guid("34636736-6a73-49e9-9b8b-ff6207305763"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("441afc7f-a0cb-4cb2-b386-312c14f24160"), 4 },
                    { new Guid("42088937-edfd-451d-b2c6-fe80736b1398"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("09b3e941-a3fc-426f-ba68-980e3a7a6a27"), 11 },
                    { new Guid("51fb42b0-1109-4f50-a413-21195f243a0a"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("a372b33c-04f9-4d86-8316-f9264775ccda"), 20 },
                    { new Guid("58ad8182-bb42-4749-9954-a88fe76532c2"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("955140b4-00aa-4ab2-a1f1-5ab51fc3f08d"), 7 },
                    { new Guid("5ca99248-d740-494b-bb6a-cf2472ff8aef"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f24a7845-057a-4b89-b83b-817822f8e4c6"), 9 },
                    { new Guid("68e04da5-d4ba-4371-888d-b7b76cd22914"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("66cdd933-7c82-466d-a3c0-43c1651064ec"), 5 },
                    { new Guid("80f3d8b8-5370-41fc-bb4e-cc8438491985"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("26f83daf-f89d-4ece-a1b3-a9d6c00307a5"), 13 },
                    { new Guid("81641724-dd69-4375-be7d-88a2174a9920"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f134eeef-7a92-4274-9115-125cf9d5373f"), 3 },
                    { new Guid("81ff8cad-2be0-49cd-af16-99c37025f6bf"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("bfaebe11-4b44-4046-8353-05b4aa658c74"), 1 },
                    { new Guid("9f211fa2-2873-43b0-9f7b-77db177f2eaf"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7bfda768-0861-456f-bb67-81e44547f005"), 10 },
                    { new Guid("a2ea8ad1-b8ab-4837-a9c1-524eeea52034"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("40cc3cb6-1bc2-4c4c-83e8-c79b360f0e4a"), 15 },
                    { new Guid("a886964b-8457-43a8-8913-6cc9f59a37f3"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7e4c3f32-9931-4cca-9672-9edbd662a8d9"), 12 },
                    { new Guid("c8b8f1ac-25fe-492a-96c3-44ba33ae89dd"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("e05bc064-2c78-4dce-bb83-caa05e894ca0"), 16 },
                    { new Guid("cae979fc-3ab3-4c51-8bd9-e6d70adc1eb8"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("db840b56-f8a1-42bc-b984-df7e134ad5a5"), 17 },
                    { new Guid("dbc097d2-517f-4bb9-88f6-8d42574d0115"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("ab057b3a-5b18-46d3-8af1-c608cbd564e0"), 14 },
                    { new Guid("e97e8ac4-383f-4385-be55-983e71ee949f"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("2b6382a0-29a5-485b-918c-4b04719ac277"), 6 }
                });
        }
    }
}
