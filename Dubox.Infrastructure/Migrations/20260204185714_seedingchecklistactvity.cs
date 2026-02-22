using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingchecklistactvity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "ChecklistSectionId", "CreatedDate", "Description", "IsActive", "Reference", "Sequence" },
                values: new object[,]
                {
                    { new Guid("0111b962-4e0c-0d88-62bb-62a75c3886d5"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of smoke detectors and heat detectors", true, "Checklist 32 Part 2", 3202 },
                    { new Guid("01201730-276a-00c1-c608-c6c1cc0e4a3a"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Primer for Internal Paint", true, "Checklist 4 Part 2", 402 },
                    { new Guid("018015c2-dccb-7ce1-b56c-d1f3ed3f6d1d"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application & Leak test of Wet Area Waterproofing", true, "Checklist 19 Part 2", 1902 },
                    { new Guid("1097223b-66a1-154e-2f00-04bdf4799371"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Suspension System of False Ceiling", true, "Checklist 18 Part 1", 1801 },
                    { new Guid("1171db9c-5953-2b71-115b-5fc6e1ce1d4a"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of light fittings and accessories", true, "Checklist 32 Part 3", 3203 },
                    { new Guid("1909f8a8-62b0-55ad-8c99-e116555ecea4"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Final inspection of Dry Wall Installation", true, "Checklist 14 Part 2", 1402 },
                    { new Guid("19aca18a-a656-3df6-7406-5a6c03aadfe1"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface Preparation for Wet Area Waterproofing", true, "Checklist 19 Part 1", 1901 },
                    { new Guid("1f049b1c-03ef-e846-62de-e2a6b3dca554"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Aluminum Windows/Curtain Wall", true, "Checklist 29", 2900 },
                    { new Guid("259d1aba-4bb1-cd12-386e-aa6feb64084f"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Pressure Test of Chilled Water Pipes (Copper)", true, "Checklist 9", 900 },
                    { new Guid("27c22670-a991-01e1-a87c-df324be0908d"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Re-pressure test of water supply pipes inside the box", true, "Checklist 31", 3100 },
                    { new Guid("2b2756b7-abd6-4a25-f0ec-0fbb9760537c"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Kitchen Cabinets & Kitchen Counter Top", true, "Checklist 23 Part 1", 2301 },
                    { new Guid("2f51cc52-1293-d0c1-05dc-90d39e05509d"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ONU installation along with conduit connection", true, "Checklist 16", 1600 },
                    { new Guid("30e57ff4-b640-a858-386c-fe2a13a78abd"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Wooden Doors/Metal Door", true, "Checklist 25", 2500 },
                    { new Guid("3128b03b-d5a6-7edc-4afc-cd89c9024313"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Floor/Wall Tile Layout", true, "Checklist 6 Part 2", 602 },
                    { new Guid("431c5ab7-7bf7-3007-ef81-0ee2af45434f"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Megger and Continuity test of Wires & Cables", true, "Checklist 22", 2200 },
                    { new Guid("4564f091-b44c-c92b-cd97-1c19ef3bb5d5"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of HVAC Duct with Flexible & Plenum Box", true, "Checklist 12", 1200 },
                    { new Guid("496f8d9a-79f3-e134-6fd4-693a77757f84"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pulling of Fire Alarm Cables", true, "Checklist 15", 1500 },
                    { new Guid("50fc4a36-b959-25a9-5d8f-ebdb6b13cefa"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Filler Coat for external Paint", true, "Checklist 4 Part 6", 406 },
                    { new Guid("62cfee7a-af2a-730e-e956-ce048cb57939"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of occupancy sensor", true, "Checklist 32 Part 6", 3206 },
                    { new Guid("6532c1a4-e651-e6a0-e649-d9f9bec7e09f"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation DX Split Unit", true, "Checklist 30", 3000 },
                    { new Guid("6733d192-a34e-ad8e-3b09-3e2ac45bcb2a"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Pressure Test of water Supply pipes", true, "Checklist 7", 700 },
                    { new Guid("6af480f7-1505-94fd-0319-3ed36cce6533"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Final Civil/Architectural Inspection of Modular Box Prior to dispatch", true, "Checklist 33", 3300 },
                    { new Guid("6b754fa8-933c-a584-e49b-9236ab75007c"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Stucco for Internal Paint", true, "Checklist 4 Part 3", 403 },
                    { new Guid("70e6ef4b-cacd-67f3-84db-74e0536b45a7"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Inspection of Fire Sealing Work", true, "Checklist 17", 1700 },
                    { new Guid("74b16cbd-6547-c8df-9be1-fc95ec696531"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & closing of False Ceiling Gypsum Board/Tiles", true, "Checklist 18 Part 2", 1802 },
                    { new Guid("7584741d-bbdb-a3d9-1445-46f04457e239"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of kitchen sink and accessories", true, "Checklist 27", 2700 },
                    { new Guid("79c57e38-3686-a7b2-0eca-15b2d52853da"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Precast Wall Elements for Modular Box", true, "Checklist 1 Part 1", 101 },
                    { new Guid("7f7be79d-0c6e-159c-f086-1939d48ee98c"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Pressure Test of fire fighting pipe", true, "Checklist 8", 800 },
                    { new Guid("94414928-1a6c-ead2-8270-956dc8913183"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DB installation along with conduit connection", true, "Checklist 20", 2000 },
                    { new Guid("9803cac1-92eb-14e7-d26f-35a9de245943"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Pressure Test of refrigeration pipes (Copper)", true, "Checklist 10", 1000 },
                    { new Guid("9a7be51b-93ef-7a3e-875f-64de1af0d91e"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Dry Wall Framing", true, "Checklist 14 Part 1", 1401 },
                    { new Guid("9c3db914-1b8f-6892-8621-04101081cb8a"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Floor Slab Erection and Mechanical & Wet Connections of Precast Elements", true, "Checklist 1 Part 2", 102 },
                    { new Guid("9d6706c2-993f-7d81-fe36-52d5bbd9c512"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface preparation of Epoxy Flooring", true, "Checklist 21 Part 1", 2101 },
                    { new Guid("9f25478c-1d37-b36f-1d3e-db0d3239442b"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pulling of Data Cables", true, "Checklist 16 Part 1", 1601 },
                    { new Guid("a31a2874-59d9-46a6-4071-42196406bd4b"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface Preparation for Floor Tiles", true, "Checklist 6 Part 1", 601 },
                    { new Guid("a41a6a7a-5b1c-c4a1-6f16-b85d11928ac6"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pulling of wires and cables through embedded pipes", true, "Checklist 13", 1300 },
                    { new Guid("b0920aa0-9b31-4b2a-2f8a-101adc9f857b"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Leak Test of Drainage pipes", true, "Checklist 5", 500 },
                    { new Guid("b4b78ed0-23aa-0fbf-eabb-27ddb072a6fe"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Connection of Electrical Conduits between Precast Walls & Slab", true, "Checklist 3", 300 },
                    { new Guid("bbbf6f7f-3b95-d174-f9dc-8867cb16ed5b"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Construction of Precast Concrete Modular Final Inspection (Assembly)", true, "Checklist 1", 100 },
                    { new Guid("c0ae49ea-b4d2-32f5-6572-f06bc9b6555e"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of thermostat", true, "Checklist 32 Part 5", 3205 },
                    { new Guid("c314b2ce-25bc-19f2-7c56-6d7b4b1c975f"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of sprinklers / grills and diffusers", true, "Checklist 32 Part 1", 3201 },
                    { new Guid("c68908e3-f97c-828c-5031-143077d9e358"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Damproofing", true, "Checklist 11", 1100 },
                    { new Guid("cdb568cb-ffb7-7dea-5bd8-7e28f81ca7af"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of External Final Coat Paint", true, "Checklist 4 Part 7", 407 },
                    { new Guid("cfe645c7-dfa3-141e-8425-2e11678a80b8"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation & Grouting of Floor/Wall Tiles", true, "Checklist 6 Part 3", 603 },
                    { new Guid("d4292cf4-1cdb-b3fd-98c5-50bd1ba9b5dc"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Epoxy Flooring", true, "Checklist 21 Part 2", 2102 },
                    { new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Final MEP Inspection / Riser Installation", true, "Checklist 32", 3200 },
                    { new Guid("d7edbe4f-9639-964d-c809-d0eed34059a2"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Insulation of hot water pipe", true, "Checklist 7 Part 1", 701 },
                    { new Guid("e67df796-5224-5982-7e5d-a48203c08893"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Wardrobes", true, "Checklist 24", 2400 },
                    { new Guid("e81f49a4-1738-513b-2b54-fa70ec6de724"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of wiring devices and accessories", true, "Checklist 32 Part 4", 3204 },
                    { new Guid("ee46ad4b-3b67-0526-054b-6238261ef818"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Internal paint Final Coat", true, "Checklist 4 Part 4", 404 },
                    { new Guid("f2ced93c-d04d-e516-87e6-9f046eecec74"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface Preparation for Paint", true, "Checklist 4 Part 1", 401 },
                    { new Guid("f2e0d734-bf96-4dea-b54a-5d266beb5aa9"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP modular installation consisting FCU and HVAC Ducting accessories", true, "Checklist 2", 200 },
                    { new Guid("f7a3e9db-924f-970c-5aef-0b93a8787a7f"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Access Control System", true, "Checklist 28", 2800 },
                    { new Guid("fb20fb47-13fb-6d14-4a80-d52d4f7e2760"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toilet Pod Installation during Assembly", true, "Checklist 1 Part 3", 103 },
                    { new Guid("fec4bedc-8ad0-78b5-641b-414ff65b3a02"), null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Primer for external Paint", true, "Checklist 4 Part 5", 405 }
                });

            migrationBuilder.InsertData(
                table: "ActivityCheckListItems",
                columns: new[] { "ActivityCheckListItemId", "ActivityMasterId", "ActivityTemplateActivityId", "CreatedBy", "CreatedDate", "IsActive", "IsMandatory", "ModifiedBy", "ModifiedDate", "PredefinedChecklistItemId", "Sequence" },
                values: new object[,]
                {
                    { new Guid("02450e1e-ed6e-a9b4-2b47-3b728b17479e"), new Guid("10000005-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("9803cac1-92eb-14e7-d26f-35a9de245943"), 2 },
                    { new Guid("0d2f24c3-9945-f4eb-63e6-ff16e9ff7ca3"), new Guid("10000002-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d7edbe4f-9639-964d-c809-d0eed34059a2"), 2 },
                    { new Guid("183ac735-4489-35c7-7efa-91f6158a8f2a"), new Guid("10000005-0000-0000-0000-000000000005"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("c0ae49ea-b4d2-32f5-6572-f06bc9b6555e"), 1 },
                    { new Guid("1ad3f6ce-8914-07ff-0de7-2777fd597db3"), new Guid("10000004-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("3128b03b-d5a6-7edc-4afc-cd89c9024313"), 2 },
                    { new Guid("1aeb9687-6d5e-275f-31c4-e1199377e271"), new Guid("10000002-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("27c22670-a991-01e1-a87c-df324be0908d"), 2 },
                    { new Guid("20e12b89-e0b8-6cc5-4cda-075d64a16eca"), new Guid("10000002-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 2 },
                    { new Guid("222d7ae1-12b3-f1e1-8849-5148621d28e4"), new Guid("10000001-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("b4b78ed0-23aa-0fbf-eabb-27ddb072a6fe"), 2 },
                    { new Guid("2b56d084-5aed-1e84-0ad4-f275c72b9216"), new Guid("10000003-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("2f51cc52-1293-d0c1-05dc-90d39e05509d"), 1 },
                    { new Guid("2cad61e8-f58e-4470-e247-10fcf6bc2eaf"), new Guid("10000004-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("cfe645c7-dfa3-141e-8425-2e11678a80b8"), 3 },
                    { new Guid("346a7847-cc32-3048-8dfa-9a5979143de5"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("01201730-276a-00c1-c608-c6c1cc0e4a3a"), 4 },
                    { new Guid("3662512f-4149-aeb6-b4e8-2896a0ae4f45"), new Guid("10000006-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 1 },
                    { new Guid("36f9147f-7050-6294-ba62-aa6c22f3a05c"), new Guid("10000001-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f2e0d734-bf96-4dea-b54a-5d266beb5aa9"), 1 },
                    { new Guid("3acb6947-c28c-1e53-bb0c-6f25bfa943b7"), new Guid("10000004-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("a31a2874-59d9-46a6-4071-42196406bd4b"), 1 },
                    { new Guid("3c6111c4-b294-6b43-cec0-382698ef4a90"), new Guid("10000005-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 4 },
                    { new Guid("4398cc01-6ec2-998e-640a-817b45d5addb"), new Guid("10000005-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("e81f49a4-1738-513b-2b54-fa70ec6de724"), 1 },
                    { new Guid("4b88699d-4dc7-77bc-2c66-8e2b77b13685"), new Guid("10000003-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("1909f8a8-62b0-55ad-8c99-e116555ecea4"), 2 },
                    { new Guid("4c9637cd-ec70-2ae8-a4da-06edc49deb95"), new Guid("10000002-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6733d192-a34e-ad8e-3b09-3e2ac45bcb2a"), 1 },
                    { new Guid("4ffe4f25-0afc-b458-f7cd-c4c45eb63fc3"), new Guid("10000002-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("b0920aa0-9b31-4b2a-2f8a-101adc9f857b"), 1 },
                    { new Guid("504a6bb9-c4c4-11c8-c22a-aa0a4d8a0608"), new Guid("10000003-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("9f25478c-1d37-b36f-1d3e-db0d3239442b"), 3 },
                    { new Guid("595c3dfa-f156-2eb9-794b-dc67516c7480"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("c68908e3-f97c-828c-5031-143077d9e358"), 3 },
                    { new Guid("5c92ef8d-76b0-49ae-8d6e-553db0c6d0f4"), new Guid("10000002-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("4564f091-b44c-c92b-cd97-1c19ef3bb5d5"), 1 },
                    { new Guid("5c9696a8-48f3-2842-8e8b-f3040c8a1525"), new Guid("10000002-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 3 },
                    { new Guid("5d803de7-0175-4bc3-5c8c-679940270944"), new Guid("10000001-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("fb20fb47-13fb-6d14-4a80-d52d4f7e2760"), 1 },
                    { new Guid("66ed0a08-d5d1-a5e5-ec51-f6b9297e9950"), new Guid("10000005-0000-0000-0000-000000000006"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("c314b2ce-25bc-19f2-7c56-6d7b4b1c975f"), 1 },
                    { new Guid("66f24e5e-d5fb-eb1f-e868-523f5089dd47"), new Guid("10000004-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("1097223b-66a1-154e-2f00-04bdf4799371"), 2 },
                    { new Guid("69697656-e8bb-f713-7a17-798cd3856a96"), new Guid("10000004-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("74b16cbd-6547-c8df-9be1-fc95ec696531"), 3 },
                    { new Guid("76b13359-f984-79cd-fc8e-f826f159495d"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6b754fa8-933c-a584-e49b-9236ab75007c"), 6 },
                    { new Guid("8abd96a0-0457-b670-417f-789e55456c9d"), new Guid("10000005-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("259d1aba-4bb1-cd12-386e-aa6feb64084f"), 1 },
                    { new Guid("8f8cd69d-2b1e-e736-b968-2ae7ab96a4ff"), new Guid("10000005-0000-0000-0000-000000000007"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("c314b2ce-25bc-19f2-7c56-6d7b4b1c975f"), 1 },
                    { new Guid("938a50b0-e24f-cc6e-3063-391bb226c4fe"), new Guid("10000003-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("94414928-1a6c-ead2-8270-956dc8913183"), 1 },
                    { new Guid("9e7b139f-c95b-7926-8316-a109d856ea08"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("79c57e38-3686-a7b2-0eca-15b2d52853da"), 1 },
                    { new Guid("a2fed9a5-ad6a-966c-0931-e98355208c0c"), new Guid("10000005-0000-0000-0000-000000000008"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("62cfee7a-af2a-730e-e956-ce048cb57939"), 2 },
                    { new Guid("a73b2411-1977-f672-0de0-2ddc0da68d83"), new Guid("10000005-0000-0000-0000-000000000008"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("0111b962-4e0c-0d88-62bb-62a75c3886d5"), 1 },
                    { new Guid("a9b3878b-8116-87dd-7626-c0ced52f84da"), new Guid("10000004-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("70e6ef4b-cacd-67f3-84db-74e0536b45a7"), 1 },
                    { new Guid("ab620464-ee0e-ef6e-055d-d4c115ffb1a7"), new Guid("10000006-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("6af480f7-1505-94fd-0319-3ed36cce6533"), 2 },
                    { new Guid("b2277b17-6fa8-2c04-1d8a-af65ae20c7e7"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f2ced93c-d04d-e516-87e6-9f046eecec74"), 2 },
                    { new Guid("b28846e4-3c45-5a37-c17e-b660cd941178"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("ee46ad4b-3b67-0526-054b-6238261ef818"), 7 },
                    { new Guid("b76d1298-6aa9-1243-bc28-07d7a27cedb5"), new Guid("10000002-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("27c22670-a991-01e1-a87c-df324be0908d"), 3 },
                    { new Guid("b7a015a1-4c13-d0c0-0b4d-5ecf06922132"), new Guid("10000003-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("431c5ab7-7bf7-3007-ef81-0ee2af45434f"), 4 },
                    { new Guid("b9233445-aacc-46f6-d38f-bb2c93815e7f"), new Guid("10000002-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 4 },
                    { new Guid("baeeb7a0-e155-6db7-815a-9305190b949b"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("f2ced93c-d04d-e516-87e6-9f046eecec74"), 1 },
                    { new Guid("be4ff175-7af6-d902-1958-952e88bcdea2"), new Guid("10000001-0000-0000-0000-000000000001"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("9c3db914-1b8f-6892-8621-04101081cb8a"), 2 },
                    { new Guid("c021348d-4e58-471f-05a7-5c3582475ab3"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("50fc4a36-b959-25a9-5d8f-ebdb6b13cefa"), 8 },
                    { new Guid("ce1c2648-44ab-8ace-a045-15471cba85c8"), new Guid("10000002-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7f7be79d-0c6e-159c-f086-1939d48ee98c"), 1 },
                    { new Guid("d209d4d3-e813-26e1-595e-c7a232f0bfe6"), new Guid("10000003-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("9a7be51b-93ef-7a3e-875f-64de1af0d91e"), 1 },
                    { new Guid("d6e1ac13-2ca8-167f-cade-fd0191b2e3d2"), new Guid("10000001-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("bbbf6f7f-3b95-d174-f9dc-8867cb16ed5b"), 1 },
                    { new Guid("d979ca16-057d-8c43-55af-cfc5ffd5a67b"), new Guid("10000003-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("a41a6a7a-5b1c-c4a1-6f16-b85d11928ac6"), 1 },
                    { new Guid("da657780-f06d-3c1a-3898-c82c67e45f80"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("fec4bedc-8ad0-78b5-641b-414ff65b3a02"), 5 },
                    { new Guid("dfd0914c-68fe-b752-5483-f5eda247c0b2"), new Guid("10000004-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("cdb568cb-ffb7-7dea-5bd8-7e28f81ca7af"), 9 },
                    { new Guid("e6180ef4-5c7f-a00c-31a4-e47ab30d1174"), new Guid("10000004-0000-0000-0000-000000000005"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("30e57ff4-b640-a858-386c-fe2a13a78abd"), 1 },
                    { new Guid("eb3a8463-a24e-7797-362c-5149eaaf03d5"), new Guid("10000005-0000-0000-0000-000000000003"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 3 },
                    { new Guid("ec0e29d9-244f-6582-feef-e8d4225ef570"), new Guid("10000005-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("7584741d-bbdb-a3d9-1445-46f04457e239"), 1 },
                    { new Guid("ec2f2fbd-2053-98f1-e6fd-c1e0b3a4657a"), new Guid("10000003-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("496f8d9a-79f3-e134-6fd4-693a77757f84"), 2 },
                    { new Guid("eee4208a-9863-eddb-ca08-67c5a6291624"), new Guid("10000004-0000-0000-0000-000000000006"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("1f049b1c-03ef-e846-62de-e2a6b3dca554"), 1 },
                    { new Guid("f0389399-e43e-3342-9f75-754521cca4f8"), new Guid("10000006-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 3 },
                    { new Guid("fba852ed-4c75-aab9-6832-a4d5f35f8002"), new Guid("10000005-0000-0000-0000-000000000002"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("1171db9c-5953-2b71-115b-5fc6e1ce1d4a"), 1 },
                    { new Guid("ffdfde72-e0cf-7e6e-c2b3-c896fd97b21e"), new Guid("10000002-0000-0000-0000-000000000004"), null, "System", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, true, null, null, new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"), 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("02450e1e-ed6e-a9b4-2b47-3b728b17479e"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("0d2f24c3-9945-f4eb-63e6-ff16e9ff7ca3"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("183ac735-4489-35c7-7efa-91f6158a8f2a"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("1ad3f6ce-8914-07ff-0de7-2777fd597db3"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("1aeb9687-6d5e-275f-31c4-e1199377e271"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("20e12b89-e0b8-6cc5-4cda-075d64a16eca"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("222d7ae1-12b3-f1e1-8849-5148621d28e4"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("2b56d084-5aed-1e84-0ad4-f275c72b9216"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("2cad61e8-f58e-4470-e247-10fcf6bc2eaf"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("346a7847-cc32-3048-8dfa-9a5979143de5"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("3662512f-4149-aeb6-b4e8-2896a0ae4f45"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("36f9147f-7050-6294-ba62-aa6c22f3a05c"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("3acb6947-c28c-1e53-bb0c-6f25bfa943b7"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("3c6111c4-b294-6b43-cec0-382698ef4a90"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("4398cc01-6ec2-998e-640a-817b45d5addb"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("4b88699d-4dc7-77bc-2c66-8e2b77b13685"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("4c9637cd-ec70-2ae8-a4da-06edc49deb95"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("4ffe4f25-0afc-b458-f7cd-c4c45eb63fc3"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("504a6bb9-c4c4-11c8-c22a-aa0a4d8a0608"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("595c3dfa-f156-2eb9-794b-dc67516c7480"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("5c92ef8d-76b0-49ae-8d6e-553db0c6d0f4"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("5c9696a8-48f3-2842-8e8b-f3040c8a1525"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("5d803de7-0175-4bc3-5c8c-679940270944"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("66ed0a08-d5d1-a5e5-ec51-f6b9297e9950"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("66f24e5e-d5fb-eb1f-e868-523f5089dd47"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("69697656-e8bb-f713-7a17-798cd3856a96"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("76b13359-f984-79cd-fc8e-f826f159495d"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("8abd96a0-0457-b670-417f-789e55456c9d"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("8f8cd69d-2b1e-e736-b968-2ae7ab96a4ff"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("938a50b0-e24f-cc6e-3063-391bb226c4fe"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("9e7b139f-c95b-7926-8316-a109d856ea08"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("a2fed9a5-ad6a-966c-0931-e98355208c0c"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("a73b2411-1977-f672-0de0-2ddc0da68d83"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("a9b3878b-8116-87dd-7626-c0ced52f84da"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("ab620464-ee0e-ef6e-055d-d4c115ffb1a7"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("b2277b17-6fa8-2c04-1d8a-af65ae20c7e7"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("b28846e4-3c45-5a37-c17e-b660cd941178"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("b76d1298-6aa9-1243-bc28-07d7a27cedb5"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("b7a015a1-4c13-d0c0-0b4d-5ecf06922132"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("b9233445-aacc-46f6-d38f-bb2c93815e7f"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("baeeb7a0-e155-6db7-815a-9305190b949b"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("be4ff175-7af6-d902-1958-952e88bcdea2"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("c021348d-4e58-471f-05a7-5c3582475ab3"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("ce1c2648-44ab-8ace-a045-15471cba85c8"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("d209d4d3-e813-26e1-595e-c7a232f0bfe6"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("d6e1ac13-2ca8-167f-cade-fd0191b2e3d2"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("d979ca16-057d-8c43-55af-cfc5ffd5a67b"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("da657780-f06d-3c1a-3898-c82c67e45f80"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("dfd0914c-68fe-b752-5483-f5eda247c0b2"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("e6180ef4-5c7f-a00c-31a4-e47ab30d1174"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("eb3a8463-a24e-7797-362c-5149eaaf03d5"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("ec0e29d9-244f-6582-feef-e8d4225ef570"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("ec2f2fbd-2053-98f1-e6fd-c1e0b3a4657a"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("eee4208a-9863-eddb-ca08-67c5a6291624"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("f0389399-e43e-3342-9f75-754521cca4f8"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("fba852ed-4c75-aab9-6832-a4d5f35f8002"));

            migrationBuilder.DeleteData(
                table: "ActivityCheckListItems",
                keyColumn: "ActivityCheckListItemId",
                keyValue: new Guid("ffdfde72-e0cf-7e6e-c2b3-c896fd97b21e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("018015c2-dccb-7ce1-b56c-d1f3ed3f6d1d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("19aca18a-a656-3df6-7406-5a6c03aadfe1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2b2756b7-abd6-4a25-f0ec-0fbb9760537c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6532c1a4-e651-e6a0-e649-d9f9bec7e09f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9d6706c2-993f-7d81-fe36-52d5bbd9c512"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d4292cf4-1cdb-b3fd-98c5-50bd1ba9b5dc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e67df796-5224-5982-7e5d-a48203c08893"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7a3e9db-924f-970c-5aef-0b93a8787a7f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0111b962-4e0c-0d88-62bb-62a75c3886d5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("01201730-276a-00c1-c608-c6c1cc0e4a3a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1097223b-66a1-154e-2f00-04bdf4799371"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1171db9c-5953-2b71-115b-5fc6e1ce1d4a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1909f8a8-62b0-55ad-8c99-e116555ecea4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1f049b1c-03ef-e846-62de-e2a6b3dca554"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("259d1aba-4bb1-cd12-386e-aa6feb64084f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("27c22670-a991-01e1-a87c-df324be0908d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2f51cc52-1293-d0c1-05dc-90d39e05509d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("30e57ff4-b640-a858-386c-fe2a13a78abd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3128b03b-d5a6-7edc-4afc-cd89c9024313"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("431c5ab7-7bf7-3007-ef81-0ee2af45434f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4564f091-b44c-c92b-cd97-1c19ef3bb5d5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("496f8d9a-79f3-e134-6fd4-693a77757f84"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50fc4a36-b959-25a9-5d8f-ebdb6b13cefa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("62cfee7a-af2a-730e-e956-ce048cb57939"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6733d192-a34e-ad8e-3b09-3e2ac45bcb2a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6af480f7-1505-94fd-0319-3ed36cce6533"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6b754fa8-933c-a584-e49b-9236ab75007c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("70e6ef4b-cacd-67f3-84db-74e0536b45a7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("74b16cbd-6547-c8df-9be1-fc95ec696531"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7584741d-bbdb-a3d9-1445-46f04457e239"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79c57e38-3686-a7b2-0eca-15b2d52853da"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7f7be79d-0c6e-159c-f086-1939d48ee98c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("94414928-1a6c-ead2-8270-956dc8913183"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9803cac1-92eb-14e7-d26f-35a9de245943"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a7be51b-93ef-7a3e-875f-64de1af0d91e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9c3db914-1b8f-6892-8621-04101081cb8a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9f25478c-1d37-b36f-1d3e-db0d3239442b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a31a2874-59d9-46a6-4071-42196406bd4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a41a6a7a-5b1c-c4a1-6f16-b85d11928ac6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b0920aa0-9b31-4b2a-2f8a-101adc9f857b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b4b78ed0-23aa-0fbf-eabb-27ddb072a6fe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbbf6f7f-3b95-d174-f9dc-8867cb16ed5b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c0ae49ea-b4d2-32f5-6572-f06bc9b6555e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c314b2ce-25bc-19f2-7c56-6d7b4b1c975f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c68908e3-f97c-828c-5031-143077d9e358"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cdb568cb-ffb7-7dea-5bd8-7e28f81ca7af"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cfe645c7-dfa3-141e-8425-2e11678a80b8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d745f3ad-e0c0-5f61-6083-84752c3606a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d7edbe4f-9639-964d-c809-d0eed34059a2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e81f49a4-1738-513b-2b54-fa70ec6de724"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ee46ad4b-3b67-0526-054b-6238261ef818"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2ced93c-d04d-e516-87e6-9f046eecec74"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2e0d734-bf96-4dea-b54a-5d266beb5aa9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fb20fb47-13fb-6d14-4a80-d52d4f7e2760"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fec4bedc-8ad0-78b5-641b-414ff65b3a02"));
        }
    }
}
