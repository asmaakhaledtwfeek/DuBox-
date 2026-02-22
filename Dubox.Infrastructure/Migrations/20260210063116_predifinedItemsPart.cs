using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class predifinedItemsPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChecklistNumber",
                table: "PredefinedChecklistItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Part",
                table: "PredefinedChecklistItems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("003369a3-9eb1-46a1-88a0-6ce1c8437c6a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("004d668e-007e-4e74-8932-2fd476b72e45"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("00fa8f95-95f7-4f0c-8ddb-7aeaab75be85"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("014331e4-ed57-41a3-b179-523c251c7b1b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("01b1105d-23d3-4a68-9960-274a08ef35fe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("01f4e629-5e23-4d2b-a5d8-60029a7eae86"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("01fa0165-9a30-4f14-b07d-0030a5d81be4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("021671e3-16a9-4ba5-846c-a7fd2fba28f0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0235daa1-0967-4d66-b793-cbb6da1f07ff"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0256c9d6-f188-424b-9dca-a15fbe850160"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0267437f-9173-449e-b9fe-f609dc9a4c3b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("026ad2af-90f5-4d93-a3df-1557fcefa60e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("02a98155-e15a-493a-a3a8-858f3714959f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("02b3d45c-8f78-4bc5-88ff-3a62cc5e5d63"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("03e92e0b-6532-43e3-afb0-e05565b32810"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04263bce-ea06-445e-9cc0-13c816a44652"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0435c429-4cb6-4649-94dd-977f7ba7e3b9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("049c0dcf-2415-4bea-a1e8-764306ef916d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04c7bd2f-5f0a-4cf8-a880-b01d51951c29"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04ccf75f-fec7-4e6d-8aa5-486fe424dc9c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04ebb69f-13bb-4de5-84bb-20e1e80f2e1d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("05b22657-2760-41c6-9d32-9d00776ad1ec"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("05c755bc-fe3c-4c52-b820-159e5e405457"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("060f9403-5536-4495-b95a-6af6eb457327"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("061a0ef7-1fa4-4ed6-835f-52fe9e1365a1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("069935b6-81a2-4be4-92ec-d06c72b342ad"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("07095085-5014-4fc1-a911-8e15a27d0048"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("077c3358-1ca1-4547-988a-e8560548ddee"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("093876f8-4b2a-419c-b69d-f5707f4c0d25"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("095f4a93-5a54-4904-b372-a19e9161aa2d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09757220-4a69-47e1-94a9-e71725b6d869"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("098c5d12-bff8-4847-99b2-0c0d41ee6ce9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09b3e191-f758-4d59-8dc4-032a2265308f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09c1dffc-cb94-4988-92ee-763b35eb05d5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09ebf665-3bf1-4f95-a539-1cc5096cbf68"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0a8e9f2a-678c-4498-9868-7597d558f54c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0ab3367a-c7dc-466f-9d5f-13f26b1991e8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0b218c7d-3952-4919-adf6-c4ea2796381b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0b9f1a33-6451-4b53-b8ca-ef641a00bced"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0bbfed09-e880-41f0-9da5-70402681338f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0ca313a7-5fb9-45ec-b6ee-f2368a18f6c0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0d7fcd09-7011-4afd-9fa7-2f401d5c7a9e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0ddd0235-6bde-4715-bf9a-4ba62541a707"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0e606e0a-f3cc-4c20-8281-e6b50fd67be6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f259f4f-ce86-49b7-ac25-eccb993e7514"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f2db243-99fc-45a9-b240-32e87ca0d853"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f518c21-55ef-4815-a19d-27fdfdf37be0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1001d97f-e609-4631-b22f-5616baaf7198"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("11a8d5db-479a-4e7c-be57-665fd7f5c068"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("11e337d8-97ba-46db-9ed0-ace4436659ff"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("12741670-ff21-4d54-890a-dcf16830a502"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("131ca61d-e70c-4ae1-938e-aaa1bf5f084e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("132e4f76-7afe-473c-b03e-8aeb9fce1ba9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1405a6c8-a988-4583-b717-2c3f8cf3725f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("144d284f-6ea2-4735-9f33-a7fdf0f4f103"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("148d1009-40c9-4e8c-9b79-ee10d527ffb2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("14d4dc59-7775-45e0-b7da-7c03e0e49380"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("14d8c774-0ce7-4d32-9ffa-5bca94f71a4e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("152b49a2-ad74-4120-ab1e-d331cdb1a49e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("153bb96b-2d77-4d6a-bec6-86866d4c4dcc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("161db0eb-b332-4a4b-a4a4-5d40011d1acc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("162581fa-58a0-4cd1-8e37-d48e59a7cd08"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1663d791-fc93-48f8-9bb6-f907ef708c77"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1743b09c-66a2-423a-89d6-cdd3c562e23e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1790149d-16ac-485e-83f5-b3a1f81b92b3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1793b9d2-c32c-48f9-a75b-757cc4d811bf"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("184b1752-1bbb-4b54-b9d3-1899fc3d1bf9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("192f93c2-19e8-4416-9c83-2f2eaaee8e1a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1ab36498-449c-461c-b9f1-43597c198d61"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1b593558-70a9-4541-bcb2-2a2e6ece700e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1c1d88e4-1afd-4cc1-a9ad-216595215ca8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1cf7703e-e801-438d-9a8b-d44f75dd418d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d2b607f-f4e8-4361-ad96-6541683da8f9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d8cf575-272e-4bd0-92d6-18d819482228"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1e83ba95-68ee-4bd7-a869-d45f4c65bc1a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1f9227c8-09da-453d-8971-1e48a71036d7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1fad81d0-047c-494a-87ef-25e76a2b584d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("205f336e-c1ec-4c2f-a4e5-5554f3986203"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2195a9c1-2022-4ef1-83de-b0327435f906"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2273e11c-615e-4551-ace4-e25966f39c6f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2294a7d0-211c-414a-bf65-704ac479d284"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("22a965bb-1b50-44fe-9878-250d0840ce92"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("22b095c4-2752-47a5-84e5-001ade385072"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("22b5ed30-ada2-4f90-8c5a-fa6247b4e080"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("23ae56ac-8cb1-4e5f-b9be-b198c0d800c1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("23b2bdc4-e83f-48a6-a682-efafa59137d3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("23f8fa00-38e5-4920-a197-fa2f56b39190"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("244ace99-fc3a-4479-8be8-3fb00f854411"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2453ba13-7431-4775-bf4c-116b814a032c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("24bd90c9-8d2d-4838-86c6-1782fd5e5627"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2643e140-a1d0-4ed8-9e93-35bf3f62e63e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("268612b9-e10d-44ff-85bc-6447a3a57f20"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("272957f7-502e-4571-9078-2f3dcf4b1b79"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2737526a-9e27-4755-8c89-c131e9409227"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("275f19e4-ecce-491f-84d1-93ec52392b3c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("28004dc8-998e-4417-9f4a-577052f1e8da"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("28b83159-92be-45e9-b5ce-fb80661efc9a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("28c53002-5447-48fb-bcf9-f957875807fe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("29b5036f-51ca-475c-aa6d-4de31094bfe3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2a30aee7-a4b3-421f-a4d8-b128772870a0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2a322c2c-8e24-4693-b4ee-5ade0b6df04d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2a82a13f-6fcd-421a-aa2b-c08642ae7dd0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2b4c0a33-8f78-47af-8146-d9264b49f544"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2b92ad8e-d7a0-4d9e-9c23-f95760fb7129"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2bb3c1e9-d478-4ae6-a863-ec689b0cc4d5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2bcca6a8-ae11-42e2-806e-c29ab393261d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c82a768-799c-429e-b5db-db1d00707f7e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c8964a3-0a04-4cec-98d4-cb38c7551176"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2d21a924-9156-466a-babf-14ce375df320"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2db4934f-f556-4df8-b7fb-364de3794866"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2e06cca0-d33e-49df-bcc0-acf00f191fb2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2e4cba14-317f-471b-b2be-8349df12a5f4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2e58fb08-19ee-4661-86d4-c5a0b9064e70"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2e5c1134-9d70-4b88-bac1-ee3e90aaefb6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2e6d9fd3-5e78-489c-9f40-e4445e6abc7a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2ee4cc95-fe97-4bd7-9da0-5b47a8401d88"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2f10b5df-f070-4067-a502-fa9286017ea4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2fc6b0be-86ed-4bcf-9c07-ab0634205e6c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2ffc8ed4-6c26-41b9-bcd9-286f5bf73d7b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3018672f-f4e9-4982-9af5-6570666f6b63"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3037c680-51ea-44b9-972c-60ab64ef1f63"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("30602cc2-18d3-4246-8b6e-2d1a713999b6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("306ec504-6fd2-4d25-92e4-a468dc0a5f74"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3168b84f-b68e-4247-aa27-9993b45f32f2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("31f660d3-3e54-4eca-b203-53c071227146"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("31f7f042-190b-4e88-85aa-33edb92bf603"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3260df31-c7f8-4223-a62e-55c451898a64"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("32d02b82-abe1-4697-9f7a-be08a4e82f71"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("32d1fc5f-1152-4474-afe9-c5f1971bda1e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3318c546-e8a4-4598-9b53-93537a256d0a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("332d047c-b05a-4216-8dc1-3e4e9f37baea"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("33c57069-7323-4909-81f5-623b159c0cd4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3446434b-fad5-4bdf-bf20-28f37c0f04d0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("34b1549c-f111-4a61-b6bb-d3505aa59f04"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3518f62d-9746-4a2a-81bd-3e6052455252"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("35746651-3a2e-451d-8630-f118ca112d5b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("35d166b2-5fb7-4087-9a08-5f72d674f920"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("35f98980-91e5-40ae-861b-0217aad4edd6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("366a81c4-3690-4462-8b09-9d53005967da"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("36a128fd-e1e0-481c-8058-364cea0f0f65"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3715d4ab-769d-4c5e-b6ae-83e9b15248b7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("37e450bb-c225-4a1d-86df-0cb878fa0b6d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3815b1cd-edd6-439d-a0b6-67e612664968"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3909af74-7e9a-45dd-a7e8-41a94bfefb8a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("39307943-2b50-4d2a-bc9f-0033ec093ef6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3a1b0480-7ff3-4ef5-8fbb-dfe21b681e58"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3a2a3648-7e1d-47db-bfc5-b9b484bfae17"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3a3f1926-85b8-4721-ade9-375b9906f6c2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3a81f6c2-231a-46f9-857e-17d23a119586"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b22d8a3-c581-47a2-844f-040043b38c53"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b44ccdf-3162-4446-9a28-dd11a755eae5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b4d582a-63b3-4351-a2d0-9673a47e9de1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b8169ac-84ef-4f5d-84f6-1cfdee792b5b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3bd2e9bc-943e-409e-bc17-215d7d316806"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3bfcfc0d-8e04-49ed-9a86-7bed791d76b9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3cc2cc5d-f8db-4223-ab0a-be9b2a0b66ca"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3cc68162-5b07-4fd6-a1a3-e33a9c09699f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3d066e17-0d13-49f6-8b4e-08e12c3115ba"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e2bf5c0-7241-4267-b2bb-c43a56c8ed98"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e6ec60a-5ffd-4452-a46d-35cf901f7e30"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e7c08f7-9069-4312-a671-a2c8dfba9444"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3ea5ad74-8675-4509-8233-68f6a323942e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3efb3cf9-a0ee-4bbe-8cde-301552f811cf"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3f3eda9c-48f7-457c-a316-bca6d456b8d5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3f919e8c-6beb-43e2-92b1-54983886cdc4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3fe91317-1af6-4d30-b353-c5fd4969dc4a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40bffd5a-55b4-483c-903a-fa52b8768bd8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40c52768-3568-4b07-8216-50d09bcf3b2e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40eb757f-abee-4fc1-8ae8-fda8c4ee35a5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("414740e6-f97c-42f3-9a61-872c8d1ab8ec"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("41bf1dc4-16d2-4ff2-bd98-de10de0cc427"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("423f9531-99ff-41e4-8eb3-ae09d3410173"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4242d0b2-78c1-4f90-a531-337bfd5a132a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("42646f43-0cf2-4d1e-b503-7b33d80da696"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("426547ca-113d-46a1-b173-cc26713164db"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("42ad2e83-5e80-4955-817d-6daec262c4a8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("43155025-7f6d-435e-81ea-3958a68a498d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("449cf9c3-4f69-4ea3-8a2f-14b8c9289cc2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("44cea87e-d98a-48cb-9508-ed3034504772"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("44fc7108-33d6-4ca2-b433-b2d444d67cce"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("45035e49-774a-4e6f-a3bb-2432cf03d122"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("45442ac3-eb8c-4d3c-a869-c61d9845e6c4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("459ca8e6-d443-4d10-812f-a8c3d00f5974"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("46413943-3db0-4ef2-b42e-b9922da6a367"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("480a98fb-7f94-4304-9e83-17b31cb83f8f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("483810d3-bec0-4e86-8167-4d539737d4f8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("486c91ac-c8d6-4454-ae57-06374e4609f1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("49f8ffac-6550-4b19-95a1-968a66202c80"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4a21b2b1-39f8-4457-9e40-2cc1fd1d4efa"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4a672d2f-98aa-44f0-92e5-a756383df52c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4aef1692-e300-4a50-a0ff-e88e5fa9b2ee"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4b7244bb-a0b8-4b6c-9cbe-de87104375b3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4c295d35-b989-47e4-a364-a782a02d5c45"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4cdbc40e-da7c-4387-891c-946bed80f575"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4ceb14da-adbe-4168-97b3-a9f81d059061"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e45cb51-721e-41ab-b8f4-d403ceae14a8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e66eae5-9128-4e62-93fb-e17e71dacbf5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e79a845-dabc-4065-a9ef-ceab8025de15"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e8f143c-0eba-4c5f-abae-2bb8da4c90e8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4ef072b4-e4fa-4f9b-8720-99ddb5616b2c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4f34ae92-39ac-41cd-b0cd-e13984bfa9d7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4f84571b-bebc-434a-8701-70e118966908"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4f858e78-20d3-49f8-8cb8-2ed355da093a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4fc68421-08b9-4bd1-8c7e-6fe0d9213e0b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5083ea4c-f6ee-4f01-adbf-e33df6007217"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50982c8e-99bf-4125-b648-c836339bab6f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50a32955-4736-4b10-aadf-30ebba82c0fe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50a6984c-cc7f-41a3-8706-53d946319683"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50dab685-6ac5-4d1a-bbb8-24dc3b3eaaea"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50e98af4-2709-4c63-961c-0f12d36ea899"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("519124ae-9660-4efc-9da5-83aa17eb40c2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("51b4c9d9-9320-4398-8dc7-2b4279ab3c2e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000001"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000002"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000003"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000004"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000005"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000006"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000007"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000008"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000009"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000010"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000011"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000012"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000013"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000014"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000015"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000016"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000017"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000018"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000019"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000020"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000021"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000022"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000023"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000024"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000025"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000026"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000027"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000028"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000029"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000030"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000031"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000032"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000033"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000034"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000035"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000036"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000037"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000038"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000039"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000040"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000041"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000042"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000043"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000044"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000045"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000046"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000047"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000048"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000049"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000050"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000051"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000052"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000053"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000054"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000055"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000056"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000057"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000058"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000059"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000060"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000061"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000062"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000063"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000064"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000065"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000066"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000067"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000068"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000069"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000070"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000071"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000072"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000073"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000074"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000075"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000076"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000077"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000078"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000079"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000080"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000081"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000082"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000083"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000084"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000085"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000086"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000087"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000088"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000089"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000090"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000091"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000092"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000093"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000094"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000095"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000096"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000097"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000098"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000099"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000100"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000101"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000102"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000103"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000104"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000105"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000106"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000107"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000108"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000109"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000110"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000111"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000112"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000113"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000114"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000115"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000116"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000117"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000118"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000119"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000120"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000121"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000122"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000123"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000124"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000125"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000126"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000127"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000128"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000129"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000130"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000131"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000132"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000133"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000134"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000135"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000136"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000137"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000138"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000139"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000140"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000141"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000142"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000143"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000144"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000145"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000146"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000147"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000148"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000149"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000150"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000151"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000152"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000153"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000154"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000155"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000156"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000157"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000158"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000159"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000160"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000161"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000162"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000163"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000164"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000165"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000166"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000167"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000168"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000169"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000170"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000171"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000172"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000173"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000174"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000175"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000176"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000177"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000178"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000179"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000180"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000181"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000182"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000183"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000184"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000185"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000186"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000187"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000188"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000189"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000190"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000191"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000192"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000193"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000194"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000195"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000196"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000197"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000198"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000199"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000200"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000201"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000202"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000203"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000204"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000205"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000206"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000207"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000208"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000209"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000210"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000211"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000212"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000213"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000214"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000215"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000216"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000217"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000218"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000219"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000220"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000221"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000222"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000223"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000224"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000225"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000226"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000227"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000228"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000229"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000230"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000231"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000232"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000233"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000234"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000235"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000236"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000237"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000238"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000239"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000240"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000241"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000242"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000243"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000244"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000245"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000246"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000247"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000248"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000249"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000250"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000251"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000252"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000253"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000254"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000255"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000256"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000257"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000258"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000259"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000260"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000261"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000262"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000263"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000264"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000265"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000266"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000267"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000268"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000269"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000270"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000271"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000272"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000273"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000274"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000275"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000276"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000277"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000278"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000279"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000280"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000281"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000282"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000283"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000284"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000285"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000286"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000287"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000288"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000289"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000290"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000291"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000292"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000293"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000294"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000295"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000296"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000297"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000298"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000299"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000300"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000301"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000302"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000303"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000304"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000305"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000306"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000307"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000308"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000309"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000310"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000311"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000312"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000313"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000314"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000315"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000316"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000317"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000318"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000319"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000320"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000321"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000322"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000323"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000324"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000325"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000326"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000327"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000328"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000329"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000330"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000331"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000332"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000333"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000334"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000335"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000336"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000337"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000338"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000339"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000340"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000341"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000342"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000343"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000344"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000345"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000346"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000347"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000348"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000349"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000350"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000351"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000352"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000353"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000354"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000355"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000356"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000357"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000358"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000359"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000360"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000361"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000362"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000363"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000364"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000365"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000366"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000367"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000368"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000369"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000370"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000371"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000372"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000373"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000374"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000375"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000376"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000377"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000378"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000379"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000380"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000381"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000382"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000383"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000384"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000385"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000386"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000387"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000388"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000389"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000390"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000391"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000392"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000393"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000394"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000395"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000396"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000397"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000398"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000399"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000400"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000401"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000402"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000403"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000404"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000405"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000406"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000407"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000408"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000409"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000410"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000411"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000412"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000413"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000414"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000415"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000416"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000417"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000418"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000419"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000420"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000421"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000422"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000423"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000424"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000425"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000426"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000427"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000428"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000429"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000430"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000431"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000432"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000433"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000434"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000435"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000436"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000437"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000438"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000439"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000440"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000441"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000442"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000443"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000444"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000445"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000446"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000447"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000448"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000449"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000450"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000451"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000452"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000453"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000454"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000455"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000456"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000457"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000458"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000459"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000460"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000461"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000462"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000463"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000464"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000465"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000466"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000467"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000468"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000469"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000470"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000471"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000472"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000473"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000474"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000475"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000476"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000477"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000478"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000479"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000480"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000481"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000482"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000483"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000484"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000485"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000486"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000487"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000488"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000489"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000490"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000491"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000492"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000493"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000494"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000495"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000496"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000497"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000498"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000499"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000500"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000501"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000502"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000503"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000504"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000505"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000506"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000507"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000508"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000509"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000510"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000511"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000512"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000513"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000514"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000515"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000516"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000517"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000518"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000519"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000520"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000521"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000522"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000523"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000524"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000525"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000526"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000527"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000528"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000529"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000530"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000531"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000532"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000533"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000534"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000535"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000536"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000537"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000538"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000539"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000540"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000541"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000542"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000543"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000544"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000545"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000546"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000547"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000548"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000549"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000550"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000551"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000552"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000553"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000554"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000555"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000556"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000557"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000558"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000559"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000560"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000561"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000562"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000563"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000564"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000565"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000566"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000567"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000568"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000569"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000570"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000571"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000572"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000573"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000574"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000575"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000576"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5220367b-21fd-4a78-ac24-9ea6c32ffde3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52e69220-82b5-48e7-8f49-c03ead60ac76"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52e7f25c-dba8-456d-b691-e854b2b3c3fb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("53c61e49-7fed-48d2-931a-095c83966051"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("545ccf7e-c7a5-424e-8396-538e53d4cf9e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("54849a4f-ae57-4b7c-b1f8-71ed34d995aa"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("55d2471d-c394-4926-992c-74fe20665a63"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("560567e0-3a43-4c4a-8907-13a599b3f376"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("569ff794-5e4e-4e0b-9ccb-8a08c7910215"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("56b8ce72-c26a-44bd-a552-66071ead1055"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("56ddc743-2253-4d9e-9cb9-6adf03cc1562"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("58894a8d-1f59-46c0-b3a7-98d14c4c5aa2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("58e3194f-d8e6-43be-87c5-5effc255579d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5971b29f-0df9-4b19-b35d-47e6a3958c12"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("59988b94-9042-4b79-8336-24a79a29679a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("59d155c0-470a-487d-961d-4487c61a0d74"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a018a85-792f-49a5-8469-cc5d6fd14833"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a1b39e5-ab34-487b-a78a-3591e0c63aa8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a2e4d26-f4b0-4a61-9b84-9222a205202f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a6fcff3-aacd-46dc-b84b-d3369e3b5e3c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a7acb4b-0718-4c97-a683-3de6cdf1be2e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5bc8b482-c70f-4760-8c6f-088f2591d953"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5c7c81f2-0c1d-4fc8-bb53-acdd29b73b68"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d4ecbca-1ef8-4aa0-9cef-324c0d28049f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d6e0318-0ab5-471a-bd18-d4dba2eeb206"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d78d34a-4d87-4fc9-8f98-9262534a60a8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d89e9ef-fee6-4898-9164-20dd10818040"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5dec00fc-80fe-4410-a272-3fcb1725bb52"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5e5d02a3-4e16-4f22-bd0b-042bb248713e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5f0d0363-e564-41bf-a1e3-cb413722a464"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("60ab8567-c837-43de-96bf-aceaddd8f9ef"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("60f65ed3-ddfc-4278-abab-8892ff9f2325"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6104b4b6-3c25-40ff-8325-68cf40a610db"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("61c65e30-d2f1-4b80-9b44-2bf14aa257b8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("61cd91b5-6f41-47de-a50f-f5e752f0b734"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("61dbf221-b3ce-45db-ad29-16bc45261ab6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("626b0f69-3c6c-4856-962f-054893755f28"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("626dde64-b18a-49fb-a413-5fa56e6c6805"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("62bcddb9-6f20-4398-9130-343bec17b840"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("631fedcb-4875-4def-9633-b89d7a04aaa4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("63792d76-cc19-40c4-bd08-a44ec74435b2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("63d5f2d6-68f6-44fa-af64-facb3a8313da"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6494382a-8448-419f-9565-563b6a8d2232"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("657781a7-a5c0-48f3-b8ea-f076f073d9d3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6623efba-8734-46d6-929a-18984d892ff0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("67855cd7-8e24-40d0-9d47-d5ebf07b345b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("67b325a9-d3cb-4270-83e9-8260bd5a40b1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("67b9c827-8f14-46d0-af96-895a799bbc17"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("682a59c7-2515-4201-adb5-cb7a8b9fa836"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("68b530bc-20a8-4570-9aed-66d75e630f8b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("68c99db4-208f-4b5e-b64f-2c47c8f57bc4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("690dbf69-9d1f-496d-9536-0b1107fb5231"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6917f5c3-4626-46e6-89c4-bf50a70d7d21"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6989a14b-0402-451d-85e7-0e74e418a0b5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6ac48b7d-7614-48c7-a9b7-aba875e4a350"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6ac5410e-4a6f-4fb5-a89f-0053a7d12bf1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6b4ddd06-27b2-4f2f-bb52-4339d9869752"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6c0d46e2-12c7-429f-97b6-d5af2028246a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6c4ab70e-662a-4de9-8dde-f1d82707ad0f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6c4b2eec-d24e-4a2d-bb0f-d67128317f06"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6cac54f3-f6de-4772-84c1-3a2132049cd9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6cc6b9e1-4999-44e5-9f4d-95f2bbe4ff2f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6cfe7644-332f-470f-b3f7-2960ada83ad8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d0901a8-575d-469b-884c-82f9e0ee29f8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d0c4063-5bb3-4c7a-a15a-661f576b140a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d1639f1-385d-4461-a74c-3120bfcc8f03"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d346a95-04d0-4015-9ff8-883af1e052a4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d3d2c0a-9fd0-462e-a9e0-a28789303e7e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6eadd7bd-f336-499d-93aa-adb7f8095cb3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6f2ca44e-d722-4c4d-b4b3-97456d3ae463"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6f675f3e-88a1-4641-aeaf-fb1cbdda6210"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6fc36320-f826-4e12-bd7d-53f819f77123"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6fd78816-190c-49c9-a046-a916a3e5096a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6ff2ba7c-2adc-48e9-bc22-56fc9d8840e1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7007375b-1b9d-4652-839a-29e75bb8231d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("717ec181-9fb8-4f3f-a1e3-6fa22e64273c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("71c55beb-dc82-4f99-805b-64dbe17ab587"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("720f23c1-de7a-4f3a-a933-e66756f81085"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("725a7ee5-8b8e-42a1-84b0-9ca55569d565"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("72a13833-7807-469d-a4b2-fd11f1977098"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("72eedc0c-8085-486c-bd33-2b86f84edfb6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("72f229ad-caa2-4a34-989d-1f2366d736bd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("72fcb551-bf36-483d-b12c-67b97dbf2ec0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7337aa1d-6ebe-4f97-b64f-008b9e43aa93"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("73907d92-9a80-4ab2-bc96-54517f78bee0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("73c338fb-9162-41de-96b9-7c14ceb30251"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7489ac04-eb35-4e18-94ef-28d0be7f5cfc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("74b2e374-29e5-411e-8518-4c1f659b7f49"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("75017889-476c-47f2-895e-c3ca065c2498"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("757cce09-ea85-45b8-b54e-4f3f6a0d73fd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("76739f98-6276-49ce-9602-5d4ab3074941"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7689d250-290e-4ad7-a944-6be49f0bbd8f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("76b00e99-10e5-4c0e-ac13-b5f10fd10635"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("76e172eb-f6d5-44f1-9c76-5853fdade679"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77211d09-0c59-4251-bc09-b38de6d6b5c8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("773ce43b-1ff0-40d1-9be9-2a6c0c38e38a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77a43e94-3e18-4caa-880c-b83f77d54e92"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77bebd5b-be73-4222-aeb6-f0ead9ca3d50"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77cdad70-70ad-40b8-a191-15823c066d41"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7915ef1a-9945-4bdd-9043-470195ecdc0d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7947b565-9198-43c3-83c3-4c79760a06e1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7963b40f-a76b-41a6-8807-7235eb7ed63d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7a16d424-a7aa-48c9-a15c-6d8a6f5562ba"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7a423526-e5ee-4d6f-808c-1dc01130a412"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7a65dfbe-b4c9-4ab7-9439-ed1338b79899"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7aa7e83d-f7be-498f-9287-af0e1f7c5802"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7ad04890-c07b-41ab-bd97-b6aa37e778e3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b4228d1-4560-4b77-be49-3a84cbc9de7c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b6cb652-94ab-4b6d-bc9f-2c0f39b63911"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7bffb033-f8a8-4ce8-b04a-54c2b065afbc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7cbb7242-61b1-465c-b7fb-4d7b0aa96ad4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7ce97c7f-8a21-4a47-95b2-20e250a976ed"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7dac1d98-58ae-40f6-bd62-9e155f4e4afd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7e2d13ac-d10a-46e5-9884-c0c8fa540f3b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7ef294b5-4267-4020-98ff-d95c91820a81"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7efe3e1d-84f6-451e-9e4a-a5ba9204bdeb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7f2926f7-162e-4e4e-85f4-cdd4f55315f7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7fbdf401-40c5-4729-9ef3-0b366d99904c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7fee0993-f3de-4d6b-b051-ea7ebfb14e43"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("80455251-7caf-4182-82de-eaa08705d2ff"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("817355c4-bd2d-47e1-89eb-77d2e49091fd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("81fed5df-543e-4808-bf63-50b135ec4c1b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8238dce0-9ff8-4bf9-8399-a228cb89a12e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("824f3d2f-77c9-4e52-9059-1f70382a070e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8287c211-5676-4c02-be6d-e62ff7836540"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("829898ab-5330-44c2-9f1b-a8fc17fda87a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("829c360c-a1ac-4ec7-8e13-b8e4ac7f01ef"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("82c75739-3247-4a93-bcb5-2b4a26160bc0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("836ce14b-45dd-4f16-a75d-d04166697a3b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("839ba238-3fff-4c3b-ae00-9eded558df16"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("83c02e64-810a-41fa-a888-126ad1fe0b45"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8436de64-aea5-4448-8569-6c8de82e2eb4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("84597bdb-452d-4912-978a-cb56447cbed2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("857f07cf-5783-43d5-abf4-11d85307585a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("85a4c99f-35e4-4ca8-85f7-366cd2076525"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("85bbd21a-8566-4959-ba4d-8db0ef4f909a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("863eab66-6996-42c2-9684-a57442728083"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("86a20818-3c5c-4dd7-bc7b-6bc10029a5ab"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("87a800b3-3f95-4d4b-865c-1949d82adbc9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88d766d9-2e0f-459d-bf54-46e338e1d5a9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("892c342c-7ed2-4aa5-ba6a-01ef0bff039e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("893ae3d5-1738-4d43-b185-5950b6423687"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8992744f-2b8e-4d83-b9ef-102e67b29b25"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("89c64bde-b524-47b7-9492-68afbc82229b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("89f9c422-eb9a-4acf-9606-f959ab38992b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8a69c0d9-d0f7-46dd-83d1-db53f669e0d9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8aca9630-fef3-4aed-b099-86a7ce31af3f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8ad24dee-22be-4fc4-9262-52aefd3fb8cb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8be21309-49e7-422e-9ee3-49d6c607b919"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8c662afd-1860-4ff5-9a4e-3aad29d6edb6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8ccf6166-b9c1-44ff-8067-e4cc27ff1db8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8d0525c1-b3c5-4c1b-91a5-5080ecf72ea8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8dfca8a3-4617-4571-a778-74cab3b54dc6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8e89ca4e-9db3-4774-82f0-5ce36573742f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f3d3f93-4a43-4580-b588-be89f1b344e0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f5fd61c-d1e5-414d-aa8e-8fc8865ceb22"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f6217b4-1852-4a21-b33f-f87463d91192"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f916deb-addb-4085-a9dc-fa723d99ef1e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8feb0b44-38d0-4b59-85fe-6d3253358d50"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9084fd9e-60f8-4061-98e0-b3af65bd5c8b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("90fe4284-e343-41a0-a582-f9e8dffe3620"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("91303861-8891-4782-8294-e3cdbf28c7d3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9152193f-727b-47b5-8d74-a2172db40e66"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9156879d-35ea-47bc-bc39-d81852a802e1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("924b2bc3-d5d4-4490-a5f6-eff6a685c829"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("926eff20-75bb-44aa-b000-6dcf1f97e765"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("945a8d2d-c178-4d37-9c05-8dce903eb818"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("94e77a4a-aee6-416d-b7f2-1223ff323e7f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("953e25ef-5846-4f4e-a2ba-092109d5eb08"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95efcaf4-b5aa-49b1-9d2d-6f6ac274d357"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95f84518-392c-4907-bbee-138c9704f133"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("96170a89-54b4-4779-99ad-91cf45346f40"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9650dfba-9dca-4741-a667-bdb77c421428"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9654fe83-0995-4b2c-b76b-a21e1c944104"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("966cb3cc-bd72-48de-ada8-ad475e3c6a3a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("96cbc744-d028-447f-8095-d16a5f9129db"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("96fa2a38-95e6-4906-86e6-c303bcec6e4a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("972d77ec-f24f-4efa-93eb-06a726a82061"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("98048334-85a9-4e51-9f08-c4c428d50ed1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("98117a8f-6421-4da5-b192-9a3f511930a0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("988b0aef-aa94-48cd-a344-7d3333d4a172"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("989c8839-6951-4ca3-a07a-baf81d5b0dfe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("98ecef42-0ddd-4199-9c95-850b6b87d78e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("98f0b383-3e5f-41a1-9b0d-fe19033805c1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("991876b8-6854-46aa-947e-5adadfe6874c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("991e699d-6170-424c-b76a-4ebe99d6e1d0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("99313cf8-5b59-4fec-9719-e7ba7ce8d07d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("99e35e9a-d5b4-4991-9ccb-7b25307365f0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("99ec8677-fa0d-423a-b7e3-8f5da6abf7a0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a8261ad-8899-411c-9707-add4fd3104b7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a9891c6-4f06-473c-a911-a055c827edbd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9aac0fef-bf19-4fcc-a7e8-fa739635785c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9ab9b7e8-e93f-45bf-a461-a0f6309627e8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b230786-8f57-4e06-85a4-7283336dc450"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b88c41d-62f3-4406-8ab4-45c9453b0cda"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9bab6704-0008-4177-85d3-d4202d0970de"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9be9f457-f8c4-4795-84ba-64b7f9069fe8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9c05dad0-b050-4dfa-bd9b-2bcf4c5b3828"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9c318d7c-31bd-48ca-96e2-d2b5f32c3b41"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9cba4c7f-9368-4c80-8a87-3b2c47399bc5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9cfa59bc-c77b-4058-b74e-acccaa29eeb5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9d78085f-46bf-47dd-9d8d-f4097a0f99ac"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9dfb8408-633f-48a7-8c37-3ef30f945eb0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9e8fbbf1-ccd6-4a53-a92e-ca780ec99e6f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9ec4582f-8977-483f-897f-b00fac69c83c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9f06aefc-5b8d-48f5-b69e-4ea15e436413"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9f7b29bb-0350-4fb5-b1f3-5f31024cf142"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9fc62452-8a78-453c-a0d7-ff27dc6f1e15"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a0171452-751f-45ab-984b-7c2b0abaac58"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a04454f1-6332-4895-967c-bd296b16f63a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a20a7a0c-69e9-490a-b4a0-22fbabb91137"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a26fd0c2-2af4-4404-add1-9376a90e6842"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a283e6e8-b9ed-4da4-b4dc-a5b76b1a6b07"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a2e86be4-11b9-41e9-a82c-6c61caa7baf6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a30ed9c5-089f-4455-9ba4-924375583552"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a30fe4f4-19bc-4ee5-8978-6762fe1a7e11"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a35d4a00-040e-4b0b-b748-5164f669a7d1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a363610e-3d39-4882-bdd8-4c10c69ab008"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a3b8e083-476a-433f-b9ba-8de6c71d3987"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a52bbf7c-baf4-439e-b76e-c61c4360342e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a52ca134-80d9-457f-9834-aa851afe710f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a585bf08-ee37-40b6-b8b8-3c118816f683"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a644a291-0219-4d4e-8a97-03964e90dfd0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a67cac3b-c555-4e95-82ac-ac9bc006c117"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a68bf77a-13f8-4472-ae56-ae922891d17f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a6c45a0a-8b0d-4508-bdd8-d8d2bc8027ca"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a6ffd7ec-e8e8-413f-8855-d7db6fcea8a0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a74d7b03-d1d6-432b-abb3-1871d8a1c305"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a74f3d8b-abc8-45c3-aee0-2110b4477dab"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a8562dc1-0e53-48c3-b023-9f2e7cea4209"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a883ac89-76e7-496c-9646-22f47bf5f1fa"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a88ab841-2554-4e54-b0a1-df62925ae010"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a8d1491f-edba-4c2a-915f-507c6633c3f5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a955e608-82c4-4df4-a4a5-1a102ad7ec52"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a99766ce-7de2-4652-bb90-e1218354f465"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aadad362-9368-438c-a750-bb1d76b110c0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab377b64-e955-498e-a9d1-464b59444463"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab3a6e39-8408-46a8-803a-766b822244a0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab833f03-90a0-41f9-8731-978e44f8194d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aba1ba5b-70f6-4f43-a402-c7292f08cf44"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abb9465e-4e44-4819-bc2e-7025d513eee3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abd76b02-af66-4908-bc75-19b9cf608d52"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ac0e4d52-da0d-4a70-a5d8-18a9aa225be6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ac898ec5-e570-4af6-802b-d7d88bd7a180"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("adcb3f21-4eec-4e5b-b1e4-157995e143b2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ae3ab520-7cd8-414a-95b0-ed127b4aa2ef"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ae77fb70-6109-4476-98c4-e389e98cfe1d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aefe4866-2a5b-4f12-aee6-efa61341b549"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("af0f01cd-815d-4d41-9fc7-35434a04480e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("af25b778-3c4b-4004-8d82-c99a87a941c2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aff20e60-ac10-47c9-a106-cbbad6294d62"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b0eab117-826e-437a-8a33-cda95498306d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b1553c84-23ef-441e-beee-11b9094dbef3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b1e0bb19-0e2b-4e6e-982b-d4cb384336ce"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b2121b2f-a67d-4436-90a6-e851582fc8e1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b32a6747-d042-42d1-bc3a-82510395069a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b3d3d7a6-8d6e-4509-adcd-7bc65e0938e3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b3e8acb5-2e7e-47f4-a6b4-f68fac55f902"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b406507a-4900-44a9-89bf-5615b20963ec"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b40d990a-10a2-4da5-8d05-29449606e6e9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b4112462-7d49-478e-bc50-adc81aa08174"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b42d3f72-13ca-4bc8-b897-9db19bd693cb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b4d372a0-746f-48b8-abe9-08410eef047d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b4d90cdd-41ae-45c1-871e-f94f510472ae"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b5103665-5c3f-456b-90e3-1d7d3db9e1bb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b65907a1-ebc0-4f69-a19e-09675a87446a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b70ee639-e760-477f-9454-0afb85f202fb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b73be9ce-c277-4991-9434-8eaf90fa67c5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b7b5bdbf-3762-43a7-9ffc-6c0c9dcab281"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b7baa524-a2f9-4015-ae40-253750965448"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b7bc2c74-6feb-437e-aabd-005c4067a671"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b7f441af-45b2-448e-b4ca-1723a10abbfb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b8b572f7-a6e9-4691-96d1-5c7b4cd89c3f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b9a0a290-f90d-4a1c-a040-088af6e7084c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b9be95bb-9eb4-4578-9b4f-0834532df91e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ba5ea50a-0efe-4089-b0b9-1115d49a1861"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ba8c0ea3-20f1-43a5-88e3-97653e35be29"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("baa3ce09-a01f-4a87-85c9-ce77f0e9046d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("baa6b4f1-ab41-4e3d-b2fe-49ecfea16367"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bade9f88-5966-46d8-af48-4a53f6ed2ac1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bafb22d8-37a3-4ca0-afba-73ebff24e3e2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bb414510-25ce-4807-8393-da74a697c946"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbb17f81-acba-4e03-be89-846c12557791"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbcb68ff-f912-46cc-ab6f-d7a37b8c4fcd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bc0c2d87-d206-4c7c-9851-44d69dbce151"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bc75eb0e-1a37-42bc-982d-6ad8b1f4a2c0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bcc9c57a-1025-4789-99e5-043a302f7866"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bd51c0c7-eaa4-4311-a88f-cbe9104f6617"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("be8cf534-2bc0-4114-8804-1898a624b2b9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bed4eccc-1d32-42b6-9005-3c4c3b964a46"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bee796aa-8146-409e-8792-834315d4afda"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf9934f3-9b9b-4a10-bd38-871d2ea17a2f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf9c69bd-dd68-4fb6-85d9-17c43359c447"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf9ed210-5464-4a25-88bd-2efea3e00929"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bfc036ce-c1b8-4688-872b-359ad84ee73a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c03eb545-85e6-468d-a70f-ffec03ec27d9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c082bd65-1df7-4f9e-a96a-c0293c61c59f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c0e157df-7898-4cc2-87e6-eb52e844b38a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c10ae965-efe4-4132-af3a-d5873a6441fa"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c120df5f-da58-4f40-ae90-c513d50bad57"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1453996-58f8-46e6-bb8f-03d71514bf0b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1509923-9736-48f2-b507-65161bffbe90"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1845dd3-c4f2-471a-9de7-dd80dcaee745"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1c5ba2b-faa5-4e7c-a9da-82bb9bd9a77b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1d81723-1ea8-4465-96f5-e88d257b6aca"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1edbdd4-e5e6-4057-b360-3138a95dd146"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c206fbed-de6a-4939-ab71-58b2608b38d1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c26fb3ab-072f-4fb1-afec-fe95711f72b3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2bebfe1-18ad-4583-b30d-40cea622656f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c3578f03-ea8e-4d2d-8173-1c0b084b455d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c381fdee-07f1-484e-a772-093300c14636"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c3aadee8-5ec8-42c9-a279-47a0e0ecd308"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c3cc03ab-c2d3-4439-adca-d72ec2abc5d6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c3d22ad1-3839-4178-b547-e2aa0ef155d1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c4db9ab6-44a4-4b73-8349-0c80b72b46b3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5ab8cab-e134-4564-959d-a7aee656a496"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5deb73f-6028-4955-a5b0-55b4af44f6e1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c65baecf-6df1-435f-a6ce-a859844f0cdb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c71e0271-8fbb-4dbd-8d71-12fc8f87b71a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c743102b-a0b7-4e6b-b515-7c9559b08b03"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c7818010-1622-43a9-81a2-d6e83ef2202d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c7cbf075-bb71-47db-a70c-24ee1c9e8571"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c8de2d1b-502f-4d10-8c92-083a67344c47"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c959869d-f8e5-4a8d-a263-3d8d6ff05a89"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c9b64e80-1ad4-4476-b0a5-d3dff37cc2c3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca69d287-c82e-4ba4-b0b8-9eabef369201"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca7a0728-f817-4125-aaef-d8b8799fefcf"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca7e2ea5-7cac-42d1-8ced-966a213c6562"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb52c317-595a-4b4b-bf49-8b37eaca4a87"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb75d150-8a53-49c0-a935-63cdd843a440"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cbae421c-c4fe-4150-916d-27f1b370322d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cbc4a160-c647-4c8d-8bcb-0ca5d57e3ba8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cbdc978f-0f4d-436c-ba0b-f26d408517c8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cc48f840-9b7c-4a3e-9957-855844e7e0bb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cc797cd6-8d1a-4c00-b66c-aa69023e0ba4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ccb8617c-d1c4-42f0-91e0-4b5ccb497e5a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ccca5b4e-a539-40ce-a3f8-5c2ca26a29fe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd003b5e-94e2-4e90-b19d-8b0ae59936ad"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd4abf01-5c94-4ef1-bb07-50d80292311c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce4adaad-5fac-43e1-8c71-d4c325864a82"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce6638a5-a16f-4d39-8e9a-ed9d9a43bce7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce776dcd-da1e-4d57-a030-462da9eed83f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce82757e-fd46-46f8-8cae-f15686f89438"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cea10df0-242b-4a53-b9c1-66dd6ccc42d5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cf28fe72-646b-4711-b699-38ea9dc44e1a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cfb378ab-afdf-4484-8263-47777e53e23c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d00cd57e-f7f7-4165-8e01-4fa5b1e0e50b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d03abafd-b740-4ef5-a709-41c22b1d5f9e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d0dc613f-011e-421f-9cf2-a4ccfcdd184c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d16cea31-413c-40c0-bd0c-6756eaef72da"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d177cc96-a137-477e-90ab-01cdfe947dad"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2171521-9ea3-4d58-9b86-30c5c356904d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2868661-9d2a-480b-a2bf-56b7439c7e98"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2e84996-4bdc-4915-b126-fbeb409716a9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d301828f-5361-4ad3-b1ab-6a34520aa425"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d3cd81d6-d7cd-4e24-b4ab-fca369cbc532"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d40c660a-853b-4f27-bf40-e525409811de"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d41dc445-c082-4ffb-a307-a0a5c35cb8ff"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d475c7c3-a442-43ca-b174-4e109fc3a63a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d5b022f2-4af9-4539-8d63-b2057dca51ad"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d5cd4b31-5313-41df-a50e-3b393320d80e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d601a1f8-3452-443a-a599-137f30d29b63"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d687bd92-0b37-452c-96ce-c0f3be44f178"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d6e459a3-16e3-411f-8c3e-8810171a1694"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d72b9550-2471-43f8-8414-2b3bfae917ec"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d741096c-39db-4856-a151-b8e6304e04fe"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d747f2a1-572c-45de-88af-1983faed6c81"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d7aaecf5-3327-453f-b627-c673ca55384f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d889dc8e-ab13-4fd0-9c31-960934966e56"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9852df8-f38c-4540-9c08-ef4f9a1ad33b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9c778bd-03f1-4df8-af23-e661f3b1a125"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9fc6fdc-2ffb-4bd4-af82-d91afecdc2b2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("da533379-f73b-457a-a46a-efd10be02ee4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("db77b66f-1451-4f43-b472-2b01f574e628"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc188967-d599-4c3f-9ae3-23f201bbba85"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dcf7f5cc-12f8-4419-a07f-c39d2d8bbee4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd3e4ee1-68e1-4ecf-b344-800b85fdcca0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dda4131f-bee9-4f33-aec2-9cafff51e587"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddcec500-52e9-4d2a-adf3-e961af46b90b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ded1c4d0-acb2-4631-b5b7-c9bc144c8884"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("defd300b-5649-4a0f-8848-375eac71e15a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("df3ebe62-8f79-4c58-92b3-c92233808e71"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("df6d95d8-7b9f-4b40-b8c3-b5043e8d7747"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e001b31f-dee9-4dc2-b974-d42ae14fe929"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e056ae90-666d-4b93-a909-6afcb6d12ea5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e0be43c5-01be-47f9-a53c-e8846f792fa8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e105122d-d12d-4a16-b02e-bae5a17d6c44"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e141c78f-1aef-463e-a59d-4aaedbdf2858"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e1768d29-e5bf-433d-8463-cceeaf9d43ba"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e1ebc8bd-1952-4c42-b3c4-10b6a5c445d4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e22fa1c1-7c5e-4e49-b7db-5b773de5986b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e257c305-e574-4124-ae53-b79b5bc402c3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e2594626-4699-4694-a266-f52e89c96db1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e2b5f47c-7407-4941-821a-696f13580a32"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e32f52c3-b5c2-4ce5-8f99-93e8267b2ab5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e3571af4-ff03-4ef2-ac48-b28072645f78"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e358dbb9-8e21-4982-bee5-985fae95cd50"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e35eeacc-154a-4683-a1be-a80db49cbb24"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e3b3c452-e2ce-41be-8445-de91a0eafa07"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4368d3d-f288-4df8-ba76-59bedb13f41d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4598587-04d4-4cbb-978d-ebfc3d6aa3cc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e483dc52-3eab-4d51-af7c-c948a731c481"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4d5831f-9a38-467f-aea7-9b156e17b5f1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4d77d5e-dd9d-49f1-90b1-5d31c11d0fd0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e52b06af-1609-490f-aeba-3074af5c431f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e591842c-88e8-4942-b8b0-db5e73d5f4ad"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e59b85aa-855b-46c5-b374-571a27ac2098"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e5df2ee3-231f-4819-89e1-2fef578906a3"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e662b1e7-d162-4789-9e4e-bdd0d7b62172"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e76c0745-a6d1-46d6-80e8-883c9c0e747d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e77b6ebc-457b-48cb-a85f-52ac3d188dcb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e7e8c41b-c535-4539-9565-e93a8baf7bbc"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e955bb2f-5186-42de-94cf-a11f9cb5691f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e95d42c9-5c26-4dfa-b7fe-7d234b1d63a1"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e9921cec-8c92-4d66-86e6-f84df8835afb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ea63c09f-dc3f-49e0-8509-d435c1a25f04"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ea719431-8cc4-4559-ab31-39b9cb8a89d8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ea921132-c270-4a95-84c5-151f69859bf4"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eaf5bf09-3a32-4a99-af41-cd27ff9d2925"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eb1217a8-39f9-44f5-a186-cb9b2cf4f780"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec0a6b67-2f9c-4f3a-84f4-5d49caa3f555"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec29d3bf-2186-4a54-bb15-9acc14cf2033"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec571a67-dc34-4c8c-94fb-d007b519914c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecb07f00-4755-4ac7-b11c-15f1096b1040"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecb1967f-beab-4894-943e-394b3c9117cb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecb2ba61-6e06-4970-9b24-5a3d961f5714"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecd6ec00-531f-4d16-a7fd-381e466a6f38"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed390ab8-7e35-4ce6-8cae-1c57a4fa7623"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed51a849-09b8-47f8-8c28-d4bfa11d343f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed63ba28-4165-48c5-82b5-d98937f1ea9d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed6a11d7-c24f-400d-984a-32e093530948"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed8e4d7b-8483-4c83-9437-8f283bf33ca7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ee27f000-09d8-463a-85e3-a60e23e5fe67"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eec5f209-bdb2-46d6-9861-14112a288cc9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef3a3b54-0c88-492b-9472-8bc965770a5a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef66e833-d403-4dd0-b270-5a81012913d6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef9a8552-8712-4ba5-acaf-08b4785d067e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f0644fa7-a443-4002-92e6-849d6c1fedd2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f13cca20-ba72-4b28-bef3-0d47869f8af9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f1c59dd4-ea9c-4309-a2c2-53779925a105"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2293ad1-7bda-404a-b5ba-b70da32543d5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f259c428-8dac-4a2b-b1d4-ca418a81995c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2a294c0-55c2-43f7-a343-650b1cc55816"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2b95828-e804-4f40-9728-a7175f43c0c9"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f331b56e-fe97-4dc3-80cb-feaac7cf2771"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f348c32a-de9b-4d76-9c55-a4f51249ce17"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f3b0e525-ed24-450c-8f5a-4615cc6d270a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f443fe9c-7530-45b8-af4f-6eed852b10da"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f4d17ba5-3da6-4377-a50a-97e3b28c01ce"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f5283651-1110-4b42-95d7-f00d6191aadd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f6584d3d-36e1-4180-93d0-aa886cec61bb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f664e4ba-1fbb-4297-9d07-60bcfee9671f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f677ccf1-7f80-4dc4-b7e8-efd620a39324"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f6be66e6-08cd-4522-93e3-a7b99ca9fb12"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f70f733e-157f-48e1-a295-c366eeecf298"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f76c18db-8644-4d91-be9f-8ad9c172c858"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f779f4ec-1e54-4092-babd-66d7ff83f462"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f78f12d6-9542-45cd-8045-b30dc98089d0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7a2ecc3-0944-4fee-9365-277d00df890b"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7aca766-9759-4400-9c8e-6276b7013f99"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f89866b0-968f-4576-9cc8-4e594f323c7a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f8b313c8-aa33-4d13-adda-58cc0b74c249"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f904a018-938e-47a9-a6ed-6458eb65c6b6"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f9105e05-8307-4d33-a8b7-0d2082b5ac33"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f95a691b-c532-4b41-b8f7-89cf4d6b065d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fa8fee07-e3ef-4be0-a4e4-f3dbd4e3c546"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("faf70235-6025-4200-9e03-66078b55a26a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fb359fea-8410-4681-9750-043e3efbce1c"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fb444b8d-0a06-48b9-9852-55797443841d"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fb87ef95-9f38-4742-af14-edab06e1d4b5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fbfb4cf1-7926-42fa-a0f0-44a2ae470eb5"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fc5911b7-7924-428e-87d3-7fa722bf2737"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fc731547-8cb5-440e-8975-3baacbffcdd7"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fcf30004-723a-464d-ba53-442ec79b1867"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fd29172b-3dae-46bc-b98d-186378bf122a"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fd3237a4-b4c4-4a69-975a-f66e3d36ee2f"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fddfd5a0-9bb3-439e-b3a0-f9cf064ae731"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fe230d42-bd15-4180-a972-ec83a40cb279"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fee5d4bd-a836-41a7-a717-4fb57422d2f0"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff0a3b47-9679-47e4-94c6-6250e0291aa2"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff5f6c29-2d7f-44a3-9d35-f89efd75dbdd"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff61c9b7-b56a-4f61-9cb1-f02e0d40f17e"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff93dcc6-9390-4859-9794-0efa7ead5af8"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ffb783aa-f1a6-4745-a576-ae3839eea255"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ffcb1225-57fb-4f67-bc32-b8802da7c1fb"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ffd8da8c-4dd5-4997-9b41-4514e6f7ca75"),
                columns: new[] { "ChecklistNumber", "Part" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChecklistNumber",
                table: "PredefinedChecklistItems");

            migrationBuilder.DropColumn(
                name: "Part",
                table: "PredefinedChecklistItems");
        }
    }
}
