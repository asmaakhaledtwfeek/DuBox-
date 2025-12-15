using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class WIR5_FinishingWorksSeedData
{
    public static void SeedWIR5Items(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var items = new List<PredefinedChecklistItem>();
        int sequence = 1;

        // ============================================
        // PAINTING - General (3 items)
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "A1", "WIR-5", "Ensure method statement, materials and drawings (finishing schedule) are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000024"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "A2", "WIR-5", "Ensure materials are stored as per manufacturers recommendations",
                Guid.Parse("40000001-0000-0000-0000-000000000024"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "A3", "WIR-5", "Verify the expiry date and number of coats of the material prior to applications",
                Guid.Parse("40000001-0000-0000-0000-000000000024"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate)
        });

        // PAINTING - Surface Preparation (4 items)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "B1", "WIR-5", "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and grease",
                Guid.Parse("40000001-0000-0000-0000-000000000025"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "B2", "WIR-5", "Check for repair of surface imperfection and protrusions (if any)",
                Guid.Parse("40000001-0000-0000-0000-000000000025"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "B3", "WIR-5", "Moisture content for the substrate and environmental conditions as per manufacturer recommendations",
                Guid.Parse("40000001-0000-0000-0000-000000000025"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "B4", "WIR-5", "Check the MEP clearance prior to start Painting works",
                Guid.Parse("40000001-0000-0000-0000-000000000025"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate)
        });

        // PAINTING - Internal Application (5 items)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "C1", "WIR-5", "Ensure application of Primer as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000026"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "C2", "WIR-5", "Ensure application of Stucco as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000026"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "C3", "WIR-5", "Touchup, grinding, undulations, corner repairs and pinholes are filled properly",
                Guid.Parse("40000001-0000-0000-0000-000000000026"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "C4", "WIR-5", "Application of first coat of Paint as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000026"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "C5", "WIR-5", "Line between two color shades is straight, no Brush marks should be visible",
                Guid.Parse("40000001-0000-0000-0000-000000000026"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate)
        });

        // PAINTING - External Application (4 items)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "D1", "WIR-5", "Ensure application of Primer as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000027"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "D2", "WIR-5", "Ensure application of Filler Coats as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000027"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "D3", "WIR-5", "Touch up, grinding, undulations, corner repairs and pinholes are filled properly",
                Guid.Parse("40000001-0000-0000-0000-000000000027"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "D4", "WIR-5", "Application of final coat of Texture Paint as per manufacturers recommendation",
                Guid.Parse("40000001-0000-0000-0000-000000000027"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate)
        });

        // CERAMIC TILING (10 items - key ones)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "E1", "WIR-5", "Ensure method statement, materials and drawings (finishing schedule) are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000028"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "E2", "WIR-5", "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal",
                Guid.Parse("40000001-0000-0000-0000-000000000028"), Guid.Parse("30000001-0000-0000-0000-000000000023"), seedDate),
            CreateItem(sequence++, "E3", "WIR-5", "Check the setting out / pattern of wall and floor tiles as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000028"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "E4", "WIR-5", "Verify the slope and level of the tiles as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000028"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "E5", "WIR-5", "Check the application of tile grout as per the manufacturer recommendations",
                Guid.Parse("40000001-0000-0000-0000-000000000028"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate)
        });

        // GYPSUM PARTITION (8 items - key ones)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "F1", "WIR-5", "Ensure method statement, material submittal and drawings are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000029"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "F2", "WIR-5", "Verify the marking and setting out of the partition walls as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000029"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "F3", "WIR-5", "Verify the location, spacing and fixation of the supporting grid as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000029"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "F4", "WIR-5", "Verify the fixation of the board as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000029"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate)
        });

        // FALSE CEILING (8 items - key ones)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "G1", "WIR-5", "Ensure method statement, material submittal and drawings are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000030"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "G2", "WIR-5", "Verify the marking of the false ceiling level on the walls as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000030"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "G3", "WIR-5", "Verify the location, spacing and fixation of the grid as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000030"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "G4", "WIR-5", "Verify the type, fixation, level and alignment of the false ceiling board / tiles",
                Guid.Parse("40000001-0000-0000-0000-000000000030"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate)
        });

        // WET AREA WATERPROOFING (8 items)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "H1", "WIR-5", "Ensure method statement and materials are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000031"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "H2", "WIR-5", "Check substrate is clean, free from contaminants",
                Guid.Parse("40000001-0000-0000-0000-000000000031"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "H3", "WIR-5", "Check the application of coats as per project requirements / manufacturer recommendations",
                Guid.Parse("40000001-0000-0000-0000-000000000031"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "H4", "WIR-5", "Check for any water seepage / leakage after 24 hours or as per the project requirements",
                Guid.Parse("40000001-0000-0000-0000-000000000031"), Guid.Parse("30000001-0000-0000-0000-000000000008"), seedDate)
        });

        // DOORS & WINDOWS (12 items - key ones)
        items.AddRange(new[]
        {
            CreateItem(sequence++, "I1", "WIR-5", "Ensure method statement, material submittal and drawings are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000032"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "I2", "WIR-5", "Check the color, type, material, coating of door and window materials as per approval",
                Guid.Parse("40000001-0000-0000-0000-000000000032"), Guid.Parse("30000001-0000-0000-0000-000000000003"), seedDate),
            CreateItem(sequence++, "I3", "WIR-5", "Verify the location and clear opening of doors / windows as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000032"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "I4", "WIR-5", "Ensure the location and No. of Door hinges provided as per the approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000032"), Guid.Parse("30000001-0000-0000-0000-000000000031"), seedDate),
            CreateItem(sequence++, "I5", "WIR-5", "Ensure required Iron mongery sets are provided as per the door schedule drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000032"), Guid.Parse("30000001-0000-0000-0000-000000000031"), seedDate)
        });

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);
    }

    private static PredefinedChecklistItem CreateItem(int sequence, string itemNumber, string wirNumber,
        string description, Guid categoryId, Guid referenceId, DateTime createdDate)
    {
        return new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse($"20000001-0000-0000-0005-{sequence:D12}"),
            ItemNumber = itemNumber,
            WIRNumber = wirNumber,
            CheckpointDescription = description,
            CategoryId = categoryId,
            ReferenceId = referenceId,
            Sequence = sequence,
            IsActive = true,
            CreatedDate = createdDate
        };
    }
}
