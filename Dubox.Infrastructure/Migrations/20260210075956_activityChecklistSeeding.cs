using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class activityChecklistSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Checklists",
                columns: new[] { "ChecklistId", "Code", "CreatedDate", "Discipline", "IsActive", "Name", "PageNumber", "ReferenceDocumentsJson", "SignatureRolesJson", "SubDiscipline", "WIRCode" },
                values: new object[,]
                {
                    { new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"), "CHK-PC-001", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil", true, "Construction of Precast Concrete Modular at Factory", 1, "[\"DWG-CHK-PC-001\", \"SPEC-CHK-PC-001\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Precast Concrete", "WIR-PC-001" },
                    { new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), "CHK-PC-033", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil", true, "Checklist for Pre-loading of Completed Precast Modular (MEP)", 33, "[\"DWG-CHK-PC-033\", \"SPEC-CHK-PC-033\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Precast Concrete", "WIR-PC-033" },
                    { new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"), "CHK-GEN-018", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "False Ceiling Installation", 18, "[\"DWG-CHK-GEN-018\", \"SPEC-CHK-GEN-018\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-018" },
                    { new Guid("1c73597e-55be-454c-bb34-da4b10c3468f"), "CHK-GEN-014", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Gypsum Partition Installation", 14, "[\"DWG-CHK-GEN-014\", \"SPEC-CHK-GEN-014\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-014" },
                    { new Guid("1e2cf3fe-f2d1-46ec-891c-50dcec20ab2a"), "CHK-PLB-019", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plumbing", true, "Wet Area Waterproofing", 19, "[\"DWG-CHK-PLB-019\", \"SPEC-CHK-PLB-019\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Sanitary & Drainage", "WIR-PLB-019" },
                    { new Guid("2adb1e68-6a26-4f1b-9fd3-6c1efb861823"), "CHK-GEN-012", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of HVAC Duct & Accessories", 12, "[\"DWG-CHK-GEN-012\", \"SPEC-CHK-GEN-012\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-012" },
                    { new Guid("2c6ed142-5550-4bda-9abd-9c8358a9fe0c"), "CHK-FIRE-008", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Fighting", true, "Installation of Above Ground Fire Fighting System", 8, "[\"DWG-CHK-FIRE-008\", \"SPEC-CHK-FIRE-008\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Fire Alarm", "WIR-FIRE-008" },
                    { new Guid("310e3e64-f69f-473c-8add-074fc3518340"), "CHK-GEN-010", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of Refrigerant Pipes & Accessories", 10, "[\"DWG-CHK-GEN-010\", \"SPEC-CHK-GEN-010\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-010" },
                    { new Guid("34da5bed-b063-49c7-9ccb-a9e1fe6cc192"), "CHK-FCU-002", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mechanical", true, "Installation of Fan Coil Unit (FCU)", 2, "[\"DWG-CHK-FCU-002\", \"SPEC-CHK-FCU-002\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "HVAC", "WIR-FCU-002" },
                    { new Guid("36adc1fc-39a0-4521-9da6-5186a494fe3e"), "CHK-GEN-028", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Aluminium & Glazing", 28, "[\"DWG-CHK-GEN-028\", \"SPEC-CHK-GEN-028\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-028" },
                    { new Guid("3cda5b74-a9dc-440e-9456-a8181f543524"), "CHK-PLB-007", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plumbing", true, "Above Ground Water Supply Network (PPR Pipes & Fittings)", 7, "[\"DWG-CHK-PLB-007\", \"SPEC-CHK-PLB-007\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Sanitary & Drainage", "WIR-PLB-007" },
                    { new Guid("64a930d5-2e4b-4adb-88e0-e58b20e39831"), "CHK-GEN-023", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Kitchen Cabinets Installation", 23, "[\"DWG-CHK-GEN-023\", \"SPEC-CHK-GEN-023\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-023" },
                    { new Guid("67e083d7-7d8f-4f70-85ae-8875936491c5"), "CHK-GEN-030", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Leak Test/Pressure Test Report", 30, "[\"DWG-CHK-GEN-030\", \"SPEC-CHK-GEN-030\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-030" },
                    { new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"), "CHK-GEN-025", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Doors & Windows Installation", 25, "[\"DWG-CHK-GEN-025\", \"SPEC-CHK-GEN-025\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-025" },
                    { new Guid("75eb0275-36e8-472a-b410-e299f7e7a212"), "CHK-GEN-021", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Epoxy Flooring", 21, "[\"DWG-CHK-GEN-021\", \"SPEC-CHK-GEN-021\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-021" },
                    { new Guid("76108167-b161-4f11-b136-c9cb62a9cb1e"), "CHK-GEN-022", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Continuity and Insulation Resistance Test for Cables", 22, "[\"DWG-CHK-GEN-022\", \"SPEC-CHK-GEN-022\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-022" },
                    { new Guid("7e5ca9b1-a0de-4a9e-8a93-c99a99d3b082"), "CHK-GEN-026", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of Sanitary Wares & Accessories", 26, "[\"DWG-CHK-GEN-026\", \"SPEC-CHK-GEN-026\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-026" },
                    { new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"), "CHK-TILE-006", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finishing", true, "Ceramic Tiling Checklist", 6, "[\"DWG-CHK-TILE-006\", \"SPEC-CHK-TILE-006\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Tiling", "WIR-TILE-006" },
                    { new Guid("8ff6870a-c524-4851-b934-3494d4343f3d"), "CHK-GEN-024", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Wardrobe Installation", 24, "[\"DWG-CHK-GEN-024\", \"SPEC-CHK-GEN-024\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-024" },
                    { new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), "CHK-PC-032", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil", true, "Pre-Loading of Completed Precast Modular", 32, "[\"DWG-CHK-PC-032\", \"SPEC-CHK-PC-032\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Precast Concrete", "WIR-PC-032" },
                    { new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), "CHK-PC-031", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil", true, "Pre-Loading of Completed Precast Modular (MEP)", 31, "[\"DWG-CHK-PC-031\", \"SPEC-CHK-PC-031\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Precast Concrete", "WIR-PC-031" },
                    { new Guid("c72ada83-e32d-4619-aa0e-32361160427c"), "CHK-PAINT-011", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finishing", true, "Bitumen Paint Application", 11, "[\"DWG-CHK-PAINT-011\", \"SPEC-CHK-PAINT-011\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Painting", "WIR-PAINT-011" },
                    { new Guid("cd676d10-0eac-43f4-a570-489ff60e8792"), "CHK-ELEC-013", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electrical", true, "Electrical Cable & Wire Pulling", 13, "[\"DWG-CHK-ELEC-013\", \"SPEC-CHK-ELEC-013\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Installation", "WIR-ELEC-013" },
                    { new Guid("d025c30e-3a8a-4b0a-af0b-11227845e246"), "CHK-FIRE-017", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Fighting", true, "Fire Sealing Works", 17, "[\"DWG-CHK-FIRE-017\", \"SPEC-CHK-FIRE-017\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Fire Alarm", "WIR-FIRE-017" },
                    { new Guid("d5177d0b-780d-4eb9-834b-5943201c163f"), "CHK-GEN-020", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of LV Distribution Panels", 20, "[\"DWG-CHK-GEN-020\", \"SPEC-CHK-GEN-020\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-020" },
                    { new Guid("d59c073f-4b9e-4e9d-9a84-2f2b177f102e"), "CHK-PLB-009", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plumbing", true, "Installation of Chilled Water Pipes & Fittings", 9, "[\"DWG-CHK-PLB-009\", \"SPEC-CHK-PLB-009\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Sanitary & Drainage", "WIR-PLB-009" },
                    { new Guid("d691830c-0215-42ef-8d86-ec6f36600c89"), "CHK-ELEC-003", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electrical", true, "Installation of Conduits & Boxes", 3, "[\"DWG-CHK-ELEC-003\", \"SPEC-CHK-ELEC-003\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Installation", "WIR-ELEC-003" },
                    { new Guid("e2243c47-c668-4969-8826-a58e2489d4c6"), "CHK-GEN-029", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of DX Split Unit", 29, "[\"DWG-CHK-GEN-029\", \"SPEC-CHK-GEN-029\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-029" },
                    { new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"), "CHK-PAINT-004", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finishing", true, "Painting Checklist", 4, "[\"DWG-CHK-PAINT-004\", \"SPEC-CHK-PAINT-004\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Painting", "WIR-PAINT-004" },
                    { new Guid("e958abd6-7756-48e5-9897-3ebef93aabc8"), "CHK-PLB-005", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plumbing", true, "Installation of Above Ground Drainage Pipes", 5, "[\"DWG-CHK-PLB-005\", \"SPEC-CHK-PLB-005\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Sanitary & Drainage", "WIR-PLB-005" },
                    { new Guid("ea724fcf-2d16-4ec6-bdb3-ab3023495885"), "CHK-GEN-016", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of ONU/Data", 16, "[\"DWG-CHK-GEN-016\", \"SPEC-CHK-GEN-016\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-016" },
                    { new Guid("ec3b0223-47d6-42fc-883b-db3bf8b13fa6"), "CHK-FIRE-015", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Fighting", true, "Installation of Fire Alarm", 15, "[\"DWG-CHK-FIRE-015\", \"SPEC-CHK-FIRE-015\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "Fire Alarm", "WIR-FIRE-015" },
                    { new Guid("f65ff99f-1c10-45f8-9293-303d956c7c06"), "CHK-GEN-027", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "General", true, "Installation of Access Control System", 27, "[\"DWG-CHK-GEN-027\", \"SPEC-CHK-GEN-027\"]", "[\"Site Engineer\", \"QC Inspector\", \"Project Manager\"]", "General", "WIR-GEN-027" }
                });

            migrationBuilder.InsertData(
                table: "ChecklistSections",
                columns: new[] { "ChecklistSectionId", "ChecklistId", "CreatedDate", "IsActive", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("00652d95-e040-4a72-956c-c051678686da"), new Guid("75eb0275-36e8-472a-b410-e299f7e7a212"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new Guid("34da5bed-b063-49c7-9ccb-a9e1fe6cc192"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new Guid("ea724fcf-2d16-4ec6-bdb3-ab3023495885"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("0632a654-0969-4475-9705-bc941c44a5cb"), new Guid("36adc1fc-39a0-4521-9da6-5186a494fe3e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Installation of Doors" },
                    { new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "4. Floor and Wall Tiling" },
                    { new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "7. Aluminium and Glazing Works" },
                    { new Guid("10f470e4-bfa7-4c2f-96a5-8bff23abbfa2"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "2. Final Inspection - Structural" },
                    { new Guid("11ab6eac-63bd-4ed0-9e65-35c35161a40c"), new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Preparation & Setting Out" },
                    { new Guid("13faeb36-32d0-4af9-a4b5-e42343cc30cb"), new Guid("c72ada83-e32d-4619-aa0e-32361160427c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Pre-Installation Activity" },
                    { new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Installation of False Ceiling" },
                    { new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 9, "Final Inspection - Risers" },
                    { new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new Guid("f65ff99f-1c10-45f8-9293-303d956c7c06"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "Final Inspection - Water Supply" },
                    { new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new Guid("1c73597e-55be-454c-bb34-da4b10c3468f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Installation Activity" },
                    { new Guid("265c08b1-3950-4f4c-bc6e-b3f170140138"), new Guid("1c73597e-55be-454c-bb34-da4b10c3468f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Setting Out" },
                    { new Guid("271226b4-5ba2-47f6-9805-9d5764857ed2"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "Final Inspection - Electrical" },
                    { new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "3. Internal and External Painting" },
                    { new Guid("2c0bf34b-4020-4d34-9a79-379ca9c8c5cf"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Final Inspection - Electrical" },
                    { new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"), new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new Guid("75eb0275-36e8-472a-b410-e299f7e7a212"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Application of Epoxy" },
                    { new Guid("31f6e8c0-f49a-4893-a92e-806b380e9ff4"), new Guid("64a930d5-2e4b-4adb-88e0-e58b20e39831"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("32f8c19d-98b5-445e-82cc-be1fa3069168"), new Guid("1e2cf3fe-f2d1-46ec-891c-50dcec20ab2a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, "6. False Ceiling Work" },
                    { new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new Guid("2adb1e68-6a26-4f1b-9fd3-6c1efb861823"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new Guid("8ff6870a-c524-4851-b934-3494d4343f3d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Installation of Wardrobe" },
                    { new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "8. Wooden/Metal Doors and Wood Works" },
                    { new Guid("3e33c9db-7934-4a42-84a4-b31909855523"), new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("3e57c0bb-ece0-4f95-a6c3-d5e151a44f2f"), new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"), new Guid("d025c30e-3a8a-4b0a-af0b-11227845e246"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "1. General" },
                    { new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "5. Dry wall" },
                    { new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new Guid("7e5ca9b1-a0de-4a9e-8a93-c99a99d3b082"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("51a829cc-1c7e-4834-a367-c52de630e82e"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "Final Inspection - Electrical" },
                    { new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Installation of Tile" },
                    { new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new Guid("76108167-b161-4f11-b136-c9cb62a9cb1e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "Test Information" },
                    { new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new Guid("e2243c47-c668-4969-8826-a58e2489d4c6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 9, "9. Other Finishes" },
                    { new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new Guid("8ff6870a-c524-4851-b934-3494d4343f3d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("60912bc7-31e2-435f-920c-15ca1465ffa5"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Grouting of Tile" },
                    { new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Final Inspection - Mechanical" },
                    { new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new Guid("1e2cf3fe-f2d1-46ec-891c-50dcec20ab2a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Installation of Windows" },
                    { new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 13, "Final Inspection - Light Fittings" },
                    { new Guid("7cb18d28-c2f9-418d-848b-fef4d1a0d2e7"), new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Setting Out" },
                    { new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 9, "Final Inspection - Wiring Devices" },
                    { new Guid("825aaefe-e63e-4948-ae0b-4bf3203d5458"), new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new Guid("d691830c-0215-42ef-8d86-ec6f36600c89"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new Guid("d59c073f-4b9e-4e9d-9a84-2f2b177f102e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Erection / Assembling of Precast Element - Internal Sides" },
                    { new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, "Final Inspection - Drainage" },
                    { new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 12, "Final Inspection - Wire, Cables, Conduits and accessories" },
                    { new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "Final Inspection - Wire, Cables, Conduits and accessories" },
                    { new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new Guid("310e3e64-f69f-473c-8add-074fc3518340"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Final Inspection - Chilled Water Pipes" },
                    { new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new Guid("1e2cf3fe-f2d1-46ec-891c-50dcec20ab2a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Application of Waterproofing" },
                    { new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new Guid("cd676d10-0eac-43f4-a570-489ff60e8792"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new Guid("d5177d0b-780d-4eb9-834b-5943201c163f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Final Inspection - Chilled Water Pipes" },
                    { new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"), new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Application of External Paint" },
                    { new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new Guid("2c6ed142-5550-4bda-9abd-9c8358a9fe0c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new Guid("3cda5b74-a9dc-440e-9456-a8181f543524"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new Guid("64a930d5-2e4b-4adb-88e0-e58b20e39831"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Cabinet Counter Top" },
                    { new Guid("aa5b8792-608a-46ae-8e88-8d662b42c97a"), new Guid("75eb0275-36e8-472a-b410-e299f7e7a212"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Final Inspection - Drainage" },
                    { new Guid("b1177a06-5499-43e2-974e-865c3bfa6735"), new Guid("d025c30e-3a8a-4b0a-af0b-11227845e246"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Application" },
                    { new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new Guid("ec3b0223-47d6-42fc-883b-db3bf8b13fa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"), new Guid("d025c30e-3a8a-4b0a-af0b-11227845e246"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Preparation Works" },
                    { new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "Final Inspection - Risers" },
                    { new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new Guid("e958abd6-7756-48e5-9897-3ebef93aabc8"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General Inspection" },
                    { new Guid("b9b52c68-a027-4de2-82db-ccbe3fb429a2"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, "Final Inspection - Firefighting" },
                    { new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new Guid("67e083d7-7d8f-4f70-85ae-8875936491c5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "Test Information" },
                    { new Guid("c6f3d346-bb91-4635-81fa-9da7415fab4a"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Final Inspection - Mechanical" },
                    { new Guid("c8b6346b-088b-4e03-b64d-a5dbd311b713"), new Guid("36adc1fc-39a0-4521-9da6-5186a494fe3e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"), new Guid("c72ada83-e32d-4619-aa0e-32361160427c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Final Inspection - Firefighting" },
                    { new Guid("df4586d9-9081-4c69-b520-25d3d293b7d1"), new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Setting Out" },
                    { new Guid("e140f9d3-d980-4801-ba40-1906302fbac0"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Final Inspection - Painting" },
                    { new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new Guid("c72ada83-e32d-4619-aa0e-32361160427c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Application of Bitumen Paint" },
                    { new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 11, "Final Inspection - Wiring Devices" },
                    { new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 11, "Final Inspection - Light Fittings" },
                    { new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new Guid("36adc1fc-39a0-4521-9da6-5186a494fe3e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Installation of Aluminium and glazing" },
                    { new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Erection / Assembling of Precast Element - External Walls" },
                    { new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 3, "Application of Internal Paint" },
                    { new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"), new Guid("1c73597e-55be-454c-bb34-da4b10c3468f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("f4150c0f-aa65-4c9e-9c06-6f01b65f9d0f"), new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, "General" },
                    { new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new Guid("98448459-61c2-4ab7-9e77-94cd67601168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "10. Others" },
                    { new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new Guid("64a930d5-2e4b-4adb-88e0-e58b20e39831"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Surface Preparation" },
                    { new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Final Inspection - HVAC" },
                    { new Guid("fc0ff806-1930-4124-a6c8-a429c09881b5"), new Guid("8ff6870a-c524-4851-b934-3494d4343f3d"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, "Setting Out" },
                    { new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Final Inspection - Water Supply" }
                });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "ChecklistNumber", "ChecklistSectionId", "CreatedDate", "Description", "IsActive", "Part", "Reference", "Sequence" },
                values: new object[,]
                {
                    { new Guid("0107278c-049d-4a73-a843-c0fbe905910e"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Sink installed Properly and as per location on dwg.", true, 1, "REF-0347", 9 },
                    { new Guid("01b4f4dc-ff27-4d0a-a4ec-bd84b62b9be7"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the WFT as per the project specifications / manufacturer recommendations.", true, 2, "REF-0293", 7 },
                    { new Guid("0220c2d3-0029-4885-bd22-68b3aa6c9779"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of coats as per project requirements / manufacturer recommandations.", true, 2, "REF-0290", 4 },
                    { new Guid("02ced833-2e61-430d-b297-a885c3c9e8d7"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire alram Cable pulled", true, null, "REF-0533", 3 },
                    { new Guid("02e45a45-e6d3-4e09-ac80-5402e7ea8322"), 31, new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PICV Installation with insulation box", true, null, "REF-0487", 5 },
                    { new Guid("03477226-c72e-4a97-9e6d-5671ed649240"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activities.", true, null, "REF-0374", 12 },
                    { new Guid("03e78227-dbf6-478e-8c01-f9d22c823fca"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wireless Switch Janitor & Linen Room", true, 4, "REF-0665", 5 },
                    { new Guid("04698a62-e44f-4e89-acc6-cbcded5c5002"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smoke detector", true, 2, "REF-0683", 5 },
                    { new Guid("04c346e9-ba12-4d6d-aa06-60eef6c346d0"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the materials are as per approved material submitted.", true, null, "REF-0183", 1 },
                    { new Guid("04d7af06-438e-450c-bfe9-eb37e06f5e6b"), 14, new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.", true, 1, "REF-0209", 3 },
                    { new Guid("04fe8436-2097-447c-b9bf-0abe2be0b48a"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.", true, null, "REF-0191", 9 },
                    { new Guid("052a5cf4-6021-408e-adb1-9c7a7764a6c4"), 31, new Guid("e140f9d3-d980-4801-ba40-1906302fbac0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Paint final coating", true, null, "REF-0489", 2 },
                    { new Guid("05ac7dcc-43ca-4a68-93e4-029a3d80b90d"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the rate of application as per the manufacturer recommendation and method statement.", true, 2, "REF-0289", 3 },
                    { new Guid("05d9ba73-0f20-4d10-a6b1-9aa80e26cc17"), 24, new Guid("fc0ff806-1930-4124-a6c8-a429c09881b5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the location and clear opening of leafs and drawers are as per the approved drawings.", true, null, "REF-0361", 2 },
                    { new Guid("062fda0b-afba-44df-8476-7779f48df1a7"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shut off Valves shall be arranged closed, when rotating in clock wise direction and provided as per approved Shop drawings", true, null, "REF-0121", 9 },
                    { new Guid("06c99882-5fea-46db-b48f-f6d68f6d52b2"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switched Flex outlet with Neon Indicator- Hood", true, null, "REF-0527", 15 },
                    { new Guid("07743bce-92f3-4b74-b141-5f55d07b539f"), 4, new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of Primer as per manufacturers recommendation", true, 5, "REF-0072", 1 },
                    { new Guid("07b8aa1e-ed4e-4665-ab9f-d0d600d154a9"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the fans are freely rotating.", true, null, "REF-0033", 6 },
                    { new Guid("081b58c5-4cbc-4750-b165-1b79d70c2c72"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Supply Pipe", true, null, "REF-0506", 4 },
                    { new Guid("085bb58e-540d-4b6c-84cc-54776d64fc89"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the routing and layout is as per approved construction drawings.", true, null, "REF-0132", 4 },
                    { new Guid("08bc6230-4d6b-42c2-be3f-3a771890958b"), 23, new Guid("31f6e8c0-f49a-4893-a92e-806b380e9ff4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method of statement is being followed.", true, 1, "REF-0338", 3 },
                    { new Guid("08d47879-25c8-4009-8a5a-43bb1ce2d4ad"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The materials/type/model/capacity as per approved material submittal.", true, null, "REF-0028", 1 },
                    { new Guid("09272c94-f0e9-44b3-8313-c067d777eebe"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installed pipes are free of sag & bend.", true, null, "REF-0455", 14 },
                    { new Guid("09b3e941-a3fc-426f-ba68-980e3a7a6a27"), 1, new Guid("825aaefe-e63e-4948-ae0b-4bf3203d5458"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, 1, "REF-0002", 2 },
                    { new Guid("09b3f44e-67bc-43bc-8b88-7bf51e8a7950"), 31, new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piping Leak test", true, null, "REF-0501", 4 },
                    { new Guid("0c00f837-611d-446d-b658-2c3a601eb921"), 25, new Guid("7cb18d28-c2f9-418d-848b-fef4d1a0d2e7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the location and clear opening of doors / windows are as per the approved drawings.", true, null, "REF-0380", 1 },
                    { new Guid("0c083254-bbf4-4496-85c5-173dec86ddef"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area.", true, 3, "REF-0027", 13 },
                    { new Guid("0c13ecc1-3223-4c16-9abb-51e69e1e727d"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waste Pipe", true, null, "REF-0504", 2 },
                    { new Guid("0c897c36-13c9-45f8-ab97-98a0a73ef823"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the rate of application as per the manufacturer recommendation and method statement.", true, 2, "REF-0324", 8 },
                    { new Guid("0c9e3cff-e7a2-4432-9831-430d56a1d002"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the drawing used for the installation are current and approved.", true, null, "REF-0115", 3 },
                    { new Guid("0d573c4b-545c-445d-a0b2-e145c1a7f9aa"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the installation of cables is co-ordinated with other cables", true, null, "REF-0205", 11 },
                    { new Guid("0dcd763b-9cbf-43c8-93a3-49b24a0592b5"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All joints on paint able surfaces filled with sealant properly.", true, 3, "REF-0024", 10 },
                    { new Guid("0dd86d11-6cf6-42b4-b489-20e51c6a5d3c"), 32, new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Final Condition of outside of the room and ensure its damage free", true, null, "REF-0618", 1 },
                    { new Guid("0e4990a2-a019-448a-9e4d-a8075fb704ef"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Calibration Due Date", true, null, "REF-0333", 8 },
                    { new Guid("0e79780d-627c-4de4-85ef-caee115df80f"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "External Paint(Application of Primer, Texture)", true, null, "REF-0558", 3 },
                    { new Guid("0e825a1e-597c-4249-969c-d9bf8ecbf217"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "No visible damage on the materials.", true, null, "REF-0148", 2 },
                    { new Guid("0f111bb0-0aeb-4000-bb91-f398ef931222"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the installed Distribution Boards have approved submittals.", true, null, "REF-0299", 1 },
                    { new Guid("0f28494f-d778-4deb-a1a6-31bb36337ac8"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reference Documents", true, null, "REF-0458", 2 },
                    { new Guid("0f93f07b-c4c2-448b-b248-12bbbfafb094"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Shelf Supports (Brackets or Pins)", true, 1, "REF-0342", 4 },
                    { new Guid("0fd1838f-30f3-4548-862a-36e0cf28e7ea"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D4-1 Spot Ceiling Mounted light Toilet", true, 3, "REF-0694", 4 },
                    { new Guid("103277d0-ab14-4d98-b096-c39cd2a39bad"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Floor drain and covers installed and free from damages", true, null, "REF-0608", 6 },
                    { new Guid("103c2da9-1b8b-4e09-80f4-1bfc2c173295"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the condensate drain pipes are connected with proper slope.", true, null, "REF-0456", 15 },
                    { new Guid("105594e5-688b-46e5-b08d-01eac81171c3"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Access panels/ Ceiling acoustic tiles are Fixed Properly", true, null, "REF-0577", 3 },
                    { new Guid("111d0079-f84b-46e7-b2ff-dbf0b74f3ba1"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "L1-Led Strip Light Under Kitchen Cabinet", true, 3, "REF-0695", 5 },
                    { new Guid("129bc385-a35c-4168-9957-2b389fc2a4aa"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The materials/type/model/capacity as per approved material submittal.", true, null, "REF-0442", 1 },
                    { new Guid("12af8392-002a-4405-885a-b366a78058c1"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Override Switch Electrical Room", true, 4, "REF-0666", 6 },
                    { new Guid("12c6986e-441d-431c-b9e1-b4fe891025ab"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Fire Blankets location as per approved drawing", true, null, "REF-0144", 16 },
                    { new Guid("13724d8f-e010-42ea-82fd-183a6e9d05bc"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, 1, "REF-0640", 1 },
                    { new Guid("13a573ec-e219-4be6-a570-ada6431e3002"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the valve package to the indoor unit as per approved.", true, null, "REF-0038", 11 },
                    { new Guid("140cc6e1-5532-4bf3-a0d2-b7520efd62e3"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the overlapping of the flashings around the frame are as per the approved drawings.", true, null, "REF-0398", 3 },
                    { new Guid("1416a5de-152c-4c95-bcc9-f983c1e4ad88"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check If wet connections are proper and have no wavy, depressed or bulge surface.", true, 3, "REF-0023", 9 },
                    { new Guid("146e6018-a542-4f56-bf5e-90cbe0f47888"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water supply line connected to the fixtures", true, null, "REF-0407", 4 },
                    { new Guid("1489ed9f-dd78-4c1c-bf36-a226bbef0f20"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe accessories as per approved drawings", true, null, "REF-0600", 14 },
                    { new Guid("14b4062e-6fb6-4172-a9ab-ee5d2f19e6be"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Direction of doors swing as per App Drawing", true, null, "REF-0588", 2 },
                    { new Guid("15629b0b-5874-4820-aaca-2110feb63147"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Termination materials as per the approved MAR.", true, null, "REF-0245", 11 },
                    { new Guid("1566dece-d7b0-4e35-9d57-8e6a42642b3d"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Obtain approval from consultant / Client to proceed with further works.", true, 1, "REF-0273", 8 },
                    { new Guid("16ad46e9-8938-49dc-81c5-1b9ae6aae443"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "No visible damage on the materials", true, null, "REF-0078", 3 },
                    { new Guid("16df93a1-6764-46e4-ae7c-f7c5e9cc094f"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check if wall plumb of all sides are within the allowable tolerance", true, 2, "REF-0009", 4 },
                    { new Guid("16e1acac-c15b-49b7-9958-ada01ee88920"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of Hardner as per manufecturers recommendation", true, 2, "REF-0318", 2 },
                    { new Guid("1715dbde-644c-49fc-b368-0f158ff81071"), 19, new Guid("32f8c19d-98b5-445e-82cc-be1fa3069168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement and materials are approved.", true, 1, "REF-0279", 1 },
                    { new Guid("172c06d1-2f4b-4308-bc82-294745d500d1"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Single Socket outlet with Neon Indicator", true, null, "REF-0522", 10 },
                    { new Guid("1772d3a5-c9cf-4b90-84c2-0495e59caa57"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Return Duct", true, null, "REF-0475", 2 },
                    { new Guid("17aeccc1-bbaa-4ef6-b59d-af67e31ddece"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Hammer Arrestor installed in the approved location.", true, null, "REF-0493", 2 },
                    { new Guid("182b9f7c-e4ba-4d4c-845b-c6572c78460d"), 18, new Guid("3e57c0bb-ece0-4f95-a6c3-d5e151a44f2f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the color, type, material, pattern and thickness are as per approved material approval and project requirements.", true, 1, "REF-0263", 3 },
                    { new Guid("18d723c7-436d-4711-8cbc-ea31d259bf39"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Pipe levels as per the approved shop / coordination drawing.", true, null, "REF-0134", 6 },
                    { new Guid("19a7f51f-b924-4082-9248-7e58393e202d"), 18, new Guid("3e57c0bb-ece0-4f95-a6c3-d5e151a44f2f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials (Gypsum board, tiles, suspension systems, etc.,) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, 1, "REF-0262", 2 },
                    { new Guid("1a9e047a-ceef-4557-a7a1-7d7ae42db8a7"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Remarks If Any", true, null, "REF-0471", 15 },
                    { new Guid("1aa26ac0-0531-4db8-b9f2-f62ae6963262"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Door Bell -230V Electromechanical chime", true, 4, "REF-0677", 17 },
                    { new Guid("1ab21f95-1979-4859-aefc-64df110e42bc"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that the piping accessories installed as per approved project specs.", true, null, "REF-0162", 2 },
                    { new Guid("1ba35898-fa1c-4e8d-9f2d-d2729089870c"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Slope should be maintained for all pipes", true, null, "REF-0081", 6 },
                    { new Guid("1c897f5d-0399-4b61-9993-1f9a58c10692"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout of False Ceiling tiles and bulk head as per App Drawing", true, null, "REF-0575", 1 },
                    { new Guid("1c955271-6607-4f41-bb6b-f070821c5375"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the fire rated doors are provided with fire rated sealant & fire rating tags as per the project requirements.", true, null, "REF-0390", 8 },
                    { new Guid("1ca61259-3a0c-46b1-8d26-1554ffea85f1"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the MEP clearance prior to start Painting works.", true, 1, "REF-0097", 6 },
                    { new Guid("1cb92d12-4546-4441-a394-7e5ec18b0eb4"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the filling of water and monitor the level of filled water during the leakage test.", true, 2, "REF-0296", 10 },
                    { new Guid("1ce8f571-d81c-45c8-bc95-a91733b63f0d"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "W1-Wall mounted recessed balcony light", true, null, "REF-0549", 7 },
                    { new Guid("1d0064cd-a381-42e3-9bd1-b226e241111c"), 19, new Guid("32f8c19d-98b5-445e-82cc-be1fa3069168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the expiry date of the material prior to applications.", true, 1, "REF-0281", 3 },
                    { new Guid("1d04d260-733e-473d-98ac-5830df530b96"), 23, new Guid("31f6e8c0-f49a-4893-a92e-806b380e9ff4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and drawings are approved.", true, 1, "REF-0336", 1 },
                    { new Guid("1d16913a-856c-4bcd-89fe-332d7b62e41f"), 17, new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the mixing of materials as per the manufacturer recommandations", true, null, "REF-0257", 4 },
                    { new Guid("1d1aeb89-68aa-49c6-b2c0-020ef825deda"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Cable pulled", true, null, "REF-0682", 4 },
                    { new Guid("1db870d3-d222-4d0f-a085-fa9541527f02"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the location of sprinkler heads as per the approved drawing.", true, 1, "REF-0142", 14 },
                    { new Guid("1e4d0c53-28fd-439b-9eb2-cae591c45edc"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Tiles are fixed with grouting properly and free from damage", true, null, "REF-0614", 12 },
                    { new Guid("1e8d2475-09b2-4432-b759-546956257b4f"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe layout/routing as per approved shop drawing.", true, null, "REF-0150", 4 },
                    { new Guid("1eb79f3e-cb02-44f2-a80d-9ed634952cd0"), 4, new Guid("3e33c9db-7934-4a42-84a4-b31909855523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Undulations or irregular corners are repaired and grinded as required.", true, 1, "REF-0066", 4 },
                    { new Guid("1eeaa530-b6cb-44e7-ae74-3718b1e5e872"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test Instrument Details", true, null, "REF-0331", 6 },
                    { new Guid("1f8f3d81-bf07-487f-b306-f3f861967a64"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "W2- Wall mounted toilet light above Mirror", true, 3, "REF-0698", 8 },
                    { new Guid("1f916cb0-a3fc-4e72-a79b-b8da1d0de105"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activities.", true, 2, "REF-0298", 12 },
                    { new Guid("1fc02631-b0c8-4697-946e-e37a8d22af7a"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conduits installed in exposed areas subject to mechanical damage shall be IMC or RSC, and those exposed but not subject to mechanical damages shall be EMT. The conduits embedded in walls or encased in concrete and structural slabs shall be PVC conduits.", true, null, "REF-0058", 11 },
                    { new Guid("1fc723a9-a922-4a2e-a37a-4271dda7a7d8"), 24, new Guid("fc0ff806-1930-4124-a6c8-a429c09881b5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the wardrobe jamb area are solid and rigid.", true, null, "REF-0362", 3 },
                    { new Guid("20284c8e-b358-49ab-8638-c91e42bed6c3"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe joints are properly made and are tight/secure", true, null, "REF-0087", 12 },
                    { new Guid("20bf8edf-7344-404c-9632-329cca228deb"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location of Doors as per App Drawing", true, null, "REF-0587", 1 },
                    { new Guid("211ce4b1-ed70-497f-9fc4-cfc3dc2ee3f8"), 31, new Guid("c6f3d346-bb91-4635-81fa-9da7415fab4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Grills/Diffuser", true, 1, "REF-0482", 3 },
                    { new Guid("21a096df-ea8e-47e1-bebe-9e1c5b79af43"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check RJ 45conector and modules are fixed properly on data and telephone outlets", true, null, "REF-0243", 9 },
                    { new Guid("21a83d6e-3a43-497c-bc26-84622f6dd12d"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the cables and accessories are as per approved material submittal", true, null, "REF-0195", 1 },
                    { new Guid("21bd8d7d-5c9e-4296-9946-31744d8a6d73"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Kitchen Hood and Flexible Duct", true, null, "REF-0478", 5 },
                    { new Guid("224dac96-da3b-4b43-ba41-35ff26695f69"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Automatic Air Release valves are provided on the highest pipe points on Risers.", true, null, "REF-0146", 18 },
                    { new Guid("22660045-5194-4abb-962e-06e1d5edcdf9"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval to proceed with further works obtained from consultant / Client.", true, 4, "REF-0112", 6 },
                    { new Guid("2307dd77-092a-459c-8cf8-9dd7d4052532"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the dielectric unions/flexible connector are connected.", true, null, "REF-0036", 9 },
                    { new Guid("24af3611-7a4d-4aa3-9c67-eb97775109d4"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe installed as per approved drawings", true, null, "REF-0599", 13 },
                    { new Guid("2603664d-ce1f-4e14-8bd6-126947a083e6"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Drainhole are free from any debris and properly closed (if applicable)", true, null, "REF-0568", 7 },
                    { new Guid("2639d198-6e90-4dd8-8c5b-9786c8ef2133"), 19, new Guid("32f8c19d-98b5-445e-82cc-be1fa3069168"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, 1, "REF-0280", 2 },
                    { new Guid("26f83daf-f89d-4ece-a1b3-a9d6c00307a5"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check If all wet & mechanical connection done & repaired and has no wavy or bulge surface", true, 2, "REF-0011", 6 },
                    { new Guid("27ba937a-9c7f-4ecc-b9f3-9a9b161eb76c"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vent pipes are provided to the manhole as per approved drawing", true, null, "REF-0086", 11 },
                    { new Guid("27cbb526-4d6a-43d8-8907-604d684b7734"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the outdoor unit space around as per manufacturer recommendations.", true, null, "REF-0039", 12 },
                    { new Guid("288645d9-3492-43e0-82bd-490517fd0c6f"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Circuit Number", true, null, "REF-0328", 3 },
                    { new Guid("290ce1bf-7ffb-4b42-94e4-17fe1242a3e1"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Obtain approval for the application of waterproofing to proceed water leakage test.", true, 2, "REF-0295", 9 },
                    { new Guid("2b6382a0-29a5-485b-918c-4b04719ac277"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erection of external walls by Temporary Support (props and brackets) Outer", true, 2, "REF-0006", 1 },
                    { new Guid("2c036e3d-7375-4a49-af8a-75725ac8467e"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "No visible damage on the materials.", true, null, "REF-0443", 2 },
                    { new Guid("2c28fec3-3c39-4cd9-bae3-f4e3ad082d99"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the approved type/capacity vibration isolator installed.", true, null, "REF-0032", 5 },
                    { new Guid("2c47d839-6b1e-4f7b-b427-169b849701c2"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duration of Test", true, null, "REF-0466", 10 },
                    { new Guid("2c65ce6c-4aeb-4dcf-adf5-79fd96a74fef"), 1, new Guid("11ab6eac-63bd-4ed0-9e65-35c35161a40c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Drawing Stamp, Signature, Element Tags are correct", true, 1, "REF-0004", 1 },
                    { new Guid("2c71ee06-19f7-4d83-8831-c9231b4263b4"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the method of loading as per the project / design requirements", true, null, "REF-0554", 4 },
                    { new Guid("2cc08f24-5f4d-421a-ae3a-cd8613d9e859"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the drawings used for installation are correct and approved.", true, null, "REF-0300", 2 },
                    { new Guid("2d211cf7-6a3f-4a0e-b41d-d75ca38bd0dd"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Area/Location", true, null, "REF-0459", 3 },
                    { new Guid("2d559d0e-0c62-4731-82a0-b65eacccb862"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pull off / adhesion test for the paint application as per the project specifications / manufacturer recommendations (if required).", true, 3, "REF-0105", 8 },
                    { new Guid("2d84680f-b347-4632-9caa-40f755c9f28a"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure equipment grounding connection is provided", true, null, "REF-0418", 5 },
                    { new Guid("2da4b5d8-2d4c-4e69-a8cc-7ad9028abcb0"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the undercut for the door leaves as per the approved drawings.", true, null, "REF-0392", 10 },
                    { new Guid("2ddd69b8-b5b6-4194-8205-45b75b615e37"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves are provided for the pipes passing through the walls/slabs.", true, null, "REF-0153", 7 },
                    { new Guid("2f04f0df-4ed9-4372-98b9-3ced3231f320"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen cabinets, counter top installed as per app drawing", true, null, "REF-0596", 10 },
                    { new Guid("2f20a7a6-7402-4bf5-b3a4-76ff474b3ec3"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Checks the location of Fire hose cabinet, Fire Hose reel, Pressure Reducing Landing valves, Fire hoses with Nozzles are as per the approved drawing.", true, null, "REF-0141", 13 },
                    { new Guid("31f46bde-e5dc-479b-be71-45bb5bdafb56"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finish Time of Test", true, null, "REF-0468", 12 },
                    { new Guid("323f619f-a585-4bdd-b32e-5aa7cdb71bd0"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varify Location of opening for curtain wall, door and windows as per drawing", true, null, "REF-0435", 4 },
                    { new Guid("33adc13d-0a8a-4d1f-9b45-5e29351b5495"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13A, DP, Simplex Switched Spur Outlet for FCUs with Neon Indicator", true, 4, "REF-0678", 18 },
                    { new Guid("33b19000-dcff-4054-a9fd-31e1b3f95233"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 2G, 1 Way switch", true, null, "REF-0514", 2 },
                    { new Guid("33d53b69-a990-4140-bfa1-73e94fccc585"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "L2-Surface Mounted linear light electrical Room & Garbage room", true, 3, "REF-0696", 6 },
                    { new Guid("34742979-182c-419f-b838-32265f835f45"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that pipe work shall be suitably identified and labeled where specified.", true, null, "REF-0166", 6 },
                    { new Guid("349bd5df-6a98-45d5-85af-062b9b403edb"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the mounting dimensions as per approved typical installation/approved architectural drawings/manufacturer recommendations.", true, null, "REF-0405", 2 },
                    { new Guid("352c036d-d372-4e89-92dc-764f8c64349a"), 25, new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the color, type, material, coating of door and window materials are as per approved material approval and project requirements.", true, null, "REF-0377", 3 },
                    { new Guid("358aedf4-b7ef-4726-89b8-f82e886540fe"), 21, new Guid("aa5b8792-608a-46ae-8e88-8d662b42c97a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the expiry date of the material prior to applications.", true, 1, "REF-0311", 3 },
                    { new Guid("35a436a3-69fe-44ed-ae55-37d57a2707bf"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation Resistance in Mega Ohms (R-Y, Y-B, B-R, R-N, Y-N, B-N, R-E, Y-E, B-E, N-E)", true, null, "REF-0330", 5 },
                    { new Guid("35c89581-d1ee-4301-aeb4-b230e856664b"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completion of Nearby Finishes", true, 1, "REF-0349", 11 },
                    { new Guid("369b6242-14d6-4fba-9b75-1235d19cf452"), 19, new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the angle fillets are provided at the floor and wall junctions of the area that to be treated.", true, 1, "REF-0284", 3 },
                    { new Guid("36aea065-6d5c-4745-8b25-5e45d1922bd1"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Grille/Defuser", true, 1, "REF-0630", 9 },
                    { new Guid("37634b72-32c2-4c01-b122-7f9724a8320a"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sensors", true, null, "REF-0537", 7 },
                    { new Guid("37ece6ee-5b00-4f59-99d7-793316077609"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Double Socket outlet with Neon Indicator", true, 4, "REF-0671", 11 },
                    { new Guid("387154b1-94a1-42a9-acb0-e8b5f784179a"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the door architrave are fixed as per the approved drawings.", true, null, "REF-0394", 12 },
                    { new Guid("38a7b53b-6f7b-467f-a8a7-19f15c50d7ea"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fire Call out", true, null, "REF-0628", 7 },
                    { new Guid("38cbe975-ccca-4392-a4df-696b155663dd"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Chilled Pipe", true, null, "REF-0624", 3 },
                    { new Guid("3930bfa3-d6de-46f6-bad0-1b53a40a2578"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the application of waterproofing on the vertical face extended upto 300mm from FFL or as per approved drawing / project requirements.", true, 2, "REF-0292", 6 },
                    { new Guid("39c663f5-cb16-4127-ade1-867d3a58a617"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of primer coat (if required).", true, null, "REF-0176", 2 },
                    { new Guid("3a916071-80b9-44de-aaf1-8277f78f6625"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Three Gang Override Switch Electrical Room", true, null, "REF-0519", 7 },
                    { new Guid("3b1c32eb-3c0b-4d71-a636-474f20ec789f"), 24, new Guid("fc0ff806-1930-4124-a6c8-a429c09881b5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the level and alignment of the wardrobe frame leaf opening with reference to the surrounding wall / cladding elevations.", true, null, "REF-0360", 1 },
                    { new Guid("3b39821f-0326-4ec1-a3b0-10e166d0e2d4"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of subsequent coats are carried out at right angle to the previous coat.", true, 2, "REF-0291", 5 },
                    { new Guid("3b5ef6f8-17de-4154-a59a-b624864e69a9"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipes are supported well with approved clamps.", true, null, "REF-0043", 16 },
                    { new Guid("3b706902-8f64-41b5-8816-1170f03dfb81"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the fixation of the board as per the approved drawings.", true, 2, "REF-0219", 8 },
                    { new Guid("3baa525a-04a9-484e-b79e-5e1b0e2bd1db"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of primer coat (if required).", true, 2, "REF-0288", 2 },
                    { new Guid("3be38508-28d0-4da7-ab04-a4309ac3a16a"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that the cables are securely fixed.", true, null, "REF-0201", 7 },
                    { new Guid("3c3552b8-391b-4e50-a2c4-fbe64ad258a6"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Kitchen hood and Flexible Duct", true, null, "REF-0626", 5 },
                    { new Guid("3c9685a6-c67d-4547-b638-14537a9d5187"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Skirting is installed/fixed properly and truly vertical", true, null, "REF-0564", 3 },
                    { new Guid("3e6d1e9e-7597-4f12-a30e-f5848931e418"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the suspension system supports are not in contact with the adjacent MEP services.", true, 1, "REF-0270", 5 },
                    { new Guid("3e75cb7f-0ae6-410b-93b4-105d000f4e30"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thermostat", true, null, "REF-0541", 11 },
                    { new Guid("3e8804eb-5eca-4c1a-b593-631961a7447d"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D1-Ceiling Mounted Light Living and Bed Room", true, null, "REF-0543", 1 },
                    { new Guid("3ec3a9e0-b3bf-461b-987d-7aebd0a58371"), 31, new Guid("60912bc7-31e2-435f-920c-15ca1465ffa5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visually inspect the MEP Services for any defects or damages", true, null, "REF-0473", 2 },
                    { new Guid("403cf397-2ad9-4f6e-889d-0d574a5f4ad4"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the all materials used have approved submittals.", true, null, "REF-0129", 1 },
                    { new Guid("40cc3cb6-1bc2-4c4c-83e8-c79b360f0e4a"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sides and corners of boxes aligned with floor layout.", true, 2, "REF-0008", 3 },
                    { new Guid("40cdd986-f749-46f0-8d6c-0a3f79daafd7"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "varify Functioning/movement of panels is as free as required", true, null, "REF-0439", 8 },
                    { new Guid("40ed5516-f7be-4b5b-999f-7cecf0a439e0"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that duct joints are sealed externally before applying insulation with approved sealant.", true, null, "REF-0187", 5 },
                    { new Guid("41260413-e43f-42c0-a861-f922cd933b19"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Discipline", true, null, "REF-0461", 5 },
                    { new Guid("41d00092-1bfc-4b42-bbd2-cb003193800a"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D2-Ceiling Mounted Light Kitchen area", true, 3, "REF-0692", 2 },
                    { new Guid("41f81e13-30db-414e-9a13-98d6374a0ef0"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Threshold installed and free from damage", true, null, "REF-0605", 3 },
                    { new Guid("42fdc704-b5e7-44b2-b45a-d8199af4d07b"), 6, new Guid("f4150c0f-aa65-4c9e-9c06-6f01b65f9d0f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored as per manufacturers recommendations.", true, 1, "REF-0090", 2 },
                    { new Guid("430bf6e6-17d8-4a63-8106-91280499b3e9"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "45 A, DP switch with flex outlet for cooker and appliances", true, null, "REF-0525", 13 },
                    { new Guid("43adc193-73e6-4cd3-8a27-5f6f42e919b4"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Alarm Control panel -ELV room", true, null, "REF-0526", 14 },
                    { new Guid("43b81437-5409-4e34-824e-985cf80fe25c"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D3-Spot Ceiling Mounted light Coridoor", true, null, "REF-0545", 3 },
                    { new Guid("43c7e739-2e6e-4aa9-966f-55544237d3f8"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the connections are done correctly (as per approved methods)", true, null, "REF-0230", 6 },
                    { new Guid("441afc7f-a0cb-4cb2-b386-312c14f24160"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Panel to Panel Connections are as per drawing", true, 2, "REF-0019", 5 },
                    { new Guid("45efd065-5c15-435d-9328-1b94f681c251"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the sleeves are provided on structural elements where the pipes are penetrating to & from the Building /Partician/ Walls / Slabs", true, null, "REF-0117", 5 },
                    { new Guid("45f74fc8-45c1-40fc-a63d-0aff25455051"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that grounding has been done according to the project Specification", true, null, "REF-0052", 5 },
                    { new Guid("4642025b-d9a5-42b2-991e-bd1110e5a444"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "45 A, DP switch with flex outlet for cooker and appliances", true, 4, "REF-0673", 13 },
                    { new Guid("4674359e-6b09-46ce-8f11-dcf690d8b575"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Exhaust Air Duct", true, null, "REF-0477", 4 },
                    { new Guid("469b11c3-9b29-432e-a40e-41f288e6bad6"), 17, new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the surface for any latiance, dirt, loose materials around the penetrations.", true, null, "REF-0255", 2 },
                    { new Guid("46de0b44-619e-4283-ada6-5fbb026ca41a"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.", true, 1, "REF-0303", 5 },
                    { new Guid("470f9567-f0f6-4722-aa32-788992c47f25"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 3G, 1 Way switch", true, null, "REF-0515", 3 },
                    { new Guid("478bf621-712d-42bf-9da7-7b51a085562e"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activities.", true, null, "REF-0182", 8 },
                    { new Guid("4935f231-22dc-4e2e-a002-736a51a32f35"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the roller slides/drawer runners as per the approved drawings.", true, null, "REF-0370", 8 },
                    { new Guid("494f70ea-6edc-4323-bd7b-9f0871b2fb91"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Supply Pipe", true, null, "REF-0654", 4 },
                    { new Guid("49cb5eb3-b64d-4e6f-b512-217892c688f5"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure adequate clearance available around panel as required and as per drawings.", true, null, "REF-0304", 6 },
                    { new Guid("4af9ee4e-e534-4724-b5ad-fc01a37a4ba3"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that the valves installed as per approved shop drawings, approved specs and as per manufacturer recommendation.", true, null, "REF-0165", 5 },
                    { new Guid("4b138d2b-15d4-46e7-93d3-0348ac2840af"), 28, new Guid("c8b6346b-088b-4e03-b64d-a5dbd311b713"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, null, "REF-0425", 1 },
                    { new Guid("4b168451-bbb5-432b-81cd-6856015d72ec"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of VCD", true, null, "REF-0479", 6 },
                    { new Guid("4baf7b8f-a015-4076-9b8e-91801d77a5aa"), 14, new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, 1, "REF-0208", 2 },
                    { new Guid("4cf893c7-8ab5-407f-8f37-954f580d197e"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that Leakage test done as per approved specs.", true, null, "REF-0167", 7 },
                    { new Guid("4d0d208f-72e9-46a0-b4af-cb811303a757"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper cable tagging/identification are provided", true, null, "REF-0420", 7 },
                    { new Guid("4d6ac49a-5417-4dd8-b419-e4bfb3934a9e"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Drain points are provided with approved valves. (Test & Drain system).", true, null, "REF-0140", 12 },
                    { new Guid("4d7872fd-5fc7-4605-819a-7707b041d18e"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "After Hydrotest Valves shall be fully wrapped with corrossion resistant adhesive tape up to spindle of the valve.", true, null, "REF-0123", 11 },
                    { new Guid("4d9a2c2a-ec21-489a-a5df-1892b56029fa"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chilled Water pipe", true, null, "REF-0507", 5 },
                    { new Guid("4e12e9de-426f-4bbb-b512-8611c61a635c"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the surface roughness is appropriate as per the tile and tile adhesive manufacturer recommendations.", true, 1, "REF-0094", 3 },
                    { new Guid("4e6dd28d-ce0b-493a-ac7d-3acf97bacb9c"), 18, new Guid("3e57c0bb-ece0-4f95-a6c3-d5e151a44f2f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, material submittal and drawings are approved.", true, 1, "REF-0261", 1 },
                    { new Guid("4ec4fcb2-4ae4-488c-8ea7-2e041f084c48"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "No visible damage on the materials.", true, null, "REF-0029", 2 },
                    { new Guid("4f15b0b2-0b65-46ed-9666-d8718ed18631"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the grounding has been provided as per approved drawing including the body and door of the panel.", true, null, "REF-0308", 10 },
                    { new Guid("50143032-0002-4d9b-ae22-d53cd37cfdc8"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installed pipes are free of sag & bend", true, null, "REF-0085", 10 },
                    { new Guid("50b8d94c-afbb-44f4-b8d2-e19ec3a51342"), 24, new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify and record the DCL product conformity certificate for the sealants to be used.", true, null, "REF-0358", 4 },
                    { new Guid("512447f2-f057-49a9-9b20-8adf07bd285f"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patch panel installation as per the approved rack schedule", true, null, "REF-0249", 15 },
                    { new Guid("514366d3-9f52-403a-907b-0eb230a3d09a"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duct Riser and connection", true, null, "REF-0509", 7 },
                    { new Guid("52389a97-13cb-4105-b765-d9f9042e1c28"), 4, new Guid("3e33c9db-7934-4a42-84a4-b31909855523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for repair of surface imperfection and protrusions (if any).", true, 1, "REF-0064", 2 },
                    { new Guid("52793a37-42d0-4912-b482-4f00fe58808a"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the closing of shaft openings, MEP penetrations and other openings as per project requirements.", true, 1, "REF-0267", 2 },
                    { new Guid("52e4c536-a852-4100-a959-340427f8a202"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.", true, 2, "REF-0221", 10 },
                    { new Guid("52f35c66-7592-4374-84bf-40f96d56bcaf"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Vanity installed and free from damage", true, null, "REF-0609", 7 },
                    { new Guid("5323fb3c-6fbb-4a90-8b09-bdf91bf90f94"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod WC and cover installed and free from damage", true, null, "REF-0610", 8 },
                    { new Guid("53e28149-b4ea-4505-a216-88d94d17bda1"), 25, new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, material submittal and drawings are approved.", true, null, "REF-0375", 1 },
                    { new Guid("53ea654d-85d1-45a3-a724-36a211ed2002"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen sink and accessories", true, 1, "REF-0645", 6 },
                    { new Guid("540425a1-235d-43cf-b6c6-243fbcd3633d"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the fire extinguisher location as per approved drawing", true, null, "REF-0143", 15 },
                    { new Guid("541e14c6-0ff2-4f1f-845a-ad45e2411cad"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the model of sanitary wares are as per approved material submittal", true, null, "REF-0404", 1 },
                    { new Guid("553f5c81-5d40-4a60-8164-e918a0c480aa"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All Wires pulled as per the approved Drawings.", true, null, "REF-0679", 1 },
                    { new Guid("5614599f-a50d-4493-9b16-0906caa3cb28"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure containment has been completed prior to start cable or wire pulling", true, null, "REF-0197", 3 },
                    { new Guid("56334ad3-57c0-4e76-8e3f-22b208c8949c"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, null, "REF-0492", 1 },
                    { new Guid("56552405-cd96-4c58-9382-ca96ade1874b"), 19, new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure grouting, angle fillet provided all around the penetrations and the projected MEP services are neat and clean from any latiance.", true, 1, "REF-0283", 2 },
                    { new Guid("570418a2-3efe-4595-82e5-4f685fa896b7"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visually inspect the modular for any defects or damages", true, null, "REF-0553", 3 },
                    { new Guid("57212d2b-4fb7-414c-a1b0-e05abafff7bf"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the sleeves are installed for piping passing through walls / penetrations as required.", true, null, "REF-0133", 5 },
                    { new Guid("5754a1ad-7e69-452b-a749-ed982ff0bd49"), 33, new Guid("271226b4-5ba2-47f6-9805-9d5764857ed2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Identification tag of the modular", true, null, "REF-0620", 1 },
                    { new Guid("578cb175-3ca9-4ae4-a51a-dddfe7833fe3"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the drawing used for the installation are current and approved", true, null, "REF-0049", 2 },
                    { new Guid("58208b50-c7ab-4aab-8abf-d12e0bc499b8"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the material used are as per the Approved Material Submittal.", true, null, "REF-0113", 1 },
                    { new Guid("585e23d7-1214-4e49-9a47-7b75c9c9e591"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gate Valve Installation", true, null, "REF-0496", 5 },
                    { new Guid("58df44d8-fc05-497c-9fba-aac2bb0812e5"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switched Flex outlet with Neon Indicator- Hood", true, 4, "REF-0675", 15 },
                    { new Guid("591499dc-c5ba-433a-a2b9-2d14f9bf952b"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of tile grout as per the manufacturer recommendations.", true, 4, "REF-0111", 5 },
                    { new Guid("593f6152-0b33-4ffc-9db4-40eef486ef60"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of VCD", true, null, "REF-0627", 6 },
                    { new Guid("594b64c5-e8b8-48df-a1fd-7c4cff067732"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the application of fire sealant / fire protection works are carried out as per the project requirements (if applicable).", true, 1, "REF-0268", 3 },
                    { new Guid("5951afd7-c2e3-466d-9a92-a4c2f7fb54e8"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe sizes are as per approved shop drawing.", true, null, "REF-0149", 3 },
                    { new Guid("59590745-cf63-4ca9-879f-2c9e41989e06"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Testing Fluid", true, null, "REF-0465", 9 },
                    { new Guid("599a1b61-2cc0-4f5c-b1cd-a29aa3b4fda9"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activities.", true, null, "REF-0403", 8 },
                    { new Guid("59dbe1e9-14dd-4498-b9fb-767b8a428a62"), 11, new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the MEP clearance are obtained prior to start bitumen application.", true, null, "REF-0174", 4 },
                    { new Guid("5a993c86-32c7-472a-a39d-b58fd2fc1c02"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Confirm Cable are tested", true, null, "REF-0421", 8 },
                    { new Guid("5a9bd856-bb54-4e89-b84e-66221357e371"), 21, new Guid("00652d95-e040-4a72-956c-c051678686da"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the MEP clearance prior to start epoxy flooring works.", true, 1, "REF-0316", 5 },
                    { new Guid("5b0ac7c9-1287-4207-b4d8-b7fe502e5bdb"), 21, new Guid("00652d95-e040-4a72-956c-c051678686da"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", true, 1, "REF-0314", 3 },
                    { new Guid("5b212cab-2c31-4594-9787-4221f854a705"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipes are supported properly vertically&horizontally", true, null, "REF-0088", 13 },
                    { new Guid("5b5068b2-6188-4bf2-84b8-219bb598d32e"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check adequate protection is available for all fixtures during construction period", true, null, "REF-0411", 8 },
                    { new Guid("5bde543b-a8f4-4c38-816d-b3fe7fd1dcc3"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check acoustic lining is properly fastened and un damaged.", true, null, "REF-0189", 7 },
                    { new Guid("5be605c2-c610-4956-abc7-7fb403011004"), 31, new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Cleanout (FCO)", true, null, "REF-0498", 1 },
                    { new Guid("5bfc2087-8b31-4675-8821-e72819978adf"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper mixing of the material as per the manufacturer recommendations", true, 2, "REF-0287", 1 },
                    { new Guid("5c3029f1-a21c-4752-b883-bd6b627c37f9"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", true, null, "REF-0363", 1 },
                    { new Guid("5c829863-31c8-4f21-a39f-b31a139886fc"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Exhaust Air Duct", true, null, "REF-0625", 4 },
                    { new Guid("5c90ef3b-9568-43bb-888b-2b6f0c3a6d61"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0642", 3 },
                    { new Guid("5c9734db-cc01-423c-b813-f70c874d1f93"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cable ID", true, null, "REF-0326", 1 },
                    { new Guid("5cb6906f-3fdf-4626-a667-9ff053be78b9"), 11, new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for repair of surface imperfection and protrusions (if any).", true, null, "REF-0172", 2 },
                    { new Guid("5cbde6d5-b477-48fc-864b-2754498a3a72"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Architraves are fixed as per Drawing around Main Entrance Door / Bedroom Door", true, null, "REF-0591", 5 },
                    { new Guid("5d061408-580a-4678-b261-4bf8c6e27464"), 31, new Guid("60912bc7-31e2-435f-920c-15ca1465ffa5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check identification tag of the modular", true, null, "REF-0472", 1 },
                    { new Guid("5da2b26a-6b12-47bb-9520-e918dc397478"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the insulation is properly done.", true, null, "REF-0454", 13 },
                    { new Guid("5dc23780-10f1-4fdd-866f-4e56d254e050"), 14, new Guid("265c08b1-3950-4f4c-bc6e-b3f170140138"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the marking and setting out of the partition walls as per the approved drawings.", true, 1, "REF-0211", 1 },
                    { new Guid("5dc52daf-bffa-417f-a1e8-6715f815fdab"), 25, new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify and record the DCL product confirmity certificate for the sealants to be used.", true, null, "REF-0378", 4 },
                    { new Guid("5e055348-85e7-438a-bf16-58ef08ad0299"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Supply Duct", true, null, "REF-0622", 1 },
                    { new Guid("5e1c762f-bd20-42f2-b2ee-dcbfcbe5e421"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smoke detector", true, null, "REF-0535", 5 },
                    { new Guid("5ee62650-4a35-4195-9bf0-07d53c074fa5"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sensors", true, 6, "REF-0685", 7 },
                    { new Guid("5f165a36-6b93-4988-ab62-3bf9dd4c2814"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation as per approved method statement", true, null, "REF-0082", 7 },
                    { new Guid("5fdb2ac0-cb75-45b4-989a-39e00097752f"), 21, new Guid("00652d95-e040-4a72-956c-c051678686da"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Undulations or irregular corners are repaired and grinded as required.", true, 1, "REF-0315", 4 },
                    { new Guid("606a1ab0-3f50-477f-928e-2c004dbf6099"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel installation and termination", true, null, "REF-0540", 10 },
                    { new Guid("60759b80-6ff6-4f13-9dc8-5a4cefc624e8"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Iron-Mongery and Accessories", true, null, "REF-0582", 3 },
                    { new Guid("62d88f73-f5c8-4cb3-893d-f561d836cc5e"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Is the cable properly dressed and labelled.", true, 1, "REF-0239", 5 },
                    { new Guid("6343fbd1-4500-4478-a986-430077720e5f"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Override Switch Electrical Room", true, null, "REF-0518", 6 },
                    { new Guid("640de90f-e5f6-4b2b-ad3c-83198e6454dc"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waste Pipe", true, null, "REF-0652", 2 },
                    { new Guid("64125285-8631-4209-b991-7f032da5a3f6"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the proper mounting & fitting of lugs & accessories", true, null, "REF-0231", 7 },
                    { new Guid("64d8d399-55a5-46cc-8dbc-27495181bcdc"), 31, new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, null, "REF-0483", 1 },
                    { new Guid("64feeab6-9865-4c8a-ad1b-63388dbc0aad"), 31, new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation of pipe", true, null, "REF-0484", 2 },
                    { new Guid("655f1500-1b80-4c9f-94d5-a54ef12f11ff"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the tile joints are clean and free from contaminants like dust etc.", true, 4, "REF-0107", 1 },
                    { new Guid("658f2282-bbfa-4273-b550-478b78965c2f"), 4, new Guid("3e33c9db-7934-4a42-84a4-b31909855523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, 1, "REF-0063", 1 },
                    { new Guid("65f1df0f-b6bb-4c35-9c66-d3c52cef90ef"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure drawings used for installation are current and approved.", true, null, "REF-0130", 2 },
                    { new Guid("66c1c064-46ba-43ff-8068-3d98c5981208"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lock/Hardware of Main Entrance Door / Bedroom Door is installed", true, null, "REF-0590", 4 },
                    { new Guid("66cdd933-7c82-466d-a3c0-43c1651064ec"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimensions (outer, inner and diagonal), Line and Level Grouting", true, 2, "REF-0020", 6 },
                    { new Guid("66ec5514-f5bd-4346-b277-3dc4efa61453"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the name plate and identification labels as per load schedule and approved submittals.", true, null, "REF-0301", 3 },
                    { new Guid("67163657-2135-4444-927f-22b00b71c895"), 23, new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check location,Size,Color and Thickness", true, 2, "REF-0350", 1 },
                    { new Guid("6745c653-2210-4851-9bb0-9795e0f1c068"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Opening for MEP services are cut properly.", true, null, "REF-0572", 3 },
                    { new Guid("681d9573-580c-47ad-a080-6b4e544c8ec4"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal and External Dimension of Box", true, 3, "REF-0026", 12 },
                    { new Guid("681fd4d5-16b2-4f26-81d6-d26664366a90"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for system is checked for Insulation resistance and continuity", true, null, "REF-0233", 9 },
                    { new Guid("6845689f-4ba7-4c30-9793-80103f0f2723"), 25, new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the fire rating of the doors are as per the project requirements.", true, null, "REF-0379", 5 },
                    { new Guid("68afb347-f6f4-4129-be0b-2cc62dfc82f2"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Material used as per the approved MAR.", true, null, "REF-0414", 1 },
                    { new Guid("6913ca24-239d-497c-825e-ae75f5593dab"), 33, new Guid("271226b4-5ba2-47f6-9805-9d5764857ed2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify with complete test Services for any defects or damages", true, null, "REF-0621", 2 },
                    { new Guid("69491148-22ea-492c-ac6f-adddc0b410b2"), 18, new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activites.", true, 2, "REF-0278", 5 },
                    { new Guid("69c8ef21-5d6a-4053-8664-a4bbf67f2002"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get Value Installation", true, 1, "REF-0644", 5 },
                    { new Guid("6aa98a35-04a4-4425-9366-d84659503f5f"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Ironmongery is installed as per drawing and free from damanges", true, 1, "REF-0346", 8 },
                    { new Guid("6ac29bce-c031-4b67-9d5d-bfc683b5fc62"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Double Socket outlet with Neon Indicator", true, null, "REF-0523", 11 },
                    { new Guid("6bbba1c9-0e88-446b-bba6-80b1bb0f25d9"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipe alignment.", true, null, "REF-0135", 7 },
                    { new Guid("6beccd5d-5bdb-424a-865a-bb2e7a27155b"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Painted walls are clean and free from stains.", true, null, "REF-0613", 11 },
                    { new Guid("6bf6376a-0349-4693-a705-eb5d0dcfc782"), 4, new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of Texturer Coat as per manufacturers recommendation", true, 6, "REF-0074", 3 },
                    { new Guid("6c02392a-e8e8-4164-b019-f31ba44a8166"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipes are supported well with approved clamps.", true, null, "REF-0453", 12 },
                    { new Guid("6c91891f-f7c4-474f-9e6d-21983a0df8c5"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ONU Panel installation and termination", true, null, "REF-0687", 9 },
                    { new Guid("6d149393-9070-4f01-857b-70c9c0c51cb8"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", true, 1, "REF-0271", 6 },
                    { new Guid("6d1fbc32-d882-4b52-9f6f-fdd69e60403e"), 31, new Guid("c6f3d346-bb91-4635-81fa-9da7415fab4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fire Damper and Back Draft Damper", true, null, "REF-0481", 2 },
                    { new Guid("6d78ce26-de03-4b4e-9198-dedd505a35c4"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All internal cracks are repaired (if any)", true, 3, "REF-0025", 11 },
                    { new Guid("6dec26f6-15c6-4330-9363-5fc625b26e1f"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.", true, null, "REF-0185", 3 },
                    { new Guid("6e25f508-ddcf-4ac4-945d-a8ad89f2b805"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", true, 2, "REF-0322", 6 },
                    { new Guid("6e6da650-d12c-4fdf-98ff-6cc2a3e99e8d"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper mixing of material as per the manufacturer recommendations.", true, 4, "REF-0108", 2 },
                    { new Guid("6ea6173e-a66d-4894-af82-e296d53d4f8d"), 11, new Guid("13faeb36-32d0-4af9-a4b5-e42343cc30cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement and materials are approved.", true, null, "REF-0168", 1 },
                    { new Guid("6f502097-445b-4934-ad76-0e45f16a7d76"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check location / size of valves are provided as per approved shop drawing and specification", true, null, "REF-0120", 8 },
                    { new Guid("6faf8916-81be-4553-9614-356e0252ef61"), 4, new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored as per manufacturers recommendations.", true, 1, "REF-0060", 2 },
                    { new Guid("70ca5dbd-cbc3-4eda-a616-1b9cf588779e"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the curing at every stages of application as per manufacturer recommendation.", true, 2, "REF-0325", 9 },
                    { new Guid("7212ab28-1323-49b7-a37c-6cd4db4e8102"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the filters are installed, clean and accessible for maintenanace.", true, null, "REF-0034", 7 },
                    { new Guid("721e6fd9-3dc4-4b31-90cc-079d32f87b58"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D2-Ceiling Mounted Light Kitchen area", true, null, "REF-0544", 2 },
                    { new Guid("729fdd41-54d6-476d-b771-b6fe144a77b1"), 33, new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Pipe Insulation and adhesive", true, null, "REF-0633", 3 },
                    { new Guid("73519f0f-51f3-4dad-8496-0e90f6b453af"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for any water seepage / leakage after 24 hours or as per the project requirements.", true, 2, "REF-0297", 11 },
                    { new Guid("73ba10b2-be36-4ef0-85fa-eda0f41df3c6"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Cable pulled", true, null, "REF-0534", 4 },
                    { new Guid("73d5aa6c-30a2-473d-8c77-c8390104bb40"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Power Distribution Unit is available and power supply is provided", true, null, "REF-0242", 8 },
                    { new Guid("7403d76b-8de5-4f92-9a10-631890079a3c"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper mixing of material as per the manufacturer recommendations.", true, 2, "REF-0098", 1 },
                    { new Guid("74a948fc-e34b-4a22-b136-d19fbe6af766"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inspect all the accessories used are new and undamaged.", true, null, "REF-0131", 3 },
                    { new Guid("7541e031-0162-4a3f-affe-b15bc6bcd374"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the completion of all embedded MEP and other dicipline works prior to closure as per the approved drawings.", true, 2, "REF-0215", 4 },
                    { new Guid("75f725b4-31cc-43b9-ba04-5f51379dac70"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piping System", true, null, "REF-0464", 8 },
                    { new Guid("76126ec8-2be1-4c8f-8e3f-c1ab289a713a"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal.", true, 2, "REF-0099", 2 },
                    { new Guid("77072599-a0dd-4018-9922-6a3e80e7319e"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper mixing as per the manufacturer recommandations.", true, null, "REF-0175", 1 },
                    { new Guid("77751558-dc8c-4b53-a628-b370c7de2372"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Three Gang Override Switch Electrical Room", true, 4, "REF-0667", 7 },
                    { new Guid("77825f8e-98e1-4c39-89dd-d9d494e1ada4"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the uniform thickness, colour and application of the sealant all around the window frame.", true, null, "REF-0402", 7 },
                    { new Guid("7803530a-d508-4040-9697-e3275b740558"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the motors are accessible for the maintenance.", true, null, "REF-0035", 8 },
                    { new Guid("7867b29b-6808-4c78-ba13-8430c3a7c19b"), 24, new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials (wardrobe, leaf, drawers , iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, null, "REF-0356", 2 },
                    { new Guid("7894ae06-2b59-4b01-861f-b1d31a86c6a5"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipes are not passing above electrical services.", true, null, "REF-0155", 9 },
                    { new Guid("78e52a23-8de2-4f14-8644-61a0e4a838ec"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "L1-Led Strip Light Under Kitchen Cabinet", true, null, "REF-0547", 5 },
                    { new Guid("79b31440-1ebe-41c9-954f-669b1a66918f"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All cables and wires including grounding are terminated and identified.", true, null, "REF-0306", 8 },
                    { new Guid("79cb402a-73e1-412b-a217-fe74bfcf88e8"), 17, new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the dampness of the substrate prior to application (if required as per manufaturer recommendations)", true, null, "REF-0256", 3 },
                    { new Guid("79d97660-c6f6-4f43-a1e5-f19c87678d79"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 2G, 1 Way switch", true, 4, "REF-0662", 2 },
                    { new Guid("79e570b8-d376-42aa-8e61-bf512332c96d"), 21, new Guid("00652d95-e040-4a72-956c-c051678686da"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for repair of surface imperfection and protrusions (if any).", true, 1, "REF-0313", 2 },
                    { new Guid("79e9a298-d74b-463d-94fe-c997e6596cd8"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ironmongery is installed and free from damages", true, null, "REF-0436", 5 },
                    { new Guid("7a10f754-bd03-45b9-b3ad-849441baaa85"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Twin Data Outlet -Euro face plate Duplex keystone adaptor", true, 4, "REF-0669", 9 },
                    { new Guid("7b22e1c1-4e06-4bf8-88db-2a5f3a730755"), 31, new Guid("e140f9d3-d980-4801-ba40-1906302fbac0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Primer paint", true, null, "REF-0488", 1 },
                    { new Guid("7b26a149-0192-456f-ade5-27f3e27860e0"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Idetification on the data socket and IDF patch panel are same.", true, null, "REF-0248", 14 },
                    { new Guid("7b75c69b-fa43-4774-a272-65eb7b913369"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The conduits shall be securely fastened at intervals not exceeding 2m for 20 and 25mm rigid metal conduits and 3 meter for all larger sizes.", true, null, "REF-0057", 10 },
                    { new Guid("7b91df5f-6edd-4042-a106-43b290732bc5"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Single/ Double Socket outlet with USB port with Neon Indicator", true, 4, "REF-0672", 12 },
                    { new Guid("7bfda768-0861-456f-bb67-81e44547f005"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check if external surface of the box is leveled and free from damages", true, 2, "REF-0014", 9 },
                    { new Guid("7c6f6b88-4eae-4346-9a4d-e38a4ce588f4"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the location and No. of Door hinges provided as per the approved drawings.", true, null, "REF-0386", 4 },
                    { new Guid("7c829bf3-d0ec-40e2-b0d0-9f70dfbc1e9f"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check hot water pipes and fittings are insulated and damaged insulation is replaced & Masking Tape is provided on every joints of insulation.", true, 1, "REF-0128", 16 },
                    { new Guid("7ce2a769-44b4-43b7-b3e6-25ba7d0757b8"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.", true, 2, "REF-0218", 7 },
                    { new Guid("7d0fc5d9-658e-465b-84a4-62c8ded87ead"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "IR Ref. No.", true, null, "REF-0457", 1 },
                    { new Guid("7d405e5a-ddab-4d18-bfd4-5b458437ab3d"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the cable and other associated material are new and undamaged.", true, null, "REF-0199", 5 },
                    { new Guid("7d603add-7f92-4a9b-9935-f88951c077fa"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval to proceed with further works obtained from consultant / Client.", true, 3, "REF-0106", 9 },
                    { new Guid("7d750f4f-1a1e-4812-b530-da3aff8b85ec"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Gypsum surface are Crackfree at joints.", true, null, "REF-0578", 4 },
                    { new Guid("7e4c3f32-9931-4cca-9672-9edbd662a8d9"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erection & Connections of Floor Slab", true, 2, "REF-0016", 2 },
                    { new Guid("7e7fd5f4-ec7d-4656-904f-0e3f387658d5"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Description", true, null, "REF-0463", 7 },
                    { new Guid("7eafc3ba-549c-4a4c-a2e3-b7bc3d4e0940"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, 1, "REF-0092", 1 },
                    { new Guid("7eb02c63-ddd7-4b15-960c-14096b59a157"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the location and No. of leaf hinges provided as per the approved drawings.", true, null, "REF-0366", 4 },
                    { new Guid("7f11eb56-9c85-41c4-9c1d-8ccaf372f749"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bitumin Applied at required Areas", true, null, "REF-0560", 5 },
                    { new Guid("7fc7b988-0a7b-4542-b483-c0f80184db4f"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Location, Colour & Type of grout as per the approved shop drawings / material submittal.", true, 4, "REF-0109", 3 },
                    { new Guid("7febb68a-bbdc-4fb1-851b-fdddf0b71cff"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The materials are approved.", true, 1, "REF-0235", 1 },
                    { new Guid("80d81715-a4c7-49f6-a876-e46978bc1171"), 21, new Guid("aa5b8792-608a-46ae-8e88-8d662b42c97a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored as per manufacturers recommendations.", true, 1, "REF-0310", 2 },
                    { new Guid("81493c56-6ed3-41c1-9998-f828b91f7799"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMU", true, null, "REF-0690", 12 },
                    { new Guid("815dca03-8329-47e2-a1e2-6c7070eb29e7"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check location,Size,Color and Thickness of the Cabinets", true, 1, "REF-0339", 1 },
                    { new Guid("8180d017-c10c-4cd0-b99a-a7d838b019af"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installed pipes are free of sag & bend.", true, null, "REF-0045", 18 },
                    { new Guid("81a0b165-630a-4a8b-b437-4c4b7f621d2a"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the wardrobe consolidated.", true, null, "REF-0365", 3 },
                    { new Guid("81aa004d-85e3-4938-94ca-32aa23ce4814"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Single Data Outlet -Euro face plate single keystone adaptor", true, 4, "REF-0668", 8 },
                    { new Guid("8241fae6-cf3b-44be-bf82-347034fa8306"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Glass Partition installed and free from damage", true, null, "REF-0607", 5 },
                    { new Guid("82d17236-eba3-416d-bac6-e1fbc2cd527b"), 18, new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the jointing & taping as per the manufacturer recommendations.", true, 2, "REF-0277", 4 },
                    { new Guid("83432d01-c256-4305-815e-25689880c225"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the sealant are provided around the gaps of the fixtures", true, null, "REF-0412", 9 },
                    { new Guid("83f4a4ee-75b0-418a-9d03-ec77d0f0f6e9"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the door frames are consolidated using foam as per the material approval and project requirements.", true, null, "REF-0385", 3 },
                    { new Guid("84116a64-f6f8-4a5e-b801-d2cd5e10b753"), 4, new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of Stuccoo as per manufecturers recommendation", true, 3, "REF-0068", 2 },
                    { new Guid("8453ed09-0800-4724-a075-2108477ea92a"), 28, new Guid("0632a654-0969-4475-9705-bc941c44a5cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for repair of surface imperfection and protrusions (if any).", true, null, "REF-0429", 2 },
                    { new Guid("847a6d48-9ae3-4e44-a8d6-7962fabb9bf2"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cleaning of corners and edges removing exccessive paint on skirting", true, null, "REF-0567", 6 },
                    { new Guid("8492bd88-5615-4ae6-aed4-afc8454fb934"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the refrigerant piping are connected with appropriate fittings.", true, null, "REF-0448", 7 },
                    { new Guid("853c9e38-26b0-4693-a767-f26e4eeec52e"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipe painting as per approved method statement.", true, null, "REF-0137", 9 },
                    { new Guid("8585aba3-f549-40ae-a3ad-27bc0b7f379b"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the location, spacing and fixation of the grid (main channel, Furing channel, wall angle, hanging wire with adjustable clip, main tee, cross tee, etc.,) as per the approved drawings.", true, 1, "REF-0269", 4 },
                    { new Guid("85a1755e-c267-4414-8212-7388f41c23a5"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation as per approved method statement.", true, null, "REF-0152", 6 },
                    { new Guid("85ce2c11-1394-4f61-a04e-2af869da95f0"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", true, 2, "REF-0220", 9 },
                    { new Guid("864d236d-ce8e-4e01-af52-aa3b765d48d4"), 6, new Guid("f4150c0f-aa65-4c9e-9c06-6f01b65f9d0f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the expiry date of the material prior to applications.", true, 1, "REF-0091", 3 },
                    { new Guid("867faaf8-e5df-4458-9fdc-5f4b598ebc82"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the application of wet area water proofing and leak test.", true, 1, "REF-0095", 4 },
                    { new Guid("86a61a68-6503-436a-92b8-0da353de43dd"), 31, new Guid("51a829cc-1c7e-4834-a367-c52de630e82e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Alignment of Wiring Devices", true, null, "REF-0510", 1 },
                    { new Guid("8768930e-a8b5-42aa-8606-4859dd18da32"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the curing of application as per manufacturer recommendation.", true, null, "REF-0181", 7 },
                    { new Guid("88b5db1d-f0fc-4b91-888b-6ded612088cb"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves are provided for the pipes passing through the walls/slabs.", true, null, "REF-0042", 15 },
                    { new Guid("88cc1dbf-d471-4216-ae6f-a9ed341400d0"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check identification tag of the modular", true, null, "REF-0552", 2 },
                    { new Guid("88e1c6b7-ec67-4579-af62-a3f27e2dce6d"), 4, new Guid("3e33c9db-7934-4a42-84a4-b31909855523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", true, 1, "REF-0065", 3 },
                    { new Guid("898b9c95-004f-4ab5-912c-436c41b9af4b"), 31, new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Pipe Insulation and adhesive.", true, null, "REF-0485", 3 },
                    { new Guid("8bdb8393-7c9c-4a06-9911-7b5d55734d4b"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout, location and position of dry wall is as per App Drawing", true, null, "REF-0570", 1 },
                    { new Guid("8bee12ea-99c0-4b2b-85fe-72d3f2960c86"), 33, new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves provided for drain pipes outlets", true, null, "REF-0650", 5 },
                    { new Guid("8c456f71-b17e-43b4-8862-12e23ba1ad1b"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipes are supported well with approved clamps.", true, null, "REF-0154", 8 },
                    { new Guid("8c7d9f38-81bf-49ea-92cb-95a1b3576d3a"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Opening Size and Direction of cabinets", true, 1, "REF-0341", 3 },
                    { new Guid("8d0a4c90-4155-4bf6-8189-977a6126371b"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D4-1 Spot Ceiling Mounted light Toilet", true, null, "REF-0546", 4 },
                    { new Guid("8d1791a0-cae1-4677-8687-b9dfc3414fbe"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gypsum curtain pelmet installed and free from damages/cracks", true, null, "REF-0603", 1 },
                    { new Guid("8d757c01-965e-4e07-80d1-bafbb637683b"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the material used are as per the Approved material submittal.", true, null, "REF-0048", 1 },
                    { new Guid("8dd4e197-d197-405d-bbc0-0c9d9c6cfe66"), 31, new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Drain", true, null, "REF-0499", 2 },
                    { new Guid("8f3b376e-a9c9-412c-9176-410878f6c35e"), 11, new Guid("13faeb36-32d0-4af9-a4b5-e42343cc30cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, null, "REF-0169", 2 },
                    { new Guid("8f7c8795-ea1f-41fe-9689-458d5641939f"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire alram Cable pulled", true, null, "REF-0681", 3 },
                    { new Guid("8f89fb85-857b-4084-9249-9821181f507e"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check adequate space is available to operate the fixtures", true, null, "REF-0409", 6 },
                    { new Guid("8fad895d-32b6-4595-aa83-4d6c937efe8e"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check and verify location, orientation, and mounting heights of data and telephone oulets as per approved drawing", true, null, "REF-0240", 6 },
                    { new Guid("9013c678-1bc4-4f73-bc3f-5effa773178d"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thickness of Dry wall is as per App Drawing", true, null, "REF-0571", 2 },
                    { new Guid("904dafc2-7584-4fa0-b097-8a9aafc93c74"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0586", 7 },
                    { new Guid("90d9276f-8f05-4498-aeaa-0284856092f3"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen Backsplash installed, grouted and free from damages", true, null, "REF-0604", 2 },
                    { new Guid("92094210-da61-47e3-a50b-d9fd847ddf8d"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Heat detector", true, 2, "REF-0684", 6 },
                    { new Guid("922d888c-00f2-455d-92a0-427415001514"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Confirm the Devices are tested for operation and are to perform as intended at full load without any signs of heating", true, null, "REF-0422", 9 },
                    { new Guid("925099a5-d33d-4596-9879-d65775da638a"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 1G, 2 Way switch", true, null, "REF-0516", 4 },
                    { new Guid("9392fbd4-16f4-4c00-acec-f323e8a40e3d"), 4, new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the expiry date and number of coats of the material prior to applications.", true, 1, "REF-0061", 3 },
                    { new Guid("93e285c6-dd4c-4802-a930-265142a77481"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the indoor unit location/height as per approved shop darwing.", true, null, "REF-0030", 3 },
                    { new Guid("93edeff9-3c3f-4090-bf19-edce625c26c8"), 1, new Guid("11ab6eac-63bd-4ed0-9e65-35c35161a40c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Floor Setting Out/Layout as per drawing", true, 1, "REF-0005", 2 },
                    { new Guid("94132e61-1f89-4e2b-ae0c-579f18e07a54"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Firefighting Pipe", true, null, "REF-0656", 6 },
                    { new Guid("946db233-fc09-4c54-b459-ee4a7ecbface"), 17, new Guid("b1177a06-5499-43e2-974e-865c3bfa6735"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application covers the whole penetrations without any gap.", true, null, "REF-0258", 1 },
                    { new Guid("955140b4-00aa-4ab2-a1f1-5ab51fc3f08d"), 1, new Guid("825aaefe-e63e-4948-ae0b-4bf3203d5458"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and shop drawings are approved.", true, 1, "REF-0001", 1 },
                    { new Guid("9583ebb9-bca8-4cd1-abeb-0a04f0a47a2d"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that the identification & labeling are provided for duct works.", true, null, "REF-0194", 12 },
                    { new Guid("95d9bf14-1a5d-483a-a930-4baf527e5de0"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Paint touch ups are completed around installed items.", true, null, "REF-0559", 4 },
                    { new Guid("95e0441f-ab15-48c9-80d0-21672b96bcec"), 25, new Guid("7cb18d28-c2f9-418d-848b-fef4d1a0d2e7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the level and alignment of the door / window frame opening with reference to the surrounding wall / cladding elevations.", true, null, "REF-0381", 2 },
                    { new Guid("95f880a2-eeef-4357-b5b4-cec981c7ea5f"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test Result (Satisfactory/Not Satisfactory)", true, null, "REF-0470", 14 },
                    { new Guid("965995a7-1d68-4978-86c2-10cd2265f36d"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe layout/routing as per approved shop drawing.", true, null, "REF-0041", 14 },
                    { new Guid("96601fab-c234-4c88-a294-98ad2e23bca5"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Application of uniform base as per manufecturers recommendations.", true, 2, "REF-0319", 3 },
                    { new Guid("968021b6-350c-4d0b-85a9-848557a4bea4"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the cutting of gypsum board on the marked locations as per the project requirements.", true, 2, "REF-0222", 11 },
                    { new Guid("96a4199e-0dd8-4daa-9b67-5380143f0d74"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", true, null, "REF-0383", 1 },
                    { new Guid("97738c09-af82-4fbb-aa44-2d9d49a3e0e2"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that Pipe work to properly fixed and supported using a recognised and approved Manufacturer's support system.", true, null, "REF-0163", 3 },
                    { new Guid("979a3394-a7a2-4aaa-88bc-69a8f075671a"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "20 A, Cable/Flex outlet Washing Machine", true, null, "REF-0528", 16 },
                    { new Guid("97ebe931-848b-476d-b2ab-10492e54e4d3"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the laying of the cable & accessories configuration is as per approved drawing", true, null, "REF-0229", 5 },
                    { new Guid("98220e2e-b428-4fd3-a8a0-8cc428a7b7d7"), 33, new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Drain", true, null, "REF-0647", 2 },
                    { new Guid("989e32f4-a50c-401e-a787-250467407afc"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "W2- Wall mounted toilet light above Mirror", true, null, "REF-0550", 8 },
                    { new Guid("993aa6ba-0edc-472b-b4ab-1f25754c9bf9"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the fans are freely rotating.", true, null, "REF-0446", 5 },
                    { new Guid("9a4546c9-6a69-44d6-b2f7-c48ca514bf8b"), 28, new Guid("c8b6346b-088b-4e03-b64d-a5dbd311b713"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the expiry date of the material prior to applications.", true, null, "REF-0427", 3 },
                    { new Guid("9a91541a-d201-4b3e-9321-6f062185cc4b"), 31, new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves provided for drain pipes outlets.", true, null, "REF-0502", 5 },
                    { new Guid("9a97608c-d973-4df6-ae9b-b09301805f73"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that the gauges installed as per approved shop drawings.", true, null, "REF-0164", 4 },
                    { new Guid("9b096988-5ff1-4b44-a0a6-77b6cb2c14c9"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water pipes are not passing above electrical services and minimum clearance provided as per project Spec's.", true, null, "REF-0119", 7 },
                    { new Guid("9b306fde-5f0e-4647-a4e7-33a1adaa976b"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe layout/routing as per approved shop drawing", true, null, "REF-0080", 5 },
                    { new Guid("9b4ad754-7bb9-429b-ba6d-e3b05a88deb0"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water leak test performed and passed.", true, null, "REF-0584", 5 },
                    { new Guid("9cca1846-e662-467e-9ef6-cd81e1ffede8"), 14, new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify and record the DCL product confirmity certificate for the insulation materials.", true, 1, "REF-0210", 4 },
                    { new Guid("9e742987-363f-428e-9c6a-84917e4784ad"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure proper mixing of material as per the manufacturer recommendations.", true, 2, "REF-0321", 5 },
                    { new Guid("9ea0df82-d199-439a-8b7c-de0862cefe12"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen sink and accessories", true, null, "REF-0497", 6 },
                    { new Guid("9f4a2292-f98f-4a76-aee8-cdb91ede9624"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Material used damage free.", true, null, "REF-0415", 2 },
                    { new Guid("a040cd39-7f2a-4c4d-ae2d-9be050aa8fe7"), 25, new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials (Door frame, leaf, window frame, window leaf, iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, null, "REF-0376", 2 },
                    { new Guid("a058982e-398c-489e-9737-fc5294b1fa4b"), 33, new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "RCU Installation with Insulation box", true, null, "REF-0635", 5 },
                    { new Guid("a0655886-1f05-4450-b322-561f7ab54234"), 31, new Guid("51a829cc-1c7e-4834-a367-c52de630e82e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For DB Panel Door", true, null, "REF-0512", 3 },
                    { new Guid("a0c868d0-f7d4-4db3-8f69-170f488afa6c"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify that all conduit covers installed properly after the cable pulling activity.", true, null, "REF-0051", 4 },
                    { new Guid("a0e2f6fe-1c80-4aef-a4e4-f3558333c916"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the alignment, plumbness, opening direction and protection of window shutters to avoid any damages during construction activities.", true, null, "REF-0401", 6 },
                    { new Guid("a0e5b184-0a4a-47cc-a64c-03429beb5c5f"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the WFT as per the project specifications / manufacturer recommandations.", true, null, "REF-0180", 6 },
                    { new Guid("a1470add-7183-4718-9d22-b8c189d0bc0c"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of supports for the countertop is as per approved drawings", true, 1, "REF-0345", 7 },
                    { new Guid("a1e493b8-e11a-4d8b-9952-d645cec459cb"), 28, new Guid("0632a654-0969-4475-9705-bc941c44a5cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, null, "REF-0428", 1 },
                    { new Guid("a1ff291e-a81a-453a-aca3-81c13b8489e4"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Type of the cable as per approved drawings.", true, null, "REF-0246", 12 },
                    { new Guid("a2289bc1-e11b-45b2-98ac-85a8eb35c37b"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtain from Consultant/Client to proceed with further activities.", true, 2, "REF-0224", 13 },
                    { new Guid("a2828cd6-54b2-4cc7-8ad2-cef27e5d4301"), 23, new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Opening Size and location for kitchen Accessories", true, 2, "REF-0351", 2 },
                    { new Guid("a2bd1dd4-5df1-4b34-a623-bbe5f677d839"), 33, new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Hose Reel", true, null, "REF-0638", 3 },
                    { new Guid("a30e8f27-06c6-450b-8ce5-ac325a5f782e"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soil Pipe", true, null, "REF-0651", 1 },
                    { new Guid("a372b33c-04f9-4d86-8316-f9264775ccda"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimensions, alignment and level are as per drawing.", true, 2, "REF-0007", 2 },
                    { new Guid("a43c8dd4-f883-45a3-bdae-e8260442c89c"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the marking, supports and pipes installation done as per approved shop drawings.", true, null, "REF-0136", 8 },
                    { new Guid("a4ed943d-30a8-4bf5-9684-0265ac0664f4"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify panel Using machine printed labels", true, null, "REF-0419", 6 },
                    { new Guid("a51dfcd1-9229-4abb-965e-375e1915a526"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure all the fixtures are clean", true, null, "REF-0413", 10 },
                    { new Guid("a5df3e69-c336-4d9e-8d58-3b5362338cb4"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the completion of the required finishes above the false ceiling level.", true, 1, "REF-0266", 1 },
                    { new Guid("a6acdd96-32d0-41e3-a91d-7e72a467e24f"), 33, new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Red Paint/coating", true, null, "REF-0637", 2 },
                    { new Guid("a6c9d888-06bc-4a22-9682-ed2ac3ca76a0"), 33, new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Pipes and fittings", true, null, "REF-0631", 1 },
                    { new Guid("a7bdf2ed-c39f-47b4-ad62-72c5817bb162"), 25, new Guid("7cb18d28-c2f9-418d-848b-fef4d1a0d2e7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the door / window jamb area are solid and rigid.", true, null, "REF-0382", 3 },
                    { new Guid("a8f96a49-e70e-418d-8406-35bbbcc6b32c"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the approved type/capacity vibration isolator installed.", true, null, "REF-0445", 4 },
                    { new Guid("a8f9f6f7-3e31-42ce-82b7-18a269871238"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Line, Level and Spacer for the Installed Tiles", true, null, "REF-0563", 2 },
                    { new Guid("a92a8534-4147-4b3d-8caa-f41097260e6b"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Glass/panels", true, null, "REF-0581", 2 },
                    { new Guid("a954cea8-945c-4b93-90ba-f41d1a58576b"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the door stopper / door coordinator provided as per the approved door schedule.", true, null, "REF-0389", 7 },
                    { new Guid("a9926b25-f332-4514-be49-3d12c2545096"), 17, new Guid("b1177a06-5499-43e2-974e-865c3bfa6735"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finishing of the openings shall be as per the approved mockup.", true, null, "REF-0259", 2 },
                    { new Guid("a9f2380a-44c8-4961-a2c3-5bb31d8aedf4"), 33, new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of Primer paint", true, null, "REF-0636", 1 },
                    { new Guid("aaad3366-c391-4c25-b0b7-0d24591d9d93"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Height,Level and Alignment of cabinets", true, 1, "REF-0340", 2 },
                    { new Guid("aac8ff6d-ce32-472f-ba22-be8f5656f374"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that door lock open during the emergency", true, null, "REF-0424", 11 },
                    { new Guid("ab04eac7-0ab2-46bd-89a8-08cf2554790d"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thermostat", true, 5, "REF-0689", 11 },
                    { new Guid("ab057b3a-5b18-46d3-8af1-c608cbd564e0"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bottom slab shall read 1020mm at 1000m FFL line or as per drawing", true, 2, "REF-0017", 3 },
                    { new Guid("ab3f8145-4e56-429f-afc8-36fc8f591bef"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the locations of zone control valve as per the approved drawing.", true, null, "REF-0139", 11 },
                    { new Guid("ab517858-1181-498e-a42d-43ffc9cbbc0a"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Protect the edges of the cable after it has pulled.", true, null, "REF-0204", 10 },
                    { new Guid("abb745e8-b1a1-44e2-8932-6f42997333f8"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", true, null, "REF-0399", 4 },
                    { new Guid("abb96f1e-7476-4265-a9c6-9de5a65721f7"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location of Window/Sliding Door as per App Drawing", true, null, "REF-0580", 1 },
                    { new Guid("abe52242-c1c6-4efe-9771-a9196274b423"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the installation of cables as per approved drawings.", true, null, "REF-0206", 12 },
                    { new Guid("ac9bc775-7085-45c4-acb5-9042bf159804"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0569", 8 },
                    { new Guid("ad45bf90-1109-4cc3-ab85-2619da1d9101"), 18, new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure additional supports are provided for the ceiling mounted fixtures as applicable.", true, 1, "REF-0272", 7 },
                    { new Guid("ad656132-ed15-45bf-b23e-8afd7c92040e"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check conduit type and size as per approved shop drawings.", true, null, "REF-0050", 3 },
                    { new Guid("ad9b045c-4ed0-480b-b3f9-038995566517"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the cables/Wires & accessories pulled/dressed and adequate spaced within as per approved drawings.", true, null, "REF-0228", 4 },
                    { new Guid("ada0df0c-3ded-4e14-b789-cc134259d9ae"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Gypsum board are free from pealing off and Crack Free", true, null, "REF-0612", 10 },
                    { new Guid("adbd1799-6f64-44e0-bb55-142bf4d25e98"), 33, new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation CDP Pipes", true, null, "REF-0648", 3 },
                    { new Guid("ae1923dd-96a1-44ee-aa59-5c82979a5c3c"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the alignment, plumbness and protection of wardrobe to avoid any damages during activities.", true, null, "REF-0372", 10 },
                    { new Guid("aea84fbe-06c5-428c-9967-6212a909d10d"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the installation of insulation works (if applicable) as per the approved drawings.", true, 2, "REF-0217", 6 },
                    { new Guid("aea89b5f-cc96-47b0-bca7-e1cff4fe18f6"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wardrobe doors and drawers funtioning smoothly and free from scratches", true, null, "REF-0601", 15 },
                    { new Guid("af7600e4-0567-4e12-8d01-6cba76a3059c"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Balcony floor drain installed as per approved drawing", true, null, "REF-0615", 13 },
                    { new Guid("af868546-4f3b-47ae-94e3-67e5d7f60cef"), 33, new Guid("2c0bf34b-4020-4d34-9a79-379ca9c8c5cf"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For ONU Panel Door", true, null, "REF-0659", 2 },
                    { new Guid("af94ae15-6ae0-49d7-a2d3-d277a3a48bff"), 33, new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Floor Cleanout (FCO)", true, null, "REF-0646", 1 },
                    { new Guid("b0325b54-d58e-476d-a2ea-25f5cd79e5b6"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of boxes, pull and Junction boxes, Outlet boxes shall be accommodate orientation of wiring devices as indicated on drawings.", true, null, "REF-0055", 8 },
                    { new Guid("b05e5faf-afce-468d-9f8d-2fcebaf0f6fe"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the slope and level of the tiles as per the approved drawings.", true, 3, "REF-0103", 6 },
                    { new Guid("b0763c83-2bc1-42d1-aa70-adc0f6998653"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the distance between the support as per specification.", true, null, "REF-0159", 13 },
                    { new Guid("b14ea2fe-5ca2-4d69-81d2-5a871fb4d18e"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.", true, 1, "REF-0213", 2 },
                    { new Guid("b1813260-2885-4243-8122-a882a1baff00"), 32, new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sign the delivery note for accepting the loading of precast modular in good condition", true, null, "REF-0619", 2 },
                    { new Guid("b2d6bb03-7edc-4965-92c2-39ad84ee6da7"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check insulation resistance test prior to start the cable pulling.", true, null, "REF-0200", 6 },
                    { new Guid("b346074a-ee4a-48e5-a007-ffb1fb299c08"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the completion of the required finishes of the adjacent substrates.", true, 1, "REF-0212", 1 },
                    { new Guid("b35b0411-9005-46ba-9615-d7a90a727686"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Single/ Double Socket outlet with USB port with Neon Indicator", true, null, "REF-0524", 12 },
                    { new Guid("b3848625-18e2-4025-acf4-f3d9ac0d47ca"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the alignment, plumbness and protection of door leafs to avoid any damages during construction activities.", true, null, "REF-0393", 11 },
                    { new Guid("b3c59182-7948-4c6c-a6c4-955089429d72"), 17, new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure H&S method statement is submitted", true, null, "REF-0251", 2 },
                    { new Guid("b43498f7-280e-4dd0-9dd4-e4799aad482f"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 1G, 1 Way switch", true, null, "REF-0513", 1 },
                    { new Guid("b435bcd5-93ab-48e8-8d70-611c966df1a5"), 33, new Guid("2c0bf34b-4020-4d34-9a79-379ca9c8c5cf"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Alignment of Wiring Devices", true, null, "REF-0658", 1 },
                    { new Guid("b44896bc-fd7d-4a0b-a47b-280a50c7327f"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the laying of tiles above the false ceiling as per the approved drawings.", true, 3, "REF-0104", 7 },
                    { new Guid("b45d19b1-8bcd-4f25-91d4-4728530503c1"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the insulation is properly done", true, null, "REF-0044", 17 },
                    { new Guid("b4d75187-8828-4249-a467-7513a65d1b11"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 1G, 2 Way switch", true, 4, "REF-0664", 4 },
                    { new Guid("b544e716-5038-4a01-ac5f-ba30cecb452f"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hot water pipes insulated.", true, null, "REF-0495", 4 },
                    { new Guid("b5a597a9-1cbf-49d8-af83-0cf43698e8e8"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erection of partition walls by Temporary Support", true, 2, "REF-0018", 4 },
                    { new Guid("b6197f0d-4eca-403e-864e-0744f12f143f"), 31, new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0494", 3 },
                    { new Guid("b6a68385-55a9-40c2-a257-0f8204db26a9"), 23, new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check for any Surface joints visibility", true, 2, "REF-0354", 5 },
                    { new Guid("b6e205a2-9e64-4a1d-b5ee-4e2e582af750"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Door Bell -230V Electromechanical chime", true, null, "REF-0529", 17 },
                    { new Guid("b6fc4834-4a37-4231-8377-dc0308d6181f"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grouting of all Joints is done properly", true, null, "REF-0565", 4 },
                    { new Guid("b8466988-21ee-4712-83b5-56b1d1403f27"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fresh Air Duct", true, null, "REF-0476", 3 },
                    { new Guid("b858ff5d-deec-4b46-a754-a26378a5885c"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Entrance Door / Bedroom Door as per App Drawing", true, null, "REF-0589", 3 },
                    { new Guid("bb900f09-a78e-4079-b98c-b7ded3c8e9f2"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Hammer arrestor installed in upright position", true, null, "REF-0124", 12 },
                    { new Guid("bbd1169f-a962-48a4-a166-e9d7a99c49cd"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elastomeric sealant under skirting is provided properly", true, null, "REF-0566", 5 },
                    { new Guid("bbe2d49a-8f18-4d83-ba50-2255bfc2f332"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The materials are approved.", true, null, "REF-0147", 1 },
                    { new Guid("bc89becf-fabc-4491-811b-7717fe102680"), 4, new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and drawings are approved.", true, 1, "REF-0059", 1 },
                    { new Guid("bce9a766-6a7a-4a33-bf9a-cdf890b7fa1a"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the outdoor unit space around as per manufacturer recommendations.", true, null, "REF-0449", 8 },
                    { new Guid("bcf06691-5334-471f-b27b-e9f5a6e182b8"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure(bar)", true, null, "REF-0469", 13 },
                    { new Guid("bdd58212-7137-4ba2-a9cf-9af4d2093f8e"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verity the fixation, level and alignment of the wardrobe is as per the approved details / drawings.", true, null, "REF-0364", 2 },
                    { new Guid("be8e4474-f481-48b1-9104-9060f18e5a38"), 33, new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piping Leak test", true, null, "REF-0649", 4 },
                    { new Guid("be8fd14b-581d-430a-9978-dc9b6c367af2"), 24, new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the fire rating of the wardrobe are as per the project requirements.", true, null, "REF-0359", 5 },
                    { new Guid("be931ba4-5b37-4296-96bd-680e77d389ab"), 31, new Guid("b9b52c68-a027-4de2-82db-ccbe3fb429a2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0491", 2 },
                    { new Guid("bf403728-0d5d-4ab4-9d90-16c26aa88bcc"), 4, new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of final coat of Paint as per manufacturers recommendation.", true, 7, "REF-0075", 4 },
                    { new Guid("bf6b0e94-1c82-4ef4-b6da-52570d087801"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen cabinets accessories installed as per app drawing", true, null, "REF-0597", 11 },
                    { new Guid("bfaebe11-4b44-4046-8353-05b4aa658c74"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All joints on paint-able surfaces filled with sealant properly", true, 2, "REF-0013", 8 },
                    { new Guid("bfc6bd01-28ee-47be-beac-2647960b8270"), 4, new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, 6, "REF-0073", 2 },
                    { new Guid("bfedd819-31bc-434f-b0bd-fb9e07f36753"), 24, new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, material submittal and drawings are approved.", true, null, "REF-0355", 1 },
                    { new Guid("c025e02c-bf77-46c3-a1fb-cb54b019185e"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check and ensure that the cable sizes are as approved drawing.", true, null, "REF-0202", 8 },
                    { new Guid("c0bfe921-cb9a-4dce-b27e-51701233d06f"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe Joints are properly made tight & secure, Check the vertical riser of the pipes are supported well with approved clamp.", true, null, "REF-0118", 6 },
                    { new Guid("c0f1e219-e407-4460-8634-ba833192ac99"), 31, new Guid("c6f3d346-bb91-4635-81fa-9da7415fab4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fan Coil Unit", true, null, "REF-0480", 1 },
                    { new Guid("c13aa488-69e4-4ec4-bc55-1de3ab7577c4"), 31, new Guid("51a829cc-1c7e-4834-a367-c52de630e82e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For ONU Panel Door", true, null, "REF-0511", 2 },
                    { new Guid("c14f2f9c-5efe-4a4e-82a5-c0e29af69edd"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0574", 5 },
                    { new Guid("c1c555b7-7771-45db-a267-ce7b6fddf3e5"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the cables/Wires & accessories and materials are new & undamaged.", true, null, "REF-0227", 3 },
                    { new Guid("c1ff045d-55cd-44b1-beb9-26b3e0fa566b"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Mirror installed and free from damage", true, null, "REF-0606", 4 },
                    { new Guid("c2008d59-9c5f-430b-88ec-ac1032eb4d90"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Locknuts shall be provided to protect the wire from abrasion unles in the design.", true, null, "REF-0053", 6 },
                    { new Guid("c2389679-0f87-407b-89c6-a5709223b2fe"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fixing of Silicone Sealant", true, null, "REF-0583", 4 },
                    { new Guid("c25a5fba-d27f-456d-beec-7914265ff91c"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the alignment of installed panels.", true, null, "REF-0305", 7 },
                    { new Guid("c29d5dd2-665a-439a-820c-eb4a01e0baf4"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure required Iron mongery sets are provided as per drawings.", true, null, "REF-0367", 5 },
                    { new Guid("c2c81d13-61ec-4559-be99-3f4dbfdae712"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tuchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, 2, "REF-0320", 4 },
                    { new Guid("c2ee20b6-2096-452e-9f42-97affdbe75a4"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location of the data socket as per approved drawings.", true, null, "REF-0247", 13 },
                    { new Guid("c3053643-fbe2-4ff4-96b5-9098697431de"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", true, null, "REF-0388", 6 },
                    { new Guid("c34ada9e-6a20-4e32-b0c6-30b749d6f669"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varify Size of opening as per drawing", true, null, "REF-0434", 3 },
                    { new Guid("c41bcf66-c933-4a58-b2cf-93e73c5d5d0d"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure wardrobe hanging rail set is installed.", true, null, "REF-0373", 11 },
                    { new Guid("c4ed391c-9b92-43d0-8077-9deb2c561cca"), 31, new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Supply Duct", true, null, "REF-0474", 1 },
                    { new Guid("c4f8a775-496e-4f4b-b14c-e848043ff388"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that a proper tags/identifications are provided", true, null, "REF-0232", 8 },
                    { new Guid("c5264e1d-04e7-4ecf-ba81-cb01b30d7b7c"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hot water pipes Insulated", true, 1, "REF-0643", 4 },
                    { new Guid("c5af12e5-445a-4598-8a43-9c7e322444e6"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Termination tools are properly managed.", true, null, "REF-0244", 10 },
                    { new Guid("c5dd4075-5da1-458f-ac95-18112b43f1b0"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lock/Hardware of Pod door is installed", true, null, "REF-0593", 7 },
                    { new Guid("c76908fd-b7f1-4ace-b87c-ea30bafda313"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod installed as per drawing - Level and alignment without any damages", true, 3, "REF-0021", 7 },
                    { new Guid("c7764709-9dd4-44b1-8555-dd0ba57d7d5f"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13 A ,Switch Single Socket outlet with Neon Indicator", true, 4, "REF-0670", 10 },
                    { new Guid("c795abab-561e-48f4-8c35-4f199e8bd06a"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the chilled water/refrigerant piping are connectred with appropriate fittings", true, null, "REF-0037", 10 },
                    { new Guid("c7a47373-9854-4971-b477-5d52b20598c2"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All Wires pulled as per the approved Drawings.", true, null, "REF-0531", 1 },
                    { new Guid("c84d19b4-30cc-4dd6-8f28-a967e176a5a7"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe layout/routing as per approved shop drawing.", true, null, "REF-0451", 10 },
                    { new Guid("c8e475f1-cd7a-4dbf-a902-49e6480baa06"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Confirm the Devices are tested and working in the event of power outage", true, null, "REF-0423", 10 },
                    { new Guid("c9b7cf99-40b5-4519-986b-86bbc0268dee"), 17, new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the availablity mockup approval for the fire sealing works (around penetrations, partitions, etc.).", true, null, "REF-0253", 4 },
                    { new Guid("ca7c0427-e7d5-4b81-b5ff-f8e2ba7d9116"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel installation and termination", true, null, "REF-0688", 10 },
                    { new Guid("ca7fccca-0923-4234-8320-47e8b748244c"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Stove installed Properly", true, 1, "REF-0348", 10 },
                    { new Guid("ca8f4146-3299-4b2c-9ca1-b690b19b4927"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Type, Size, Colour, Thickness and Opening Direction", true, null, "REF-0433", 2 },
                    { new Guid("cadcc053-801d-48bf-9527-cb6d52d98928"), 32, new Guid("48962238-ba77-431b-93cb-104d24939ec0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, ITP, materials and shop drawings are approved", true, null, "REF-0551", 1 },
                    { new Guid("caed0266-0388-41af-8a93-fdcc084b2907"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Horizontal pipes are supported well and with approved clamps.", true, null, "REF-0116", 4 },
                    { new Guid("cb353b87-d350-476a-bea5-706999583474"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the fixation of the board (on one side of the supports) as per the approved drawings.", true, 2, "REF-0214", 3 },
                    { new Guid("cb4a6ae1-4a8b-47a3-9fde-220e926ed20f"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "CAT-6 Cable pulled", true, null, "REF-0532", 2 },
                    { new Guid("cb79f23d-81a7-49f4-b810-e730814446ff"), 17, new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check all the materials are approved by consultant.", true, null, "REF-0252", 3 },
                    { new Guid("cb88439c-f3d1-4999-ad00-08c69aaeb797"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "No visible damage on the materials.", true, 1, "REF-0236", 2 },
                    { new Guid("cb8baaf8-1d8e-4230-b4df-0c3ac94fd0d4"), 32, new Guid("489adba2-e657-4f02-a5f3-dbca1488077a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure Gypsum surface are Crackfree at joints.", true, null, "REF-0573", 4 },
                    { new Guid("cc262511-61fd-4bac-a22a-b990997e7ae0"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Joints of the Cabinets", true, 1, "REF-0343", 5 },
                    { new Guid("cc8cfe61-66bf-46e7-abcd-547be6c11d63"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "CAT-6 Cable pulled", true, null, "REF-0680", 2 },
                    { new Guid("ccf78f91-1eda-4ad1-a8e2-9004105d77bd"), 33, new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Water Hammer Arrestor installed in the approved location.", true, 1, "REF-0641", 2 },
                    { new Guid("cd09e3bc-301a-4dbb-b90d-ba68cc56f551"), 32, new Guid("10f470e4-bfa7-4c2f-96a5-8bff23abbfa2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal and External Dimensions of the modular", true, null, "REF-0555", 1 },
                    { new Guid("cd23880b-d8d5-4906-8bbe-524b6fb2a026"), 17, new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure relevant shop drawing is approved.", true, null, "REF-0250", 1 },
                    { new Guid("cd6456a4-e734-4044-b31b-0ef06170232e"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the filters are installed, clean.", true, null, "REF-0447", 6 },
                    { new Guid("cd8c0c83-3458-401a-ac03-781f2a4e2873"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alignment of the pipes as per approved shop drawing.", true, null, "REF-0151", 5 },
                    { new Guid("cd97222f-0ae5-4822-84a9-398a52188da1"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erection & Connection of Roof Slab as per approved drawing and ensure box clear height", true, 3, "REF-0022", 8 },
                    { new Guid("cda90f80-a64a-4d43-833d-a9af574dc069"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Single Data Outlet -Euro face plate single keystone adaptor", true, null, "REF-0520", 8 },
                    { new Guid("cdcad9b8-cc0b-4d4d-b811-52b405235bd4"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drainage pipes are not passing above electrical services", true, null, "REF-0084", 9 },
                    { new Guid("ce231580-a82b-4bf5-a80f-60f8f43472c0"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cable indentification as per the approved drawings.", true, 1, "REF-0238", 4 },
                    { new Guid("ce34e1ed-00c5-4ee8-b88c-bfd5d72e65de"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure panel interiors are cleaned and free from dust and small metallic particles.", true, null, "REF-0307", 9 },
                    { new Guid("cf2b96fc-b547-4bfc-aea9-ac2a53e626b0"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the drip flashings are provided properly around the frame as per the approved drawings", true, null, "REF-0397", 2 },
                    { new Guid("cf6d1e8c-3730-4527-a151-d3c15eecfffe"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hydrotest of piping shall included the valves, valves shall be in open position at the tome of testing. Hydrotest pressure shall be equivalent to piping test pressure (i.e) 1.5 Times the operating pressure.", true, 1, "REF-0122", 10 },
                    { new Guid("cf8948ba-a70c-44e9-86a1-37d658113833"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flexible duct connectors are provided at building expansion joints.", true, null, "REF-0193", 11 },
                    { new Guid("cfefdbd3-fb55-40e2-8c69-436c7262b42e"), 18, new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the cutting of gypsum board / tiles on the marked locations as per the project requirements.", true, 2, "REF-0276", 3 },
                    { new Guid("d014b5eb-d37b-4c57-ae02-7e2253ccaf8d"), 11, new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the angle fillets and chamfering of all sharp edges (if required).", true, null, "REF-0173", 3 },
                    { new Guid("d0837def-c9cb-4ee2-8a0e-d1786b147fd0"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the completion of finishes around the jamb area prior to installation.", true, null, "REF-0396", 1 },
                    { new Guid("d09451df-687e-4a49-8f8e-afd2a2d86dbe"), 28, new Guid("0632a654-0969-4475-9705-bc941c44a5cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the protection of nearby finishes / MEP services.", true, null, "REF-0430", 3 },
                    { new Guid("d1260a09-491d-4341-9e69-14c1d5d9abcd"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Calibration Date", true, null, "REF-0332", 7 },
                    { new Guid("d162b339-6395-419a-a780-fab677a1a43b"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the drawings used for installation are current & approved.", true, null, "REF-0226", 2 },
                    { new Guid("d1bf6e74-6579-4410-a0e8-96670c387d09"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test Voltage", true, null, "REF-0334", 9 },
                    { new Guid("d200b52b-da7d-423f-af36-e8572d69a711"), 17, new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the MEP penetrations are installed, inspected and approved.", true, null, "REF-0254", 1 },
                    { new Guid("d23ff95e-ac50-4ca3-9d43-017bc279f545"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves are provided for the pipes passing through the walls/slabs.", true, null, "REF-0452", 11 },
                    { new Guid("d26383d5-2148-45ef-b7f3-9fff778842af"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Interior boxes shall be cleaned to remove dust, derbis and other material. Exposed surfaces shall also be cleaned and finish restored.", true, null, "REF-0056", 9 },
                    { new Guid("d2729cb6-566d-456f-b375-bd21557428e4"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vent Pipe", true, null, "REF-0653", 3 },
                    { new Guid("d2ceaffc-0907-4fcc-bb53-d91462d79a2f"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drainage pipe line connected to the sytem", true, null, "REF-0408", 5 },
                    { new Guid("d2e32821-15ef-4d1a-a62b-668ebb3bd724"), 31, new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "L2-Surface Mounted linear light electrical Room & Garbage room", true, null, "REF-0548", 6 },
                    { new Guid("d38ccb25-66d5-41f7-bfff-1ebaadb40941"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cable containment and accessories as per approved drawings.", true, 1, "REF-0237", 3 },
                    { new Guid("d3acc70b-d069-45bb-aa23-6b580d5798d3"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe joints are properly made and are tight/secure.", true, null, "REF-0158", 12 },
                    { new Guid("d3cbbe30-c25a-4bd9-aed3-14535b5ba324"), 19, new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure check dam constructed at the entrance of the room for the stagnation of water for water leakage testing after the completion of the wet area water proofing.", true, 1, "REF-0286", 5 },
                    { new Guid("d402f880-5fa5-4162-ac63-fbfaf810610f"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod door is installed as per App Drawing", true, null, "REF-0592", 6 },
                    { new Guid("d5f41566-352c-4247-b12f-59b664d1f0b2"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check surface is levelled and sloped as per the approved drawings.", true, 1, "REF-0093", 2 },
                    { new Guid("d6023c45-7825-4d5d-aa49-91bb08b09fc0"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toilet accessories installed and free from damage", true, null, "REF-0616", 14 },
                    { new Guid("d6e6598c-93d7-46b5-a1f5-6925b03ddff0"), 10, new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure that all pipes installation shall be as per manufacturer recommendation and project specs.", true, null, "REF-0161", 1 },
                    { new Guid("d7394cb4-78be-4d87-b56f-3b570983111b"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vent Pipe", true, null, "REF-0505", 3 },
                    { new Guid("d7dc5f54-dc65-4178-bb4e-7c6d68dd4887"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", true, null, "REF-0368", 6 },
                    { new Guid("d8683107-69be-46a3-be3c-0bf9c7c55359"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure additional supports are provided for the wall mounted fixtures as applicable.", true, 2, "REF-0216", 5 },
                    { new Guid("d8a00696-ebfb-4112-8688-1e3d3786927e"), 4, new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", true, 1, "REF-0062", 4 },
                    { new Guid("d8dc1b76-e2df-4b01-a431-26bdc7bdf5b9"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Continuity Test", true, null, "REF-0329", 4 },
                    { new Guid("d905cf97-5b30-46bf-8f54-12f2fca5a007"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check tidiness of system installations.", true, null, "REF-0234", 10 },
                    { new Guid("d90da177-4043-444b-8b38-53dc49e9fd37"), 23, new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check wethere Drawer and Shelf functioning properly", true, 1, "REF-0344", 6 },
                    { new Guid("d91b6f34-7c68-447a-94a5-03c083f46a4c"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0579", 5 },
                    { new Guid("d91e0777-034c-44e1-9053-01eb3e02040c"), 14, new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the jointing & taping as per the manufacturer recommendations.", true, 2, "REF-0223", 12 },
                    { new Guid("d9497849-3db6-4cd8-94b6-81dad6c3475b"), 21, new Guid("aa5b8792-608a-46ae-8e88-8d662b42c97a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, 1, "REF-0309", 1 },
                    { new Guid("d9547769-376d-4f26-93a1-8fce5bdbf80b"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Give a temporary tagging before cable pulling to track the circuits during termination", true, null, "REF-0203", 9 },
                    { new Guid("d9abd2a8-853e-4101-904e-c731c1193318"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Date", true, null, "REF-0460", 4 },
                    { new Guid("da066bca-1cbb-4546-b3de-ef557bd83220"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation shall only be applied to piping after all testing has been completed.", true, null, "REF-0125", 13 },
                    { new Guid("da71752b-8646-46f9-aa7d-a9c1aa536dea"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Heat detector", true, null, "REF-0536", 6 },
                    { new Guid("db275e55-9586-4f2d-84a5-7865d47f6be9"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "13A, DP, Simplex Switched Spur Outlet for FCUs with Neon Indicator", true, null, "REF-0530", 18 },
                    { new Guid("db840b56-f8a1-42bc-b984-df7e134ad5a5"), 1, new Guid("825aaefe-e63e-4948-ae0b-4bf3203d5458"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the expiry date of the material prior to applications.", true, 1, "REF-0003", 3 },
                    { new Guid("dc01ca3a-cb6a-4313-afed-13c5e4f6261f"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the fixtures are correctly levelled and bolted", true, null, "REF-0406", 3 },
                    { new Guid("dc126c65-f23e-4403-a9b3-ff1fd148b5cd"), 31, new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation CDP Pipes", true, null, "REF-0500", 3 },
                    { new Guid("dc63b3a7-0ece-4efb-80d0-0c1db4a272bd"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of subsequent coats are carried out at right angle to the previous coat.", true, null, "REF-0179", 5 },
                    { new Guid("dc8d1393-6c24-4750-a81a-a1649b33302f"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of coats as per project requirements / manufacturer recommendations.", true, 2, "REF-0323", 7 },
                    { new Guid("dc9487a4-4ac2-4a8f-90b0-40181bf660fb"), 11, new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, null, "REF-0171", 1 },
                    { new Guid("dcb733c0-b4d6-4911-9c3d-502cb00bc9f8"), 15, new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check cable & accessories, used for installation have approved submittals", true, null, "REF-0225", 1 },
                    { new Guid("dce86551-5429-4c69-9c7b-c2a19dc21106"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All surface to be insulated shall be dry and free from loose scale, dirt, oil or water when insulation is applied.", true, null, "REF-0127", 15 },
                    { new Guid("dd082a2c-d35e-45a4-b861-5b551519ddbb"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", true, null, "REF-0384", 2 },
                    { new Guid("dd47d609-400e-4cb0-89ca-b543861e7dc3"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fire Alarm Control panel -ELV room", true, 4, "REF-0674", 14 },
                    { new Guid("dd51c76d-cda3-4be1-82ad-2a563f19798d"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, If any", true, null, "REF-0561", 6 },
                    { new Guid("dd6cd258-a3f9-4ca2-aab0-d06caa4e7a42"), 6, new Guid("641bde3c-9eca-4752-abaa-6049807018de"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the width of tile grout is proper and uniform.", true, 4, "REF-0110", 4 },
                    { new Guid("dd99c604-bfa0-41e8-8730-c8bf1c3cc0de"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sealant is applied properly", true, null, "REF-0437", 6 },
                    { new Guid("ddb86f1c-d13b-44ae-aae0-4e0e508acbb8"), 16, new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the IDF rack is cleaned without any unwanted materials", true, null, "REF-0241", 7 },
                    { new Guid("ddb909e5-bf6a-4732-a472-035524973db6"), 32, new Guid("0b148e2b-a91d-44d5-8f40-4dab165f551f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layout and Fixing of Tiles as per App Drawing", true, null, "REF-0562", 1 },
                    { new Guid("ddf0e5c9-774d-4edb-a7d2-f53012694063"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure drawings are used for installation are current and approved.", true, null, "REF-0184", 2 },
                    { new Guid("de0683c3-e647-47a0-8f41-0d8b1c8cb7a0"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ONU Panel installation and termination", true, null, "REF-0539", 9 },
                    { new Guid("df162ee4-0c3b-4878-be2b-fb7d1f32a190"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe sizes are as per approved shop drawing", true, null, "REF-0079", 4 },
                    { new Guid("df99b660-0f74-4072-81d3-5ccdb8b6c941"), 32, new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Height of the False Ceiling as per App Drawing", true, null, "REF-0576", 2 },
                    { new Guid("e05bc064-2c78-4dce-bb83-caa05e894ca0"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check if panels are aligned to each other at joints.", true, 2, "REF-0012", 7 },
                    { new Guid("e1231319-bdfd-492e-be42-8d54eda27a64"), 17, new Guid("b1177a06-5499-43e2-974e-865c3bfa6735"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the final cleaning and sealant (if required) applied as per the approved mockup.", true, null, "REF-0260", 3 },
                    { new Guid("e14c918d-0431-4b7d-9ff6-80ff3a1b9bec"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D3-Spot Ceiling Mounted light Corridor", true, 3, "REF-0693", 3 },
                    { new Guid("e1686ab9-a5a8-4390-9ff6-14c1efbfb798"), 33, new Guid("2c0bf34b-4020-4d34-9a79-379ca9c8c5cf"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check For DB Panel Door", true, null, "REF-0660", 3 },
                    { new Guid("e2690a8c-bb7e-4af1-97af-4bdc2f0dc11b"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 1G, 1 Way switch", true, 4, "REF-0661", 1 },
                    { new Guid("e2ee1c2e-e2e2-419a-81cb-ff92b9c9195b"), 4, new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application of final coat of Paint as per manufacturers recommendation.", true, 4, "REF-0070", 4 },
                    { new Guid("e31bfb18-ab34-43db-a4cd-bc17739f0073"), 25, new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the location and No. of hinges, iron mongeries provided as per the approved drawings.", true, null, "REF-0400", 5 },
                    { new Guid("e323fd8f-a799-427c-a13f-228673d5d7c3"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profile and glass country of origin / Manufacturer", true, null, "REF-0432", 1 },
                    { new Guid("e34c466f-2932-4a76-bfd2-40f4a0aff536"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "water leak test is performed and no leakage is found", true, null, "REF-0438", 7 },
                    { new Guid("e3d45fa2-0eac-4953-bdcc-b3d24e39deb9"), 28, new Guid("0632a654-0969-4475-9705-bc941c44a5cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the MEP clearance prior to start of aluminium works.", true, null, "REF-0431", 4 },
                    { new Guid("e40c65ef-3658-493e-af56-031c223df3ad"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure required Iron mongery sets are provided as per the door schedule drawings.", true, null, "REF-0387", 5 },
                    { new Guid("e45b067d-582b-4e49-815e-3eca574fcee0"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel Tags and identification.", true, null, "REF-0538", 8 },
                    { new Guid("e4a73a28-4b50-4c18-9acc-1539ff6639ff"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the tile spacers width is uniform and aligned as per the appoved drawings.", true, 2, "REF-0101", 4 },
                    { new Guid("e4bee9e3-62d7-4096-bd95-3fb9881ac02b"), 20, new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the mounting channels are even and free from any damage", true, null, "REF-0302", 4 },
                    { new Guid("e5bc132a-d5f8-46cf-a462-7d149fe2c698"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Confirm the mounting heights and location of card readers, Touch Screen Access Terminals, push buttons", true, null, "REF-0416", 3 },
                    { new Guid("e5dcf94a-08f2-4f2f-9355-fbff7b252a36"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure the rubber gaskets are provided at the door jamb as per the approved drawings.", true, null, "REF-0391", 9 },
                    { new Guid("e5deec6c-d0b4-4baa-adb8-5993e3df37b3"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check fixing of supports and spacing as approved drawings & submittal.", true, null, "REF-0188", 6 },
                    { new Guid("e6a5a23c-9801-4888-bfac-54d0fbdfd35f"), 14, new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, material submittal and drawings are approved.", true, 1, "REF-0207", 1 },
                    { new Guid("e6e4da3a-a8b5-4dcf-a891-4b2c3b1c4d88"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Firefighting Pipe", true, null, "REF-0508", 6 },
                    { new Guid("e70fbe5d-3ae4-4871-9698-91c816aea508"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Remarks", true, null, "REF-0335", 10 },
                    { new Guid("e72ad4e2-9596-4129-8ab5-46cc458fde31"), 23, new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Level of the surface", true, 2, "REF-0352", 3 },
                    { new Guid("e7496dc0-9775-4ca4-beca-053b892659c5"), 33, new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0639", 4 },
                    { new Guid("e7a0e349-3f2d-4b3b-9858-de097e44bb8d"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe sizes are as per approved shop drawing.", true, null, "REF-0040", 13 },
                    { new Guid("e7cf3ce6-4886-4a56-88a0-29eb6cb33b22"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chilled Water pipe", true, null, "REF-0655", 5 },
                    { new Guid("e80d2f89-8c4c-4afe-b396-7709593f2e61"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The materials are approved", true, null, "REF-0077", 2 },
                    { new Guid("e89acfc1-4629-4cb4-8cbd-96c514f826aa"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wireless Switch Janitor & Linen Room", true, null, "REF-0517", 5 },
                    { new Guid("e8e82513-ed02-406c-94ff-bcb6abec9be5"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe sizes are as per approved shop drawing.", true, null, "REF-0450", 9 },
                    { new Guid("e911461f-ae4d-46f8-b82f-f534ef2ae717"), 19, new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Obtain MEP clearance prior to start the wet area water proofing.", true, 1, "REF-0285", 4 },
                    { new Guid("ea95acb3-aac2-4337-8121-59e25743c831"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "20 A, Cable/Flex outlet Washing Machine", true, 4, "REF-0676", 16 },
                    { new Guid("eac92d42-c6d9-477f-9f35-b346da9fd8ba"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damages, if any", true, null, "REF-0602", 16 },
                    { new Guid("eb047e8f-6dcc-4ef4-b4d8-9ba117ab7aa4"), 23, new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check Applied Sealant and its Color", true, 2, "REF-0353", 4 },
                    { new Guid("eb06003c-f9de-4e67-92dc-4fee24df8b7a"), 32, new Guid("0de59c28-ca94-48c9-9c47-27efa66959e6"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paint touch completed around the frame.", true, null, "REF-0585", 6 },
                    { new Guid("eb139554-48bb-42a0-9fb1-4d5fc4b7dd20"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Element", true, null, "REF-0462", 6 },
                    { new Guid("ec66f868-c7d8-4309-8e13-7f686470236d"), 18, new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the type, fixation, level and alignment of the false ceiling board / tiles as per the approved drawings.", true, 2, "REF-0274", 1 },
                    { new Guid("ec6ac050-354f-40f5-870e-cb26cbd38dc8"), 31, new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0486", 4 },
                    { new Guid("ec889fee-8818-4413-8617-699c7e3768c4"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "D1-Ceiling Mounted Light Living and Bed Room", true, 3, "REF-0691", 1 },
                    { new Guid("ec96cb4d-0a87-423c-851f-76fa7f64fde3"), 11, new Guid("13faeb36-32d0-4af9-a4b5-e42343cc30cb"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the expiry date of the material prior to applications.", true, null, "REF-0170", 3 },
                    { new Guid("ecc10bd2-ad34-48ba-8bc3-8a40d44fa0a4"), 33, new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation of pipe", true, null, "REF-0632", 2 },
                    { new Guid("ecc9d7dc-847b-4a94-896c-596e9d9b8186"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure containment has been cleaned prior to start pulling", true, null, "REF-0198", 4 },
                    { new Guid("ed858d8a-9235-450d-ab73-ed95c204208d"), 23, new Guid("31f6e8c0-f49a-4893-a92e-806b380e9ff4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored as per manufacturers recommendations.", true, 1, "REF-0337", 2 },
                    { new Guid("edb98c9d-35cf-42fa-821c-e5aa29eb06e8"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of tile adhesive using notch trowel on the backside of tile and over the substrate.", true, 2, "REF-0100", 3 },
                    { new Guid("edc36925-d91f-43d8-8986-f9a113ca3437"), 4, new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of Primer as per manufacturers recommendation", true, 2, "REF-0067", 1 },
                    { new Guid("ede76791-a5a8-41cb-8fc7-bf9d94c7a91f"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pipe sizes are as per approved shop drawing.", true, null, "REF-0114", 2 },
                    { new Guid("ee17dc5f-37f3-46b4-b00b-938ea724739f"), 21, new Guid("00652d95-e040-4a72-956c-c051678686da"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, 1, "REF-0312", 1 },
                    { new Guid("ee40823b-bb7e-4fbc-89d3-29101b3ff4d6"), 26, new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check bracket/screws/angle valves/P-traps are properly installed", true, null, "REF-0410", 7 },
                    { new Guid("ee9eb2a1-b74f-4261-b3f8-3e5a47fb9a4e"), 19, new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the curing of application as per manufacturer recommendation.", true, 2, "REF-0294", 8 },
                    { new Guid("eef55547-7f41-4087-9f28-eeb5d059b98f"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the pipe routing are coordinated with other services.", true, null, "REF-0160", 14 },
                    { new Guid("ef21b2a7-8594-4272-883c-74094a99a543"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal Paint (Application of Primer, Stucco and 2nd Coat of Paint)", true, null, "REF-0557", 2 },
                    { new Guid("ef4c44ac-4039-4bcc-9826-e5325a1a1007"), 3, new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The fittings used with embedded conduit shall be concrete tight and water tight.", true, null, "REF-0054", 7 },
                    { new Guid("ef95a7a6-c1c3-4d7e-839d-37d50edc9834"), 28, new Guid("c8b6346b-088b-4e03-b64d-a5dbd311b713"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure materials are stored as per manufacturers recommendations.", true, null, "REF-0426", 2 },
                    { new Guid("f0429c4f-e6f4-4f60-813d-3fb03aebf219"), 33, new Guid("80069559-da26-4d37-8799-6927aed7e80c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10A 3G, 1 Way switch", true, 4, "REF-0663", 3 },
                    { new Guid("f113debb-cab9-44ad-b00e-b5e4018e1f5c"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the duct connection to the unit, there is no deformity in the connector.", true, null, "REF-0047", 20 },
                    { new Guid("f134eeef-7a92-4274-9115-125cf9d5373f"), 1, new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check height of the box at different location as per drawing", true, 2, "REF-0010", 5 },
                    { new Guid("f14d841d-7b32-41e6-8133-0d6d7603f40a"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installed pipes are free of sag & bend.", true, null, "REF-0156", 10 },
                    { new Guid("f155fbaa-f686-4517-92bd-4dc759374bdb"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleeves are provided for the pipes passing the structural memebers", true, null, "REF-0083", 8 },
                    { new Guid("f17c38a8-611b-4a4f-a672-d891ec5d123a"), 4, new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Line between two color shades is straight, no Brush marks should be visible.", true, 4, "REF-0071", 5 },
                    { new Guid("f1d436c5-826f-4891-acec-26dc7bbac7f8"), 4, new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, 3, "REF-0069", 3 },
                    { new Guid("f21a1667-ab07-4d0a-ae8e-1a7b5513fa31"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varify Line and Level", true, null, "REF-0441", 10 },
                    { new Guid("f23db52c-5216-4c9d-ab46-7520823ef225"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes & MEP fittings.", true, null, "REF-0617", 15 },
                    { new Guid("f24a7845-057a-4b89-b83b-817822f8e4c6"), 1, new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Level Line to be marked at walls on 1000mm from FFL.", true, 2, "REF-0015", 1 },
                    { new Guid("f2603bb4-f950-49fa-9706-e1c0a37ee75e"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the application of coats as per project requirements / manufacturer recommandations.", true, null, "REF-0178", 4 },
                    { new Guid("f2f39eea-7144-4030-9045-2a8c03f076e7"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure location/dimensions of drawers are per drawing", true, null, "REF-0369", 7 },
                    { new Guid("f3007f58-fd72-4809-b5ab-e6f36f257f07"), 7, new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All open pipe ends or fitting openings shall be plugged or capped immediately during construction.", true, null, "REF-0126", 14 },
                    { new Guid("f3ede0ce-64e0-433d-9ac1-263a89620e6b"), 24, new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the color, type, material, coating of leaf, materials are as per approved material approval and project requirements.", true, null, "REF-0357", 3 },
                    { new Guid("f4291dc4-25d6-4c9e-b292-6cbf333d5393"), 24, new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify Cloth Hangers/Tie Holders are installed as per the approved drawings.", true, null, "REF-0371", 9 },
                    { new Guid("f4406a32-3e71-47b7-a307-373e83246d5d"), 18, new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the marking, position and alignment of MEP & ceiling mounted fixtures (light, access panel, sprinklers, signages etc.) in the ceiling as per the approved drawings.", true, 2, "REF-0275", 2 },
                    { new Guid("f45f776f-1819-4b89-a33d-42aef2b68a5e"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Architraves are fixed as per Drawing around Pod door", true, null, "REF-0594", 8 },
                    { new Guid("f51cfbac-61d2-41b6-9acf-c06f819d3ef1"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.", true, null, "REF-0192", 10 },
                    { new Guid("f522dac1-ebfd-41f5-b806-da3809cc4eb6"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check that the identification labels are provided for the installed piping.", true, null, "REF-0138", 10 },
                    { new Guid("f545fd0a-f082-4279-bd73-d5ea4c5c4ddf"), 31, new Guid("b9b52c68-a027-4de2-82db-ccbe3fb429a2"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Sprinklers", true, null, "REF-0490", 1 },
                    { new Guid("f56c730c-7b84-4f92-b7da-c8f9aafb5481"), 27, new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Confirm installation and Cables are as per approved system schematic diagram", true, null, "REF-0417", 4 },
                    { new Guid("f56fd362-0129-4572-8472-fc212d7097e5"), 33, new Guid("b2fc0404-4059-4431-8816-92c314be5523"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duct Riser and connection", true, null, "REF-0657", 7 },
                    { new Guid("f5a1c3e7-a35f-46e7-9571-a146012d4a33"), 29, new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the indoor unit location/height as per approved shop drawing.", true, null, "REF-0444", 3 },
                    { new Guid("f5b4ca81-f3f8-43e4-be6f-e7480fbbf78e"), 31, new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMU", true, null, "REF-0542", 12 },
                    { new Guid("f5bab168-1f30-42d2-a704-cfdd6a0a6aa7"), 11, new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the rate of application as per the manufacturer recommendation and method statement.", true, null, "REF-0177", 3 },
                    { new Guid("f6d11a24-c99c-491b-aa13-156010ff8f28"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insulation is applied as per manufacturer's instructions anf finished smooth and straight without any damages.", true, null, "REF-0190", 8 },
                    { new Guid("f6f49553-2d9d-4807-88fb-8754e1b6b7bf"), 8, new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the location of O,S & Y Gate Valve, NRV, pressure reducing valve, pressure relief valve and pressure gauge as per the approved drawing and accessible for maintenance.", true, null, "REF-0145", 17 },
                    { new Guid("f70f6090-7425-4023-bb93-8286eceec229"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kitchen sink and sink mixer installed", true, null, "REF-0598", 12 },
                    { new Guid("f72519f8-49b9-4876-82e7-d46fc87e8818"), 6, new Guid("f4150c0f-aa65-4c9e-9c06-6f01b65f9d0f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, 1, "REF-0089", 1 },
                    { new Guid("f75036e3-6dad-4313-9e9b-4e693cc1e88f"), 6, new Guid("6938c75b-e0d2-4786-8c86-912124520624"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the location and level of floor drain as per the approved drawings.", true, 1, "REF-0096", 5 },
                    { new Guid("f7528494-9836-4455-9da9-351a053b529d"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the unit slope toward the condensate tray oulet.", true, null, "REF-0031", 4 },
                    { new Guid("f77b091e-68f1-4a39-8a44-723fb0291312"), 32, new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Locking of Doors and Shutters securely to avoid movement during transportation", true, null, "REF-0595", 9 },
                    { new Guid("f78928e3-4a97-4ad6-8e9c-0ee1d4e9c533"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Fire Damper and Back Draft Damper", true, null, "REF-0629", 8 },
                    { new Guid("f7b1dd13-c8a9-4079-a522-ad4d414432a4"), 22, new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cable Size", true, null, "REF-0327", 2 },
                    { new Guid("f7b2e0df-941b-4231-bf83-710047d2519a"), 32, new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pod Shower installted and free from damage", true, null, "REF-0611", 9 },
                    { new Guid("f7bd3c96-fb5c-4281-9bb4-5b631aca5290"), 31, new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soil Pipe", true, null, "REF-0503", 1 },
                    { new Guid("f8d1caa3-df11-43af-a593-77e2b72c440e"), 13, new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check and ensure the drawings used for installation are current and approved.", true, null, "REF-0196", 2 },
                    { new Guid("f98de2d8-cb2e-4557-8ace-afe42869dee0"), 21, new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ensure application of primer as per manufecturers recommendation", true, 2, "REF-0317", 1 },
                    { new Guid("f9fe2e06-3ad3-42a6-842c-9efac4bccad0"), 33, new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pressure testing of the Piping", true, null, "REF-0634", 4 },
                    { new Guid("fa17f164-ca80-4ea1-a1a5-50044a94ba96"), 25, new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval obtained from Consultant/Client to proceed with further activities.", true, null, "REF-0395", 13 },
                    { new Guid("fa367570-0059-4e5d-914b-ea736d74d929"), 33, new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Installation of Return Duct", true, 1, "REF-0623", 2 },
                    { new Guid("fa858ddc-40e4-430e-8cd8-a3b2ee09818c"), 31, new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Twin Data Outlet -Euro face plate Duplex keystone adaptor", true, null, "REF-0521", 9 },
                    { new Guid("fb4c4e21-b0a4-4fb1-aa46-378b38b4d769"), 18, new Guid("df4586d9-9081-4c69-b520-25d3d293b7d1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the marking / location of suspension system supports at ceiling as per the approved drawings.", true, 1, "REF-0265", 2 },
                    { new Guid("fbe8cd1d-3428-4f5c-89db-df03ab0be959"), 9, new Guid("847ba81f-2ba5-4624-a0f9-425752367897"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Valves & accessories are intalled as per approved layout", true, null, "REF-0157", 11 },
                    { new Guid("fc9a846f-3894-4023-b0ce-0ab2a8a99909"), 5, new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved shop drawings should be followed", true, null, "REF-0076", 1 },
                    { new Guid("fcb5a35d-2c3c-4bc7-b054-9ccc2d0efd15"), 32, new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Location and color of Painting as per the App Drawing", true, null, "REF-0556", 1 },
                    { new Guid("fde06e4a-b0dd-45ae-bc86-c9e04f962809"), 19, new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, 1, "REF-0282", 1 },
                    { new Guid("fec1e24a-187f-4304-b285-5c0259d4f607"), 30, new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Start Time of Test", true, null, "REF-0467", 11 },
                    { new Guid("fef0be3f-fbc9-4976-9d08-1dbd21fc6105"), 18, new Guid("df4586d9-9081-4c69-b520-25d3d293b7d1"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verify the marking of the false ceiling level on the walls as per the approved drawings.", true, 1, "REF-0264", 1 },
                    { new Guid("ff086f41-276d-41ef-a442-5599aed24178"), 28, new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varify the Rigidity of frame and moveable panels", true, null, "REF-0440", 9 },
                    { new Guid("ff097399-f610-463b-8dbc-2d5ec3ff2a13"), 12, new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The joints and flanges are correctly made, jointed and sealed.", true, null, "REF-0186", 4 },
                    { new Guid("ff776cae-3f8f-4fe3-af3d-c286ccc7bc6c"), 33, new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "DB Panel Tags and identification.", true, null, "REF-0686", 8 },
                    { new Guid("ff990c53-fa5c-4f77-b19f-36ada11518b0"), 2, new Guid("015053bc-53a9-43fd-8040-c93ec3082027"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the condensate drain pipes are connected with flexible hose & p trap with proper slope.", true, null, "REF-0046", 19 },
                    { new Guid("ffa63bfd-1e97-4f24-b806-b4014d2c3b25"), 6, new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check the setting out / pattern of wall and floor tiles as per the approved drawings.", true, 2, "REF-0102", 5 },
                    { new Guid("ffe42576-2b31-4f7d-890b-78015bb52ccb"), 33, new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"), new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "W1-Wall mounted recessed balcony light", true, 3, "REF-0697", 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0107278c-049d-4a73-a843-c0fbe905910e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("01b4f4dc-ff27-4d0a-a4ec-bd84b62b9be7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0220c2d3-0029-4885-bd22-68b3aa6c9779"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("02ced833-2e61-430d-b297-a885c3c9e8d7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("02e45a45-e6d3-4e09-ac80-5402e7ea8322"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("03477226-c72e-4a97-9e6d-5671ed649240"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("03e78227-dbf6-478e-8c01-f9d22c823fca"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04698a62-e44f-4e89-acc6-cbcded5c5002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04c346e9-ba12-4d6d-aa06-60eef6c346d0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04d7af06-438e-450c-bfe9-eb37e06f5e6b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("04fe8436-2097-447c-b9bf-0abe2be0b48a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("052a5cf4-6021-408e-adb1-9c7a7764a6c4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("05ac7dcc-43ca-4a68-93e4-029a3d80b90d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("05d9ba73-0f20-4d10-a6b1-9aa80e26cc17"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("062fda0b-afba-44df-8476-7779f48df1a7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("06c99882-5fea-46db-b48f-f6d68f6d52b2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("07743bce-92f3-4b74-b141-5f55d07b539f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("07b8aa1e-ed4e-4665-ab9f-d0d600d154a9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("081b58c5-4cbc-4750-b165-1b79d70c2c72"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("085bb58e-540d-4b6c-84cc-54776d64fc89"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("08bc6230-4d6b-42c2-be3f-3a771890958b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("08d47879-25c8-4009-8a5a-43bb1ce2d4ad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09272c94-f0e9-44b3-8313-c067d777eebe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09b3e941-a3fc-426f-ba68-980e3a7a6a27"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("09b3f44e-67bc-43bc-8b88-7bf51e8a7950"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0c00f837-611d-446d-b658-2c3a601eb921"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0c083254-bbf4-4496-85c5-173dec86ddef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0c13ecc1-3223-4c16-9abb-51e69e1e727d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0c897c36-13c9-45f8-ab97-98a0a73ef823"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0c9e3cff-e7a2-4432-9831-430d56a1d002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0d573c4b-545c-445d-a0b2-e145c1a7f9aa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0dcd763b-9cbf-43c8-93a3-49b24a0592b5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0dd86d11-6cf6-42b4-b489-20e51c6a5d3c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0e4990a2-a019-448a-9e4d-a8075fb704ef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0e79780d-627c-4de4-85ef-caee115df80f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0e825a1e-597c-4249-969c-d9bf8ecbf217"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f111bb0-0aeb-4000-bb91-f398ef931222"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f28494f-d778-4deb-a1a6-31bb36337ac8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0f93f07b-c4c2-448b-b248-12bbbfafb094"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("0fd1838f-30f3-4548-862a-36e0cf28e7ea"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("103277d0-ab14-4d98-b096-c39cd2a39bad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("103c2da9-1b8b-4e09-80f4-1bfc2c173295"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("105594e5-688b-46e5-b08d-01eac81171c3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("111d0079-f84b-46e7-b2ff-dbf0b74f3ba1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("129bc385-a35c-4168-9957-2b389fc2a4aa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("12af8392-002a-4405-885a-b366a78058c1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("12c6986e-441d-431c-b9e1-b4fe891025ab"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("13724d8f-e010-42ea-82fd-183a6e9d05bc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("13a573ec-e219-4be6-a570-ada6431e3002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("140cc6e1-5532-4bf3-a0d2-b7520efd62e3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1416a5de-152c-4c95-bcc9-f983c1e4ad88"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("146e6018-a542-4f56-bf5e-90cbe0f47888"));

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
                keyValue: new Guid("15629b0b-5874-4820-aaca-2110feb63147"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1566dece-d7b0-4e35-9d57-8e6a42642b3d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("16ad46e9-8938-49dc-81c5-1b9ae6aae443"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("16df93a1-6764-46e4-ae7c-f7c5e9cc094f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("16e1acac-c15b-49b7-9958-ada01ee88920"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1715dbde-644c-49fc-b368-0f158ff81071"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("172c06d1-2f4b-4308-bc82-294745d500d1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1772d3a5-c9cf-4b90-84c2-0495e59caa57"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("17aeccc1-bbaa-4ef6-b59d-af67e31ddece"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("182b9f7c-e4ba-4d4c-845b-c6572c78460d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("18d723c7-436d-4711-8cbc-ea31d259bf39"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("19a7f51f-b924-4082-9248-7e58393e202d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1a9e047a-ceef-4557-a7a1-7d7ae42db8a7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1aa26ac0-0531-4db8-b9f2-f62ae6963262"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1ab21f95-1979-4859-aefc-64df110e42bc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1ba35898-fa1c-4e8d-9f2d-d2729089870c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1c897f5d-0399-4b61-9993-1f9a58c10692"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1c955271-6607-4f41-bb6b-f070821c5375"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1ca61259-3a0c-46b1-8d26-1554ffea85f1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1cb92d12-4546-4441-a394-7e5ec18b0eb4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1ce8f571-d81c-45c8-bc95-a91733b63f0d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d0064cd-a381-42e3-9bd1-b226e241111c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d04d260-733e-473d-98ac-5830df530b96"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d16913a-856c-4bcd-89fe-332d7b62e41f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1d1aeb89-68aa-49c6-b2c0-020ef825deda"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1db870d3-d222-4d0f-a085-fa9541527f02"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1e4d0c53-28fd-439b-9eb2-cae591c45edc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1e8d2475-09b2-4432-b759-546956257b4f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1eb79f3e-cb02-44f2-a80d-9ed634952cd0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1eeaa530-b6cb-44e7-ae74-3718b1e5e872"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1f8f3d81-bf07-487f-b306-f3f861967a64"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1f916cb0-a3fc-4e72-a79b-b8da1d0de105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1fc02631-b0c8-4697-946e-e37a8d22af7a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("1fc723a9-a922-4a2e-a37a-4271dda7a7d8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20284c8e-b358-49ab-8638-c91e42bed6c3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("20bf8edf-7344-404c-9632-329cca228deb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("211ce4b1-ed70-497f-9fc4-cfc3dc2ee3f8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("21a096df-ea8e-47e1-bebe-9e1c5b79af43"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("21a83d6e-3a43-497c-bc26-84622f6dd12d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("21bd8d7d-5c9e-4296-9946-31744d8a6d73"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("224dac96-da3b-4b43-ba41-35ff26695f69"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("22660045-5194-4abb-962e-06e1d5edcdf9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2307dd77-092a-459c-8cf8-9dd7d4052532"));

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
                keyValue: new Guid("2639d198-6e90-4dd8-8c5b-9786c8ef2133"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("26f83daf-f89d-4ece-a1b3-a9d6c00307a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("27ba937a-9c7f-4ecc-b9f3-9a9b161eb76c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("27cbb526-4d6a-43d8-8907-604d684b7734"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("288645d9-3492-43e0-82bd-490517fd0c6f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("290ce1bf-7ffb-4b42-94e4-17fe1242a3e1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2b6382a0-29a5-485b-918c-4b04719ac277"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c036e3d-7375-4a49-af8a-75725ac8467e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c28fec3-3c39-4cd9-bae3-f4e3ad082d99"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c47d839-6b1e-4f7b-b427-169b849701c2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c65ce6c-4aeb-4dcf-adf5-79fd96a74fef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2c71ee06-19f7-4d83-8831-c9231b4263b4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2cc08f24-5f4d-421a-ae3a-cd8613d9e859"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2d211cf7-6a3f-4a0e-b41d-d75ca38bd0dd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2d559d0e-0c62-4731-82a0-b65eacccb862"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2d84680f-b347-4632-9caa-40f755c9f28a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2da4b5d8-2d4c-4e69-a8cc-7ad9028abcb0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2ddd69b8-b5b6-4194-8205-45b75b615e37"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2f04f0df-4ed9-4372-98b9-3ced3231f320"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("2f20a7a6-7402-4bf5-b3a4-76ff474b3ec3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("31f46bde-e5dc-479b-be71-45bb5bdafb56"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("323f619f-a585-4bdd-b32e-5aa7cdb71bd0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("33adc13d-0a8a-4d1f-9b45-5e29351b5495"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("33b19000-dcff-4054-a9fd-31e1b3f95233"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("33d53b69-a990-4140-bfa1-73e94fccc585"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("34742979-182c-419f-b838-32265f835f45"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("349bd5df-6a98-45d5-85af-062b9b403edb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("352c036d-d372-4e89-92dc-764f8c64349a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("358aedf4-b7ef-4726-89b8-f82e886540fe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("35a436a3-69fe-44ed-ae55-37d57a2707bf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("35c89581-d1ee-4301-aeb4-b230e856664b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("369b6242-14d6-4fba-9b75-1235d19cf452"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("36aea065-6d5c-4745-8b25-5e45d1922bd1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("37634b72-32c2-4c01-b122-7f9724a8320a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("37ece6ee-5b00-4f59-99d7-793316077609"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("387154b1-94a1-42a9-acb0-e8b5f784179a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("38a7b53b-6f7b-467f-a8a7-19f15c50d7ea"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("38cbe975-ccca-4392-a4df-696b155663dd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3930bfa3-d6de-46f6-bad0-1b53a40a2578"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("39c663f5-cb16-4127-ade1-867d3a58a617"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3a916071-80b9-44de-aaf1-8277f78f6625"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b1c32eb-3c0b-4d71-a636-474f20ec789f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b39821f-0326-4ec1-a3b0-10e166d0e2d4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b5ef6f8-17de-4154-a59a-b624864e69a9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3b706902-8f64-41b5-8816-1170f03dfb81"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3baa525a-04a9-484e-b79e-5e1b0e2bd1db"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3be38508-28d0-4da7-ab04-a4309ac3a16a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3c3552b8-391b-4e50-a2c4-fbe64ad258a6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3c9685a6-c67d-4547-b638-14537a9d5187"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e6d1e9e-7597-4f12-a30e-f5848931e418"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e75cb7f-0ae6-410b-93b4-105d000f4e30"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3e8804eb-5eca-4c1a-b593-631961a7447d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("3ec3a9e0-b3bf-461b-987d-7aebd0a58371"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("403cf397-2ad9-4f6e-889d-0d574a5f4ad4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40cc3cb6-1bc2-4c4c-83e8-c79b360f0e4a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40cdd986-f749-46f0-8d6c-0a3f79daafd7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("40ed5516-f7be-4b5b-999f-7cecf0a439e0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("41260413-e43f-42c0-a861-f922cd933b19"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("41d00092-1bfc-4b42-bbd2-cb003193800a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("41f81e13-30db-414e-9a13-98d6374a0ef0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("42fdc704-b5e7-44b2-b45a-d8199af4d07b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("430bf6e6-17d8-4a63-8106-91280499b3e9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("43adc193-73e6-4cd3-8a27-5f6f42e919b4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("43b81437-5409-4e34-824e-985cf80fe25c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("43c7e739-2e6e-4aa9-966f-55544237d3f8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("441afc7f-a0cb-4cb2-b386-312c14f24160"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("45efd065-5c15-435d-9328-1b94f681c251"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("45f74fc8-45c1-40fc-a63d-0aff25455051"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4642025b-d9a5-42b2-991e-bd1110e5a444"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4674359e-6b09-46ce-8f11-dcf690d8b575"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("469b11c3-9b29-432e-a40e-41f288e6bad6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("46de0b44-619e-4283-ada6-5fbb026ca41a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("470f9567-f0f6-4722-aa32-788992c47f25"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("478bf621-712d-42bf-9da7-7b51a085562e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4935f231-22dc-4e2e-a002-736a51a32f35"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("494f70ea-6edc-4323-bd7b-9f0871b2fb91"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("49cb5eb3-b64d-4e6f-b512-217892c688f5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4af9ee4e-e534-4724-b5ad-fc01a37a4ba3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4b138d2b-15d4-46e7-93d3-0348ac2840af"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4b168451-bbb5-432b-81cd-6856015d72ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4baf7b8f-a015-4076-9b8e-91801d77a5aa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4cf893c7-8ab5-407f-8f37-954f580d197e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4d0d208f-72e9-46a0-b4af-cb811303a757"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4d6ac49a-5417-4dd8-b419-e4bfb3934a9e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4d7872fd-5fc7-4605-819a-7707b041d18e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4d9a2c2a-ec21-489a-a5df-1892b56029fa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e12e9de-426f-4bbb-b512-8611c61a635c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4e6dd28d-ce0b-493a-ac7d-3acf97bacb9c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4ec4fcb2-4ae4-488c-8ea7-2e041f084c48"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("4f15b0b2-0b65-46ed-9666-d8718ed18631"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50143032-0002-4d9b-ae22-d53cd37cfdc8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("50b8d94c-afbb-44f4-b8d2-e19ec3a51342"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("512447f2-f057-49a9-9b20-8adf07bd285f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("514366d3-9f52-403a-907b-0eb230a3d09a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52389a97-13cb-4105-b765-d9f9042e1c28"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52793a37-42d0-4912-b482-4f00fe58808a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52e4c536-a852-4100-a959-340427f8a202"));

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
                keyValue: new Guid("53e28149-b4ea-4505-a216-88d94d17bda1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("53ea654d-85d1-45a3-a724-36a211ed2002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("540425a1-235d-43cf-b6c6-243fbcd3633d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("541e14c6-0ff2-4f1f-845a-ad45e2411cad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("553f5c81-5d40-4a60-8164-e918a0c480aa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5614599f-a50d-4493-9b16-0906caa3cb28"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("56334ad3-57c0-4e76-8e3f-22b208c8949c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("56552405-cd96-4c58-9382-ca96ade1874b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("570418a2-3efe-4595-82e5-4f685fa896b7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("57212d2b-4fb7-414c-a1b0-e05abafff7bf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5754a1ad-7e69-452b-a749-ed982ff0bd49"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("578cb175-3ca9-4ae4-a51a-dddfe7833fe3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("58208b50-c7ab-4aab-8abf-d12e0bc499b8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("585e23d7-1214-4e49-9a47-7b75c9c9e591"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("58df44d8-fc05-497c-9fba-aac2bb0812e5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("591499dc-c5ba-433a-a2b9-2d14f9bf952b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("593f6152-0b33-4ffc-9db4-40eef486ef60"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("594b64c5-e8b8-48df-a1fd-7c4cff067732"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5951afd7-c2e3-466d-9a92-a4c2f7fb54e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("59590745-cf63-4ca9-879f-2c9e41989e06"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("599a1b61-2cc0-4f5c-b1cd-a29aa3b4fda9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("59dbe1e9-14dd-4498-b9fb-767b8a428a62"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a993c86-32c7-472a-a39d-b58fd2fc1c02"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5a9bd856-bb54-4e89-b84e-66221357e371"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5b0ac7c9-1287-4207-b4d8-b7fe502e5bdb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5b212cab-2c31-4594-9787-4221f854a705"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5b5068b2-6188-4bf2-84b8-219bb598d32e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5bde543b-a8f4-4c38-816d-b3fe7fd1dcc3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5be605c2-c610-4956-abc7-7fb403011004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5bfc2087-8b31-4675-8821-e72819978adf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5c3029f1-a21c-4752-b883-bd6b627c37f9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5c829863-31c8-4f21-a39f-b31a139886fc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5c90ef3b-9568-43bb-888b-2b6f0c3a6d61"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5c9734db-cc01-423c-b813-f70c874d1f93"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5cb6906f-3fdf-4626-a667-9ff053be78b9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5cbde6d5-b477-48fc-864b-2754498a3a72"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5d061408-580a-4678-b261-4bf8c6e27464"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5da2b26a-6b12-47bb-9520-e918dc397478"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5dc23780-10f1-4fdd-866f-4e56d254e050"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5dc52daf-bffa-417f-a1e8-6715f815fdab"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5e055348-85e7-438a-bf16-58ef08ad0299"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5e1c762f-bd20-42f2-b2ee-dcbfcbe5e421"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5ee62650-4a35-4195-9bf0-07d53c074fa5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5f165a36-6b93-4988-ab62-3bf9dd4c2814"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("5fdb2ac0-cb75-45b4-989a-39e00097752f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("606a1ab0-3f50-477f-928e-2c004dbf6099"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("60759b80-6ff6-4f13-9dc8-5a4cefc624e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("62d88f73-f5c8-4cb3-893d-f561d836cc5e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6343fbd1-4500-4478-a986-430077720e5f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("640de90f-e5f6-4b2b-ad3c-83198e6454dc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("64125285-8631-4209-b991-7f032da5a3f6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("64d8d399-55a5-46cc-8dbc-27495181bcdc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("64feeab6-9865-4c8a-ad1b-63388dbc0aad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("655f1500-1b80-4c9f-94d5-a54ef12f11ff"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("658f2282-bbfa-4273-b550-478b78965c2f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("65f1df0f-b6bb-4c35-9c66-d3c52cef90ef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("66c1c064-46ba-43ff-8068-3d98c5981208"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("66cdd933-7c82-466d-a3c0-43c1651064ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("66ec5514-f5bd-4346-b277-3dc4efa61453"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("67163657-2135-4444-927f-22b00b71c895"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6745c653-2210-4851-9bb0-9795e0f1c068"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("681d9573-580c-47ad-a080-6b4e544c8ec4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("681fd4d5-16b2-4f26-81d6-d26664366a90"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6845689f-4ba7-4c30-9793-80103f0f2723"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("68afb347-f6f4-4129-be0b-2cc62dfc82f2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6913ca24-239d-497c-825e-ae75f5593dab"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("69491148-22ea-492c-ac6f-adddc0b410b2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("69c8ef21-5d6a-4053-8664-a4bbf67f2002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6aa98a35-04a4-4425-9366-d84659503f5f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6ac29bce-c031-4b67-9d5d-bfc683b5fc62"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6bbba1c9-0e88-446b-bba6-80b1bb0f25d9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6beccd5d-5bdb-424a-865a-bb2e7a27155b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6bf6376a-0349-4693-a705-eb5d0dcfc782"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6c02392a-e8e8-4164-b019-f31ba44a8166"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6c91891f-f7c4-474f-9e6d-21983a0df8c5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d149393-9070-4f01-857b-70c9c0c51cb8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d1fbc32-d882-4b52-9f6f-fdd69e60403e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6d78ce26-de03-4b4e-9198-dedd505a35c4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6dec26f6-15c6-4330-9363-5fc625b26e1f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6e25f508-ddcf-4ac4-945d-a8ad89f2b805"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6e6da650-d12c-4fdf-98ff-6cc2a3e99e8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6ea6173e-a66d-4894-af82-e296d53d4f8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6f502097-445b-4934-ad76-0e45f16a7d76"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("6faf8916-81be-4553-9614-356e0252ef61"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("70ca5dbd-cbc3-4eda-a616-1b9cf588779e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7212ab28-1323-49b7-a37c-6cd4db4e8102"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("721e6fd9-3dc4-4b31-90cc-079d32f87b58"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("729fdd41-54d6-476d-b771-b6fe144a77b1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("73519f0f-51f3-4dad-8496-0e90f6b453af"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("73ba10b2-be36-4ef0-85fa-eda0f41df3c6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("73d5aa6c-30a2-473d-8c77-c8390104bb40"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7403d76b-8de5-4f92-9a10-631890079a3c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("74a948fc-e34b-4a22-b136-d19fbe6af766"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7541e031-0162-4a3f-affe-b15bc6bcd374"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("75f725b4-31cc-43b9-ba04-5f51379dac70"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("76126ec8-2be1-4c8f-8e3f-c1ab289a713a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77072599-a0dd-4018-9922-6a3e80e7319e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77751558-dc8c-4b53-a628-b370c7de2372"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("77825f8e-98e1-4c39-89dd-d9d494e1ada4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7803530a-d508-4040-9697-e3275b740558"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7867b29b-6808-4c78-ba13-8430c3a7c19b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7894ae06-2b59-4b01-861f-b1d31a86c6a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("78e52a23-8de2-4f14-8644-61a0e4a838ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79b31440-1ebe-41c9-954f-669b1a66918f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79cb402a-73e1-412b-a217-fe74bfcf88e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79d97660-c6f6-4f43-a1e5-f19c87678d79"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79e570b8-d376-42aa-8e61-bf512332c96d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("79e9a298-d74b-463d-94fe-c997e6596cd8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7a10f754-bd03-45b9-b3ad-849441baaa85"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b22e1c1-4e06-4bf8-88db-2a5f3a730755"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b26a149-0192-456f-ade5-27f3e27860e0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b75c69b-fa43-4774-a272-65eb7b913369"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7b91df5f-6edd-4042-a106-43b290732bc5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7bfda768-0861-456f-bb67-81e44547f005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7c6f6b88-4eae-4346-9a4d-e38a4ce588f4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7c829bf3-d0ec-40e2-b0d0-9f70dfbc1e9f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7ce2a769-44b4-43b7-b3e6-25ba7d0757b8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7d0fc5d9-658e-465b-84a4-62c8ded87ead"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7d405e5a-ddab-4d18-bfd4-5b458437ab3d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7d603add-7f92-4a9b-9935-f88951c077fa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7d750f4f-1a1e-4812-b530-da3aff8b85ec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7e4c3f32-9931-4cca-9672-9edbd662a8d9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7e7fd5f4-ec7d-4656-904f-0e3f387658d5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7eafc3ba-549c-4a4c-a2e3-b7bc3d4e0940"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7eb02c63-ddd7-4b15-960c-14096b59a157"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7f11eb56-9c85-41c4-9c1d-8ccaf372f749"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7fc7b988-0a7b-4542-b483-c0f80184db4f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("7febb68a-bbdc-4fb1-851b-fdddf0b71cff"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("80d81715-a4c7-49f6-a876-e46978bc1171"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("81493c56-6ed3-41c1-9998-f828b91f7799"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("815dca03-8329-47e2-a1e2-6c7070eb29e7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8180d017-c10c-4cd0-b99a-a7d838b019af"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("81a0b165-630a-4a8b-b437-4c4b7f621d2a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("81aa004d-85e3-4938-94ca-32aa23ce4814"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8241fae6-cf3b-44be-bf82-347034fa8306"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("82d17236-eba3-416d-bac6-e1fbc2cd527b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("83432d01-c256-4305-815e-25689880c225"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("83f4a4ee-75b0-418a-9d03-ec77d0f0f6e9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("84116a64-f6f8-4a5e-b801-d2cd5e10b753"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8453ed09-0800-4724-a075-2108477ea92a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("847a6d48-9ae3-4e44-a8d6-7962fabb9bf2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8492bd88-5615-4ae6-aed4-afc8454fb934"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("853c9e38-26b0-4693-a767-f26e4eeec52e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8585aba3-f549-40ae-a3ad-27bc0b7f379b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("85a1755e-c267-4414-8212-7388f41c23a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("85ce2c11-1394-4f61-a04e-2af869da95f0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("864d236d-ce8e-4e01-af52-aa3b765d48d4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("867faaf8-e5df-4458-9fdc-5f4b598ebc82"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("86a61a68-6503-436a-92b8-0da353de43dd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8768930e-a8b5-42aa-8606-4859dd18da32"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88b5db1d-f0fc-4b91-888b-6ded612088cb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88cc1dbf-d471-4216-ae6f-a9ed341400d0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("88e1c6b7-ec67-4579-af62-a3f27e2dce6d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("898b9c95-004f-4ab5-912c-436c41b9af4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8bdb8393-7c9c-4a06-9911-7b5d55734d4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8bee12ea-99c0-4b2b-85fe-72d3f2960c86"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8c456f71-b17e-43b4-8862-12e23ba1ad1b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8c7d9f38-81bf-49ea-92cb-95a1b3576d3a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8d0a4c90-4155-4bf6-8189-977a6126371b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8d1791a0-cae1-4677-8687-b9dfc3414fbe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8d757c01-965e-4e07-80d1-bafbb637683b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8dd4e197-d197-405d-bbc0-0c9d9c6cfe66"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f3b376e-a9c9-412c-9176-410878f6c35e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f7c8795-ea1f-41fe-9689-458d5641939f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8f89fb85-857b-4084-9249-9821181f507e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("8fad895d-32b6-4595-aa83-4d6c937efe8e"));

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
                keyValue: new Guid("92094210-da61-47e3-a50b-d9fd847ddf8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("922d888c-00f2-455d-92a0-427415001514"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("925099a5-d33d-4596-9879-d65775da638a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9392fbd4-16f4-4c00-acec-f323e8a40e3d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("93e285c6-dd4c-4802-a930-265142a77481"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("93edeff9-3c3f-4090-bf19-edce625c26c8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("94132e61-1f89-4e2b-ae0c-579f18e07a54"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("946db233-fc09-4c54-b459-ee4a7ecbface"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("955140b4-00aa-4ab2-a1f1-5ab51fc3f08d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9583ebb9-bca8-4cd1-abeb-0a04f0a47a2d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95d9bf14-1a5d-483a-a930-4baf527e5de0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95e0441f-ab15-48c9-80d0-21672b96bcec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("95f880a2-eeef-4357-b5b4-cec981c7ea5f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("965995a7-1d68-4978-86c2-10cd2265f36d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("96601fab-c234-4c88-a294-98ad2e23bca5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("968021b6-350c-4d0b-85a9-848557a4bea4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("96a4199e-0dd8-4daa-9b67-5380143f0d74"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("97738c09-af82-4fbb-aa44-2d9d49a3e0e2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("979a3394-a7a2-4aaa-88bc-69a8f075671a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("97ebe931-848b-476d-b2ab-10492e54e4d3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("98220e2e-b428-4fd3-a8a0-8cc428a7b7d7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("989e32f4-a50c-401e-a787-250467407afc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("993aa6ba-0edc-472b-b4ab-1f25754c9bf9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a4546c9-6a69-44d6-b2f7-c48ca514bf8b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a91541a-d201-4b3e-9321-6f062185cc4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9a97608c-d973-4df6-ae9b-b09301805f73"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b096988-5ff1-4b44-a0a6-77b6cb2c14c9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b306fde-5f0e-4647-a4e7-33a1adaa976b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9b4ad754-7bb9-429b-ba6d-e3b05a88deb0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9cca1846-e662-467e-9ef6-cd81e1ffede8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9e742987-363f-428e-9c6a-84917e4784ad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9ea0df82-d199-439a-8b7c-de0862cefe12"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("9f4a2292-f98f-4a76-aee8-cdb91ede9624"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a040cd39-7f2a-4c4d-ae2d-9be050aa8fe7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a058982e-398c-489e-9737-fc5294b1fa4b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a0655886-1f05-4450-b322-561f7ab54234"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a0c868d0-f7d4-4db3-8f69-170f488afa6c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a0e2f6fe-1c80-4aef-a4e4-f3558333c916"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a0e5b184-0a4a-47cc-a64c-03429beb5c5f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a1470add-7183-4718-9d22-b8c189d0bc0c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a1e493b8-e11a-4d8b-9952-d645cec459cb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a1ff291e-a81a-453a-aca3-81c13b8489e4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a2289bc1-e11b-45b2-98ac-85a8eb35c37b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a2828cd6-54b2-4cc7-8ad2-cef27e5d4301"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a2bd1dd4-5df1-4b34-a623-bbe5f677d839"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a30e8f27-06c6-450b-8ce5-ac325a5f782e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a372b33c-04f9-4d86-8316-f9264775ccda"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a43c8dd4-f883-45a3-bdae-e8260442c89c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a4ed943d-30a8-4bf5-9684-0265ac0664f4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a51dfcd1-9229-4abb-965e-375e1915a526"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a5df3e69-c336-4d9e-8d58-3b5362338cb4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a6acdd96-32d0-41e3-a91d-7e72a467e24f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a6c9d888-06bc-4a22-9682-ed2ac3ca76a0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a7bdf2ed-c39f-47b4-ad62-72c5817bb162"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a8f96a49-e70e-418d-8406-35bbbcc6b32c"));

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
                keyValue: new Guid("a954cea8-945c-4b93-90ba-f41d1a58576b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a9926b25-f332-4514-be49-3d12c2545096"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("a9f2380a-44c8-4961-a2c3-5bb31d8aedf4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aaad3366-c391-4c25-b0b7-0d24591d9d93"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aac8ff6d-ce32-472f-ba22-be8f5656f374"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab04eac7-0ab2-46bd-89a8-08cf2554790d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab057b3a-5b18-46d3-8af1-c608cbd564e0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab3f8145-4e56-429f-afc8-36fc8f591bef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ab517858-1181-498e-a42d-43ffc9cbbc0a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abb745e8-b1a1-44e2-8932-6f42997333f8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abb96f1e-7476-4265-a9c6-9de5a65721f7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("abe52242-c1c6-4efe-9771-a9196274b423"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ac9bc775-7085-45c4-acb5-9042bf159804"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ad45bf90-1109-4cc3-ab85-2619da1d9101"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ad656132-ed15-45bf-b23e-8afd7c92040e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ad9b045c-4ed0-480b-b3f9-038995566517"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ada0df0c-3ded-4e14-b789-cc134259d9ae"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("adbd1799-6f64-44e0-bb55-142bf4d25e98"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ae1923dd-96a1-44ee-aa59-5c82979a5c3c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("aea84fbe-06c5-428c-9967-6212a909d10d"));

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
                keyValue: new Guid("af868546-4f3b-47ae-94e3-67e5d7f60cef"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("af94ae15-6ae0-49d7-a2d3-d277a3a48bff"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b0325b54-d58e-476d-a2ea-25f5cd79e5b6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b05e5faf-afce-468d-9f8d-2fcebaf0f6fe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b0763c83-2bc1-42d1-aa70-adc0f6998653"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b14ea2fe-5ca2-4d69-81d2-5a871fb4d18e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b1813260-2885-4243-8122-a882a1baff00"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b2d6bb03-7edc-4965-92c2-39ad84ee6da7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b346074a-ee4a-48e5-a007-ffb1fb299c08"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b35b0411-9005-46ba-9615-d7a90a727686"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b3848625-18e2-4025-acf4-f3d9ac0d47ca"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b3c59182-7948-4c6c-a6c4-955089429d72"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b43498f7-280e-4dd0-9dd4-e4799aad482f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b435bcd5-93ab-48e8-8d70-611c966df1a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b44896bc-fd7d-4a0b-a47b-280a50c7327f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b45d19b1-8bcd-4f25-91d4-4728530503c1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b4d75187-8828-4249-a467-7513a65d1b11"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b544e716-5038-4a01-ac5f-ba30cecb452f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b5a597a9-1cbf-49d8-af83-0cf43698e8e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b6197f0d-4eca-403e-864e-0744f12f143f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b6a68385-55a9-40c2-a257-0f8204db26a9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b6e205a2-9e64-4a1d-b5ee-4e2e582af750"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b6fc4834-4a37-4231-8377-dc0308d6181f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b8466988-21ee-4712-83b5-56b1d1403f27"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("b858ff5d-deec-4b46-a754-a26378a5885c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bb900f09-a78e-4079-b98c-b7ded3c8e9f2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbd1169f-a962-48a4-a166-e9d7a99c49cd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bbe2d49a-8f18-4d83-ba50-2255bfc2f332"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bc89becf-fabc-4491-811b-7717fe102680"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bce9a766-6a7a-4a33-bf9a-cdf890b7fa1a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bcf06691-5334-471f-b27b-e9f5a6e182b8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bdd58212-7137-4ba2-a9cf-9af4d2093f8e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("be8e4474-f481-48b1-9104-9060f18e5a38"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("be8fd14b-581d-430a-9978-dc9b6c367af2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("be931ba4-5b37-4296-96bd-680e77d389ab"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf403728-0d5d-4ab4-9d90-16c26aa88bcc"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bf6b0e94-1c82-4ef4-b6da-52570d087801"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bfaebe11-4b44-4046-8353-05b4aa658c74"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bfc6bd01-28ee-47be-beac-2647960b8270"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("bfedd819-31bc-434f-b0bd-fb9e07f36753"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c025e02c-bf77-46c3-a1fb-cb54b019185e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c0bfe921-cb9a-4dce-b27e-51701233d06f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c0f1e219-e407-4460-8634-ba833192ac99"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c13aa488-69e4-4ec4-bc55-1de3ab7577c4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c14f2f9c-5efe-4a4e-82a5-c0e29af69edd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1c555b7-7771-45db-a267-ce7b6fddf3e5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c1ff045d-55cd-44b1-beb9-26b3e0fa566b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2008d59-9c5f-430b-88ec-ac1032eb4d90"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2389679-0f87-407b-89c6-a5709223b2fe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c25a5fba-d27f-456d-beec-7914265ff91c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c29d5dd2-665a-439a-820c-eb4a01e0baf4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2c81d13-61ec-4559-be99-3f4dbfdae712"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c2ee20b6-2096-452e-9f42-97affdbe75a4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c3053643-fbe2-4ff4-96b5-9098697431de"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c34ada9e-6a20-4e32-b0c6-30b749d6f669"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c41bcf66-c933-4a58-b2cf-93e73c5d5d0d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c4ed391c-9b92-43d0-8077-9deb2c561cca"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c4f8a775-496e-4f4b-b14c-e848043ff388"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5264e1d-04e7-4ecf-ba81-cb01b30d7b7c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5af12e5-445a-4598-8a43-9c7e322444e6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c5dd4075-5da1-458f-ac95-18112b43f1b0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c76908fd-b7f1-4ace-b87c-ea30bafda313"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c7764709-9dd4-44b1-8555-dd0ba57d7d5f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c795abab-561e-48f4-8c35-4f199e8bd06a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c7a47373-9854-4971-b477-5d52b20598c2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c84d19b4-30cc-4dd6-8f28-a967e176a5a7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c8e475f1-cd7a-4dbf-a902-49e6480baa06"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("c9b7cf99-40b5-4519-986b-86bbc0268dee"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca7c0427-e7d5-4b81-b5ff-f8e2ba7d9116"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca7fccca-0923-4234-8320-47e8b748244c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ca8f4146-3299-4b2c-9ca1-b690b19b4927"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cadcc053-801d-48bf-9527-cb6d52d98928"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("caed0266-0388-41af-8a93-fdcc084b2907"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb353b87-d350-476a-bea5-706999583474"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb4a6ae1-4a8b-47a3-9fde-220e926ed20f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb79f23d-81a7-49f4-b810-e730814446ff"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb88439c-f3d1-4999-ad00-08c69aaeb797"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cb8baaf8-1d8e-4230-b4df-0c3ac94fd0d4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cc262511-61fd-4bac-a22a-b990997e7ae0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cc8cfe61-66bf-46e7-abcd-547be6c11d63"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ccf78f91-1eda-4ad1-a8e2-9004105d77bd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd09e3bc-301a-4dbb-b90d-ba68cc56f551"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd23880b-d8d5-4906-8bbe-524b6fb2a026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd6456a4-e734-4044-b31b-0ef06170232e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd8c0c83-3458-401a-ac03-781f2a4e2873"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cd97222f-0ae5-4822-84a9-398a52188da1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cda90f80-a64a-4d43-833d-a9af574dc069"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cdcad9b8-cc0b-4d4d-b811-52b405235bd4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce231580-a82b-4bf5-a80f-60f8f43472c0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ce34e1ed-00c5-4ee8-b88c-bfd5d72e65de"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cf2b96fc-b547-4bfc-aea9-ac2a53e626b0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cf6d1e8c-3730-4527-a151-d3c15eecfffe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cf8948ba-a70c-44e9-86a1-37d658113833"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("cfefdbd3-fb55-40e2-8c69-436c7262b42e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d014b5eb-d37b-4c57-ae02-7e2253ccaf8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d0837def-c9cb-4ee2-8a0e-d1786b147fd0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d09451df-687e-4a49-8f8e-afd2a2d86dbe"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d1260a09-491d-4341-9e69-14c1d5d9abcd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d162b339-6395-419a-a780-fab677a1a43b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d1bf6e74-6579-4410-a0e8-96670c387d09"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d200b52b-da7d-423f-af36-e8572d69a711"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d23ff95e-ac50-4ca3-9d43-017bc279f545"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d26383d5-2148-45ef-b7f3-9fff778842af"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2729cb6-566d-456f-b375-bd21557428e4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2ceaffc-0907-4fcc-bb53-d91462d79a2f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d2e32821-15ef-4d1a-a62b-668ebb3bd724"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d38ccb25-66d5-41f7-bfff-1ebaadb40941"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d3acc70b-d069-45bb-aa23-6b580d5798d3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d3cbbe30-c25a-4bd9-aed3-14535b5ba324"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d402f880-5fa5-4162-ac63-fbfaf810610f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d5f41566-352c-4247-b12f-59b664d1f0b2"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d6023c45-7825-4d5d-aa49-91bb08b09fc0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d6e6598c-93d7-46b5-a1f5-6925b03ddff0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d7394cb4-78be-4d87-b56f-3b570983111b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d7dc5f54-dc65-4178-bb4e-7c6d68dd4887"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d8683107-69be-46a3-be3c-0bf9c7c55359"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d8a00696-ebfb-4112-8688-1e3d3786927e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d8dc1b76-e2df-4b01-a431-26bdc7bdf5b9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d905cf97-5b30-46bf-8f54-12f2fca5a007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d90da177-4043-444b-8b38-53dc49e9fd37"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d91b6f34-7c68-447a-94a5-03c083f46a4c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d91e0777-034c-44e1-9053-01eb3e02040c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9497849-3db6-4cd8-94b6-81dad6c3475b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9547769-376d-4f26-93a1-8fce5bdbf80b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("d9abd2a8-853e-4101-904e-c731c1193318"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("da066bca-1cbb-4546-b3de-ef557bd83220"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("da71752b-8646-46f9-aa7d-a9c1aa536dea"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("db275e55-9586-4f2d-84a5-7865d47f6be9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("db840b56-f8a1-42bc-b984-df7e134ad5a5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc01ca3a-cb6a-4313-afed-13c5e4f6261f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc126c65-f23e-4403-a9b3-ff1fd148b5cd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc63b3a7-0ece-4efb-80d0-0c1db4a272bd"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc8d1393-6c24-4750-a81a-a1649b33302f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dc9487a4-4ac2-4a8f-90b0-40181bf660fb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dcb733c0-b4d6-4911-9c3d-502cb00bc9f8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dce86551-5429-4c69-9c7b-c2a19dc21106"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd082a2c-d35e-45a4-b861-5b551519ddbb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd47d609-400e-4cb0-89ca-b543861e7dc3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd51c76d-cda3-4be1-82ad-2a563f19798d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd6cd258-a3f9-4ca2-aab0-d06caa4e7a42"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("dd99c604-bfa0-41e8-8730-c8bf1c3cc0de"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddb86f1c-d13b-44ae-aae0-4e0e508acbb8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddb909e5-bf6a-4732-a472-035524973db6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ddf0e5c9-774d-4edb-a7d2-f53012694063"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("de0683c3-e647-47a0-8f41-0d8b1c8cb7a0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("df162ee4-0c3b-4878-be2b-fb7d1f32a190"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("df99b660-0f74-4072-81d3-5ccdb8b6c941"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e05bc064-2c78-4dce-bb83-caa05e894ca0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e1231319-bdfd-492e-be42-8d54eda27a64"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e14c918d-0431-4b7d-9ff6-80ff3a1b9bec"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e1686ab9-a5a8-4390-9ff6-14c1efbfb798"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e2690a8c-bb7e-4af1-97af-4bdc2f0dc11b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e2ee1c2e-e2e2-419a-81cb-ff92b9c9195b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e31bfb18-ab34-43db-a4cd-bc17739f0073"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e323fd8f-a799-427c-a13f-228673d5d7c3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e34c466f-2932-4a76-bfd2-40f4a0aff536"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e3d45fa2-0eac-4953-bdcc-b3d24e39deb9"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e40c65ef-3658-493e-af56-031c223df3ad"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e45b067d-582b-4e49-815e-3eca574fcee0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4a73a28-4b50-4c18-9acc-1539ff6639ff"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e4bee9e3-62d7-4096-bd95-3fb9881ac02b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e5bc132a-d5f8-46cf-a462-7d149fe2c698"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e5dcf94a-08f2-4f2f-9355-fbff7b252a36"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e5deec6c-d0b4-4baa-adb8-5993e3df37b3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e6a5a23c-9801-4888-bfac-54d0fbdfd35f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e6e4da3a-a8b5-4dcf-a891-4b2c3b1c4d88"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e70fbe5d-3ae4-4871-9698-91c816aea508"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e72ad4e2-9596-4129-8ab5-46cc458fde31"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e7496dc0-9775-4ca4-beca-053b892659c5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e7a0e349-3f2d-4b3b-9858-de097e44bb8d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e7cf3ce6-4886-4a56-88a0-29eb6cb33b22"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e80d2f89-8c4c-4afe-b396-7709593f2e61"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e89acfc1-4629-4cb4-8cbd-96c514f826aa"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e8e82513-ed02-406c-94ff-bcb6abec9be5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("e911461f-ae4d-46f8-b82f-f534ef2ae717"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ea95acb3-aac2-4337-8121-59e25743c831"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eac92d42-c6d9-477f-9f35-b346da9fd8ba"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eb047e8f-6dcc-4ef4-b4d8-9ba117ab7aa4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eb06003c-f9de-4e67-92dc-4fee24df8b7a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eb139554-48bb-42a0-9fb1-4d5fc4b7dd20"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec66f868-c7d8-4309-8e13-7f686470236d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec6ac050-354f-40f5-870e-cb26cbd38dc8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec889fee-8818-4413-8617-699c7e3768c4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ec96cb4d-0a87-423c-851f-76fa7f64fde3"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecc10bd2-ad34-48ba-8bc3-8a40d44fa0a4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ecc9d7dc-847b-4a94-896c-596e9d9b8186"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ed858d8a-9235-450d-ab73-ed95c204208d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("edb98c9d-35cf-42fa-821c-e5aa29eb06e8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("edc36925-d91f-43d8-8986-f9a113ca3437"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ede76791-a5a8-41cb-8fc7-bf9d94c7a91f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ee17dc5f-37f3-46b4-b00b-938ea724739f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ee40823b-bb7e-4fbc-89d3-29101b3ff4d6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ee9eb2a1-b74f-4261-b3f8-3e5a47fb9a4e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("eef55547-7f41-4087-9f28-eeb5d059b98f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef21b2a7-8594-4272-883c-74094a99a543"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef4c44ac-4039-4bcc-9826-e5325a1a1007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ef95a7a6-c1c3-4d7e-839d-37d50edc9834"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f0429c4f-e6f4-4f60-813d-3fb03aebf219"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f113debb-cab9-44ad-b00e-b5e4018e1f5c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f134eeef-7a92-4274-9115-125cf9d5373f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f14d841d-7b32-41e6-8133-0d6d7603f40a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f155fbaa-f686-4517-92bd-4dc759374bdb"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f17c38a8-611b-4a4f-a672-d891ec5d123a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f1d436c5-826f-4891-acec-26dc7bbac7f8"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f21a1667-ab07-4d0a-ae8e-1a7b5513fa31"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f23db52c-5216-4c9d-ab46-7520823ef225"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f24a7845-057a-4b89-b83b-817822f8e4c6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2603bb4-f950-49fa-9706-e1c0a37ee75e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f2f39eea-7144-4030-9045-2a8c03f076e7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f3007f58-fd72-4809-b5ab-e6f36f257f07"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f3ede0ce-64e0-433d-9ac1-263a89620e6b"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f4291dc4-25d6-4c9e-b292-6cbf333d5393"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f4406a32-3e71-47b7-a307-373e83246d5d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f45f776f-1819-4b89-a33d-42aef2b68a5e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f51cfbac-61d2-41b6-9acf-c06f819d3ef1"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f522dac1-ebfd-41f5-b806-da3809cc4eb6"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f545fd0a-f082-4279-bd73-d5ea4c5c4ddf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f56c730c-7b84-4f92-b7da-c8f9aafb5481"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f56fd362-0129-4572-8472-fc212d7097e5"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f5a1c3e7-a35f-46e7-9571-a146012d4a33"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f5b4ca81-f3f8-43e4-be6f-e7480fbbf78e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f5bab168-1f30-42d2-a704-cfdd6a0a6aa7"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f6d11a24-c99c-491b-aa13-156010ff8f28"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f6f49553-2d9d-4807-88fb-8754e1b6b7bf"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f70f6090-7425-4023-bb93-8286eceec229"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f72519f8-49b9-4876-82e7-d46fc87e8818"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f75036e3-6dad-4313-9e9b-4e693cc1e88f"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7528494-9836-4455-9da9-351a053b529d"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f77b091e-68f1-4a39-8a44-723fb0291312"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f78928e3-4a97-4ad6-8e9c-0ee1d4e9c533"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7b1dd13-c8a9-4079-a522-ad4d414432a4"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7b2e0df-941b-4231-bf83-710047d2519a"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f7bd3c96-fb5c-4281-9bb4-5b631aca5290"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f8d1caa3-df11-43af-a593-77e2b72c440e"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f98de2d8-cb2e-4557-8ace-afe42869dee0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("f9fe2e06-3ad3-42a6-842c-9efac4bccad0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fa17f164-ca80-4ea1-a1a5-50044a94ba96"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fa367570-0059-4e5d-914b-ea736d74d929"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fa858ddc-40e4-430e-8cd8-a3b2ee09818c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fb4c4e21-b0a4-4fb1-aa46-378b38b4d769"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fbe8cd1d-3428-4f5c-89db-df03ab0be959"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fc9a846f-3894-4023-b0ce-0ab2a8a99909"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fcb5a35d-2c3c-4bc7-b054-9ccc2d0efd15"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fde06e4a-b0dd-45ae-bc86-c9e04f962809"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fec1e24a-187f-4304-b285-5c0259d4f607"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("fef0be3f-fbc9-4976-9d08-1dbd21fc6105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff086f41-276d-41ef-a442-5599aed24178"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff097399-f610-463b-8dbc-2d5ec3ff2a13"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff776cae-3f8f-4fe3-af3d-c286ccc7bc6c"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ff990c53-fa5c-4f77-b19f-36ada11518b0"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ffa63bfd-1e97-4f24-b806-b4014d2c3b25"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("ffe42576-2b31-4f7d-890b-78015bb52ccb"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("00652d95-e040-4a72-956c-c051678686da"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("015053bc-53a9-43fd-8040-c93ec3082027"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("044c1493-ccdd-466c-abd8-e846b4997b4c"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("04b6b898-14b6-49eb-8da9-9c33b47518f1"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("0632a654-0969-4475-9705-bc941c44a5cb"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("085bb3d9-590d-4b3f-b54b-63e330f71724"));

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
                keyValue: new Guid("11ab6eac-63bd-4ed0-9e65-35c35161a40c"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("13faeb36-32d0-4af9-a4b5-e42343cc30cb"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("14cb1789-2c19-46ee-bf9b-7de6405abfc3"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("17066bfe-932f-48cd-99fc-66870d6fbcce"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("1a2d0a0e-4ecd-4bff-9be0-acbc44ea9692"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("1b4f36c1-3537-41a7-a377-f1a86430c067"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("1ea40174-758f-4150-a59a-d5a4e887efc3"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("20ce70cf-8e12-487f-af5c-0d7157fa44f0"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("265c08b1-3950-4f4c-bc6e-b3f170140138"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("271226b4-5ba2-47f6-9805-9d5764857ed2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("28c7e4dd-ba88-4a46-bdbc-bf5e501d1606"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("2c0bf34b-4020-4d34-9a79-379ca9c8c5cf"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("2c32f4f4-cb8d-4bdb-b381-7b005fb37e19"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("2e91395a-2809-4238-8e62-30fde07b2a3f"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("31f6e8c0-f49a-4893-a92e-806b380e9ff4"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("32f8c19d-98b5-445e-82cc-be1fa3069168"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("37eb330a-6ebe-4e45-9a3a-933f38573721"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3a3f1cf5-cc71-4834-aa90-001b10f5b259"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3c20f87e-f2f6-4597-8d47-4cab68b4829a"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3e2de98a-e87a-40d2-91ee-fafab6cc4b38"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3e33c9db-7934-4a42-84a4-b31909855523"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("3e57c0bb-ece0-4f95-a6c3-d5e151a44f2f"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("47b6471b-f5dc-4096-af9f-ede33c57c208"));

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
                keyValue: new Guid("4ed2678f-ec8b-4852-b6df-742e0da2ee9f"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51a829cc-1c7e-4834-a367-c52de630e82e"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("538e4425-5ee4-4b96-ac7d-78a4520a025e"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("55973df6-0b3c-4a81-a885-8e5d4a715950"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("55e9d5f4-8381-48cd-83c9-6038a79cc23c"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("56ed181c-440f-45bd-990d-4bb38afd0f77"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("58241ce0-2323-41e0-b9b1-b54d9c8c37a3"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("60912bc7-31e2-435f-920c-15ca1465ffa5"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("641bde3c-9eca-4752-abaa-6049807018de"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("679c5252-7264-45d2-afa0-1aad756f01ae"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("68ed79c2-1813-4bba-8851-ae7d138b2a66"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("6938c75b-e0d2-4786-8c86-912124520624"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("69fc8936-351d-41fa-a8c8-f3e43d121ade"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("6da6771a-e409-4c6f-bee7-211dee7eb7f9"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("7cb18d28-c2f9-418d-848b-fef4d1a0d2e7"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("80069559-da26-4d37-8799-6927aed7e80c"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("825aaefe-e63e-4948-ae0b-4bf3203d5458"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8479df9d-74bf-412e-bd83-0f60b5192b4e"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("847ba81f-2ba5-4624-a0f9-425752367897"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8926a734-bda7-4776-b63e-9333bd54b2b2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8b28e9fc-6a76-4793-b91e-b7d7ec7d7768"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8c0b46b6-19c8-473e-8d2c-21556e441c4a"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8c755e64-880a-4b4e-9c37-d0af828e60bc"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8f85c1d8-4c71-4563-8959-4112eebbf8ce"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8f9ee6b2-0571-439a-9fd6-9ccf2745bd16"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("8fd41fa3-437c-4a26-8e43-4e52307fc999"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("94d64a3e-4dad-48eb-80d1-546d50d0ef04"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("9570ce55-84b2-48cd-b21e-e295e6c97a26"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("9a176302-bb75-4271-abd4-a4f726e3ea8b"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("9d5ed1e0-13ce-431a-9193-d1a6eecea5c4"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("9e107b80-6748-4cb4-8945-75febd25cca9"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("a76c23ab-feea-46f5-98ba-50702b513bb4"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("a8b083e4-6792-42f3-b1d5-fc2bc8350519"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("aa5b8792-608a-46ae-8e88-8d662b42c97a"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("af361a2e-9e7c-4adf-96b5-0cac283b0625"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b1177a06-5499-43e2-974e-865c3bfa6735"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b1d6a066-bf74-4820-8c49-c23568e5a389"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b2af8b19-ba71-424b-b3d6-56db071bb198"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b2fc0404-4059-4431-8816-92c314be5523"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b7c1cc26-b036-480f-9c1d-9404626fbe81"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("b9b52c68-a027-4de2-82db-ccbe3fb429a2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("c501f45b-9ae2-41d7-be18-96a9596872c7"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("c6f3d346-bb91-4635-81fa-9da7415fab4a"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("c8b6346b-088b-4e03-b64d-a5dbd311b713"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("c9c8425d-6a10-4ebc-99d4-4f7685522f29"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("cb751e0b-0179-41b4-af46-7ab0ae4bd723"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("df4586d9-9081-4c69-b520-25d3d293b7d1"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e140f9d3-d980-4801-ba40-1906302fbac0"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e1c962f4-bffa-4537-9dc6-27a2dd6c9ee5"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e31eb5c0-de81-4120-865e-0f5de1718514"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e49b4952-09a0-43e4-9bc2-59151c16d012"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("e57204b3-5f51-4e71-8505-1ff73ca02f20"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("eb51351f-6a77-4db8-8108-f1b8cea45c8c"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f00b2912-9f25-4010-9f41-61aad2fe4c34"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f08c2002-9d22-4184-a7d2-e5b6b2a15e6b"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f4150c0f-aa65-4c9e-9c06-6f01b65f9d0f"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f447041e-1d47-4aa9-8b53-50cde41ab9c2"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f6a08a0f-a040-49b6-997e-aa110c33b412"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("f8fb8f7a-0ac6-4203-ad78-603c48d20e94"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("fc0ff806-1930-4124-a6c8-a429c09881b5"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("fed0d5d6-33a2-4b83-9bf2-9160a83a5aa6"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("07df7a05-3f98-4cef-8603-2cf2a3438995"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("1117ecd9-76e7-4339-94b3-73dbe2578bd0"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("1a142ec5-3683-44c5-9407-b24366aa8de7"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("1c73597e-55be-454c-bb34-da4b10c3468f"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("1e2cf3fe-f2d1-46ec-891c-50dcec20ab2a"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("2adb1e68-6a26-4f1b-9fd3-6c1efb861823"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("2c6ed142-5550-4bda-9abd-9c8358a9fe0c"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("310e3e64-f69f-473c-8add-074fc3518340"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("34da5bed-b063-49c7-9ccb-a9e1fe6cc192"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("36adc1fc-39a0-4521-9da6-5186a494fe3e"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("3cda5b74-a9dc-440e-9456-a8181f543524"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("64a930d5-2e4b-4adb-88e0-e58b20e39831"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("67e083d7-7d8f-4f70-85ae-8875936491c5"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("686bd220-1f01-41d4-8f79-4ac7bccdfd07"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("75eb0275-36e8-472a-b410-e299f7e7a212"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("76108167-b161-4f11-b136-c9cb62a9cb1e"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("7e5ca9b1-a0de-4a9e-8a93-c99a99d3b082"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("8c7664a9-e777-4a3b-988f-c45d6e8e258c"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("8ff6870a-c524-4851-b934-3494d4343f3d"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("98448459-61c2-4ab7-9e77-94cd67601168"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("bdc7bde7-e8c3-4ac8-bd1d-7157e4c0f5c3"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("c72ada83-e32d-4619-aa0e-32361160427c"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("cd676d10-0eac-43f4-a570-489ff60e8792"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("d025c30e-3a8a-4b0a-af0b-11227845e246"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("d5177d0b-780d-4eb9-834b-5943201c163f"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("d59c073f-4b9e-4e9d-9a84-2f2b177f102e"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("d691830c-0215-42ef-8d86-ec6f36600c89"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("e2243c47-c668-4969-8826-a58e2489d4c6"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("e373e259-bcab-4fa8-a534-9da92f0d9c09"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("e958abd6-7756-48e5-9897-3ebef93aabc8"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("ea724fcf-2d16-4ec6-bdb3-ab3023495885"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("ec3b0223-47d6-42fc-883b-db3bf8b13fa6"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("f65ff99f-1c10-45f8-9293-303d956c7c06"));



        }
    }
}
