using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewChecklist32Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, delete all ActivityCheckListItems that reference the PredefinedChecklistItems we're about to delete
            // This prevents foreign key constraint violations
            migrationBuilder.Sql(@"
                DELETE FROM ActivityCheckListItems
                WHERE PredefinedChecklistItemId IN (
                    SELECT PredefinedItemId 
                    FROM PredefinedChecklistItems 
                    WHERE ChecklistNumber = 32
                )
            ");

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0dd86d11-6cf6-42b4-b489-20e51c6a5d3c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0e79780d-627c-4de4-85ef-caee115df80f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("103277d0-ab14-4d98-b096-c39cd2a39bad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("105594e5-688b-46e5-b08d-01eac81171c3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1489ed9f-dd78-4c1c-bf36-a226bbef0f20"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("14b4062e-6fb6-4172-a9ab-ee5d2f19e6be"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1c897f5d-0399-4b61-9993-1f9a58c10692"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1e4d0c53-28fd-439b-9eb2-cae591c45edc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20bf8edf-7344-404c-9632-329cca228deb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("24af3611-7a4d-4aa3-9c67-eb97775109d4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2603664d-ce1f-4e14-8bd6-126947a083e6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c71ee06-19f7-4d83-8831-c9231b4263b4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2f04f0df-4ed9-4372-98b9-3ced3231f320"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3c9685a6-c67d-4547-b638-14537a9d5187"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("41f81e13-30db-414e-9a13-98d6374a0ef0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52f35c66-7592-4374-84bf-40f96d56bcaf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5323fb3c-6fbb-4a90-8b09-bdf91bf90f94"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("570418a2-3efe-4595-82e5-4f685fa896b7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5cbde6d5-b477-48fc-864b-2754498a3a72"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("60759b80-6ff6-4f13-9dc8-5a4cefc624e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("66c1c064-46ba-43ff-8068-3d98c5981208"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6745c653-2210-4851-9bb0-9795e0f1c068"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6beccd5d-5bdb-424a-865a-bb2e7a27155b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7d750f4f-1a1e-4812-b530-da3aff8b85ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7f11eb56-9c85-41c4-9c1d-8ccaf372f749"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8241fae6-cf3b-44be-bf82-347034fa8306"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("847a6d48-9ae3-4e44-a8d6-7962fabb9bf2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8bdb8393-7c9c-4a06-9911-7b5d55734d4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8d1791a0-cae1-4677-8687-b9dfc3414fbe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9013c678-1bc4-4f73-bc3f-5effa773178d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("904dafc2-7584-4fa0-b097-8a9aafc93c74"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("90d9276f-8f05-4498-aeaa-0284856092f3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95d9bf14-1a5d-483a-a930-4baf527e5de0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b4ad754-7bb9-429b-ba6d-e3b05a88deb0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a8f9f6f7-3e31-42ce-82b7-18a269871238"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a92a8534-4147-4b3d-8caa-f41097260e6b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abb96f1e-7476-4265-a9c6-9de5a65721f7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ac9bc775-7085-45c4-acb5-9042bf159804"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ada0df0c-3ded-4e14-b789-cc134259d9ae"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aea89b5f-cc96-47b0-bca7-e1cff4fe18f6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("af7600e4-0567-4e12-8d01-6cba76a3059c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b1813260-2885-4243-8122-a882a1baff00"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b6fc4834-4a37-4231-8377-dc0308d6181f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b858ff5d-deec-4b46-a754-a26378a5885c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbd1169f-a962-48a4-a166-e9d7a99c49cd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf6b0e94-1c82-4ef4-b6da-52570d087801"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c14f2f9c-5efe-4a4e-82a5-c0e29af69edd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1ff045d-55cd-44b1-beb9-26b3e0fa566b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2389679-0f87-407b-89c6-a5709223b2fe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5dd4075-5da1-458f-ac95-18112b43f1b0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cadcc053-801d-48bf-9527-cb6d52d98928"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb8baaf8-1d8e-4230-b4df-0c3ac94fd0d4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd09e3bc-301a-4dbb-b90d-ba68cc56f551"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d402f880-5fa5-4162-ac63-fbfaf810610f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d6023c45-7825-4d5d-aa49-91bb08b09fc0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d91b6f34-7c68-447a-94a5-03c083f46a4c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd51c76d-cda3-4be1-82ad-2a563f19798d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddb909e5-bf6a-4732-a472-035524973db6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("df99b660-0f74-4072-81d3-5ccdb8b6c941"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eac92d42-c6d9-477f-9f35-b346da9fd8ba"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eb06003c-f9de-4e67-92dc-4fee24df8b7a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef21b2a7-8594-4272-883c-74094a99a543"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f23db52c-5216-4c9d-ab46-7520823ef225"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f45f776f-1819-4b89-a33d-42aef2b68a5e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f70f6090-7425-4023-bb93-8286eceec229"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f77b091e-68f1-4a39-8a44-723fb0291312"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7b2e0df-941b-4231-bf83-710047d2519a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fcb5a35d-2c3c-4bc7-b054-9ccc2d0efd15"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("10f470e4-bfa7-4c2f-96a5-8bff23abbfa2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("48962238-ba77-431b-93cb-104d24939ec0"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"));

            migrationBuilder.InsertData(
                table: "ChecklistSections",
                columns: new[] { "ChecklistSectionId", "ChecklistId", "CreatedDate", "IsActive", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("259689a3-d62f-483d-8476-ef8e28d212a2"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Final Inspection - Electrical" },
                    { new Guid("38289349-435f-423b-bade-eeb1f3e96603"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Final Inspection - Fire Fighting" },
                    { new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, "Final Inspection - Drainage" },
                    { new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 9, "Final Inspection - Wiring Devices" },
                    { new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Final Inspection - HVAC" },
                    { new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "Final Inspection - Wire, Cables, Conduits and accessories" },
                    { new Guid("8d1fbfc6-9e1a-43fd-8830-08b2c2fa21d8"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "Final Inspection - Risers" },
                    { new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Final Inspection - Water Supply" },
                    { new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 11, "Final Inspection - Light Fittings" },
                    { new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Final Inspection - Chilled Water Pipes" }
                });

            migrationBuilder.UpdateData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("98448459-61c2-4ab7-9e77-94cd67601168"),
                column: "Name",
                value: "Check List for Pre-Loading of Completed Precast Modular (MEP & Electrical)");

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                columns: new[] { "ChecklistSectionId", "Description", "Reference", "Sequence" },
                values: new object[] { new Guid("8d1fbfc6-9e1a-43fd-8830-08b2c2fa21d8"), "Check Identification tag of the modular", "REF-0001", 1 });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "ChecklistNumber", "ChecklistSectionId", "CreatedDate", "Description", "IsActive", "Part", "Reference", "Sequence" },
                values: new object[,]
                {
                    { new Guid("025a157c-d1d2-46cb-9a70-6af87031c935"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "WL-1 Wall mounted recessed balcony light", true, 3, "REF-0078", 78 },
                    { new Guid("04b4344e-91bd-4386-8218-626d7abddba0"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All wires pulled as per the approved Drawings.", true, null, "REF-0060", 60 },
                    { new Guid("09749601-1157-43d5-bd9b-43ae7994d520"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doorbell Switch behind Room", true, 4, "REF-0047", 47 },
                    { new Guid("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL Surface Mounted Linear light electrical Room & Garbage room", true, 3, "REF-0077", 77 },
                    { new Guid("0cd699e5-ca4d-4797-93a1-029308ade190"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A, 2G with Double Socket outlet with Neon indicator", true, 4, "REF-0052", 52 },
                    { new Guid("113778e7-dca1-43e7-9539-22c3caf843dc"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "45 A, D.P switch with Neon outlet", true, 4, "REF-0054", 54 },
                    { new Guid("147809cd-7f6e-4f39-9426-d74b9db3b490"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Heat detector", true, 2, "REF-0065", 65 },
                    { new Guid("15c7f5fd-b05c-49a0-a35f-a548d9f86126"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "6A,1G, 1 Way switch", true, 4, "REF-0042", 42 },
                    { new Guid("17780e95-9d21-43ee-9cea-df7209a59b59"), 32, new Guid("38289349-435f-423b-bade-eeb1f3e96603"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Primer paint", true, null, "REF-0017", 17 },
                    { new Guid("190be8df-7b70-4c79-a737-1df209ba7e6d"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Grills/Diffuser", true, 1, "REF-0011", 11 },
                    { new Guid("1af7c222-6b43-43a7-868b-c3f41cb29d46"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fresh Air Duct", true, null, "REF-0005", 5 },
                    { new Guid("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"), 32, new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation COP Pipes", true, null, "REF-0029", 29 },
                    { new Guid("1f3f8664-1c77-4f87-b7b7-87b7c7250569"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0023", 23 },
                    { new Guid("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"), 32, new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, null, "REF-0012", 12 },
                    { new Guid("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL Spot Ceiling Mounted light Corridor", true, 3, "REF-0074", 74 },
                    { new Guid("2bdab2ef-3541-4e57-bd37-c092e66cc603"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMU", true, null, "REF-0071", 71 },
                    { new Guid("2d7fbe6b-2411-4413-9294-1b807e6da3f0"), 32, new Guid("259689a3-d62f-483d-8476-ef8e28d212a2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For DB/ Panel Door", true, null, "REF-0040", 40 },
                    { new Guid("32ca540d-0700-4e5d-9b33-fe084dbecf04"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chilled Water pipe", true, null, "REF-0036", 36 },
                    { new Guid("37360f3d-9505-4382-a4c3-e3b7ce068acd"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL Ceiling Mounted Light Kitchen area", true, 3, "REF-0073", 73 },
                    { new Guid("37901174-a631-4d12-8335-dda5bd07b626"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Cable pulled", true, null, "REF-0063", 63 },
                    { new Guid("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hot water pipes insulated", true, null, "REF-0024", 24 },
                    { new Guid("387e21fd-ad8c-432a-b08e-059a790522a4"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Alarm Control panel JL1/Y room", true, 4, "REF-0055", 55 },
                    { new Guid("3b481135-7d60-408f-81d9-25cf9363ee8d"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duct Riser and connection", true, null, "REF-0038", 38 },
                    { new Guid("3d2e1461-488c-46ef-b662-019f097a515a"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gate Valve installation", true, null, "REF-0025", 25 },
                    { new Guid("408dfdc3-f5fb-460a-bfa6-035782f57535"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Exhaust Air Duct", true, null, "REF-0006", 6 },
                    { new Guid("498e4400-83af-4482-932f-8716bc72aaa1"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waste Pipe", true, null, "REF-0033", 33 },
                    { new Guid("4a3986e4-0079-4e49-81e9-7973dd0b888f"), 32, new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PICV Installation with insulation box", true, null, "REF-0016", 16 },
                    { new Guid("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"), 32, new Guid("38289349-435f-423b-bade-eeb1f3e96603"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0020", 20 },
                    { new Guid("4b602baf-0fcf-4985-a31d-64707c18558a"), 32, new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piping Leak test", true, null, "REF-0030", 30 },
                    { new Guid("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Door Bell, 230 V Electromechanical chime", true, 4, "REF-0058", 58 },
                    { new Guid("5677ef27-1524-4d76-be31-60682e67dcc1"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fan Coil Unit", true, null, "REF-0009", 9 },
                    { new Guid("59136623-44ad-4d1d-bb7c-92dd241da546"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Kitchen Hood and Flexible Duct", true, null, "REF-0007", 7 },
                    { new Guid("5b9f0de3-a164-47c0-9ae3-2def089f998e"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "6/10 A Cable pulled", true, null, "REF-0061", 61 },
                    { new Guid("5d103945-bcd7-484c-a3e1-ea63598bad72"), 32, new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Pipe Insulation and adhesive", true, null, "REF-0014", 14 },
                    { new Guid("5d730e37-85be-462c-9170-41a5e3d711b2"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL Ceiling Mounted Light Living and Bed Room", true, 3, "REF-0072", 72 },
                    { new Guid("64bc0366-95f0-4de8-a817-4dba4fdf20c2"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sensors", true, 6, "REF-0066", 66 },
                    { new Guid("67ddfe9b-8c23-4cea-8216-22a5168c94d3"), 32, new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Cleanout (FCO)", true, null, "REF-0027", 27 },
                    { new Guid("683581ae-9676-4651-b627-28663576d682"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Firefighting Pipe", true, null, "REF-0037", 37 },
                    { new Guid("68f28143-1d00-4a4e-b853-7724bcbc46ba"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel Tag and Identification", true, null, "REF-0067", 67 },
                    { new Guid("6deff94b-68f3-4827-8af9-06b37affbaec"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "20 A, D.P, Smoke/ Solenoid Flex Outlet", true, 4, "REF-0059", 59 },
                    { new Guid("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"), 32, new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation of pipe", true, null, "REF-0013", 13 },
                    { new Guid("6f12fb28-c711-4937-8341-1a030c6a996f"), 32, new Guid("8d1fbfc6-9e1a-43fd-8830-08b2c2fa21d8"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visually inspect the MEP Services for any defects or damages", true, null, "REF-0002", 2 },
                    { new Guid("6f868e6e-7a11-4c65-9cef-f2aeae768964"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wireless Switch Junior & Linen Room", true, 4, "REF-0046", 46 },
                    { new Guid("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vent Pipe", true, null, "REF-0034", 34 },
                    { new Guid("72cf8727-5ae5-4c63-9640-483853577ea9"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thermostat", true, 5, "REF-0070", 70 },
                    { new Guid("7762e342-6be1-4849-a614-b59106040118"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL Led Strip Light Under Kitchen Cabinet", true, 3, "REF-0076", 76 },
                    { new Guid("7852e526-2969-49ea-8ca1-4028a99d0679"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Supply Duct", true, null, "REF-0003", 3 },
                    { new Guid("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, null, "REF-0021", 21 },
                    { new Guid("8323dcd7-2fed-41dd-8b0e-aa21393ee086"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Return Duct", true, null, "REF-0004", 4 },
                    { new Guid("83770710-147d-46d3-8b71-414f5c09b677"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A, 1G with Single (Unswitched) Socket outlet for Fridge", true, 4, "REF-0053", 53 },
                    { new Guid("861dad54-79db-42f7-ab37-68466a23f743"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soil Pipe", true, null, "REF-0032", 32 },
                    { new Guid("8fe1b6f2-b823-4677-9827-23fa5f523e61"), 32, new Guid("259689a3-d62f-483d-8476-ef8e28d212a2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For DB/ Panel Door", true, null, "REF-0041", 41 },
                    { new Guid("9c4da2e4-6734-4e94-9037-d2e8f88f931f"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A, Outlet for Washing Machine", true, 4, "REF-0057", 57 },
                    { new Guid("9e3a024d-57ad-46ce-b27b-1fc403f68b15"), 32, new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Drain", true, null, "REF-0028", 28 },
                    { new Guid("a62eed08-be48-4b31-880a-c478c8e3b671"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Hammer Arrestor installed in the approved location", true, null, "REF-0022", 22 },
                    { new Guid("a65828cb-6694-46ff-b252-f92626d6d6c3"), 32, new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen sink and accessories", true, null, "REF-0026", 26 },
                    { new Guid("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "6A,2G, 1 Way switch", true, 4, "REF-0043", 43 },
                    { new Guid("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Single Data Outlet", true, 4, "REF-0049", 49 },
                    { new Guid("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel Insulation and termination", true, null, "REF-0069", 69 },
                    { new Guid("ba041522-26d3-48cd-acae-729cb0b667ab"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A, Switched Flex outlet with Neon indicator for Hood", true, 4, "REF-0056", 56 },
                    { new Guid("c468d5dd-3a81-4512-ac31-60aa0d667abc"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fire Damper and Back Draft Damper", true, null, "REF-0010", 10 },
                    { new Guid("ca1ab126-cd47-4d96-9819-de6c0c931649"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire alarm Cable pulled", true, null, "REF-0062", 62 },
                    { new Guid("cbb97e1d-c8df-443e-bbf7-84e98c937d71"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smoke detector", true, 2, "REF-0064", 64 },
                    { new Guid("cce7b248-d364-41fa-84f4-489bbe5ba3f6"), 32, new Guid("38289349-435f-423b-bade-eeb1f3e96603"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Paint final coating", true, null, "REF-0018", 18 },
                    { new Guid("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"), 32, new Guid("38289349-435f-423b-bade-eeb1f3e96603"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Sprinklers", true, null, "REF-0019", 19 },
                    { new Guid("d938728b-f6f4-4006-ad53-df88d75800ae"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "WL-1 Wall mounted shelf light above Mirror", true, 3, "REF-0079", 79 },
                    { new Guid("da4112c9-e84a-48ed-9ee0-67ddf655fd04"), 32, new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves provided for drain pipes outlets", true, null, "REF-0031", 31 },
                    { new Guid("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "6A,3G, 1 Way switch", true, 4, "REF-0044", 44 },
                    { new Guid("ddf6192a-38b4-4eab-b81b-ac7178214cc1"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "6A,1G, 2 Way switch", true, 4, "REF-0045", 45 },
                    { new Guid("de5ae193-0f9a-4a87-800b-186dd1ad03e2"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Twin Data Outlet", true, 4, "REF-0050", 50 },
                    { new Guid("de8bc96d-aaa0-4176-8415-1f5c222116c6"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A, 1G with Single Socket outlet with Neon indicator", true, 4, "REF-0051", 51 },
                    { new Guid("e4f673d6-7685-48d7-b00f-5b909c181493"), 32, new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Supply Pipe", true, null, "REF-0035", 35 },
                    { new Guid("e66380f3-38a2-4392-b82b-fced24df44d9"), 32, new Guid("510849bd-e577-4363-80b5-be06627cfef6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Two Gang Doorbell switch behind Room", true, 4, "REF-0048", 48 },
                    { new Guid("e876f8f0-fc0f-4feb-93af-22afb87e851d"), 32, new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0015", 15 },
                    { new Guid("ecb258bf-f26b-4232-afa7-457d523569db"), 32, new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel Installation and termination", true, null, "REF-0068", 68 },
                    { new Guid("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"), 32, new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of VCD", true, null, "REF-0008", 8 },
                    { new Guid("f339ecfd-447b-483b-92b1-31b24987de87"), 32, new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DL-1 Spot Ceiling Mounted light Toilet", true, 3, "REF-0075", 75 },
                    { new Guid("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"), 32, new Guid("259689a3-d62f-483d-8476-ef8e28d212a2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Alignment of Wiring Devices", true, null, "REF-0039", 39 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("025a157c-d1d2-46cb-9a70-6af87031c935"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04b4344e-91bd-4386-8218-626d7abddba0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09749601-1157-43d5-bd9b-43ae7994d520"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0cd699e5-ca4d-4797-93a1-029308ade190"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("113778e7-dca1-43e7-9539-22c3caf843dc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("147809cd-7f6e-4f39-9426-d74b9db3b490"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("15c7f5fd-b05c-49a0-a35f-a548d9f86126"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("17780e95-9d21-43ee-9cea-df7209a59b59"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("190be8df-7b70-4c79-a737-1df209ba7e6d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1af7c222-6b43-43a7-868b-c3f41cb29d46"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1f3f8664-1c77-4f87-b7b7-87b7c7250569"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2bdab2ef-3541-4e57-bd37-c092e66cc603"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2d7fbe6b-2411-4413-9294-1b807e6da3f0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("32ca540d-0700-4e5d-9b33-fe084dbecf04"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("37360f3d-9505-4382-a4c3-e3b7ce068acd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("37901174-a631-4d12-8335-dda5bd07b626"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("387e21fd-ad8c-432a-b08e-059a790522a4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b481135-7d60-408f-81d9-25cf9363ee8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3d2e1461-488c-46ef-b662-019f097a515a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("408dfdc3-f5fb-460a-bfa6-035782f57535"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("498e4400-83af-4482-932f-8716bc72aaa1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4a3986e4-0079-4e49-81e9-7973dd0b888f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4b602baf-0fcf-4985-a31d-64707c18558a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5677ef27-1524-4d76-be31-60682e67dcc1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("59136623-44ad-4d1d-bb7c-92dd241da546"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5b9f0de3-a164-47c0-9ae3-2def089f998e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d103945-bcd7-484c-a3e1-ea63598bad72"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d730e37-85be-462c-9170-41a5e3d711b2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("64bc0366-95f0-4de8-a817-4dba4fdf20c2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("67ddfe9b-8c23-4cea-8216-22a5168c94d3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("683581ae-9676-4651-b627-28663576d682"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("68f28143-1d00-4a4e-b853-7724bcbc46ba"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6deff94b-68f3-4827-8af9-06b37affbaec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6f12fb28-c711-4937-8341-1a030c6a996f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6f868e6e-7a11-4c65-9cef-f2aeae768964"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("72cf8727-5ae5-4c63-9640-483853577ea9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7762e342-6be1-4849-a614-b59106040118"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7852e526-2969-49ea-8ca1-4028a99d0679"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8323dcd7-2fed-41dd-8b0e-aa21393ee086"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("83770710-147d-46d3-8b71-414f5c09b677"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("861dad54-79db-42f7-ab37-68466a23f743"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8fe1b6f2-b823-4677-9827-23fa5f523e61"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9c4da2e4-6734-4e94-9037-d2e8f88f931f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9e3a024d-57ad-46ce-b27b-1fc403f68b15"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a62eed08-be48-4b31-880a-c478c8e3b671"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a65828cb-6694-46ff-b252-f92626d6d6c3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ba041522-26d3-48cd-acae-729cb0b667ab"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c468d5dd-3a81-4512-ac31-60aa0d667abc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca1ab126-cd47-4d96-9819-de6c0c931649"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cbb97e1d-c8df-443e-bbf7-84e98c937d71"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cce7b248-d364-41fa-84f4-489bbe5ba3f6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d938728b-f6f4-4006-ad53-df88d75800ae"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("da4112c9-e84a-48ed-9ee0-67ddf655fd04"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddf6192a-38b4-4eab-b81b-ac7178214cc1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("de5ae193-0f9a-4a87-800b-186dd1ad03e2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("de8bc96d-aaa0-4176-8415-1f5c222116c6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4f673d6-7685-48d7-b00f-5b909c181493"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e66380f3-38a2-4392-b82b-fced24df44d9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e876f8f0-fc0f-4feb-93af-22afb87e851d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecb258bf-f26b-4232-afa7-457d523569db"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f339ecfd-447b-483b-92b1-31b24987de87"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("259689a3-d62f-483d-8476-ef8e28d212a2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("38289349-435f-423b-bade-eeb1f3e96603"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("4e1752a4-da49-4fbe-8947-4b5ba00f640b"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("510849bd-e577-4363-80b5-be06627cfef6"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("785af531-ce48-45b1-9c4f-c8c213b5a463"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("7d6c58bd-0051-4841-9d17-d3de8cf1c595"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8d1fbfc6-9e1a-43fd-8830-08b2c2fa21d8"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("9b8ada99-6e0b-4064-a2db-0103e411e722"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("de5a8523-deaf-4f39-9a5c-06116c56892d"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e2cb0ac2-5d5a-4a64-9d54-8f4c97effaf9"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("fa8528f6-ab67-404c-885b-162b4c731c9b"));

            migrationBuilder.InsertData(
                table: "ChecklistSections",
                columns: new[] { "ChecklistSectionId", "ChecklistId", "CreatedDate", "IsActive", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "4. Floor and Wall Tiling" },
                    { new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "7. Aluminium and Glazing Works" },
                    { new Guid("10f470e4-bfa7-4c2f-96a5-8bff23abbfa2"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "2. Final Inspection - Structural" },
                    { new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "3. Internal and External Painting" },
                    { new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, "6. False Ceiling Work" },
                    { new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "8. Wooden/Metal Doors and Wood Works" },
                    { new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "1. General" },
                    { new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "5. Dry wall" },
                    { new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 9, "9. Other Finishes" },
                    { new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "10. Others" }
                });

            migrationBuilder.UpdateData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("98448459-61c2-4ab7-9e77-94cd67601168"),
                column: "Name",
                value: "Pre-Loading of Completed Precast Modular");

            migrationBuilder.UpdateData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                columns: new[] { "ChecklistSectionId", "Description", "Reference", "Sequence" },
                values: new object[] { new Guid("48962238-ba77-431b-93cb-104d24939ec0"), "Check identification tag of the modular", "REF-0552", 2 });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "ChecklistNumber", "ChecklistSectionId", "CreatedDate", "Description", "IsActive", "Part", "Reference", "Sequence" },
                values: new object[,]
                {
                    { new Guid("0dd86d11-6cf6-42b4-b489-20e51c6a5d3c"), 32, new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Final Condition of outside of the room and ensure its damage free", true, null, "REF-0618", 1 },
                    { new Guid("0e79780d-627c-4de4-85ef-caee115df80f"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "External Paint(Application of Primer, Texture)", true, null, "REF-0558", 3 },
                    { new Guid("103277d0-ab14-4d98-b096-c39cd2a39bad"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Floor drain and covers installed and free from damages", true, null, "REF-0608", 6 },
                    { new Guid("105594e5-688b-46e5-b08d-01eac81171c3"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Access panels/ Ceiling acoustic tiles are Fixed Properly", true, null, "REF-0577", 3 },
                    { new Guid("1489ed9f-dd78-4c1c-bf36-a226bbef0f20"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe accessories as per approved drawings", true, null, "REF-0600", 14 },
                    { new Guid("14b4062e-6fb6-4172-a9ab-ee5d2f19e6be"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Direction of doors swing as per App Drawing", true, null, "REF-0588", 2 },
                    { new Guid("1c897f5d-0399-4b61-9993-1f9a58c10692"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout of False Ceiling tiles and bulk head as per App Drawing", true, null, "REF-0575", 1 },
                    { new Guid("1e4d0c53-28fd-439b-9eb2-cae591c45edc"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Tiles are fixed with grouting properly and free from damage", true, null, "REF-0614", 12 },
                    { new Guid("20bf8edf-7344-404c-9632-329cca228deb"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location of Doors as per App Drawing", true, null, "REF-0587", 1 },
                    { new Guid("24af3611-7a4d-4aa3-9c67-eb97775109d4"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe installed as per approved drawings", true, null, "REF-0599", 13 },
                    { new Guid("2603664d-ce1f-4e14-8bd6-126947a083e6"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Drainhole are free from any debris and properly closed (if applicable)", true, null, "REF-0568", 7 },
                    { new Guid("2c71ee06-19f7-4d83-8831-c9231b4263b4"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the method of loading as per the project / design requirements", true, null, "REF-0554", 4 },
                    { new Guid("2f04f0df-4ed9-4372-98b9-3ced3231f320"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen cabinets, counter top installed as per app drawing", true, null, "REF-0596", 10 },
                    { new Guid("3c9685a6-c67d-4547-b638-14537a9d5187"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Skirting is installed/fixed properly and truly vertical", true, null, "REF-0564", 3 },
                    { new Guid("41f81e13-30db-414e-9a13-98d6374a0ef0"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Threshold installed and free from damage", true, null, "REF-0605", 3 },
                    { new Guid("52f35c66-7592-4374-84bf-40f96d56bcaf"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Vanity installed and free from damage", true, null, "REF-0609", 7 },
                    { new Guid("5323fb3c-6fbb-4a90-8b09-bdf91bf90f94"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod WC and cover installed and free from damage", true, null, "REF-0610", 8 },
                    { new Guid("570418a2-3efe-4595-82e5-4f685fa896b7"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visually inspect the modular for any defects or damages", true, null, "REF-0553", 3 },
                    { new Guid("5cbde6d5-b477-48fc-864b-2754498a3a72"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Architraves are fixed as per Drawing around Main Entrance Door / Bedroom Door", true, null, "REF-0591", 5 },
                    { new Guid("60759b80-6ff6-4f13-9dc8-5a4cefc624e8"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Iron-Mongery and Accessories", true, null, "REF-0582", 3 },
                    { new Guid("66c1c064-46ba-43ff-8068-3d98c5981208"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lock/Hardware of Main Entrance Door / Bedroom Door is installed", true, null, "REF-0590", 4 },
                    { new Guid("6745c653-2210-4851-9bb0-9795e0f1c068"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Opening for MEP services are cut properly.", true, null, "REF-0572", 3 },
                    { new Guid("6beccd5d-5bdb-424a-865a-bb2e7a27155b"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Painted walls are clean and free from stains.", true, null, "REF-0613", 11 },
                    { new Guid("7d750f4f-1a1e-4812-b530-da3aff8b85ec"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Gypsum surface are Crackfree at joints.", true, null, "REF-0578", 4 },
                    { new Guid("7f11eb56-9c85-41c4-9c1d-8ccaf372f749"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bitumin Applied at required Areas", true, null, "REF-0560", 5 },
                    { new Guid("8241fae6-cf3b-44be-bf82-347034fa8306"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Glass Partition installed and free from damage", true, null, "REF-0607", 5 },
                    { new Guid("847a6d48-9ae3-4e44-a8d6-7962fabb9bf2"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cleaning of corners and edges removing exccessive paint on skirting", true, null, "REF-0567", 6 },
                    { new Guid("8bdb8393-7c9c-4a06-9911-7b5d55734d4b"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout, location and position of dry wall is as per App Drawing", true, null, "REF-0570", 1 },
                    { new Guid("8d1791a0-cae1-4677-8687-b9dfc3414fbe"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gypsum curtain pelmet installed and free from damages/cracks", true, null, "REF-0603", 1 },
                    { new Guid("9013c678-1bc4-4f73-bc3f-5effa773178d"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thickness of Dry wall is as per App Drawing", true, null, "REF-0571", 2 },
                    { new Guid("904dafc2-7584-4fa0-b097-8a9aafc93c74"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0586", 7 },
                    { new Guid("90d9276f-8f05-4498-aeaa-0284856092f3"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen Backsplash installed, grouted and free from damages", true, null, "REF-0604", 2 },
                    { new Guid("95d9bf14-1a5d-483a-a930-4baf527e5de0"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Paint touch ups are completed around installed items.", true, null, "REF-0559", 4 },
                    { new Guid("9b4ad754-7bb9-429b-ba6d-e3b05a88deb0"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water leak test performed and passed.", true, null, "REF-0584", 5 },
                    { new Guid("a8f9f6f7-3e31-42ce-82b7-18a269871238"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Line, Level and Spacer for the Installed Tiles", true, null, "REF-0563", 2 },
                    { new Guid("a92a8534-4147-4b3d-8caa-f41097260e6b"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Glass/panels", true, null, "REF-0581", 2 },
                    { new Guid("abb96f1e-7476-4265-a9c6-9de5a65721f7"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location of Window/Sliding Door as per App Drawing", true, null, "REF-0580", 1 },
                    { new Guid("ac9bc775-7085-45c4-acb5-9042bf159804"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0569", 8 },
                    { new Guid("ada0df0c-3ded-4e14-b789-cc134259d9ae"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Gypsum board are free from pealing off and Crack Free", true, null, "REF-0612", 10 },
                    { new Guid("aea89b5f-cc96-47b0-bca7-e1cff4fe18f6"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe doors and drawers funtioning smoothly and free from scratches", true, null, "REF-0601", 15 },
                    { new Guid("af7600e4-0567-4e12-8d01-6cba76a3059c"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Balcony floor drain installed as per approved drawing", true, null, "REF-0615", 13 },
                    { new Guid("b1813260-2885-4243-8122-a882a1baff00"), 32, new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sign the delivery note for accepting the loading of precast modular in good condition", true, null, "REF-0619", 2 },
                    { new Guid("b6fc4834-4a37-4231-8377-dc0308d6181f"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grouting of all Joints is done properly", true, null, "REF-0565", 4 },
                    { new Guid("b858ff5d-deec-4b46-a754-a26378a5885c"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Entrance Door / Bedroom Door as per App Drawing", true, null, "REF-0589", 3 },
                    { new Guid("bbd1169f-a962-48a4-a166-e9d7a99c49cd"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elastomeric sealant under skirting is provided properly", true, null, "REF-0566", 5 },
                    { new Guid("bf6b0e94-1c82-4ef4-b6da-52570d087801"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen cabinets accessories installed as per app drawing", true, null, "REF-0597", 11 },
                    { new Guid("c14f2f9c-5efe-4a4e-82a5-c0e29af69edd"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0574", 5 },
                    { new Guid("c1ff045d-55cd-44b1-beb9-26b3e0fa566b"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Mirror installed and free from damage", true, null, "REF-0606", 4 },
                    { new Guid("c2389679-0f87-407b-89c6-a5709223b2fe"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Silicone Sealant", true, null, "REF-0583", 4 },
                    { new Guid("c5dd4075-5da1-458f-ac95-18112b43f1b0"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lock/Hardware of Pod door is installed", true, null, "REF-0593", 7 },
                    { new Guid("cadcc053-801d-48bf-9527-cb6d52d98928"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, ITP, materials and shop drawings are approved", true, null, "REF-0551", 1 },
                    { new Guid("cb8baaf8-1d8e-4230-b4df-0c3ac94fd0d4"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Gypsum surface are Crackfree at joints.", true, null, "REF-0573", 4 },
                    { new Guid("cd09e3bc-301a-4dbb-b90d-ba68cc56f551"), 32, new Guid("10f470e4-bfa7-4c2f-96a5-8bff23abbfa2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal and External Dimensions of the modular", true, null, "REF-0555", 1 },
                    { new Guid("d402f880-5fa5-4162-ac63-fbfaf810610f"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod door is installed as per App Drawing", true, null, "REF-0592", 6 },
                    { new Guid("d6023c45-7825-4d5d-aa49-91bb08b09fc0"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toilet accessories installed and free from damage", true, null, "REF-0616", 14 },
                    { new Guid("d91b6f34-7c68-447a-94a5-03c083f46a4c"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0579", 5 },
                    { new Guid("dd51c76d-cda3-4be1-82ad-2a563f19798d"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, If any", true, null, "REF-0561", 6 },
                    { new Guid("ddb909e5-bf6a-4732-a472-035524973db6"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout and Fixing of Tiles as per App Drawing", true, null, "REF-0562", 1 },
                    { new Guid("df99b660-0f74-4072-81d3-5ccdb8b6c941"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Height of the False Ceiling as per App Drawing", true, null, "REF-0576", 2 },
                    { new Guid("eac92d42-c6d9-477f-9f35-b346da9fd8ba"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0602", 16 },
                    { new Guid("eb06003c-f9de-4e67-92dc-4fee24df8b7a"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paint touch completed around the frame.", true, null, "REF-0585", 6 },
                    { new Guid("ef21b2a7-8594-4272-883c-74094a99a543"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal Paint (Application of Primer, Stucco and 2nd Coat of Paint)", true, null, "REF-0557", 2 },
                    { new Guid("f23db52c-5216-4c9d-ab46-7520823ef225"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes & MEP fittings.", true, null, "REF-0617", 15 },
                    { new Guid("f45f776f-1819-4b89-a33d-42aef2b68a5e"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Architraves are fixed as per Drawing around Pod door", true, null, "REF-0594", 8 },
                    { new Guid("f70f6090-7425-4023-bb93-8286eceec229"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen sink and sink mixer installed", true, null, "REF-0598", 12 },
                    { new Guid("f77b091e-68f1-4a39-8a44-723fb0291312"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Locking of Doors and Shutters securely to avoid movement during transportation", true, null, "REF-0595", 9 },
                    { new Guid("f7b2e0df-941b-4231-bf83-710047d2519a"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Shower installted and free from damage", true, null, "REF-0611", 9 },
                    { new Guid("fcb5a35d-2c3c-4bc7-b054-9ccc2d0efd15"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location and color of Painting as per the App Drawing", true, null, "REF-0556", 1 }
                });
        }
    }
}
