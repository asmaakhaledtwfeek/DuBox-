using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class continueChecklistSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Checklists",
                columns: new[] { "ChecklistId", "Code", "CreatedDate", "Discipline", "IsActive", "Name", "PageNumber", "ReferenceDocumentsJson", "SignatureRolesJson", "SubDiscipline", "WIRCode" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "Material-Verification", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Material Verification Inspection Checklist", 1, "[\"Material Approval\",\"Delivery Order\",\"MSDS\"]", "[\"Quality Dept.\",\"Operation Team\"]", "General", null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "ASA-IMS-FRM-13-081", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Construction of Precast Concrete Modular at Factory Checklist", 2, "[\"Project Specifications Section- 233113 \\u0026 230713\"]", "[\"Civil Works\",\"Sub Contractor\"]", "Precast", null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "Paint-Work", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Painting Checklist", 4, null, "[\"Civil Engineer\",\"QC Engineer\"]", "Painting", null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "ASA-IMS-FRM-13-049", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Ceramic Tiling Checklist", 5, null, "[\"Civil Works\",\"Sub Contractor\"]", "Tiling", null },
                    { new Guid("50000000-0000-0000-0000-000000000005"), "ASA-IMS-FRM-13-036", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Bitumen Paint Application Checklist", 6, null, "[\"Civil Works\",\"Sub Contractor\"]", "Waterproofing", null },
                    { new Guid("50000000-0000-0000-0000-000000000006"), "ASA-IMS-FRM-13-054", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Gypsum Partition Installation Checklist", 7, null, "[\"Civil Works\",\"Sub Contractor\"]", "Drywall", null },
                    { new Guid("50000000-0000-0000-0000-000000000007"), "ASA-IMS-FRM-13-052", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "False Ceiling Installation Checklist", 8, null, "[\"Civil Works\",\"Sub Contractor\"]", "False Ceiling", null },
                    { new Guid("50000000-0000-0000-0000-000000000008"), "ASA-IMS-FRM-13-046", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Wet Area Waterproofing Checklist", 9, null, "[\"Civil Works\",\"Sub Contractor\"]", "Waterproofing", null },
                    { new Guid("50000000-0000-0000-0000-000000000009"), "Epoxy-Work", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Epoxy Flooring Checklist", 10, null, "[\"Civil Engineer\",\"QC Engineer\"]", "Flooring", null },
                    { new Guid("50000000-0000-0000-0000-000000000010"), "Kitchen-Cabinet", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Kitchen Cabinets Installation Checklist", 11, null, "[\"Civil Engineer\",\"QC Engineer\"]", "Joinery", null },
                    { new Guid("50000000-0000-0000-0000-000000000011"), "Wardrobe-Installation", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Wardrobe Installation Checklist", 12, null, "[\"Civil Engineer\",\"QC Engineer\"]", "Joinery", null },
                    { new Guid("50000000-0000-0000-0000-000000000012"), "ASA-IMS-FRM-13-051", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Doors & Windows Installation Checklist", 13, null, "[\"Civil Works\",\"Sub Contractor\"]", "Doors & Windows", null },
                    { new Guid("50000000-0000-0000-0000-000000000013"), "ASA-IMS-FRM-13-069", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Fire Sealing Works Checklist", 15, null, "[\"Civil Works\",\"Sub Contractor\"]", "Fire Protection", null },
                    { new Guid("50000000-0000-0000-0000-000000000014"), "Aluminium-Glazing", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Aluminium & Glazing Checklist", 16, null, "[\"Civil Engineer\",\"QC Engineer\"]", "Aluminium Works", null },
                    { new Guid("50000000-0000-0000-0000-000000000015"), "Pre-Loading-Civil", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CIVIL", true, "Check List for Pre-Loading of Completed Precast Modular", 17, null, "[\"Civil Works\",\"QC\",\"MEP Works\"]", "Pre-Loading", null },
                    { new Guid("50000000-0000-0000-0000-000000000020"), "Material-Receiving-MEP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Material Receiving Inspection", 20, null, "[\"Amana\",\"Consultant\",\"Client\"]", "General", null },
                    { new Guid("50000000-0000-0000-0000-000000000021"), "AA-ITP-9004-ME-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of HVAC Duct", 21, "[\"Project Specifications Section- 233113 \\u0026 230713\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "HVAC", null },
                    { new Guid("50000000-0000-0000-0000-000000000022"), "AA-ITP-8501-DR-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of Above Ground Drainage Pipes", 22, "[\"Project Specification: Section- 221300\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Drainage", null },
                    { new Guid("50000000-0000-0000-0000-000000000023"), "AA-ITP-8501-DR-03", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Leak Test of Above Ground Drainage Pipes", 23, "[\"Project Specification: Section- 221300\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Drainage", null },
                    { new Guid("50000000-0000-0000-0000-000000000024"), "Test-Report-Drainage", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation and Testing of Above Ground Drainage Pipes and fittings - Test Report", 24, null, "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Drainage", null },
                    { new Guid("50000000-0000-0000-0000-000000000025"), "AA-ITP-8001-WS-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation Above ground Water Supply pipes and fittings", 25, "[\"Project Specification: Section- 221116\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Water Supply", null },
                    { new Guid("50000000-0000-0000-0000-000000000026"), "AA-ITP-8001-WS-03", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Testing of Above ground Water Supply pipes and fittings", 26, "[\"Project Specification: Section- 221116\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Water Supply", null },
                    { new Guid("50000000-0000-0000-0000-000000000027"), "Test-Report-Water-Supply", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation and Testing of Above Ground Water Supply Pipes and fittings - Test Report", 27, null, "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Water Supply", null },
                    { new Guid("50000000-0000-0000-0000-000000000028"), "AA-ITP-6001-FP-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of Above Ground Fire Fighting pipes system", 28, "[\"Project Specification: Section- 211100\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Fire Fighting", null },
                    { new Guid("50000000-0000-0000-0000-000000000029"), "AA-ITP-6001-FP-03", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Testing of Above Ground Fire Fighting pipes and fittings", 29, "[\"Project Specification: Section- 211100\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Fire Fighting", null },
                    { new Guid("50000000-0000-0000-0000-000000000030"), "Test-Report-Fire-Fighting", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation and Testing of Above ground Fire Fighting Pipes and fittings - Test Report", 30, null, "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Fire Fighting", null },
                    { new Guid("50000000-0000-0000-0000-000000000031"), "AA-ITP-9003-ME-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of Refrigerant Pipe", 31, "[\"Project Specifications Section- 232300\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "HVAC", null },
                    { new Guid("50000000-0000-0000-0000-000000000032"), "AA-ITP-9003-ME-03", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Pressure Testing of Refrigerant Pipe", 32, "[\"Project Specifications Section- 232300\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "HVAC", null },
                    { new Guid("50000000-0000-0000-0000-000000000033"), "Test-Report-Refrigerant", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation and Testing of Refrigerant Pipes and fittings - Test Report", 33, null, "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "HVAC", null },
                    { new Guid("50000000-0000-0000-0000-000000000034"), "AA-ITP-5005-EL-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of LV Cables & Wires", 34, null, "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Electrical", null },
                    { new Guid("50000000-0000-0000-0000-000000000035"), "AA-ITP-5005-EL-03", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Testing of LV Cables & Wires", 35, "[\"Project Specification: Section- 260513\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Electrical", null },
                    { new Guid("50000000-0000-0000-0000-000000000036"), "Electrical-Test-Results", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Electrical Systems - Circuit Insulation Resistance Test", 36, null, "[\"Amana Operation\",\"Amana QC\",\"Employer / Consultant\"]", "Electrical", null },
                    { new Guid("50000000-0000-0000-0000-000000000040"), "AA-ITP-5006-EL-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of LV Panels", 40, "[\"Project Specification: Section- 262416\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Electrical", null },
                    { new Guid("50000000-0000-0000-0000-000000000041"), "AA-ITP-5004-EL-02", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Installation of Conduits & accessories", 41, "[\"Project Specification: Section- 260533\"]", "[\"QC Inspector\",\"PMC\",\"AMAALA Representative\"]", "Electrical", null },
                    { new Guid("50000000-0000-0000-0000-000000000042"), "Pre-Loading-MEP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP", true, "Check List for Pre-Loading of Completed Precast Modular (MEP)", 42, null, "[\"Civil Works\",\"QC\",\"MEP Works\"]", "Pre-Loading", null }
                });

            migrationBuilder.InsertData(
                table: "ChecklistSections",
                columns: new[] { "ChecklistSectionId", "ChecklistId", "CreatedDate", "IsActive", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("51000000-0000-0000-0000-000000000001"), new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Verification Items" },
                    { new Guid("51000000-0000-0000-0000-000000000002"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000003"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Preparation & Setting Out" },
                    { new Guid("51000000-0000-0000-0000-000000000004"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Erection / Assembling of Precast Element - External Walls" },
                    { new Guid("51000000-0000-0000-0000-000000000005"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "C. Erection / Assembling - Floor Slab(s)" },
                    { new Guid("51000000-0000-0000-0000-000000000006"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, "C. Erection / Assembling - Internal Partition" },
                    { new Guid("51000000-0000-0000-0000-000000000007"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, "C. Erection / Assembling - Top Slabs" },
                    { new Guid("51000000-0000-0000-0000-000000000008"), new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, "Civil Works" },
                    { new Guid("51000000-0000-0000-0000-000000000009"), new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000010"), new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000011"), new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Application of Internal Paint" },
                    { new Guid("51000000-0000-0000-0000-000000000012"), new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "D. Application of External Paint" },
                    { new Guid("51000000-0000-0000-0000-000000000013"), new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000014"), new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000015"), new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Installation of Tile" },
                    { new Guid("51000000-0000-0000-0000-000000000016"), new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "D. Grouting of Tile" },
                    { new Guid("51000000-0000-0000-0000-000000000017"), new Guid("50000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000018"), new Guid("50000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000019"), new Guid("50000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Application of Bitumen Paint" },
                    { new Guid("51000000-0000-0000-0000-000000000020"), new Guid("50000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000021"), new Guid("50000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Setting Out" },
                    { new Guid("51000000-0000-0000-0000-000000000022"), new Guid("50000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Installation Activity" },
                    { new Guid("51000000-0000-0000-0000-000000000023"), new Guid("50000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000024"), new Guid("50000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Setting Out" },
                    { new Guid("51000000-0000-0000-0000-000000000025"), new Guid("50000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Pre-Installation Activity" },
                    { new Guid("51000000-0000-0000-0000-000000000026"), new Guid("50000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "D. Installation of False Ceiling" },
                    { new Guid("51000000-0000-0000-0000-000000000027"), new Guid("50000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000028"), new Guid("50000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000029"), new Guid("50000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Application of Waterproofing" },
                    { new Guid("51000000-0000-0000-0000-000000000030"), new Guid("50000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000031"), new Guid("50000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000032"), new Guid("50000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Application of Epoxy" },
                    { new Guid("51000000-0000-0000-0000-000000000033"), new Guid("50000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000034"), new Guid("50000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000035"), new Guid("50000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Cabinet Counter Top" },
                    { new Guid("51000000-0000-0000-0000-000000000036"), new Guid("50000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000037"), new Guid("50000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Setting Out" },
                    { new Guid("51000000-0000-0000-0000-000000000038"), new Guid("50000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Installation of Wardrobe" },
                    { new Guid("51000000-0000-0000-0000-000000000039"), new Guid("50000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000040"), new Guid("50000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Setting Out" },
                    { new Guid("51000000-0000-0000-0000-000000000041"), new Guid("50000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Installation of Doors" },
                    { new Guid("51000000-0000-0000-0000-000000000042"), new Guid("50000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "D. Installation of Windows" },
                    { new Guid("51000000-0000-0000-0000-000000000043"), new Guid("50000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000044"), new Guid("50000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Preparation Works" },
                    { new Guid("51000000-0000-0000-0000-000000000045"), new Guid("50000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Application" },
                    { new Guid("51000000-0000-0000-0000-000000000046"), new Guid("50000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. General" },
                    { new Guid("51000000-0000-0000-0000-000000000047"), new Guid("50000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "B. Surface Preparation" },
                    { new Guid("51000000-0000-0000-0000-000000000048"), new Guid("50000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "C. Installation of Aluminium and glazing" },
                    { new Guid("51000000-0000-0000-0000-000000000049"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "1. General" },
                    { new Guid("51000000-0000-0000-0000-000000000050"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "2. Final Inspection - Structural" },
                    { new Guid("51000000-0000-0000-0000-000000000051"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "3. Internal and External Painting" },
                    { new Guid("51000000-0000-0000-0000-000000000052"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "4. Floor and Wall Tiling" },
                    { new Guid("51000000-0000-0000-0000-000000000053"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, "5. Dry wall" },
                    { new Guid("51000000-0000-0000-0000-000000000054"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, "6. False Ceiling Work" },
                    { new Guid("51000000-0000-0000-0000-000000000055"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, "7. Aluminium and Glazing Works" },
                    { new Guid("51000000-0000-0000-0000-000000000056"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, "8. Wooden/Metal Doors and Wood Works" },
                    { new Guid("51000000-0000-0000-0000-000000000057"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 9, "9. Other Finishes" },
                    { new Guid("51000000-0000-0000-0000-000000000058"), new Guid("50000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 10, "10. Others" },
                    { new Guid("51000000-0000-0000-0000-000000000059"), new Guid("50000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Inspection Items" },
                    { new Guid("51000000-0000-0000-0000-000000000060"), new Guid("50000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of HVAC Duct" },
                    { new Guid("51000000-0000-0000-0000-000000000061"), new Guid("50000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of Above Ground Drainage Pipes" },
                    { new Guid("51000000-0000-0000-0000-000000000062"), new Guid("50000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Leak Test of Above Ground Drainage Pipes" },
                    { new Guid("51000000-0000-0000-0000-000000000063"), new Guid("50000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Test Parameters" },
                    { new Guid("51000000-0000-0000-0000-000000000064"), new Guid("50000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of Above ground Water Supply pipes and fittings" },
                    { new Guid("51000000-0000-0000-0000-000000000065"), new Guid("50000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Testing of Above ground Water Supply pipes and fittings" },
                    { new Guid("51000000-0000-0000-0000-000000000066"), new Guid("50000000-0000-0000-0000-000000000027"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Test Parameters" },
                    { new Guid("51000000-0000-0000-0000-000000000067"), new Guid("50000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of Above Ground Fire Fighting pipes system" },
                    { new Guid("51000000-0000-0000-0000-000000000068"), new Guid("50000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Testing of Above Ground Fire Fighting pipes and fittings" },
                    { new Guid("51000000-0000-0000-0000-000000000069"), new Guid("50000000-0000-0000-0000-000000000030"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Test Parameters" },
                    { new Guid("51000000-0000-0000-0000-000000000070"), new Guid("50000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of Refrigerant Pipe" },
                    { new Guid("51000000-0000-0000-0000-000000000071"), new Guid("50000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Pressure Testing of Refrigerant Pipe" },
                    { new Guid("51000000-0000-0000-0000-000000000072"), new Guid("50000000-0000-0000-0000-000000000033"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Test Parameters" },
                    { new Guid("51000000-0000-0000-0000-000000000073"), new Guid("50000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of LV Cables & Wires" },
                    { new Guid("51000000-0000-0000-0000-000000000074"), new Guid("50000000-0000-0000-0000-000000000035"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Testing of LV Cables & Wires" },
                    { new Guid("51000000-0000-0000-0000-000000000075"), new Guid("50000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "Type A - Circuit Testing" },
                    { new Guid("51000000-0000-0000-0000-000000000076"), new Guid("50000000-0000-0000-0000-000000000040"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of LV Panels" },
                    { new Guid("51000000-0000-0000-0000-000000000077"), new Guid("50000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "A. Installation of Conduits & accessories" },
                    { new Guid("51000000-0000-0000-0000-000000000078"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, "General" },
                    { new Guid("51000000-0000-0000-0000-000000000079"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, "Final Inspection - HVAC" },
                    { new Guid("51000000-0000-0000-0000-000000000080"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, "Final Inspection - Refrigerant pipes" },
                    { new Guid("51000000-0000-0000-0000-000000000081"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 4, "Final Inspection - Firefighting" },
                    { new Guid("51000000-0000-0000-0000-000000000082"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, "Final Inspection - Water Supply" },
                    { new Guid("51000000-0000-0000-0000-000000000083"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, "Final Inspection - Drainage" },
                    { new Guid("51000000-0000-0000-0000-000000000084"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, "Final Inspection - Risers" },
                    { new Guid("51000000-0000-0000-0000-000000000085"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, "Final Inspection - Electrical" },
                    { new Guid("51000000-0000-0000-0000-000000000086"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 9, "Final Inspection - Wiring Devices" },
                    { new Guid("51000000-0000-0000-0000-000000000087"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 10, "Final Inspection - Wire, Cables, Conduits and accessories" },
                    { new Guid("51000000-0000-0000-0000-000000000088"), new Guid("50000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 11, "Final Inspection - Light Fittings" }
                });

            migrationBuilder.InsertData(
                table: "PredefinedChecklistItems",
                columns: new[] { "PredefinedItemId", "ChecklistSectionId", "CreatedDate", "Description", "IsActive", "Reference", "Sequence" },
                values: new object[,]
                {
                    { new Guid("52000000-0000-0000-0000-000000000001"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is there material approval for received item?", true, "MA", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000002"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is Manufacturer Name as per material approval?", true, "MA", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000003"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is Supplier Name as per material approval?", true, "MA", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000004"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is received material matching with approved sample?", true, "Approved Sample", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000005"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Related mill test certificate (or) test reports?", true, "MTC", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000006"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Expiry date", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000007"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Item / product description", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000008"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Item / product code", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000009"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Dimensions (length, width, thickness etc.)", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000010"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Colour", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000011"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for any defects", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000012"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for Packaging Conditions", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000013"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for received Quantity (approx) as per DO", true, "DO", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000014"), new Guid("51000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the area of storage as per MSDS", true, "MSDS", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000015"), new Guid("51000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and shop drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000016"), new Guid("51000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000017"), new Guid("51000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000018"), new Guid("51000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Drawing Stamp & Signature", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000019"), new Guid("51000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Element Tag, QC Approval for Element", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000020"), new Guid("51000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Floor Setting Out", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000021"), new Guid("51000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Erection with temporary support.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000022"), new Guid("51000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panel to panel connections.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000023"), new Guid("51000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dimensions (outer, inner and diagonal), line and level.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000024"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Backer Rod / Shim Pad", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000025"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slab Position / Level", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000026"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panel to Slab Connections", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000027"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1000mm FFL to be marked clearly.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000028"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP Clearance", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000029"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Slab level (shall read 1016mm at 1000m FFL line)", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000030"), new Guid("51000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grouting", true, "Approved Drawing", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000031"), new Guid("51000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Erection with Temporary Support", true, "Approved Drawing", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000032"), new Guid("51000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panel to Panel Connections", true, "Approved Drawing", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000033"), new Guid("51000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dimensions (outer, inner and diagonal), Line and Level", true, "Approved Drawing", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000034"), new Guid("51000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grouting", true, "Approved Drawing", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000035"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Backer Rod / Shim Pad", true, "Approved Drawing", 21 },
                    { new Guid("52000000-0000-0000-0000-000000000036"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slab Position / Top Height", true, "Approved Drawing", 22 },
                    { new Guid("52000000-0000-0000-0000-000000000037"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panel to Slab Connections", true, "Approved Drawing", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000038"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MEP Clearance", true, "Approved Drawing", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000039"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grouting", true, "Approved Drawing", 25 },
                    { new Guid("52000000-0000-0000-0000-000000000040"), new Guid("51000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Curing", true, "Approved Drawing", 26 },
                    { new Guid("52000000-0000-0000-0000-000000000041"), new Guid("51000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Internal and External Dimension of Box", true, "Approved Drawing", 27 },
                    { new Guid("52000000-0000-0000-0000-000000000042"), new Guid("51000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area.", true, "General", 28 },
                    { new Guid("52000000-0000-0000-0000-000000000043"), new Guid("51000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000044"), new Guid("51000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored as per manufacturers recommendations.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000045"), new Guid("51000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the expiry date and number of coats of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000046"), new Guid("51000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000047"), new Guid("51000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000048"), new Guid("51000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for repair of surface imperfection and protrusions (if any).", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000049"), new Guid("51000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000050"), new Guid("51000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Undulations or irregular corners are repaired and grinded as required.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000051"), new Guid("51000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of Primer as per manufacturers recommendation", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000052"), new Guid("51000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of Stuccoo as per manufecturers recommendation", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000053"), new Guid("51000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000054"), new Guid("51000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of first coat of Paint as per manufacturers recommendation.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000055"), new Guid("51000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Line between two color shades is straight, no Brush marks should be visible.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000056"), new Guid("51000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of Primer as per manufacturers recommendation", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000057"), new Guid("51000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of Filler Coats as per manufacturers recommendation", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000058"), new Guid("51000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000059"), new Guid("51000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of final coat of Texture Paint as per manufacturers recommendation.", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000060"), new Guid("51000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000061"), new Guid("51000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored as per manufacturers recommendations.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000062"), new Guid("51000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000063"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000064"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check surface is levelled and sloped as per the approved drawings.", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000065"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the surface roughness is appropriate as per the tile and tile adhesive manufacturer recommendations.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000066"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the application of wet area water proofing and leak test.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000067"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the location and level of floor drain as per the approved drawings.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000068"), new Guid("51000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the MEP clearance prior to start Painting works.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000069"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure proper mixing of material as per the manufacturer recommendations.", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000070"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal.", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000071"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of tile adhesive using notch trowel on the backside of tile and over the substrate.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000072"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the tile spacers width is uniform and aligned as per the appoved drawings.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000073"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the setting out / pattern of wall and floor tiles as per the approved drawings.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000074"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the slope and level of the tiles as per the approved drawings.", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000075"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the laying of tiles above the false ceiling as per the approved drawings.", true, "Approved Drawing", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000076"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the pull off / adhesion test for the paint application as per the project specifications / manufacturer recommendations (if required).", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000077"), new Guid("51000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval to proceed with further works obtained from consultant / Client.", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000078"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the tile joints are clean and free from contaminants like dust etc.", true, "General", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000079"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure proper mixing of material as per the manufacturer recommendations.", true, "General", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000080"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Location, Colour & Type of grout as per the approved shop drawings / material submittal.", true, "Approved Drawing", 21 },
                    { new Guid("52000000-0000-0000-0000-000000000081"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the width of tile grout is proper and uniform.", true, "General", 22 },
                    { new Guid("52000000-0000-0000-0000-000000000082"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of tile grout as per the manufacturer recommendations.", true, "General", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000083"), new Guid("51000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval to proceed with further works obtained from consultant / Client.", true, "General", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000084"), new Guid("51000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement and materials are approved.", true, "Approved MTS", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000085"), new Guid("51000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000086"), new Guid("51000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000087"), new Guid("51000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000088"), new Guid("51000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for repair of surface imperfection and protrusions (if any).", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000089"), new Guid("51000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the angle fillets and chamfering of all sharp edges (if required).", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000090"), new Guid("51000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the MEP clearance are obtained prior to start bitumen application.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000091"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure proper mixing as per the manufacturer recommandations.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000092"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of primer coat (if required).", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000093"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the rate of application as per the manufacturer recommendation and method statement.", true, "Approved MTS", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000094"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of coats as per project requirements / manufacturer recommandations.", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000095"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of subsequent coats are carried out at right angle to the previous coat.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000096"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the WFT as per the project specifications / manufacturer recommandations.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000097"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the curing of application as per manufacturer recommendation.", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000098"), new Guid("51000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activities.", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000099"), new Guid("51000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, material submittal and drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000100"), new Guid("51000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000101"), new Guid("51000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000102"), new Guid("51000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify and record the DCL product confirmity certificate for the insulation materials.", true, "DCL Certificate", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000103"), new Guid("51000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the marking and setting out of the partition walls as per the approved drawings.", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000104"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the completion of the required finishes of the adjacent substrates.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000105"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000106"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the fixation of the board (on one side of the supports) as per the approved drawings.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000107"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the completion of all embedded MEP and other dicipline works prior to closure as per the approved drawings.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000108"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure additional supports are provided for the wall mounted fixtures as applicable.", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000109"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the installation of insulation works (if applicable) as per the approved drawings.", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000110"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000111"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the fixation of the board as per the approved drawings.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000112"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000113"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000114"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the cutting of gypsum board on the marked locations as per the project requirements.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000115"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the jointing & taping as per the manufacturer recommendations.", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000116"), new Guid("51000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtain from Consultant/Client to proceed with further activities.", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000117"), new Guid("51000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, material submittal and drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000118"), new Guid("51000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials (Gypsum board, tiles, suspension systems, etc.,) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000119"), new Guid("51000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the color, type, material, pattern and thickness are as per approved material approval and project requirements.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000120"), new Guid("51000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the marking of the false ceiling level on the walls as per the approved drawings.", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000121"), new Guid("51000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the marking / location of suspension system supports at ceiling as per the approved drawings.", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000122"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the completion of the required finishes above the false ceiling level.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000123"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the closing of shaft openings, MEP penetrations and other openings as per project requirements.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000124"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the application of fire sealant / fire protection works are carried out as per the project requirements (if applicable).", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000125"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the location, spacing and fixation of the grid (main channel, Furing channel, wall angle, hanging wire with adjustable clip, main tee, cross tee, etc.,) as per the approved drawings.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000126"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the suspension system supports are not in contact with the adjacent MEP services.", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000127"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000128"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure additional supports are provided for the ceiling mounted fixtures as applicable.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000129"), new Guid("51000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtain approval from consultant / Client to proceed with further works.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000130"), new Guid("51000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the type, fixation, level and alignment of the false ceiling board / tiles as per the approved drawings.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000131"), new Guid("51000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the marking, position and alignment of MEP & ceiling mounted fixtures (light, access panel, sprinklers, signages etc.) in the ceiling as per the approved drawings.", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000132"), new Guid("51000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the cutting of gypsum board / tiles on the marked locations as per the project requirements.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000133"), new Guid("51000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the jointing & taping as per the manufacturer recommendations.", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000134"), new Guid("51000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activites.", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000135"), new Guid("51000000-0000-0000-0000-000000000027"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement and materials are approved.", true, "Approved MTS", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000136"), new Guid("51000000-0000-0000-0000-000000000027"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000137"), new Guid("51000000-0000-0000-0000-000000000027"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000138"), new Guid("51000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000139"), new Guid("51000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure grouting, angle fillet provided all around the penetrations and the projected MEP services are neat and clean from any latiance.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000140"), new Guid("51000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the angle fillets are provided at the floor and wall junctions of the area that to be treated.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000141"), new Guid("51000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtain MEP clearance prior to start the wet area water proofing.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000142"), new Guid("51000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure check dam constructed at the entrance of the room for the stagnation of water for water leakage testing after the completion of the wet area water proofing.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000143"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure proper mixing of the material as per the manufacturer recommendations", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000144"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of primer coat (if required).", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000145"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the rate of application as per the manufacturer recommendation and method statement.", true, "Approved MTS", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000146"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of coats as per project requirements / manufacturer recommandations.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000147"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of subsequent coats are carried out at right angle to the previous coat.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000148"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the application of waterproofing on the vertical face extended upto 300mm from FFL or as per approved drawing / project requirements.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000149"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the WFT as per the project specifications / manufacturer recommendations.", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000150"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the curing of application as per manufacturer recommendation.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000151"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtain approval for the application of waterproofing to proceed water leakage test.", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000152"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the filling of water and monitor the level of filled water during the leakage test.", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000153"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for any water seepage / leakage after 24 hours or as per the project requirements.", true, "General", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000154"), new Guid("51000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activities.", true, "General", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000155"), new Guid("51000000-0000-0000-0000-000000000030"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000156"), new Guid("51000000-0000-0000-0000-000000000030"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored as per manufacturers recommendations.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000157"), new Guid("51000000-0000-0000-0000-000000000030"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000158"), new Guid("51000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000159"), new Guid("51000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for repair of surface imperfection and protrusions (if any).", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000160"), new Guid("51000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000161"), new Guid("51000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Undulations or irregular corners are repaired and grinded as required.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000162"), new Guid("51000000-0000-0000-0000-000000000031"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the MEP clearance prior to start epoxy flooring works.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000163"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of primer as per manufecturers recommendation", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000164"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure application of Hardner as per manufecturers recommendation", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000165"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Application of uniform base as per manufecturers recommendations.", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000166"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tuchup, grinding, undulations, corner repairs and pinholes are filled properly.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000167"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure proper mixing of material as per the manufacturer recommendations.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000168"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000169"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application of coats as per project requirements / manufacturer recommendations.", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000170"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the rate of application as per the manufacturer recommendation and method statement.", true, "Approved MTS", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000171"), new Guid("51000000-0000-0000-0000-000000000032"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the curing at every stages of application as per manufacturer recommendation.", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000172"), new Guid("51000000-0000-0000-0000-000000000033"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000173"), new Guid("51000000-0000-0000-0000-000000000033"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored as per manufacturers recommendations.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000174"), new Guid("51000000-0000-0000-0000-000000000033"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method of statement is being followed.", true, "Approved MTS", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000175"), new Guid("51000000-0000-0000-0000-000000000033"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check location,Size,Color and Thickness of the Cabinets", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000176"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Height,Level and Alignment of cabinets", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000177"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check wethere Drawer and Shelf functioning properly", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000178"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Opening Size and Direction of cabinets", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000179"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Shelf Supports (Brackets or Pins)", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000180"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Joints of the Cabinets", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000181"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fixing of supports for the countertop is as per approved drawings", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000182"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Ironmongery is installed as per drawing and free from damanges", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000183"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Sink installed Properly and as per location on dwg.", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000184"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Level of the surface", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000185"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Applied Sealant and its Color", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000186"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Stove installed Properly", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000187"), new Guid("51000000-0000-0000-0000-000000000034"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Completion of Nearby Finishes", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000188"), new Guid("51000000-0000-0000-0000-000000000035"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for any Surface joints visibility", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000189"), new Guid("51000000-0000-0000-0000-000000000035"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check location,Size,Color and Thickness", true, "Approved Drawing", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000190"), new Guid("51000000-0000-0000-0000-000000000035"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Opening Size and location for kitchen Accessories", true, "Approved Drawing", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000191"), new Guid("51000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, material submittal and drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000192"), new Guid("51000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials (wardrobe, leaf, drawers , iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000193"), new Guid("51000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the color, type, material, coating of leaf, materials are as per approved material approval and project requirements.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000194"), new Guid("51000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify and record the DCL product conformity certificate for the sealants to be used.", true, "DCL Certificate", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000195"), new Guid("51000000-0000-0000-0000-000000000036"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the fire rating of the wardrobe are as per the project requirements.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000196"), new Guid("51000000-0000-0000-0000-000000000037"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the level and alignment of the wardrobe frame leaf opening with reference to the surrounding wall / cladding elevations.", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000197"), new Guid("51000000-0000-0000-0000-000000000037"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the location and clear opening of leafs and drawers are as per the approved drawings.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000198"), new Guid("51000000-0000-0000-0000-000000000037"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the wardrobe jamb area are solid and rigid.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000199"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000200"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verity the fixation, level and alignment of the wardrobe is as per the approved details / drawings.", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000201"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the wardrobe consolidated.", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000202"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the location and No. of leaf hinges provided as per the approved drawings.", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000203"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure required Iron mongery sets are provided as per drawings.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000204"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000205"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure location/dimensions of drawers are per drawing", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000206"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the roller slides/drawer runners as per the approved drawings.", true, "Approved Drawing", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000207"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify Cloth Hangers/Tie Holders are installed as per the approved drawings.", true, "Approved Drawing", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000208"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the alignment, plumbness and protection of wardrobe to avoid any damages during activities.", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000209"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure wardrobe hanging rail set is installed.", true, "General", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000210"), new Guid("51000000-0000-0000-0000-000000000038"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activities.", true, "General", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000211"), new Guid("51000000-0000-0000-0000-000000000039"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, material submittal and drawings are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000212"), new Guid("51000000-0000-0000-0000-000000000039"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials (Door frame, leaf, window frame, window leaf, iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000213"), new Guid("51000000-0000-0000-0000-000000000039"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the color, type, material, coating of door and window materials are as per approved material approval and project requirements.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000214"), new Guid("51000000-0000-0000-0000-000000000039"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify and record the DCL product confirmity certificate for the sealants to be used.", true, "DCL Certificate", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000215"), new Guid("51000000-0000-0000-0000-000000000039"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the fire rating of the doors are as per the project requirements.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000216"), new Guid("51000000-0000-0000-0000-000000000040"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the location and clear opening of doors / windows are as per the approved drawings.", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000217"), new Guid("51000000-0000-0000-0000-000000000040"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the level and alignment of the door / window frame opening with reference to the surrounding wall / cladding elevations.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000218"), new Guid("51000000-0000-0000-0000-000000000040"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the door / window jamb area are solid and rigid.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000219"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000220"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000221"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the door frames are consolidated using foam as per the material approval and project requirements.", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000222"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the location and No. of Door hinges provided as per the approved drawings.", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000223"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure required Iron mongery sets are provided as per the door schedule drawings.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000224"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000225"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the door stopper / door coordinator provided as per the approved door schedule.", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000226"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the fire rated doors are provided with fire rated sealant & fire rating tags as per the project requirements.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000227"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the rubber gaskets are provided at the door jamb as per the approved drawings.", true, "Approved Drawing", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000228"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the undercut for the door leaves as per the approved drawings.", true, "Approved Drawing", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000229"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the alignment, plumbness and protection of door leafs to avoid any damages during construction activities.", true, "General", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000230"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the door architrave are fixed as per the approved drawings.", true, "Approved Drawing", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000231"), new Guid("51000000-0000-0000-0000-000000000041"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activities.", true, "General", 21 },
                    { new Guid("52000000-0000-0000-0000-000000000232"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the completion of finishes around the jamb area prior to installation.", true, "General", 22 },
                    { new Guid("52000000-0000-0000-0000-000000000233"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the drip flashings are provided properly around the frame as per the approved drawings", true, "Approved Drawing", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000234"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the overlapping of the flashings around the frame are as per the approved drawings.", true, "Approved Drawing", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000235"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", true, "Approved Drawing", 25 },
                    { new Guid("52000000-0000-0000-0000-000000000236"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the location and No. of hinges, iron mongeries provided as per the approved drawings.", true, "Approved Drawing", 26 },
                    { new Guid("52000000-0000-0000-0000-000000000237"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the alignment, plumbness, opening direction and protection of window shutters to avoid any damages during construction activities.", true, "General", 27 },
                    { new Guid("52000000-0000-0000-0000-000000000238"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the uniform thickness, colour and application of the sealant all around the window frame.", true, "General", 28 },
                    { new Guid("52000000-0000-0000-0000-000000000239"), new Guid("51000000-0000-0000-0000-000000000042"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approval obtained from Consultant/Client to proceed with further activities.", true, "General", 29 },
                    { new Guid("52000000-0000-0000-0000-000000000240"), new Guid("51000000-0000-0000-0000-000000000043"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure relevant shop drawing is approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000241"), new Guid("51000000-0000-0000-0000-000000000043"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure H&S method statement is submitted", true, "Approved MTS", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000242"), new Guid("51000000-0000-0000-0000-000000000043"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check all the materials are approved by consultant.", true, "MA", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000243"), new Guid("51000000-0000-0000-0000-000000000043"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the availablity mockup approval for the fire sealing works (around penetrations, partitions, etc.).", true, "Mockup Approval", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000244"), new Guid("51000000-0000-0000-0000-000000000044"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the MEP penetrations are installed, inspected and approved.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000245"), new Guid("51000000-0000-0000-0000-000000000044"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the surface for any latiance, dirt, loose materials around the penetrations.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000246"), new Guid("51000000-0000-0000-0000-000000000044"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the dampness of the substrate prior to application (if required as per manufaturer recommendations)", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000247"), new Guid("51000000-0000-0000-0000-000000000044"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the mixing of materials as per the manufacturer recommandations", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000248"), new Guid("51000000-0000-0000-0000-000000000045"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the application covers the whole penetrations without any gap.", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000249"), new Guid("51000000-0000-0000-0000-000000000045"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finishing of the openings shall be as per the approved mockup.", true, "Mockup Approval", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000250"), new Guid("51000000-0000-0000-0000-000000000045"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the final cleaning and sealant (if required) applied as per the approved mockup.", true, "Mockup Approval", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000251"), new Guid("51000000-0000-0000-0000-000000000046"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, materials and drawings (finishing schedule) are approved.", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000252"), new Guid("51000000-0000-0000-0000-000000000046"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure materials are stored as per manufacturers recommendations.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000253"), new Guid("51000000-0000-0000-0000-000000000046"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the expiry date of the material prior to applications.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000254"), new Guid("51000000-0000-0000-0000-000000000047"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000255"), new Guid("51000000-0000-0000-0000-000000000047"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for repair of surface imperfection and protrusions (if any).", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000256"), new Guid("51000000-0000-0000-0000-000000000047"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the protection of nearby finishes / MEP services.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000257"), new Guid("51000000-0000-0000-0000-000000000047"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the MEP clearance prior to start of aluminium works.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000258"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Profile and glass country of origin / Manufacturer", true, "MA", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000259"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Type, Size, Colour, Thickness and Opening Direction", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000260"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Varify Size of opening as per drawing", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000261"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Varify Location of opening for curtain wall, door and windows as per drawing", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000262"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ironmongery is installed and free from damages", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000263"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sealant is applied properly", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000264"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "water leak test is performed and no leakage is found", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000265"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "varify Functioning/movement of panels is as free as required", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000266"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Varify the Rigidity of frame and moveable panels", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000267"), new Guid("51000000-0000-0000-0000-000000000048"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Varify Line and Level", true, "General", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000268"), new Guid("51000000-0000-0000-0000-000000000049"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure method statement, ITP, materials and shop drawings are approved", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000269"), new Guid("51000000-0000-0000-0000-000000000049"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check identification tag of the modular", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000270"), new Guid("51000000-0000-0000-0000-000000000049"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Visually inspect the modular for any defects or damages", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000271"), new Guid("51000000-0000-0000-0000-000000000049"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the method of loading as per the project / design requirements", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000272"), new Guid("51000000-0000-0000-0000-000000000050"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Internal and External Dimensions of the modular", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000273"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Location and color of Painting as per the App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000274"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Internal Paint (Application of Primer, Stucco and 1st Coat of Paint)", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000275"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "External Paint(Application of Primer, Filler and Final Coat Texture Paint)", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000276"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Paint touch ups are completed around installed items.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000277"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bitumin Applied at required Areas", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000278"), new Guid("51000000-0000-0000-0000-000000000051"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, If any", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000279"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Layout and Fixing of Tiles as per App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000280"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Line, Level and Spacer for the Installed Tiles", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000281"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Skirting is installed/fixed properly and truly vertical", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000282"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grouting of all Joints is done properly", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000283"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elastomeric sealant under skirting is provided properly", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000284"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cleaning of corners and edges removing exccessive paint on skirting", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000285"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Drainhole are free from any debris and properly closed (if applicable)", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000286"), new Guid("51000000-0000-0000-0000-000000000052"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, if any", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000287"), new Guid("51000000-0000-0000-0000-000000000053"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Layout, location and position of dry wall is as per App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000288"), new Guid("51000000-0000-0000-0000-000000000053"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thickness of Dry wall is as per App Drawing", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000289"), new Guid("51000000-0000-0000-0000-000000000053"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Opening for MEP services are cut properly.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000290"), new Guid("51000000-0000-0000-0000-000000000053"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Gypsum surface are Crackfree at joints.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000291"), new Guid("51000000-0000-0000-0000-000000000053"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, if any", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000292"), new Guid("51000000-0000-0000-0000-000000000054"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Layout of False Ceiling tiles and bulk head as per App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000293"), new Guid("51000000-0000-0000-0000-000000000054"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Height of the False Ceiling as per App Drawing", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000294"), new Guid("51000000-0000-0000-0000-000000000054"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access panels/ Ceiling tiles are Fixed Properly", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000295"), new Guid("51000000-0000-0000-0000-000000000054"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Gypsum surface are Crackfree at joints.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000296"), new Guid("51000000-0000-0000-0000-000000000054"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, if any", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000297"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Location of Window as per App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000298"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fixing of Glass/panels", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000299"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fixing of Iron-Mongery and Accessories", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000300"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fixing of Silicone Sealant", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000301"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water leak test performed and passed.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000302"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Paint touch completed around the frame.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000303"), new Guid("51000000-0000-0000-0000-000000000055"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, if any", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000304"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Location of Doors as per App Drawing", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000305"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Direction of doors swing as per App Drawing", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000306"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main enterance door as per App Drawing", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000307"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lock of Main enterance door is installed", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000308"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Architraves are fixed as per Drawing around main entrance door", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000309"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pod door is installed as per App Drawing", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000310"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lock of Main Pod door is installed", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000311"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Architraves are fixed as per Drawing around Pod door", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000312"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Locking of Doors and Shutters securely to avoid movement during transportation", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000313"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kitchen cabinets, counter top and accessories installed as per app drawing", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000314"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kitchen sink and sink mixer installed", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000315"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wardrobe installed as per approved drawings", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000316"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wardrobe doors and drawers funtioning smoothly and free from scratches", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000317"), new Guid("51000000-0000-0000-0000-000000000056"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Damages, if any", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000318"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mirror installed and free from damage", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000319"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Threshold installed and free from damage", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000320"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Glass Partition installed and free from damage", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000321"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Floor drain and covers installed and free from damages", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000322"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vanity installed and free from damage", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000323"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "WC and cover installed and free from damage", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000324"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shower installted and free from damage", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000325"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gypsum board are free from pealing off", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000326"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Painted walls are clean and free from stains.", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000327"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tiles are fixed with grouting properly and free from damage", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000328"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toilet accessories installed and free from damage", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000329"), new Guid("51000000-0000-0000-0000-000000000057"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes & MEP fittings.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000330"), new Guid("51000000-0000-0000-0000-000000000058"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Final Condition of outside of the room and ensure its damage free", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000331"), new Guid("51000000-0000-0000-0000-000000000058"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sign the delivery note for accepting the loading of precast modular in good condition", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000332"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Review documents for received materials", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000333"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Materials outside visual checking", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000334"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check for any damages (General & Visual)", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000335"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify original bill of landing / Delivery Note", true, "DO", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000336"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Supplier Certificate / Warranty letter", true, "Certificate", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000337"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check and Verify the material as per delivery list / details.", true, "DO", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000338"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the accessories.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000339"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Name Plate.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000340"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Materials Storage and preservation as per manufacturer.", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000341"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the identification of components.", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000342"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the rating as per approved drawing", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000343"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the loose part.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000344"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the dimension of delivered equipment as per approved drawing.", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000345"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the availability of spare breakers / relays/ terminals.", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000346"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delivered material photos.", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000347"), new Guid("51000000-0000-0000-0000-000000000059"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Material Site test.", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000348"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the materials as per approved material submittal.", true, "MA", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000349"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure drawings are used for installation are current and approved.", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000350"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000351"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The joints and flanges are correctly made, jointed and sealed.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000352"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that duct joints are sealed externally with approved sealant.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000353"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check fixing of supports and spacing as approved drawings & submittal.", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000354"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check acoustic lining is properly fastened and un damaged.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000355"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000356"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000357"), new Guid("51000000-0000-0000-0000-000000000060"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that the identification & labeling are provided for duct works.", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000358"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the materials are approved", true, "MIR", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000359"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No visible damage on the materials", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000360"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe sizes are as per approved shop drawing", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000361"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe layout/routing as per approved shop drawing", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000362"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slope of the pipes as per approved shop drawing", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000363"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation as per approved method statement", true, "Approved MTS", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000364"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sleeves are provided for the pipes passing through the walls /slabs", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000365"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the pipes are supported well with approved clamps.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000366"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Drainage pipes are not passing above electrical services", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000367"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installed pipes are free of sag & bend", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000368"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Drainage pipes are connected to the vent system", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000369"), new Guid("51000000-0000-0000-0000-000000000061"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe joints are properly made and are tight / secure", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000370"), new Guid("51000000-0000-0000-0000-000000000062"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Drainage Pipe has completed", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000371"), new Guid("51000000-0000-0000-0000-000000000062"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All pipe joints shall be inspected.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000372"), new Guid("51000000-0000-0000-0000-000000000062"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Drainage pipe should generally be subjected to an internal pressure test of 3m head of water above the crown of the pipe at the high end", true, "Project specification", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000373"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site Location: Dubox Factory", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000374"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System to be Tested: Drainage", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000375"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Fluid: Potable water", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000376"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duration of Test: 4 Hours", true, "Project Spec", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000377"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Start Time of Test", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000378"), new Guid("51000000-0000-0000-0000-000000000063"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finish Time of Test", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000379"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the materials are approved", true, "MIR", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000380"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No visible damage on the materials.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000381"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe sizes are as per approved shop drawing.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000382"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe layout/routing as per approved shop drawing.", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000383"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation as per approved method statement.", true, "Approved MST", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000384"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sleeves are provided for the pipes passing through the walls /slabs.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000385"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Horizontal pipes are supported well and with approved clamps.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000386"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the vertical riser of the pipes are supported well with approved clamp.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000387"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water pipes are not passing above electrical services.", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000388"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installed pipes are free of sag & bend.", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000389"), new Guid("51000000-0000-0000-0000-000000000064"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe joints are properly made and are tight / secure", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000390"), new Guid("51000000-0000-0000-0000-000000000065"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Water Supply pipes completed", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000391"), new Guid("51000000-0000-0000-0000-000000000065"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Pressure Gauge used is Calibrated", true, "Calibration Certificate", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000392"), new Guid("51000000-0000-0000-0000-000000000065"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure", true, "Project Spec.-221116", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000393"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site Location: Dubox Factory", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000394"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System to be Tested: Water Supply", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000395"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Fluid: Potable water", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000396"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Pressure (Bar): 8 Bar", true, "Project Spec", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000397"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duration of Test: 2 Hours", true, "Project Spec", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000398"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Start Time of Test", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000399"), new Guid("51000000-0000-0000-0000-000000000066"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finish Time of Test", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000400"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the piping, fitting, installation and valves materials are as per specification and approved material submittal", true, "MIR", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000401"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Checks the piping layouts are as per the approved shop drawing and site conditions.", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000402"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the approved supports& accessories are used for installation firefighting piping &Accessories.", true, "MIR", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000403"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Checks the distance between the supports are maintained as per specification and method statement.", true, "Approved Drawing & MTS", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000404"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Checks the proper sizing's of hangers are used as per specification.", true, "Approved Drawing & Project Specification", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000405"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check threaded joints are provided for 2\" and below and grooved fitting are provided for 2 ½\" and above in piping.", true, "Project Specification & Approved MTS", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000406"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check location / size of valves are provided as per approved shop drawing and specification.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000407"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check unions are provided for 2\" and below piping in direction of flow.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000408"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check flexible pipe connector/expansion compensator are installed in expansion joints and equipment connections.", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000409"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check drain points are provided in lowest piping points.", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000410"), new Guid("51000000-0000-0000-0000-000000000067"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check air vents are provided in highest piping points.", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000411"), new Guid("51000000-0000-0000-0000-000000000068"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire fighting pipes installation completed", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000412"), new Guid("51000000-0000-0000-0000-000000000068"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the Pressure Gauge used are calibrated", true, "Calibration certificate", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000413"), new Guid("51000000-0000-0000-0000-000000000068"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure in accordance to NFPA 13. Test shall be maintained for two hours as a minimum.", true, "Project Spec", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000414"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site Location: Dubox Factory", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000415"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System to be Tested: Fire Fighting", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000416"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Fluid: Potable water", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000417"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Pressure (Bar): 200 psi", true, "Project Spec", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000418"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duration of Test: 2 Hours", true, "Project Spec", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000419"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Start Time of Test", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000420"), new Guid("51000000-0000-0000-0000-000000000069"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finish Time of Test", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000421"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the materials as per approved material submittal.", true, "MA", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000422"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No visible damage on the materials.", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000423"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe sizes are as per approved shop drawing.", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000424"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe layout/routing as per approved shop drawing.", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000425"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation as per approved method statement.", true, "MTS", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000426"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sleeves are provided for the pipes passing through the walls/slabs.", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000427"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Horizontal pipes are supported well and with approved clamps.", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000428"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the vertical riser of the pipes are supported well with approved clamp.", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000429"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pipe joints are properly brazed with no leak", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000430"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the proper insulation done.", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000431"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the cladding and sealant on the cladding joints.", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000432"), new Guid("51000000-0000-0000-0000-000000000070"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the system charged with approved refrigerant", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000433"), new Guid("51000000-0000-0000-0000-000000000071"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of FAHU, FCU & VRF units are completed", true, "Project Spec.-237400 & 238129", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000434"), new Guid("51000000-0000-0000-0000-000000000071"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Refrigerant pipes completed", true, "Project Spec.-232300", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000435"), new Guid("51000000-0000-0000-0000-000000000071"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation inspection completed", true, "Project Spec.-232300", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000436"), new Guid("51000000-0000-0000-0000-000000000071"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pressure Test: After installation, charge system and test for leaks. Repair leaks and retest until no leaks exist.", true, "Project Spec.-232300", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000437"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Site Location: Dubox Factory", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000438"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System to be Tested: Refrigerant pipe", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000439"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Fluid: Nitrogen", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000440"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Test Pressure (Bar): 39 bar", true, "Project Spec", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000441"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duration of Test: 24 hrs", true, "Project Spec", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000442"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Start Time of Test", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000443"), new Guid("51000000-0000-0000-0000-000000000072"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finish Time of Test", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000444"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check all materials installed are as per approved material submittal.", true, "MAR", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000445"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dimensions as per approved drawings", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000446"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "the cable/wire provided with Ties appropriately?", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000447"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify measurement of lengths.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000448"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that there are no damages to the cables", true, "MIR", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000449"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify that identification, grouping, spacing, markings and clamps are as required.", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000450"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000451"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that cable type and routes are as per approved Drawing", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000452"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that correct types and sizes of wires and LV Cables are installed", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000453"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify that routes and marked locations are correct as per approved drawings", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000454"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is cable/wire provided with Lugs?", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000455"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is the cable/wire termination done correctly", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000456"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000457"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000458"), new Guid("51000000-0000-0000-0000-000000000073"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000459"), new Guid("51000000-0000-0000-0000-000000000074"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is the Cable/Wire Continuity test Setup, okay?", true, "Project Spec.-260513", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000460"), new Guid("51000000-0000-0000-0000-000000000074"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Is the Cable/Wire Megger test Setup, okay?", true, "Project Spec.-260513", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000461"), new Guid("51000000-0000-0000-0000-000000000074"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Megger test is performed for Cables and accepted", true, "Project Spec.-260513-1.7 C", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000462"), new Guid("51000000-0000-0000-0000-000000000074"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure Continuity test performed and accepted", true, "Project Spec.-260513-1.7 C", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000463"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toilet lighting - 2.5mm² - Y1", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000464"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Room Lighting - 2.5mm² - B1", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000465"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FCU - 4mm² - B2", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000466"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "General Purpose Power-usb - 4mm² - Y3", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000467"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shaver - 4mm² - B3", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000468"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hob - 6mm² - R4", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000469"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water Kettle - 4mm² - Y4", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000470"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Washing Machine - 4mm² - B4", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000471"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kitchen hood - 4mm² - R5", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000472"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water cooler/Refrigeration - 4mm² - Y5", true, "General", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000473"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "General Purpose Power-TV - 4mm² - B5", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000474"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Microwave oven - 4mm² - Y6", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000475"), new Guid("51000000-0000-0000-0000-000000000075"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Living UD - 4mm² - B6", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000476"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Verify the installed Panel boards have approved submittals.", true, "MAR", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000477"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure the drawings used for installation are correct and approved.", true, "Approved Shop Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000478"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Align, level and securely fasten panelboards to structure", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000479"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the Name Plate and Identification labels as per load schedule and approved submittals.", true, "Approved Shop Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000480"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.", true, "MIR", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000481"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.", true, "General", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000482"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Do not use connecting conduits to support panelboards.", true, "General", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000483"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panelboard Interiors: Do not install in cabinets until all conduit connections to cabinet have been completed", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000484"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.", true, "General", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000485"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Equipment grounding for LV Panel Board is provided", true, "Approved Shop Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000486"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that all unused openings are closed in the panels", true, "General", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000487"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.", true, "Approved Shop Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000488"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000489"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Touch up and cleaning of the panel", true, "General", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000490"), new Guid("51000000-0000-0000-0000-000000000076"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ensure that each Panel Marking should be done as per approved drawing", true, "Approved Shop Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000491"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the conduits and accessories are as per approved material submittal.", true, "MA", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000492"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check and ensure the drawings used for installation are current and approved.", true, "Approved Shop Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000493"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the conduits and other associated material are new and undamaged.", true, "MIR", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000494"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that the conduits are leveled and aligned properly.", true, "General", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000495"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that the conduits are securely fixed.", true, "General", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000496"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check and ensure that the conduits and back boxes are sizely adequated.", true, "Approved Shop Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000497"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that the bottom hight of the back boxes is as per the shop drawing.", true, "Approved Shop Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000498"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.", true, "General", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000499"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the installation of conduits is co-ordinated with other services.", true, "Approved Shop Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000500"), new Guid("51000000-0000-0000-0000-000000000077"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check the installation of conduiting as per approved drawings.", true, "Approved Shop Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000501"), new Guid("51000000-0000-0000-0000-000000000078"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check identification tag of the modular", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000502"), new Guid("51000000-0000-0000-0000-000000000078"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Visually inspect the MEP Services for any defects or damages", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000503"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Supply Duct", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000504"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Return Duct", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000505"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Fresh Air Duct", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000506"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Exhaust Air Duct", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000507"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Kitchen Hood and Flexible Duct", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000508"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of VCD", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000509"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Fan Coil Unit", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000510"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Fire Damper and Back Draft Damper", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000511"), new Guid("51000000-0000-0000-0000-000000000079"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Grills/Diffuser", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000512"), new Guid("51000000-0000-0000-0000-000000000080"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Pipes and fittings", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000513"), new Guid("51000000-0000-0000-0000-000000000080"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Insulation of pipe", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000514"), new Guid("51000000-0000-0000-0000-000000000080"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Pipe Insulation and adhesive.", true, "General", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000515"), new Guid("51000000-0000-0000-0000-000000000080"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pressure testing of the Piping", true, "General", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000516"), new Guid("51000000-0000-0000-0000-000000000081"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Primer paint", true, "General", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000517"), new Guid("51000000-0000-0000-0000-000000000081"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Application of Paint final coating", true, "General", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000518"), new Guid("51000000-0000-0000-0000-000000000081"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Sprinklers", true, "Approved Drawing", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000519"), new Guid("51000000-0000-0000-0000-000000000081"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pressure testing of the Piping", true, "General", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000520"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Pipes and fittings", true, "Approved Drawing", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000521"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water Hammer Arrestor installed in the approved location.", true, "Approved Drawing", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000522"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pressure testing of the Piping", true, "General", 21 },
                    { new Guid("52000000-0000-0000-0000-000000000523"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hot water pipes insulated.", true, "Approved Drawing", 22 },
                    { new Guid("52000000-0000-0000-0000-000000000524"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gate Valve Installation", true, "Approved Drawing", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000525"), new Guid("51000000-0000-0000-0000-000000000082"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kitchen sink and accessories", true, "Approved Drawing", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000526"), new Guid("51000000-0000-0000-0000-000000000083"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Floor Cleanout (FCO)", true, "Approved Drawing", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000527"), new Guid("51000000-0000-0000-0000-000000000083"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation of Floor Drain", true, "Approved Drawing", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000528"), new Guid("51000000-0000-0000-0000-000000000083"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Installation CDP Pipes", true, "Approved Drawing", 25 },
                    { new Guid("52000000-0000-0000-0000-000000000529"), new Guid("51000000-0000-0000-0000-000000000083"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Piping Leak test", true, "General", 26 },
                    { new Guid("52000000-0000-0000-0000-000000000530"), new Guid("51000000-0000-0000-0000-000000000083"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sleeves provided for drain pipes outlets.", true, "Approved Drawing", 27 },
                    { new Guid("52000000-0000-0000-0000-000000000531"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Soil Pipe", true, "Approved Drawing", 28 },
                    { new Guid("52000000-0000-0000-0000-000000000532"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Waste Pipe", true, "Approved Drawing", 29 },
                    { new Guid("52000000-0000-0000-0000-000000000533"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vent Pipe", true, "Approved Drawing", 30 },
                    { new Guid("52000000-0000-0000-0000-000000000534"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water Supply Pipe", true, "Approved Drawing", 31 },
                    { new Guid("52000000-0000-0000-0000-000000000535"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Refrigerant Pipe", true, "Approved Drawing", 32 },
                    { new Guid("52000000-0000-0000-0000-000000000536"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Firefighting Pipe", true, "Approved Drawing", 33 },
                    { new Guid("52000000-0000-0000-0000-000000000537"), new Guid("51000000-0000-0000-0000-000000000084"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duct Riser and connection", true, "Approved Drawing", 34 },
                    { new Guid("52000000-0000-0000-0000-000000000538"), new Guid("51000000-0000-0000-0000-000000000085"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check Alignment of Wiring Devices", true, "General", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000539"), new Guid("51000000-0000-0000-0000-000000000085"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check For ONU Panel Door", true, "General", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000540"), new Guid("51000000-0000-0000-0000-000000000085"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check For DB Panel Door", true, "General", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000541"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10A 1G, 2 Way switch", true, "Approved Drawing", 1 },
                    { new Guid("52000000-0000-0000-0000-000000000542"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10A 2G, 2 Way switch", true, "Approved Drawing", 2 },
                    { new Guid("52000000-0000-0000-0000-000000000543"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10A 3G, 2 Way switch", true, "Approved Drawing", 3 },
                    { new Guid("52000000-0000-0000-0000-000000000544"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Single Data Outlet -Euro face plate single keystone adaptor", true, "Approved Drawing", 4 },
                    { new Guid("52000000-0000-0000-0000-000000000545"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Twin Data Outlet -Euro face plate Duplex keystone adaptor", true, "Approved Drawing", 5 },
                    { new Guid("52000000-0000-0000-0000-000000000546"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "13A, Double pole Duplex switched Power socket", true, "Approved Drawing", 6 },
                    { new Guid("52000000-0000-0000-0000-000000000547"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "20 A, DP switch Washing Machine", true, "Approved Drawing", 7 },
                    { new Guid("52000000-0000-0000-0000-000000000548"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "45 A, DP switch for HOB", true, "Approved Drawing", 8 },
                    { new Guid("52000000-0000-0000-0000-000000000549"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Unswitched Fused Connection Unit + cord outlet - FACP", true, "Approved Drawing", 9 },
                    { new Guid("52000000-0000-0000-0000-000000000550"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Switched Fused Connection Unit + cord outlet - Hood", true, "Approved Drawing", 10 },
                    { new Guid("52000000-0000-0000-0000-000000000551"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "20 A, Cable/Flex outlet Washing Machine", true, "Approved Drawing", 11 },
                    { new Guid("52000000-0000-0000-0000-000000000552"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "45 A, Cable/Flex outlet for HOB", true, "Approved Drawing", 12 },
                    { new Guid("52000000-0000-0000-0000-000000000553"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Door Bell -230V Electromechanical chime", true, "Approved Drawing", 13 },
                    { new Guid("52000000-0000-0000-0000-000000000554"), new Guid("51000000-0000-0000-0000-000000000086"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "13A, DP, Simplex Switched & fused Spur Outlet for FCUs", true, "Approved Drawing", 14 },
                    { new Guid("52000000-0000-0000-0000-000000000555"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All Wires pulled as per the approved Drawings.", true, "Approved Drawing", 15 },
                    { new Guid("52000000-0000-0000-0000-000000000556"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CAT-6 Cable pulled", true, "Approved Drawing", 16 },
                    { new Guid("52000000-0000-0000-0000-000000000557"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire alarm Cable pulled", true, "Approved Drawing", 17 },
                    { new Guid("52000000-0000-0000-0000-000000000558"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "VRV Cable pulled", true, "Approved Drawing", 18 },
                    { new Guid("52000000-0000-0000-0000-000000000559"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main Cable pulled", true, "Approved Drawing", 19 },
                    { new Guid("52000000-0000-0000-0000-000000000560"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Smoke detector", true, "Approved Drawing", 20 },
                    { new Guid("52000000-0000-0000-0000-000000000561"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Heat detector", true, "Approved Drawing", 21 },
                    { new Guid("52000000-0000-0000-0000-000000000562"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sensor", true, "Approved Drawing", 22 },
                    { new Guid("52000000-0000-0000-0000-000000000563"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DB Panel Tags and identification.", true, "Approved Drawing", 23 },
                    { new Guid("52000000-0000-0000-0000-000000000564"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ONU Panel installation and termination", true, "Approved Drawing", 24 },
                    { new Guid("52000000-0000-0000-0000-000000000565"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DB Panel installation and termination", true, "Approved Drawing", 25 },
                    { new Guid("52000000-0000-0000-0000-000000000566"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thermostat", true, "Approved Drawing", 26 },
                    { new Guid("52000000-0000-0000-0000-000000000567"), new Guid("51000000-0000-0000-0000-000000000087"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PMU", true, "Approved Drawing", 27 },
                    { new Guid("52000000-0000-0000-0000-000000000568"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LT03 STUDIO", true, "Approved Drawing", 28 },
                    { new Guid("52000000-0000-0000-0000-000000000569"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LT9B STUDIO", true, "Approved Drawing", 29 },
                    { new Guid("52000000-0000-0000-0000-000000000570"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LT06 STUDIO", true, "Approved Drawing", 30 },
                    { new Guid("52000000-0000-0000-0000-000000000571"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LT09A LOUNGE 2", true, "Approved Drawing", 31 },
                    { new Guid("52000000-0000-0000-0000-000000000572"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "C LOUNGE 2", true, "Approved Drawing", 32 },
                    { new Guid("52000000-0000-0000-0000-000000000573"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "L01 GABRGE ROOM", true, "Approved Drawing", 33 },
                    { new Guid("52000000-0000-0000-0000-000000000574"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "L05 ELECTRICAL ROOM", true, "Approved Drawing", 34 },
                    { new Guid("52000000-0000-0000-0000-000000000575"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "L07 STAIR", true, "Approved Drawing", 35 },
                    { new Guid("52000000-0000-0000-0000-000000000576"), new Guid("51000000-0000-0000-0000-000000000088"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "L08 ICT ROOM", true, "Approved Drawing", 36 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000083"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000087"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000088"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000089"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000090"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000091"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000092"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000093"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000094"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000095"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000096"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000097"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000098"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000099"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000100"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000104"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000105"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000106"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000107"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000108"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000109"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000110"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000111"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000112"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000113"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000114"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000115"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000116"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000117"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000118"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000119"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000120"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000121"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000122"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000123"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000124"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000125"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000126"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000127"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000128"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000129"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000130"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000131"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000132"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000133"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000134"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000135"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000136"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000137"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000138"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000139"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000140"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000141"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000142"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000143"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000144"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000145"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000146"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000147"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000148"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000149"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000150"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000151"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000152"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000153"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000154"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000155"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000156"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000157"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000158"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000159"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000160"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000161"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000162"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000163"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000164"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000165"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000166"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000167"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000168"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000169"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000170"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000171"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000172"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000173"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000174"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000175"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000176"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000177"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000178"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000179"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000180"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000181"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000182"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000183"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000184"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000185"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000186"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000187"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000188"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000189"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000190"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000191"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000192"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000193"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000194"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000195"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000196"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000197"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000198"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000199"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000200"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000201"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000202"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000203"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000204"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000205"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000206"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000207"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000208"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000209"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000210"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000211"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000212"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000213"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000214"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000215"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000216"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000217"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000218"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000219"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000220"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000221"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000222"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000223"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000224"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000225"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000226"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000227"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000228"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000229"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000230"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000231"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000232"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000233"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000234"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000235"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000236"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000237"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000238"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000239"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000240"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000241"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000242"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000243"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000244"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000245"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000246"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000247"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000248"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000249"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000250"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000251"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000252"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000253"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000254"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000255"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000256"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000257"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000258"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000259"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000260"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000261"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000262"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000263"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000264"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000265"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000266"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000267"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000268"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000269"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000270"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000271"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000272"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000273"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000274"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000275"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000276"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000277"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000278"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000279"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000280"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000281"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000282"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000283"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000284"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000285"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000286"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000287"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000288"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000289"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000290"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000291"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000292"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000293"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000294"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000295"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000296"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000297"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000298"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000299"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000300"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000301"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000302"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000303"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000304"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000305"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000306"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000307"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000308"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000309"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000310"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000311"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000312"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000313"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000314"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000315"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000316"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000317"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000318"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000319"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000320"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000321"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000322"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000323"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000324"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000325"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000326"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000327"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000328"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000329"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000330"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000331"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000332"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000333"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000334"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000335"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000336"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000337"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000338"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000339"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000340"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000341"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000342"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000343"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000344"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000345"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000346"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000347"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000348"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000349"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000350"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000351"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000352"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000353"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000354"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000355"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000356"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000357"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000358"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000359"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000360"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000361"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000362"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000363"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000364"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000365"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000366"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000367"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000368"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000369"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000370"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000371"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000372"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000373"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000374"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000375"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000376"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000377"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000378"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000379"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000380"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000381"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000382"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000383"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000384"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000385"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000386"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000387"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000388"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000389"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000390"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000391"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000392"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000393"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000394"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000395"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000396"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000397"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000398"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000399"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000400"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000401"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000402"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000403"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000404"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000405"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000406"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000407"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000408"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000409"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000410"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000411"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000412"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000413"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000414"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000415"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000416"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000417"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000418"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000419"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000420"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000421"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000422"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000423"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000424"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000425"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000426"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000427"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000428"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000429"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000430"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000431"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000432"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000433"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000434"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000435"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000436"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000437"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000438"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000439"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000440"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000441"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000442"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000443"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000444"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000445"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000446"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000447"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000448"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000449"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000450"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000451"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000452"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000453"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000454"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000455"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000456"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000457"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000458"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000459"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000460"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000461"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000462"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000463"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000464"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000465"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000466"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000467"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000468"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000469"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000470"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000471"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000472"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000473"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000474"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000475"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000476"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000477"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000478"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000479"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000480"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000481"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000482"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000483"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000484"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000485"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000486"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000487"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000488"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000489"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000490"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000491"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000492"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000493"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000494"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000495"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000496"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000497"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000498"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000499"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000500"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000501"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000502"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000503"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000504"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000505"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000506"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000507"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000508"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000509"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000510"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000511"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000512"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000513"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000514"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000515"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000516"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000517"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000518"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000519"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000520"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000521"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000522"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000523"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000524"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000525"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000526"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000527"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000528"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000529"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000530"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000531"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000532"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000533"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000534"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000535"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000536"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000537"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000538"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000539"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000540"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000541"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000542"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000543"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000544"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000545"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000546"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000547"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000548"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000549"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000550"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000551"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000552"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000553"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000554"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000555"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000556"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000557"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000558"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000559"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000560"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000561"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000562"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000563"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000564"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000565"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000566"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000567"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000568"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000569"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000570"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000571"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000572"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000573"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000574"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000575"));

            migrationBuilder.DeleteData(
                table: "PredefinedChecklistItems",
                keyColumn: "PredefinedItemId",
                keyValue: new Guid("52000000-0000-0000-0000-000000000576"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000083"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000087"));

            migrationBuilder.DeleteData(
                table: "ChecklistSections",
                keyColumn: "ChecklistSectionId",
                keyValue: new Guid("51000000-0000-0000-0000-000000000088"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "Checklists",
                keyColumn: "ChecklistId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000042"));
        }
    }
}
