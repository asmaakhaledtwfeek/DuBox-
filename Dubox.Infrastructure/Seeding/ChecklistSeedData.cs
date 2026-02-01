using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dubox.Infrastructure.Seeding;

public static class ChecklistSeedData
{
    // Static seed date to prevent EF Core model changes warning
    private static readonly DateTime SeedDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Checklist GUIDs (50000000-0000-0000-0000-000000000001 to 50000000-0000-0000-0000-000000000043)
    private static readonly Guid MaterialVerificationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    private static readonly Guid PrecastConcreteModularChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000002");
    private static readonly Guid PaintingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000003");
    private static readonly Guid CeramicTilingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000004");
    private static readonly Guid BitumenPaintApplicationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000005");
    private static readonly Guid GypsumPartitionChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000006");
    private static readonly Guid FalseCeilingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000007");
    private static readonly Guid WetAreaWaterproofingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000008");
    private static readonly Guid EpoxyFlooringChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000009");
    private static readonly Guid KitchenCabinetsChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000010");
    private static readonly Guid WardrobeInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000011");
    private static readonly Guid DoorsWindowsInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000012");
    private static readonly Guid FireSealingWorksChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000013");
    private static readonly Guid AluminiumGlazingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000014");
    private static readonly Guid PreLoadingCivilChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000015");
    // MEP Checklists
    private static readonly Guid MaterialReceivingMEPChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000020");
    private static readonly Guid HVACDuctInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000021");
    private static readonly Guid DrainagePipesInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000022");
    private static readonly Guid DrainagePipesLeakTestChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000023");
    private static readonly Guid DrainagePipesTestReportChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000024");
    private static readonly Guid WaterSupplyInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000025");
    private static readonly Guid WaterSupplyTestingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000026");
    private static readonly Guid WaterSupplyTestReportChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000027");
    private static readonly Guid FireFightingInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000028");
    private static readonly Guid FireFightingTestingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000029");
    private static readonly Guid FireFightingTestReportChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000030");
    private static readonly Guid RefrigerantPipeInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000031");
    private static readonly Guid RefrigerantPipeTestingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000032");
    private static readonly Guid RefrigerantPipeTestReportChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000033");
    private static readonly Guid LVCablesInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000034");
    private static readonly Guid LVCablesTestingChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000035");
    private static readonly Guid LVCablesTestResultChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000036");
    private static readonly Guid LVPanelsInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000040");
    private static readonly Guid ConduitsInstallationChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000041");
    private static readonly Guid PreLoadingMEPChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000042");

    public static void SeedChecklists(ModelBuilder modelBuilder)
    {
        var checklists = new List<Checklist>();
        var sections = new List<ChecklistSection>();
        var items = new List<PredefinedChecklistItem>();

        // Helper method to add checklist data
        void AddChecklistData(Checklist checklist, List<ChecklistSection> checklistSections, List<PredefinedChecklistItem> checklistItems)
        {
            checklists.Add(checklist);
            sections.AddRange(checklistSections);
            items.AddRange(checklistItems);
        }

        // PAGE 1: Material Verification
        var materialVerificationSections = new List<ChecklistSection>
        {
            new ChecklistSection
            {
                ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000001"),
                ChecklistId = MaterialVerificationChecklistId,
                Title = "Verification Items",
                Order = 1,
                IsActive = true,
                CreatedDate = SeedDate
            }
        };

        var materialVerificationItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000001"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 1, Description = "Is there material approval for received item?", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000002"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 2, Description = "Is Manufacturer Name as per material approval?", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000003"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 3, Description = "Is Supplier Name as per material approval?", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000004"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 4, Description = "Is received material matching with approved sample?", Reference = "Approved Sample", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000005"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 5, Description = "Related mill test certificate (or) test reports?", Reference = "MTC", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000006"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 6, Description = "Check Expiry date", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000007"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 7, Description = "Check Item / product description", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000008"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 8, Description = "Check Item / product code", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000009"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 9, Description = "Check Dimensions (length, width, thickness etc.)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000010"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 10, Description = "Check Colour", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000011"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 11, Description = "Check for any defects", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000012"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 12, Description = "Check for Packaging Conditions", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000013"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 13, Description = "Check for received Quantity (approx) as per DO", Reference = "DO", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000014"), ChecklistSectionId = materialVerificationSections[0].ChecklistSectionId, Sequence = 14, Description = "Check the area of storage as per MSDS", Reference = "MSDS", IsActive = true, CreatedDate = SeedDate }
        };

        var materialVerificationChecklist = new Checklist
        {
            ChecklistId = MaterialVerificationChecklistId,
            Name = "Material Verification Inspection Checklist",
            Code = "Material-Verification",
            Discipline = "CIVIL",
            SubDiscipline = "General",
            PageNumber = 1,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Material Approval", "Delivery Order", "MSDS" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Quality Dept.", "Operation Team" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(materialVerificationChecklist, materialVerificationSections, materialVerificationItems);

        // PAGES 2-3: Construction of Precast Concrete Modular
        var precastSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000002"), ChecklistId = PrecastConcreteModularChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000003"), ChecklistId = PrecastConcreteModularChecklistId, Title = "B. Preparation & Setting Out", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000004"), ChecklistId = PrecastConcreteModularChecklistId, Title = "C. Erection / Assembling of Precast Element - External Walls", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000005"), ChecklistId = PrecastConcreteModularChecklistId, Title = "C. Erection / Assembling - Floor Slab(s)", Order = 4, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000006"), ChecklistId = PrecastConcreteModularChecklistId, Title = "C. Erection / Assembling - Internal Partition", Order = 5, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000007"), ChecklistId = PrecastConcreteModularChecklistId, Title = "C. Erection / Assembling - Top Slabs", Order = 6, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = Guid.Parse("51000000-0000-0000-0000-000000000008"), ChecklistId = PrecastConcreteModularChecklistId, Title = "Civil Works", Order = 7, IsActive = true, CreatedDate = SeedDate }
        };

        var precastItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000015"), ChecklistSectionId = precastSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and shop drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000016"), ChecklistSectionId = precastSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000017"), ChecklistSectionId = precastSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000018"), ChecklistSectionId = precastSections[1].ChecklistSectionId, Sequence = 4, Description = "Drawing Stamp & Signature", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000019"), ChecklistSectionId = precastSections[1].ChecklistSectionId, Sequence = 5, Description = "Element Tag, QC Approval for Element", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000020"), ChecklistSectionId = precastSections[1].ChecklistSectionId, Sequence = 6, Description = "Floor Setting Out", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000021"), ChecklistSectionId = precastSections[2].ChecklistSectionId, Sequence = 7, Description = "Erection with temporary support.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000022"), ChecklistSectionId = precastSections[2].ChecklistSectionId, Sequence = 8, Description = "Panel to panel connections.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000023"), ChecklistSectionId = precastSections[2].ChecklistSectionId, Sequence = 9, Description = "Dimensions (outer, inner and diagonal), line and level.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000024"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 10, Description = "Backer Rod / Shim Pad", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000025"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 11, Description = "Slab Position / Level", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000026"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 12, Description = "Panel to Slab Connections", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000027"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 13, Description = "1000mm FFL to be marked clearly.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000028"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 14, Description = "MEP Clearance", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000029"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 15, Description = "Check the Slab level (shall read 1016mm at 1000m FFL line)", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000030"), ChecklistSectionId = precastSections[3].ChecklistSectionId, Sequence = 16, Description = "Grouting", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000031"), ChecklistSectionId = precastSections[4].ChecklistSectionId, Sequence = 17, Description = "Erection with Temporary Support", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000032"), ChecklistSectionId = precastSections[4].ChecklistSectionId, Sequence = 18, Description = "Panel to Panel Connections", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000033"), ChecklistSectionId = precastSections[4].ChecklistSectionId, Sequence = 19, Description = "Dimensions (outer, inner and diagonal), Line and Level", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000034"), ChecklistSectionId = precastSections[4].ChecklistSectionId, Sequence = 20, Description = "Grouting", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000035"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 21, Description = "Backer Rod / Shim Pad", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000036"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 22, Description = "Slab Position / Top Height", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000037"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 23, Description = "Panel to Slab Connections", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000038"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 24, Description = "MEP Clearance", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000039"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 25, Description = "Grouting", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000040"), ChecklistSectionId = precastSections[5].ChecklistSectionId, Sequence = 26, Description = "Curing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000041"), ChecklistSectionId = precastSections[6].ChecklistSectionId, Sequence = 27, Description = "Internal and External Dimension of Box", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = Guid.Parse("52000000-0000-0000-0000-000000000042"), ChecklistSectionId = precastSections[6].ChecklistSectionId, Sequence = 28, Description = "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var precastChecklist = new Checklist
        {
            ChecklistId = PrecastConcreteModularChecklistId,
            Name = "Construction of Precast Concrete Modular at Factory Checklist",
            Code = "ASA-IMS-FRM-13-081",
            Discipline = "CIVIL",
            SubDiscipline = "Precast",
            PageNumber = 2,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specifications Section- 233113 & 230713" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(precastChecklist, precastSections, precastItems);

        // Helper method to generate GUIDs
        int sectionCounter = 9; // Continue from 9 (after precast sections 2-8)
        int itemCounter = 43; // Continue from 43 (after precast items 15-42)

        Guid GetNextSectionId() => Guid.Parse($"51000000-0000-0000-0000-{sectionCounter++:D012}");
        Guid GetNextItemId() => Guid.Parse($"52000000-0000-0000-0000-{itemCounter++:D012}");

        // PAGE 4: Painting
        var paintingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PaintingChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PaintingChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PaintingChecklistId, Title = "C. Application of Internal Paint", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PaintingChecklistId, Title = "D. Application of External Paint", Order = 4, IsActive = true, CreatedDate = SeedDate }
        };

        var paintingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored as per manufacturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[0].ChecklistSectionId, Sequence = 3, Description = "Verify the expiry date and number of coats of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[0].ChecklistSectionId, Sequence = 4, Description = "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[1].ChecklistSectionId, Sequence = 5, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[1].ChecklistSectionId, Sequence = 6, Description = "Check for repair of surface imperfection and protrusions (if any).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[1].ChecklistSectionId, Sequence = 7, Description = "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[1].ChecklistSectionId, Sequence = 8, Description = "Undulations or irregular corners are repaired and grinded as required.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[2].ChecklistSectionId, Sequence = 9, Description = "Ensure application of Primer as per manufacturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[2].ChecklistSectionId, Sequence = 10, Description = "Ensure application of Stuccoo as per manufecturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[2].ChecklistSectionId, Sequence = 11, Description = "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[2].ChecklistSectionId, Sequence = 12, Description = "Application of first coat of Paint as per manufacturers recommendation.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[2].ChecklistSectionId, Sequence = 13, Description = "Line between two color shades is straight, no Brush marks should be visible.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[3].ChecklistSectionId, Sequence = 14, Description = "Ensure application of Primer as per manufacturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[3].ChecklistSectionId, Sequence = 15, Description = "Ensure application of Filler Coats as per manufacturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[3].ChecklistSectionId, Sequence = 16, Description = "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = paintingSections[3].ChecklistSectionId, Sequence = 17, Description = "Application of final coat of Texture Paint as per manufacturers recommendation.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var paintingChecklist = new Checklist
        {
            ChecklistId = PaintingChecklistId,
            Name = "Painting Checklist",
            Code = "Paint-Work",
            Discipline = "CIVIL",
            SubDiscipline = "Painting",
            PageNumber = 4,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Engineer", "QC Engineer" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(paintingChecklist, paintingSections, paintingItems);

        // PAGE 5: Ceramic Tiling
        var tilingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = CeramicTilingChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = CeramicTilingChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = CeramicTilingChecklistId, Title = "C. Installation of Tile", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = CeramicTilingChecklistId, Title = "D. Grouting of Tile", Order = 4, IsActive = true, CreatedDate = SeedDate }
        };

        var tilingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored as per manufacturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[0].ChecklistSectionId, Sequence = 3, Description = "Verify the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 4, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 5, Description = "Check surface is levelled and sloped as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 6, Description = "Check the surface roughness is appropriate as per the tile and tile adhesive manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 7, Description = "Verify the application of wet area water proofing and leak test.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 8, Description = "Verify the location and level of floor drain as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[1].ChecklistSectionId, Sequence = 9, Description = "Check the MEP clearance prior to start Painting works.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 10, Description = "Ensure proper mixing of material as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 11, Description = "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 12, Description = "Check the application of tile adhesive using notch trowel on the backside of tile and over the substrate.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 13, Description = "Verify the tile spacers width is uniform and aligned as per the appoved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 14, Description = "Check the setting out / pattern of wall and floor tiles as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 15, Description = "Verify the slope and level of the tiles as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 16, Description = "Check the laying of tiles above the false ceiling as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 17, Description = "Check the pull off / adhesion test for the paint application as per the project specifications / manufacturer recommendations (if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[2].ChecklistSectionId, Sequence = 18, Description = "Approval to proceed with further works obtained from consultant / Client.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 19, Description = "Check the tile joints are clean and free from contaminants like dust etc.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 20, Description = "Ensure proper mixing of material as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 21, Description = "Check the Location, Colour & Type of grout as per the approved shop drawings / material submittal.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 22, Description = "Check the width of tile grout is proper and uniform.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 23, Description = "Check the application of tile grout as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = tilingSections[3].ChecklistSectionId, Sequence = 24, Description = "Approval to proceed with further works obtained from consultant / Client.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var tilingChecklist = new Checklist
        {
            ChecklistId = CeramicTilingChecklistId,
            Name = "Ceramic Tiling Checklist",
            Code = "ASA-IMS-FRM-13-049",
            Discipline = "CIVIL",
            SubDiscipline = "Tiling",
            PageNumber = 5,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(tilingChecklist, tilingSections, tilingItems);

        // PAGE 6: Bitumen Paint Application
        var bitumenSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = BitumenPaintApplicationChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = BitumenPaintApplicationChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = BitumenPaintApplicationChecklistId, Title = "C. Application of Bitumen Paint", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var bitumenItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement and materials are approved.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[1].ChecklistSectionId, Sequence = 4, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[1].ChecklistSectionId, Sequence = 5, Description = "Check for repair of surface imperfection and protrusions (if any).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[1].ChecklistSectionId, Sequence = 6, Description = "Check the angle fillets and chamfering of all sharp edges (if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[1].ChecklistSectionId, Sequence = 7, Description = "Check the MEP clearance are obtained prior to start bitumen application.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 8, Description = "Ensure proper mixing as per the manufacturer recommandations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 9, Description = "Check the application of primer coat (if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 10, Description = "Check the rate of application as per the manufacturer recommendation and method statement.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 11, Description = "Check the application of coats as per project requirements / manufacturer recommandations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 12, Description = "Check the application of subsequent coats are carried out at right angle to the previous coat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 13, Description = "Check the WFT as per the project specifications / manufacturer recommandations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 14, Description = "Check the curing of application as per manufacturer recommendation.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = bitumenSections[2].ChecklistSectionId, Sequence = 15, Description = "Approval obtained from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var bitumenChecklist = new Checklist
        {
            ChecklistId = BitumenPaintApplicationChecklistId,
            Name = "Bitumen Paint Application Checklist",
            Code = "ASA-IMS-FRM-13-036",
            Discipline = "CIVIL",
            SubDiscipline = "Waterproofing",
            PageNumber = 6,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(bitumenChecklist, bitumenSections, bitumenItems);

        // PAGE 7: Gypsum Partition
        var gypsumSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = GypsumPartitionChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = GypsumPartitionChecklistId, Title = "B. Setting Out", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = GypsumPartitionChecklistId, Title = "C. Installation Activity", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var gypsumItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, material submittal and drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify and record the DCL product confirmity certificate for the insulation materials.", Reference = "DCL Certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[1].ChecklistSectionId, Sequence = 5, Description = "Verify the marking and setting out of the partition walls as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 6, Description = "Verify the completion of the required finishes of the adjacent substrates.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 7, Description = "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 8, Description = "Verify the fixation of the board (on one side of the supports) as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 9, Description = "Ensure the completion of all embedded MEP and other dicipline works prior to closure as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 10, Description = "Ensure additional supports are provided for the wall mounted fixtures as applicable.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 11, Description = "Verify the installation of insulation works (if applicable) as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 12, Description = "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 13, Description = "Verify the fixation of the board as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 14, Description = "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 15, Description = "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 16, Description = "Ensure the cutting of gypsum board on the marked locations as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 17, Description = "Verify the jointing & taping as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = gypsumSections[2].ChecklistSectionId, Sequence = 18, Description = "Approval obtain from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var gypsumChecklist = new Checklist
        {
            ChecklistId = GypsumPartitionChecklistId,
            Name = "Gypsum Partition Installation Checklist",
            Code = "ASA-IMS-FRM-13-054",
            Discipline = "CIVIL",
            SubDiscipline = "Drywall",
            PageNumber = 7,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(gypsumChecklist, gypsumSections, gypsumItems);

        // PAGE 8: False Ceiling
        var falseCeilingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FalseCeilingChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FalseCeilingChecklistId, Title = "B. Setting Out", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FalseCeilingChecklistId, Title = "C. Pre-Installation Activity", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FalseCeilingChecklistId, Title = "D. Installation of False Ceiling", Order = 4, IsActive = true, CreatedDate = SeedDate }
        };

        var falseCeilingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, material submittal and drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials (Gypsum board, tiles, suspension systems, etc.,) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the color, type, material, pattern and thickness are as per approved material approval and project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[1].ChecklistSectionId, Sequence = 4, Description = "Verify the marking of the false ceiling level on the walls as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[1].ChecklistSectionId, Sequence = 5, Description = "Verify the marking / location of suspension system supports at ceiling as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 6, Description = "Verify the completion of the required finishes above the false ceiling level.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 7, Description = "Verify the closing of shaft openings, MEP penetrations and other openings as per project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 8, Description = "Verify the application of fire sealant / fire protection works are carried out as per the project requirements (if applicable).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 9, Description = "Verify the location, spacing and fixation of the grid (main channel, Furing channel, wall angle, hanging wire with adjustable clip, main tee, cross tee, etc.,) as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 10, Description = "Ensure the suspension system supports are not in contact with the adjacent MEP services.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 11, Description = "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 12, Description = "Ensure additional supports are provided for the ceiling mounted fixtures as applicable.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[2].ChecklistSectionId, Sequence = 13, Description = "Obtain approval from consultant / Client to proceed with further works.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[3].ChecklistSectionId, Sequence = 14, Description = "Verify the type, fixation, level and alignment of the false ceiling board / tiles as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[3].ChecklistSectionId, Sequence = 15, Description = "Verify the marking, position and alignment of MEP & ceiling mounted fixtures (light, access panel, sprinklers, signages etc.) in the ceiling as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[3].ChecklistSectionId, Sequence = 16, Description = "Ensure the cutting of gypsum board / tiles on the marked locations as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[3].ChecklistSectionId, Sequence = 17, Description = "Verify the jointing & taping as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = falseCeilingSections[3].ChecklistSectionId, Sequence = 18, Description = "Approval obtained from Consultant/Client to proceed with further activites.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var falseCeilingChecklist = new Checklist
        {
            ChecklistId = FalseCeilingChecklistId,
            Name = "False Ceiling Installation Checklist",
            Code = "ASA-IMS-FRM-13-052",
            Discipline = "CIVIL",
            SubDiscipline = "False Ceiling",
            PageNumber = 8,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(falseCeilingChecklist, falseCeilingSections, falseCeilingItems);

        // PAGE 9: Wet Area Waterproofing
        var wetAreaSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WetAreaWaterproofingChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WetAreaWaterproofingChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WetAreaWaterproofingChecklistId, Title = "C. Application of Waterproofing", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var wetAreaItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement and materials are approved.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[1].ChecklistSectionId, Sequence = 4, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[1].ChecklistSectionId, Sequence = 5, Description = "Ensure grouting, angle fillet provided all around the penetrations and the projected MEP services are neat and clean from any latiance.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[1].ChecklistSectionId, Sequence = 6, Description = "Ensure the angle fillets are provided at the floor and wall junctions of the area that to be treated.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[1].ChecklistSectionId, Sequence = 7, Description = "Obtain MEP clearance prior to start the wet area water proofing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[1].ChecklistSectionId, Sequence = 8, Description = "Ensure check dam constructed at the entrance of the room for the stagnation of water for water leakage testing after the completion of the wet area water proofing.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 9, Description = "Ensure proper mixing of the material as per the manufacturer recommendations", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 10, Description = "Check the application of primer coat (if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 11, Description = "Check the rate of application as per the manufacturer recommendation and method statement.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 12, Description = "Check the application of coats as per project requirements / manufacturer recommandations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 13, Description = "Check the application of subsequent coats are carried out at right angle to the previous coat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 14, Description = "Ensure the application of waterproofing on the vertical face extended upto 300mm from FFL or as per approved drawing / project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 15, Description = "Check the WFT as per the project specifications / manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 16, Description = "Check the curing of application as per manufacturer recommendation.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 17, Description = "Obtain approval for the application of waterproofing to proceed water leakage test.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 18, Description = "Check the filling of water and monitor the level of filled water during the leakage test.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 19, Description = "Check for any water seepage / leakage after 24 hours or as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wetAreaSections[2].ChecklistSectionId, Sequence = 20, Description = "Approval obtained from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var wetAreaChecklist = new Checklist
        {
            ChecklistId = WetAreaWaterproofingChecklistId,
            Name = "Wet Area Waterproofing Checklist",
            Code = "ASA-IMS-FRM-13-046",
            Discipline = "CIVIL",
            SubDiscipline = "Waterproofing",
            PageNumber = 9,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(wetAreaChecklist, wetAreaSections, wetAreaItems);

        // PAGE 10: Epoxy Flooring
        var epoxySections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = EpoxyFlooringChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = EpoxyFlooringChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = EpoxyFlooringChecklistId, Title = "C. Application of Epoxy", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var epoxyItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored as per manufacturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[0].ChecklistSectionId, Sequence = 3, Description = "Verify the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[1].ChecklistSectionId, Sequence = 4, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[1].ChecklistSectionId, Sequence = 5, Description = "Check for repair of surface imperfection and protrusions (if any).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[1].ChecklistSectionId, Sequence = 6, Description = "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[1].ChecklistSectionId, Sequence = 7, Description = "Undulations or irregular corners are repaired and grinded as required.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[1].ChecklistSectionId, Sequence = 8, Description = "Check the MEP clearance prior to start epoxy flooring works.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 9, Description = "Ensure application of primer as per manufecturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 10, Description = "Ensure application of Hardner as per manufecturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 11, Description = "Ensure Application of uniform base as per manufecturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 12, Description = "Tuchup, grinding, undulations, corner repairs and pinholes are filled properly.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 13, Description = "Ensure proper mixing of material as per the manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 14, Description = "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 15, Description = "Check the application of coats as per project requirements / manufacturer recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 16, Description = "Check the rate of application as per the manufacturer recommendation and method statement.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = epoxySections[2].ChecklistSectionId, Sequence = 17, Description = "Check the curing at every stages of application as per manufacturer recommendation.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var epoxyChecklist = new Checklist
        {
            ChecklistId = EpoxyFlooringChecklistId,
            Name = "Epoxy Flooring Checklist",
            Code = "Epoxy-Work",
            Discipline = "CIVIL",
            SubDiscipline = "Flooring",
            PageNumber = 10,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Engineer", "QC Engineer" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(epoxyChecklist, epoxySections, epoxyItems);

        // PAGE 11: Kitchen Cabinets
        var kitchenSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = KitchenCabinetsChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = KitchenCabinetsChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = KitchenCabinetsChecklistId, Title = "C. Cabinet Counter Top", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var kitchenItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored as per manufacturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[0].ChecklistSectionId, Sequence = 3, Description = "Ensure method of statement is being followed.", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[0].ChecklistSectionId, Sequence = 4, Description = "Check location,Size,Color and Thickness of the Cabinets", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 5, Description = "Check Height,Level and Alignment of cabinets", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 6, Description = "Check wethere Drawer and Shelf functioning properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 7, Description = "Check Opening Size and Direction of cabinets", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 8, Description = "Check Shelf Supports (Brackets or Pins)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 9, Description = "Check Joints of the Cabinets", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 10, Description = "Fixing of supports for the countertop is as per approved drawings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 11, Description = "Check Ironmongery is installed as per drawing and free from damanges", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 12, Description = "Check Sink installed Properly and as per location on dwg.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 13, Description = "Check Level of the surface", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 14, Description = "Check Applied Sealant and its Color", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 15, Description = "Check Stove installed Properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[1].ChecklistSectionId, Sequence = 16, Description = "Completion of Nearby Finishes", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[2].ChecklistSectionId, Sequence = 17, Description = "Check for any Surface joints visibility", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[2].ChecklistSectionId, Sequence = 18, Description = "Check location,Size,Color and Thickness", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = kitchenSections[2].ChecklistSectionId, Sequence = 19, Description = "Check Opening Size and location for kitchen Accessories", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var kitchenChecklist = new Checklist
        {
            ChecklistId = KitchenCabinetsChecklistId,
            Name = "Kitchen Cabinets Installation Checklist",
            Code = "Kitchen-Cabinet",
            Discipline = "CIVIL",
            SubDiscipline = "Joinery",
            PageNumber = 11,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Engineer", "QC Engineer" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(kitchenChecklist, kitchenSections, kitchenItems);

        // PAGE 12: Wardrobe Installation
        var wardrobeSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WardrobeInstallationChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WardrobeInstallationChecklistId, Title = "B. Setting Out", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WardrobeInstallationChecklistId, Title = "C. Installation of Wardrobe", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var wardrobeItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, material submittal and drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials (wardrobe, leaf, drawers , iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the color, type, material, coating of leaf, materials are as per approved material approval and project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify and record the DCL product conformity certificate for the sealants to be used.", Reference = "DCL Certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[0].ChecklistSectionId, Sequence = 5, Description = "Ensure the fire rating of the wardrobe are as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[1].ChecklistSectionId, Sequence = 6, Description = "Verify the level and alignment of the wardrobe frame leaf opening with reference to the surrounding wall / cladding elevations.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[1].ChecklistSectionId, Sequence = 7, Description = "Verify the location and clear opening of leafs and drawers are as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[1].ChecklistSectionId, Sequence = 8, Description = "Verify the wardrobe jamb area are solid and rigid.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 9, Description = "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 10, Description = "Verity the fixation, level and alignment of the wardrobe is as per the approved details / drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 11, Description = "Ensure the wardrobe consolidated.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 12, Description = "Ensure the location and No. of leaf hinges provided as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 13, Description = "Ensure required Iron mongery sets are provided as per drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 14, Description = "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 15, Description = "Ensure location/dimensions of drawers are per drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 16, Description = "Ensure the roller slides/drawer runners as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 17, Description = "Verify Cloth Hangers/Tie Holders are installed as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 18, Description = "Ensure the alignment, plumbness and protection of wardrobe to avoid any damages during activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 19, Description = "Ensure wardrobe hanging rail set is installed.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = wardrobeSections[2].ChecklistSectionId, Sequence = 20, Description = "Approval obtained from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var wardrobeChecklist = new Checklist
        {
            ChecklistId = WardrobeInstallationChecklistId,
            Name = "Wardrobe Installation Checklist",
            Code = "Wardrobe-Installation",
            Discipline = "CIVIL",
            SubDiscipline = "Joinery",
            PageNumber = 12,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Engineer", "QC Engineer" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(wardrobeChecklist, wardrobeSections, wardrobeItems);

        // PAGE 13: Doors & Windows Installation
        var doorsWindowsSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DoorsWindowsInstallationChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DoorsWindowsInstallationChecklistId, Title = "B. Setting Out", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DoorsWindowsInstallationChecklistId, Title = "C. Installation of Doors", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DoorsWindowsInstallationChecklistId, Title = "D. Installation of Windows", Order = 4, IsActive = true, CreatedDate = SeedDate }
        };

        var doorsWindowsItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, material submittal and drawings are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials (Door frame, leaf, window frame, window leaf, iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the color, type, material, coating of door and window materials are as per approved material approval and project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify and record the DCL product confirmity certificate for the sealants to be used.", Reference = "DCL Certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[0].ChecklistSectionId, Sequence = 5, Description = "Ensure the fire rating of the doors are as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[1].ChecklistSectionId, Sequence = 6, Description = "Verify the location and clear opening of doors / windows are as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[1].ChecklistSectionId, Sequence = 7, Description = "Verify the level and alignment of the door / window frame opening with reference to the surrounding wall / cladding elevations.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[1].ChecklistSectionId, Sequence = 8, Description = "Verify the door / window jamb area are solid and rigid.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 9, Description = "Verify the fixation of subframe with applicable moisture resistant coating.(if required).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 10, Description = "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 11, Description = "Ensure the door frames are consolidated using foam as per the material approval and project requirements.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 12, Description = "Ensure the location and No. of Door hinges provided as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 13, Description = "Ensure required Iron mongery sets are provided as per the door schedule drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 14, Description = "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 15, Description = "Ensure the door stopper / door coordinator provided as per the approved door schedule.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 16, Description = "Ensure the fire rated doors are provided with fire rated sealant & fire rating tags as per the project requirements.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 17, Description = "Ensure the rubber gaskets are provided at the door jamb as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 18, Description = "Verify the undercut for the door leaves as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 19, Description = "Ensure the alignment, plumbness and protection of door leafs to avoid any damages during construction activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 20, Description = "Ensure the door architrave are fixed as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[2].ChecklistSectionId, Sequence = 21, Description = "Approval obtained from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 22, Description = "Ensure the completion of finishes around the jamb area prior to installation.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 23, Description = "Ensure the drip flashings are provided properly around the frame as per the approved drawings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 24, Description = "Ensure the overlapping of the flashings around the frame are as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 25, Description = "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 26, Description = "Ensure the location and No. of hinges, iron mongeries provided as per the approved drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 27, Description = "Ensure the alignment, plumbness, opening direction and protection of window shutters to avoid any damages during construction activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 28, Description = "Verify the uniform thickness, colour and application of the sealant all around the window frame.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = doorsWindowsSections[3].ChecklistSectionId, Sequence = 29, Description = "Approval obtained from Consultant/Client to proceed with further activities.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var doorsWindowsChecklist = new Checklist
        {
            ChecklistId = DoorsWindowsInstallationChecklistId,
            Name = "Doors & Windows Installation Checklist",
            Code = "ASA-IMS-FRM-13-051",
            Discipline = "CIVIL",
            SubDiscipline = "Doors & Windows",
            PageNumber = 13,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(doorsWindowsChecklist, doorsWindowsSections, doorsWindowsItems);

        // PAGE 15: Fire Sealing Works
        var fireSealingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireSealingWorksChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireSealingWorksChecklistId, Title = "B. Preparation Works", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireSealingWorksChecklistId, Title = "C. Application", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var fireSealingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure relevant shop drawing is approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure H&S method statement is submitted", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[0].ChecklistSectionId, Sequence = 3, Description = "Check all the materials are approved by consultant.", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[0].ChecklistSectionId, Sequence = 4, Description = "Check the availablity mockup approval for the fire sealing works (around penetrations, partitions, etc.).", Reference = "Mockup Approval", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[1].ChecklistSectionId, Sequence = 5, Description = "Check the MEP penetrations are installed, inspected and approved.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[1].ChecklistSectionId, Sequence = 6, Description = "Check the surface for any latiance, dirt, loose materials around the penetrations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[1].ChecklistSectionId, Sequence = 7, Description = "Check the dampness of the substrate prior to application (if required as per manufaturer recommendations)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[1].ChecklistSectionId, Sequence = 8, Description = "Check the mixing of materials as per the manufacturer recommandations", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[2].ChecklistSectionId, Sequence = 9, Description = "Check the application covers the whole penetrations without any gap.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[2].ChecklistSectionId, Sequence = 10, Description = "Finishing of the openings shall be as per the approved mockup.", Reference = "Mockup Approval", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireSealingSections[2].ChecklistSectionId, Sequence = 11, Description = "Check the final cleaning and sealant (if required) applied as per the approved mockup.", Reference = "Mockup Approval", IsActive = true, CreatedDate = SeedDate }
        };

        var fireSealingChecklist = new Checklist
        {
            ChecklistId = FireSealingWorksChecklistId,
            Name = "Fire Sealing Works Checklist",
            Code = "ASA-IMS-FRM-13-069",
            Discipline = "CIVIL",
            SubDiscipline = "Fire Protection",
            PageNumber = 15,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "Sub Contractor" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(fireSealingChecklist, fireSealingSections, fireSealingItems);

        // PAGE 16: Aluminium & Glazing
        var aluminiumSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = AluminiumGlazingChecklistId, Title = "A. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = AluminiumGlazingChecklistId, Title = "B. Surface Preparation", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = AluminiumGlazingChecklistId, Title = "C. Installation of Aluminium and glazing", Order = 3, IsActive = true, CreatedDate = SeedDate }
        };

        var aluminiumItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure materials are stored as per manufacturers recommendations.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[0].ChecklistSectionId, Sequence = 3, Description = "Verify the expiry date of the material prior to applications.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[1].ChecklistSectionId, Sequence = 4, Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[1].ChecklistSectionId, Sequence = 5, Description = "Check for repair of surface imperfection and protrusions (if any).", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[1].ChecklistSectionId, Sequence = 6, Description = "Ensure the protection of nearby finishes / MEP services.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[1].ChecklistSectionId, Sequence = 7, Description = "Check the MEP clearance prior to start of aluminium works.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 8, Description = "Profile and glass country of origin / Manufacturer", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 9, Description = "Type, Size, Colour, Thickness and Opening Direction", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 10, Description = "Varify Size of opening as per drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 11, Description = "Varify Location of opening for curtain wall, door and windows as per drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 12, Description = "Ironmongery is installed and free from damages", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 13, Description = "Sealant is applied properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 14, Description = "water leak test is performed and no leakage is found", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 15, Description = "varify Functioning/movement of panels is as free as required", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 16, Description = "Varify the Rigidity of frame and moveable panels", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = aluminiumSections[2].ChecklistSectionId, Sequence = 17, Description = "Varify Line and Level", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var aluminiumChecklist = new Checklist
        {
            ChecklistId = AluminiumGlazingChecklistId,
            Name = "Aluminium & Glazing Checklist",
            Code = "Aluminium-Glazing",
            Discipline = "CIVIL",
            SubDiscipline = "Aluminium Works",
            PageNumber = 16,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Engineer", "QC Engineer" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(aluminiumChecklist, aluminiumSections, aluminiumItems);

        // PAGE 17: Pre-Loading Civil Checklist
        var preLoadingCivilSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "1. General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "2. Final Inspection - Structural", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "3. Internal and External Painting", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "4. Floor and Wall Tiling", Order = 4, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "5. Dry wall", Order = 5, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "6. False Ceiling Work", Order = 6, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "7. Aluminium and Glazing Works", Order = 7, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "8. Wooden/Metal Doors and Wood Works", Order = 8, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "9. Other Finishes", Order = 9, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingCivilChecklistId, Title = "10. Others", Order = 10, IsActive = true, CreatedDate = SeedDate }
        };

        var preLoadingCivilItems = new List<PredefinedChecklistItem>
        {
            // Section 1: General
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure method statement, ITP, materials and shop drawings are approved", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[0].ChecklistSectionId, Sequence = 2, Description = "Check identification tag of the modular", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[0].ChecklistSectionId, Sequence = 3, Description = "Visually inspect the modular for any defects or damages", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify the method of loading as per the project / design requirements", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 2: Structural
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[1].ChecklistSectionId, Sequence = 1, Description = "Internal and External Dimensions of the modular", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Section 3: Painting
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 1, Description = "Location and color of Painting as per the App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 2, Description = "Internal Paint (Application of Primer, Stucco and 1st Coat of Paint)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 3, Description = "External Paint(Application of Primer, Filler and Final Coat Texture Paint)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 4, Description = "Ensure Paint touch ups are completed around installed items.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 5, Description = "Bitumin Applied at required Areas", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[2].ChecklistSectionId, Sequence = 6, Description = "Damages, If any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 4: Tiling
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 1, Description = "Layout and Fixing of Tiles as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 2, Description = "Line, Level and Spacer for the Installed Tiles", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 3, Description = "Skirting is installed/fixed properly and truly vertical", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 4, Description = "Grouting of all Joints is done properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 5, Description = "Elastomeric sealant under skirting is provided properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 6, Description = "Cleaning of corners and edges removing exccessive paint on skirting", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 7, Description = "Ensure Drainhole are free from any debris and properly closed (if applicable)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[3].ChecklistSectionId, Sequence = 8, Description = "Damages, if any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 5: Drywall
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[4].ChecklistSectionId, Sequence = 1, Description = "Layout, location and position of dry wall is as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[4].ChecklistSectionId, Sequence = 2, Description = "Thickness of Dry wall is as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[4].ChecklistSectionId, Sequence = 3, Description = "Opening for MEP services are cut properly.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[4].ChecklistSectionId, Sequence = 4, Description = "Ensure Gypsum surface are Crackfree at joints.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[4].ChecklistSectionId, Sequence = 5, Description = "Damages, if any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 6: False Ceiling
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[5].ChecklistSectionId, Sequence = 1, Description = "Layout of False Ceiling tiles and bulk head as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[5].ChecklistSectionId, Sequence = 2, Description = "Height of the False Ceiling as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[5].ChecklistSectionId, Sequence = 3, Description = "Access panels/ Ceiling tiles are Fixed Properly", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[5].ChecklistSectionId, Sequence = 4, Description = "Ensure Gypsum surface are Crackfree at joints.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[5].ChecklistSectionId, Sequence = 5, Description = "Damages, if any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 7: Aluminium and Glazing
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 1, Description = "Location of Window as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 2, Description = "Fixing of Glass/panels", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 3, Description = "Fixing of Iron-Mongery and Accessories", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 4, Description = "Fixing of Silicone Sealant", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 5, Description = "Water leak test performed and passed.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 6, Description = "Paint touch completed around the frame.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[6].ChecklistSectionId, Sequence = 7, Description = "Damages, if any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 8: Doors and Wood Works
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 1, Description = "Location of Doors as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 2, Description = "Direction of doors swing as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 3, Description = "Main enterance door as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 4, Description = "Lock of Main enterance door is installed", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 5, Description = "Architraves are fixed as per Drawing around main entrance door", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 6, Description = "Pod door is installed as per App Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 7, Description = "Lock of Main Pod door is installed", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 8, Description = "Architraves are fixed as per Drawing around Pod door", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 9, Description = "Locking of Doors and Shutters securely to avoid movement during transportation", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 10, Description = "Kitchen cabinets, counter top and accessories installed as per app drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 11, Description = "Kitchen sink and sink mixer installed", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 12, Description = "Wardrobe installed as per approved drawings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 13, Description = "Wardrobe doors and drawers funtioning smoothly and free from scratches", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[7].ChecklistSectionId, Sequence = 14, Description = "Damages, if any", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 9: Other Finishes
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 1, Description = "Mirror installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 2, Description = "Threshold installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 3, Description = "Glass Partition installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 4, Description = "Floor drain and covers installed and free from damages", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 5, Description = "Vanity installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 6, Description = "WC and cover installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 7, Description = "Shower installted and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 8, Description = "Gypsum board are free from pealing off", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 9, Description = "Painted walls are clean and free from stains.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 10, Description = "Tiles are fixed with grouting properly and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 11, Description = "Toilet accessories installed and free from damage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[8].ChecklistSectionId, Sequence = 12, Description = "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes & MEP fittings.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Section 10: Others
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[9].ChecklistSectionId, Sequence = 1, Description = "Check Final Condition of outside of the room and ensure its damage free", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingCivilSections[9].ChecklistSectionId, Sequence = 2, Description = "Sign the delivery note for accepting the loading of precast modular in good condition", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var preLoadingCivilChecklist = new Checklist
        {
            ChecklistId = PreLoadingCivilChecklistId,
            Name = "Check List for Pre-Loading of Completed Precast Modular",
            Code = "Pre-Loading-Civil",
            Discipline = "CIVIL",
            SubDiscipline = "Pre-Loading",
            PageNumber = 17,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "QC", "MEP Works" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(preLoadingCivilChecklist, preLoadingCivilSections, preLoadingCivilItems);

        // PAGE 20: Material Receiving Inspection
        var materialReceivingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = Guid.Parse("50000000-0000-0000-0000-000000000020"), Title = "A. Inspection Items", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var materialReceivingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 1, Description = "Review documents for received materials", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 2, Description = "Materials outside visual checking", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 3, Description = "Check for any damages (General & Visual)", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify original bill of landing / Delivery Note", Reference = "DO", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 5, Description = "Supplier Certificate / Warranty letter", Reference = "Certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 6, Description = "Check and Verify the material as per delivery list / details.", Reference = "DO", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 7, Description = "Check the accessories.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 8, Description = "Check the Name Plate.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 9, Description = "Materials Storage and preservation as per manufacturer.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 10, Description = "Check the identification of components.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 11, Description = "Check the rating as per approved drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 12, Description = "Check the loose part.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 13, Description = "Check the dimension of delivered equipment as per approved drawing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 14, Description = "Check the availability of spare breakers / relays/ terminals.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 15, Description = "Delivered material photos.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = materialReceivingSections[0].ChecklistSectionId, Sequence = 16, Description = "Material Site test.", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var materialReceivingChecklist = new Checklist
        {
            ChecklistId = MaterialReceivingMEPChecklistId,
            Name = "Material Receiving Inspection",
            Code = "Material-Receiving-MEP",
            Discipline = "MEP",
            SubDiscipline = "General",
            PageNumber = 20,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Amana", "Consultant", "Client" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(materialReceivingChecklist, materialReceivingSections, materialReceivingItems);

        // PAGE 21: Installation of HVAC Duct
        var hvacDuctSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = HVACDuctInstallationChecklistId, Title = "A. Installation of HVAC Duct", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var hvacDuctItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure the materials as per approved material submittal.", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure drawings are used for installation are current and approved.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 3, Description = "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 4, Description = "The joints and flanges are correctly made, jointed and sealed.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 5, Description = "Check that duct joints are sealed externally with approved sealant.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 6, Description = "Check fixing of supports and spacing as approved drawings & submittal.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 7, Description = "Check acoustic lining is properly fastened and un damaged.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 8, Description = "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 9, Description = "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = hvacDuctSections[0].ChecklistSectionId, Sequence = 10, Description = "Ensure that the identification & labeling are provided for duct works.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var hvacDuctChecklist = new Checklist
        {
            ChecklistId = HVACDuctInstallationChecklistId,
            Name = "Installation of HVAC Duct",
            Code = "AA-ITP-9004-ME-02",
            Discipline = "MEP",
            SubDiscipline = "HVAC",
            PageNumber = 21,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specifications Section- 233113 & 230713" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(hvacDuctChecklist, hvacDuctSections, hvacDuctItems);

        // PAGE 22: Installation of Above Ground Drainage Pipes
        var drainageInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DrainagePipesInstallationChecklistId, Title = "A. Installation of Above Ground Drainage Pipes", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var drainageInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure the materials are approved", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "No visible damage on the materials", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Pipe sizes are as per approved shop drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Pipe layout/routing as per approved shop drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Slope of the pipes as per approved shop drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Installation as per approved method statement", Reference = "Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Sleeves are provided for the pipes passing through the walls /slabs", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Check the pipes are supported well with approved clamps.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Ensure Drainage pipes are not passing above electrical services", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Installed pipes are free of sag & bend", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Drainage pipes are connected to the vent system", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageInstallSections[0].ChecklistSectionId, Sequence = 12, Description = "Pipe joints are properly made and are tight / secure", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var drainageInstallChecklist = new Checklist
        {
            ChecklistId = DrainagePipesInstallationChecklistId,
            Name = "Installation of Above Ground Drainage Pipes",
            Code = "AA-ITP-8501-DR-02",
            Discipline = "MEP",
            SubDiscipline = "Drainage",
            PageNumber = 22,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 221300" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(drainageInstallChecklist, drainageInstallSections, drainageInstallItems);

        // PAGE 23: Leak Test of Above Ground Drainage Pipes
        var drainageLeakTestSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DrainagePipesLeakTestChecklistId, Title = "A. Leak Test of Above Ground Drainage Pipes", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var drainageLeakTestItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageLeakTestSections[0].ChecklistSectionId, Sequence = 1, Description = "Installation of Drainage Pipe has completed", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageLeakTestSections[0].ChecklistSectionId, Sequence = 2, Description = "All pipe joints shall be inspected.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageLeakTestSections[0].ChecklistSectionId, Sequence = 3, Description = "Drainage pipe should generally be subjected to an internal pressure test of 3m head of water above the crown of the pipe at the high end", Reference = "Project specification", IsActive = true, CreatedDate = SeedDate }
        };

        var drainageLeakTestChecklist = new Checklist
        {
            ChecklistId = DrainagePipesLeakTestChecklistId,
            Name = "Leak Test of Above Ground Drainage Pipes",
            Code = "AA-ITP-8501-DR-03",
            Discipline = "MEP",
            SubDiscipline = "Drainage",
            PageNumber = 23,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 221300" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(drainageLeakTestChecklist, drainageLeakTestSections, drainageLeakTestItems);

        // PAGE 24: Test Report - Drainage Pipes
        var drainageTestReportSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = DrainagePipesTestReportChecklistId, Title = "Test Parameters", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var drainageTestReportItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 1, Description = "Site Location: Dubox Factory", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 2, Description = "System to be Tested: Drainage", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 3, Description = "Test Fluid: Potable water", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 4, Description = "Duration of Test: 4 Hours", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 5, Description = "Start Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = drainageTestReportSections[0].ChecklistSectionId, Sequence = 6, Description = "Finish Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var drainageTestReportChecklist = new Checklist
        {
            ChecklistId = DrainagePipesTestReportChecklistId,
            Name = "Installation and Testing of Above Ground Drainage Pipes and fittings - Test Report",
            Code = "Test-Report-Drainage",
            Discipline = "MEP",
            SubDiscipline = "Drainage",
            PageNumber = 24,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(drainageTestReportChecklist, drainageTestReportSections, drainageTestReportItems);

        // PAGE 25: Installation of Above Ground Water Supply Pipes
        var waterSupplyInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WaterSupplyInstallationChecklistId, Title = "A. Installation of Above ground Water Supply pipes and fittings", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure the materials are approved", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "No visible damage on the materials.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Pipe sizes are as per approved shop drawing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Pipe layout/routing as per approved shop drawing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Installation as per approved method statement.", Reference = "Approved MST", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Sleeves are provided for the pipes passing through the walls /slabs.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Check the Horizontal pipes are supported well and with approved clamps.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Check the vertical riser of the pipes are supported well with approved clamp.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Water pipes are not passing above electrical services.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Installed pipes are free of sag & bend.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Pipe joints are properly made and are tight / secure", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyInstallChecklist = new Checklist
        {
            ChecklistId = WaterSupplyInstallationChecklistId,
            Name = "Installation Above ground Water Supply pipes and fittings",
            Code = "AA-ITP-8001-WS-02",
            Discipline = "MEP",
            SubDiscipline = "Water Supply",
            PageNumber = 25,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 221116" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(waterSupplyInstallChecklist, waterSupplyInstallSections, waterSupplyInstallItems);

        // PAGE 26: Testing of Above Ground Water Supply Pipes
        var waterSupplyTestingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WaterSupplyTestingChecklistId, Title = "A. Testing of Above ground Water Supply pipes and fittings", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyTestingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestingSections[0].ChecklistSectionId, Sequence = 1, Description = "Installation of Water Supply pipes completed", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure Pressure Gauge used is Calibrated", Reference = "Calibration Certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestingSections[0].ChecklistSectionId, Sequence = 3, Description = "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure", Reference = "Project Spec.-221116", IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyTestingChecklist = new Checklist
        {
            ChecklistId = WaterSupplyTestingChecklistId,
            Name = "Testing of Above ground Water Supply pipes and fittings",
            Code = "AA-ITP-8001-WS-03",
            Discipline = "MEP",
            SubDiscipline = "Water Supply",
            PageNumber = 26,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 221116" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(waterSupplyTestingChecklist, waterSupplyTestingSections, waterSupplyTestingItems);

        // PAGE 27: Test Report - Water Supply
        var waterSupplyTestReportSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = WaterSupplyTestReportChecklistId, Title = "Test Parameters", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyTestReportItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 1, Description = "Site Location: Dubox Factory", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 2, Description = "System to be Tested: Water Supply", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 3, Description = "Test Fluid: Potable water", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 4, Description = "Test Pressure (Bar): 8 Bar", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 5, Description = "Duration of Test: 2 Hours", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 6, Description = "Start Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = waterSupplyTestReportSections[0].ChecklistSectionId, Sequence = 7, Description = "Finish Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var waterSupplyTestReportChecklist = new Checklist
        {
            ChecklistId = WaterSupplyTestReportChecklistId,
            Name = "Installation and Testing of Above Ground Water Supply Pipes and fittings - Test Report",
            Code = "Test-Report-Water-Supply",
            Discipline = "MEP",
            SubDiscipline = "Water Supply",
            PageNumber = 27,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(waterSupplyTestReportChecklist, waterSupplyTestReportSections, waterSupplyTestReportItems);

        // PAGE 28: Installation of Above Ground Fire Fighting Pipes
        var fireFightingInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireFightingInstallationChecklistId, Title = "A. Installation of Above Ground Fire Fighting pipes system", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Check the piping, fitting, installation and valves materials are as per specification and approved material submittal", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "Checks the piping layouts are as per the approved shop drawing and site conditions.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the approved supports& accessories are used for installation firefighting piping &Accessories.", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Checks the distance between the supports are maintained as per specification and method statement.", Reference = "Approved Drawing & MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Checks the proper sizing's of hangers are used as per specification.", Reference = "Approved Drawing & Project Specification", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Check threaded joints are provided for 2\" and below and grooved fitting are provided for 2 ½\" and above in piping.", Reference = "Project Specification & Approved MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Check location / size of valves are provided as per approved shop drawing and specification.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Check unions are provided for 2\" and below piping in direction of flow.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Check flexible pipe connector/expansion compensator are installed in expansion joints and equipment connections.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Check drain points are provided in lowest piping points.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Check air vents are provided in highest piping points.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingInstallChecklist = new Checklist
        {
            ChecklistId = FireFightingInstallationChecklistId,
            Name = "Installation of Above Ground Fire Fighting pipes system",
            Code = "AA-ITP-6001-FP-02",
            Discipline = "MEP",
            SubDiscipline = "Fire Fighting",
            PageNumber = 28,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 211100" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(fireFightingInstallChecklist, fireFightingInstallSections, fireFightingInstallItems);

        // PAGE 29: Testing of Above Ground Fire Fighting Pipes
        var fireFightingTestingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireFightingTestingChecklistId, Title = "A. Testing of Above Ground Fire Fighting pipes and fittings", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingTestingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestingSections[0].ChecklistSectionId, Sequence = 1, Description = "Fire fighting pipes installation completed", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestingSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure the Pressure Gauge used are calibrated", Reference = "Calibration certificate", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestingSections[0].ChecklistSectionId, Sequence = 3, Description = "The system shall be subjected to a hydrostatic pressure test, to the satisfaction and in the presence of the Engineer. Pressure shall be 1.5 times than working pressure in accordance to NFPA 13. Test shall be maintained for two hours as a minimum.", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingTestingChecklist = new Checklist
        {
            ChecklistId = FireFightingTestingChecklistId,
            Name = "Testing of Above Ground Fire Fighting pipes and fittings",
            Code = "AA-ITP-6001-FP-03",
            Discipline = "MEP",
            SubDiscipline = "Fire Fighting",
            PageNumber = 29,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 211100" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(fireFightingTestingChecklist, fireFightingTestingSections, fireFightingTestingItems);

        // PAGE 30: Test Report - Fire Fighting
        var fireFightingTestReportSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = FireFightingTestReportChecklistId, Title = "Test Parameters", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingTestReportItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 1, Description = "Site Location: Dubox Factory", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 2, Description = "System to be Tested: Fire Fighting", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 3, Description = "Test Fluid: Potable water", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 4, Description = "Test Pressure (Bar): 200 psi", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 5, Description = "Duration of Test: 2 Hours", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 6, Description = "Start Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = fireFightingTestReportSections[0].ChecklistSectionId, Sequence = 7, Description = "Finish Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var fireFightingTestReportChecklist = new Checklist
        {
            ChecklistId = FireFightingTestReportChecklistId,
            Name = "Installation and Testing of Above ground Fire Fighting Pipes and fittings - Test Report",
            Code = "Test-Report-Fire-Fighting",
            Discipline = "MEP",
            SubDiscipline = "Fire Fighting",
            PageNumber = 30,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(fireFightingTestReportChecklist, fireFightingTestReportSections, fireFightingTestReportItems);

        // PAGE 31: Installation of Refrigerant Pipe
        var refrigerantInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = RefrigerantPipeInstallationChecklistId, Title = "A. Installation of Refrigerant Pipe", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Ensure the materials as per approved material submittal.", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "No visible damage on the materials.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Pipe sizes are as per approved shop drawing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Pipe layout/routing as per approved shop drawing.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Installation as per approved method statement.", Reference = "MTS", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Sleeves are provided for the pipes passing through the walls/slabs.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Check the Horizontal pipes are supported well and with approved clamps.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Check the vertical riser of the pipes are supported well with approved clamp.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Pipe joints are properly brazed with no leak", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Ensure the proper insulation done.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Ensure the cladding and sealant on the cladding joints.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantInstallSections[0].ChecklistSectionId, Sequence = 12, Description = "Ensure the system charged with approved refrigerant", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantInstallChecklist = new Checklist
        {
            ChecklistId = RefrigerantPipeInstallationChecklistId,
            Name = "Installation of Refrigerant Pipe",
            Code = "AA-ITP-9003-ME-02",
            Discipline = "MEP",
            SubDiscipline = "HVAC",
            PageNumber = 31,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specifications Section- 232300" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(refrigerantInstallChecklist, refrigerantInstallSections, refrigerantInstallItems);

        // PAGE 32: Pressure Testing of Refrigerant Pipe
        var refrigerantTestingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = RefrigerantPipeTestingChecklistId, Title = "A. Pressure Testing of Refrigerant Pipe", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantTestingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestingSections[0].ChecklistSectionId, Sequence = 1, Description = "Installation of FAHU, FCU & VRF units are completed", Reference = "Project Spec.-237400 & 238129", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestingSections[0].ChecklistSectionId, Sequence = 2, Description = "Installation of Refrigerant pipes completed", Reference = "Project Spec.-232300", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestingSections[0].ChecklistSectionId, Sequence = 3, Description = "Installation inspection completed", Reference = "Project Spec.-232300", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestingSections[0].ChecklistSectionId, Sequence = 4, Description = "Pressure Test: After installation, charge system and test for leaks. Repair leaks and retest until no leaks exist.", Reference = "Project Spec.-232300", IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantTestingChecklist = new Checklist
        {
            ChecklistId = RefrigerantPipeTestingChecklistId,
            Name = "Pressure Testing of Refrigerant Pipe",
            Code = "AA-ITP-9003-ME-03",
            Discipline = "MEP",
            SubDiscipline = "HVAC",
            PageNumber = 32,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specifications Section- 232300" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(refrigerantTestingChecklist, refrigerantTestingSections, refrigerantTestingItems);

        // PAGE 33: Test Report - Refrigerant Pipes
        var refrigerantTestReportSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = RefrigerantPipeTestReportChecklistId, Title = "Test Parameters", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantTestReportItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 1, Description = "Site Location: Dubox Factory", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 2, Description = "System to be Tested: Refrigerant pipe", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 3, Description = "Test Fluid: Nitrogen", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 4, Description = "Test Pressure (Bar): 39 bar", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 5, Description = "Duration of Test: 24 hrs", Reference = "Project Spec", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 6, Description = "Start Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = refrigerantTestReportSections[0].ChecklistSectionId, Sequence = 7, Description = "Finish Time of Test", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var refrigerantTestReportChecklist = new Checklist
        {
            ChecklistId = RefrigerantPipeTestReportChecklistId,
            Name = "Installation and Testing of Refrigerant Pipes and fittings - Test Report",
            Code = "Test-Report-Refrigerant",
            Discipline = "MEP",
            SubDiscipline = "HVAC",
            PageNumber = 33,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(refrigerantTestReportChecklist, refrigerantTestReportSections, refrigerantTestReportItems);

        // PAGE 34: Installation of LV Cables & Wires
        var lvCablesInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = LVCablesInstallationChecklistId, Title = "A. Installation of LV Cables & Wires", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Check all materials installed are as per approved material submittal.", Reference = "MAR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "Dimensions as per approved drawings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "the cable/wire provided with Ties appropriately?", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Verify measurement of lengths.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Ensure that there are no damages to the cables", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Verify that identification, grouping, spacing, markings and clamps are as required.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Ensure the cable laying in trunking, trays and conduit installations are conducted in approved and accepted manner & as per project specification", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Ensure that cable type and routes are as per approved Drawing", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Ensure that correct types and sizes of wires and LV Cables are installed", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Verify that routes and marked locations are correct as per approved drawings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Is cable/wire provided with Lugs?", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 12, Description = "Is the cable/wire termination done correctly", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 13, Description = "Before pulling wires in conduit check that inside of conduit (and raceway in general) is free of burrs and is dry and clean.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 14, Description = "Ensure that extra Length left at every branch circuit outlet and pull-box, every cable passing through in order to allow inspection and for connections to be made.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesInstallSections[0].ChecklistSectionId, Sequence = 15, Description = "Ensure that tightening of electrical connectors and terminals including screws and bolts, in accordance with manufacturers recommendation", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesInstallChecklist = new Checklist
        {
            ChecklistId = LVCablesInstallationChecklistId,
            Name = "Installation of LV Cables & Wires",
            Code = "AA-ITP-5005-EL-02",
            Discipline = "MEP",
            SubDiscipline = "Electrical",
            PageNumber = 34,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(lvCablesInstallChecklist, lvCablesInstallSections, lvCablesInstallItems);

        // PAGE 35: Testing of LV Cables & Wires
        var lvCablesTestingSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = LVCablesTestingChecklistId, Title = "A. Testing of LV Cables & Wires", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesTestingItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestingSections[0].ChecklistSectionId, Sequence = 1, Description = "Is the Cable/Wire Continuity test Setup, okay?", Reference = "Project Spec.-260513", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestingSections[0].ChecklistSectionId, Sequence = 2, Description = "Is the Cable/Wire Megger test Setup, okay?", Reference = "Project Spec.-260513", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestingSections[0].ChecklistSectionId, Sequence = 3, Description = "Ensure Megger test is performed for Cables and accepted", Reference = "Project Spec.-260513-1.7 C", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestingSections[0].ChecklistSectionId, Sequence = 4, Description = "Ensure Continuity test performed and accepted", Reference = "Project Spec.-260513-1.7 C", IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesTestingChecklist = new Checklist
        {
            ChecklistId = LVCablesTestingChecklistId,
            Name = "Testing of LV Cables & Wires",
            Code = "AA-ITP-5005-EL-03",
            Discipline = "MEP",
            SubDiscipline = "Electrical",
            PageNumber = 35,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 260513" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(lvCablesTestingChecklist, lvCablesTestingSections, lvCablesTestingItems);

        // PAGE 36: Electrical Test Results
        var lvCablesTestResultSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = LVCablesTestResultChecklistId, Title = "Type A - Circuit Testing", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesTestResultItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 1, Description = "Toilet lighting - 2.5mm² - Y1", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 2, Description = "Room Lighting - 2.5mm² - B1", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 3, Description = "FCU - 4mm² - B2", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 4, Description = "General Purpose Power-usb - 4mm² - Y3", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 5, Description = "Shaver - 4mm² - B3", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 6, Description = "Hob - 6mm² - R4", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 7, Description = "Water Kettle - 4mm² - Y4", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 8, Description = "Washing Machine - 4mm² - B4", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 9, Description = "Kitchen hood - 4mm² - R5", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 10, Description = "Water cooler/Refrigeration - 4mm² - Y5", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 11, Description = "General Purpose Power-TV - 4mm² - B5", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 12, Description = "Microwave oven - 4mm² - Y6", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvCablesTestResultSections[0].ChecklistSectionId, Sequence = 13, Description = "Living UD - 4mm² - B6", Reference = "General", IsActive = true, CreatedDate = SeedDate }
        };

        var lvCablesTestResultChecklist = new Checklist
        {
            ChecklistId = LVCablesTestResultChecklistId,
            Name = "Electrical Systems - Circuit Insulation Resistance Test",
            Code = "Electrical-Test-Results",
            Discipline = "MEP",
            SubDiscipline = "Electrical",
            PageNumber = 36,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Amana Operation", "Amana QC", "Employer / Consultant" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(lvCablesTestResultChecklist, lvCablesTestResultSections, lvCablesTestResultItems);

        // PAGE 40: Installation of LV Panels
        var lvPanelsInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = LVPanelsInstallationChecklistId, Title = "A. Installation of LV Panels", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var lvPanelsInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Verify the installed Panel boards have approved submittals.", Reference = "MAR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "Ensure the drawings used for installation are correct and approved.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Align, level and securely fasten panelboards to structure", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Check the Name Plate and Identification labels as per load schedule and approved submittals.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Fix surface mounted outdoor panelboards at least 25mm from wall ensuring supporting members do not prevent flow of air.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Do not use connecting conduits to support panelboards.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Panelboard Interiors: Do not install in cabinets until all conduit connections to cabinet have been completed", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Wiring: Inside Panelboards: Neatly arranged, accessible and strapped to prevent tension on circuit breaker terminals. Tap-off connections shall be split and bolted type, fully insulated.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Equipment grounding for LV Panel Board is provided", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 11, Description = "Ensure that all unused openings are closed in the panels", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 12, Description = "Ensure that updated circuit directory and other relevant documents provided in the panel directory accordingly.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 13, Description = "Ensure that the Panel fixing bolts have been tightened with the suitable nut and washer.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 14, Description = "Touch up and cleaning of the panel", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = lvPanelsInstallSections[0].ChecklistSectionId, Sequence = 15, Description = "Ensure that each Panel Marking should be done as per approved drawing", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var lvPanelsInstallChecklist = new Checklist
        {
            ChecklistId = LVPanelsInstallationChecklistId,
            Name = "Installation of LV Panels",
            Code = "AA-ITP-5006-EL-02",
            Discipline = "MEP",
            SubDiscipline = "Electrical",
            PageNumber = 40,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 262416" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(lvPanelsInstallChecklist, lvPanelsInstallSections, lvPanelsInstallItems);

        // PAGE 41: Installation of Conduits & Accessories
        var conduitsInstallSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = ConduitsInstallationChecklistId, Title = "A. Installation of Conduits & accessories", Order = 1, IsActive = true, CreatedDate = SeedDate }
        };

        var conduitsInstallItems = new List<PredefinedChecklistItem>
        {
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 1, Description = "Check the conduits and accessories are as per approved material submittal.", Reference = "MA", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 2, Description = "Check and ensure the drawings used for installation are current and approved.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 3, Description = "Check the conduits and other associated material are new and undamaged.", Reference = "MIR", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 4, Description = "Check that the conduits are leveled and aligned properly.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 5, Description = "Check that the conduits are securely fixed.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 6, Description = "Check and ensure that the conduits and back boxes are sizely adequated.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 7, Description = "Check that the bottom hight of the back boxes is as per the shop drawing.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 8, Description = "Check that a minimum of 5mm thick cover is provided for conduits concealed in walls.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 9, Description = "Check the installation of conduits is co-ordinated with other services.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = conduitsInstallSections[0].ChecklistSectionId, Sequence = 10, Description = "Check the installation of conduiting as per approved drawings.", Reference = "Approved Shop Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var conduitsInstallChecklist = new Checklist
        {
            ChecklistId = ConduitsInstallationChecklistId,
            Name = "Installation of Conduits & accessories",
            Code = "AA-ITP-5004-EL-02",
            Discipline = "MEP",
            SubDiscipline = "Electrical",
            PageNumber = 41,
            ReferenceDocumentsJson = JsonSerializer.Serialize(new List<string> { "Project Specification: Section- 260533" }),
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "QC Inspector", "PMC", "AMAALA Representative" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(conduitsInstallChecklist, conduitsInstallSections, conduitsInstallItems);

        // PAGES 42-43: Pre-Loading MEP Checklist
        var preLoadingMEPSections = new List<ChecklistSection>
        {
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "General", Order = 1, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - HVAC", Order = 2, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Refrigerant pipes", Order = 3, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Firefighting", Order = 4, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Water Supply", Order = 5, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Drainage", Order = 6, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Risers", Order = 7, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Electrical", Order = 8, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Wiring Devices", Order = 9, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Wire, Cables, Conduits and accessories", Order = 10, IsActive = true, CreatedDate = SeedDate },
            new ChecklistSection { ChecklistSectionId = GetNextSectionId(), ChecklistId = PreLoadingMEPChecklistId, Title = "Final Inspection - Light Fittings", Order = 11, IsActive = true, CreatedDate = SeedDate }
        };

        var preLoadingMEPItems = new List<PredefinedChecklistItem>
        {
            // General section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[0].ChecklistSectionId, Sequence = 1, Description = "Check identification tag of the modular", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[0].ChecklistSectionId, Sequence = 2, Description = "Visually inspect the MEP Services for any defects or damages", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // HVAC section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 1, Description = "Installation of Supply Duct", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 2, Description = "Installation of Return Duct", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 3, Description = "Installation of Fresh Air Duct", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 4, Description = "Installation of Exhaust Air Duct", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 5, Description = "Installation of Kitchen Hood and Flexible Duct", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 6, Description = "Installation of VCD", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 7, Description = "Installation of Fan Coil Unit", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 8, Description = "Installation of Fire Damper and Back Draft Damper", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[1].ChecklistSectionId, Sequence = 9, Description = "Installation of Grills/Diffuser", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Refrigerant pipes section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[2].ChecklistSectionId, Sequence = 10, Description = "Installation of Pipes and fittings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[2].ChecklistSectionId, Sequence = 11, Description = "Insulation of pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[2].ChecklistSectionId, Sequence = 12, Description = "Check Pipe Insulation and adhesive.", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[2].ChecklistSectionId, Sequence = 13, Description = "Pressure testing of the Piping", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Firefighting section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[3].ChecklistSectionId, Sequence = 15, Description = "Application of Primer paint", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[3].ChecklistSectionId, Sequence = 16, Description = "Application of Paint final coating", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[3].ChecklistSectionId, Sequence = 17, Description = "Installation of Sprinklers", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[3].ChecklistSectionId, Sequence = 18, Description = "Pressure testing of the Piping", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Water Supply section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 19, Description = "Installation of Pipes and fittings", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 20, Description = "Water Hammer Arrestor installed in the approved location.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 21, Description = "Pressure testing of the Piping", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 22, Description = "Hot water pipes insulated.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 23, Description = "Gate Valve Installation", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[4].ChecklistSectionId, Sequence = 24, Description = "Kitchen sink and accessories", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Drainage section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[5].ChecklistSectionId, Sequence = 23, Description = "Installation of Floor Cleanout (FCO)", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[5].ChecklistSectionId, Sequence = 24, Description = "Installation of Floor Drain", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[5].ChecklistSectionId, Sequence = 25, Description = "Installation CDP Pipes", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[5].ChecklistSectionId, Sequence = 26, Description = "Piping Leak test", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[5].ChecklistSectionId, Sequence = 27, Description = "Sleeves provided for drain pipes outlets.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Risers section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 28, Description = "Soil Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 29, Description = "Waste Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 30, Description = "Vent Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 31, Description = "Water Supply Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 32, Description = "Refrigerant Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 33, Description = "Firefighting Pipe", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[6].ChecklistSectionId, Sequence = 34, Description = "Duct Riser and connection", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Electrical section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[7].ChecklistSectionId, Sequence = 1, Description = "Check Alignment of Wiring Devices", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[7].ChecklistSectionId, Sequence = 2, Description = "Check For ONU Panel Door", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[7].ChecklistSectionId, Sequence = 3, Description = "Check For DB Panel Door", Reference = "General", IsActive = true, CreatedDate = SeedDate },
            // Wiring Devices section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 1, Description = "10A 1G, 2 Way switch", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 2, Description = "10A 2G, 2 Way switch", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 3, Description = "10A 3G, 2 Way switch", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 4, Description = "Single Data Outlet -Euro face plate single keystone adaptor", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 5, Description = "Twin Data Outlet -Euro face plate Duplex keystone adaptor", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 6, Description = "13A, Double pole Duplex switched Power socket", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 7, Description = "20 A, DP switch Washing Machine", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 8, Description = "45 A, DP switch for HOB", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 9, Description = "Unswitched Fused Connection Unit + cord outlet - FACP", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 10, Description = "Switched Fused Connection Unit + cord outlet - Hood", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 11, Description = "20 A, Cable/Flex outlet Washing Machine", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 12, Description = "45 A, Cable/Flex outlet for HOB", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 13, Description = "Door Bell -230V Electromechanical chime", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[8].ChecklistSectionId, Sequence = 14, Description = "13A, DP, Simplex Switched & fused Spur Outlet for FCUs", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Wire, Cables, Conduits section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 15, Description = "All Wires pulled as per the approved Drawings.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 16, Description = "CAT-6 Cable pulled", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 17, Description = "Fire alarm Cable pulled", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 18, Description = "VRV Cable pulled", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 19, Description = "Main Cable pulled", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 20, Description = "Smoke detector", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 21, Description = "Heat detector", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 22, Description = "Sensor", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 23, Description = "DB Panel Tags and identification.", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 24, Description = "ONU Panel installation and termination", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 25, Description = "DB Panel installation and termination", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 26, Description = "Thermostat", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[9].ChecklistSectionId, Sequence = 27, Description = "PMU", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            // Light Fittings section
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 28, Description = "LT03 STUDIO", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 29, Description = "LT9B STUDIO", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 30, Description = "LT06 STUDIO", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 31, Description = "LT09A LOUNGE 2", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 32, Description = "C LOUNGE 2", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 33, Description = "L01 GABRGE ROOM", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 34, Description = "L05 ELECTRICAL ROOM", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 35, Description = "L07 STAIR", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate },
            new PredefinedChecklistItem { PredefinedItemId = GetNextItemId(), ChecklistSectionId = preLoadingMEPSections[10].ChecklistSectionId, Sequence = 36, Description = "L08 ICT ROOM", Reference = "Approved Drawing", IsActive = true, CreatedDate = SeedDate }
        };

        var preLoadingMEPChecklist = new Checklist
        {
            ChecklistId = PreLoadingMEPChecklistId,
            Name = "Check List for Pre-Loading of Completed Precast Modular (MEP)",
            Code = "Pre-Loading-MEP",
            Discipline = "MEP",
            SubDiscipline = "Pre-Loading",
            PageNumber = 42,
            ReferenceDocumentsJson = null,
            SignatureRolesJson = JsonSerializer.Serialize(new List<string> { "Civil Works", "QC", "MEP Works" }),
            IsActive = true,
            CreatedDate = SeedDate
        };

        AddChecklistData(preLoadingMEPChecklist, preLoadingMEPSections, preLoadingMEPItems);

        // Seed all data
        modelBuilder.Entity<Checklist>().HasData(checklists);
        modelBuilder.Entity<ChecklistSection>().HasData(sections);
        modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);
    }
}

