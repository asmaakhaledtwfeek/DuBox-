using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class WIR6_FinalInspectionSeedData
{
    public static void SeedWIR6Items(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var items = new List<PredefinedChecklistItem>();
        int sequence = 1;

        // ============================================
        // 1. GENERAL & STRUCTURAL VERIFICATION
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "1.1", "WIR-6", "Ensure method statement, ITP, materials and shop drawings are approved",
                Guid.Parse("40000001-0000-0000-0000-000000000033"), Guid.Parse("30000001-0000-0000-0000-000000000007"), seedDate),
            CreateItem(sequence++, "1.2", "WIR-6", "Check identification tag of the modular",
                Guid.Parse("40000001-0000-0000-0000-000000000033"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "1.3", "WIR-6", "Visually inspect the modular for any defects or damages",
                Guid.Parse("40000001-0000-0000-0000-000000000033"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "1.4", "WIR-6", "Verify the method of loading as per the project / design requirements",
                Guid.Parse("40000001-0000-0000-0000-000000000033"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "2.1", "WIR-6", "Internal and External Dimensions of the modular",
                Guid.Parse("40000001-0000-0000-0000-000000000033"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate)
        });

        // ============================================
        // 2. PAINTING INSPECTION
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "3.1", "WIR-6", "Location and color of Painting as per the App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "3.2", "WIR-6", "Internal Paint (Application of Primer, Stucco and 1st Coat of Paint)",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000029"), seedDate),
            CreateItem(sequence++, "3.3", "WIR-6", "External Paint(Application of Primer, Filler and Final Coat Texture Paint)",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000029"), seedDate),
            CreateItem(sequence++, "3.4", "WIR-6", "Ensure Paint touch ups are completed around installed items",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "3.5", "WIR-6", "Bitumen Applied at required Areas",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "3.6", "WIR-6", "Damages, If any",
                Guid.Parse("40000001-0000-0000-0000-000000000034"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate)
        });

        // ============================================
        // 3. FLOOR & WALL TILING
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "4.1", "WIR-6", "Layout and Fixing of Tiles as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "4.2", "WIR-6", "Line, Level and Spacer for the Installed Tiles",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "4.3", "WIR-6", "Skirting is installed/fixed properly and truly vertical",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "4.4", "WIR-6", "Grouting of all Joints is done properly",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "4.5", "WIR-6", "Elastomeric sealant under skirting is provided properly",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000026"), seedDate),
            CreateItem(sequence++, "4.6", "WIR-6", "Ensure Drain holes are free from any debris and properly closed (if applicable)",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate),
            CreateItem(sequence++, "4.7", "WIR-6", "Damages, if any",
                Guid.Parse("40000001-0000-0000-0000-000000000035"), Guid.Parse("30000001-0000-0000-0000-000000000004"), seedDate)
        });

        // ============================================
        // 4. DRY WALL INSPECTION
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "5.1", "WIR-6", "Layout, location and position of dry wall is as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000036"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "5.2", "WIR-6", "Thickness of Dry wall is as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000036"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "5.3", "WIR-6", "Opening for MEP services are cut properly",
                Guid.Parse("40000001-0000-0000-0000-000000000036"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "5.4", "WIR-6", "Ensure Gypsum surface are Crack free at joints",
                Guid.Parse("40000001-0000-0000-0000-000000000036"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "5.5", "WIR-6", "Damages, if any",
                Guid.Parse("40000001-0000-0000-0000-000000000036"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 5. FALSE CEILING INSPECTION
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "6.1", "WIR-6", "Layout of False Ceiling tiles and bulk head as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000037"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "6.2", "WIR-6", "Height of the False Ceiling as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000037"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "6.3", "WIR-6", "Access panels/ Ceiling tiles are Fixed Properly",
                Guid.Parse("40000001-0000-0000-0000-000000000037"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "6.4", "WIR-6", "Ensure Gypsum surface are Crack free at joints",
                Guid.Parse("40000001-0000-0000-0000-000000000037"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 6. ALUMINIUM & GLAZING
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "7.1", "WIR-6", "Location of Window as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "7.2", "WIR-6", "Fixing of Glass/panels",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "7.3", "WIR-6", "Fixing of Iron-Mongery and Accessories",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "7.4", "WIR-6", "Fixing of Silicone Sealant",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000015"), seedDate),
            CreateItem(sequence++, "7.5", "WIR-6", "Water leak test performed and passed",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000005"), seedDate),
            CreateItem(sequence++, "7.6", "WIR-6", "Paint touch completed around the frame",
                Guid.Parse("40000001-0000-0000-0000-000000000038"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 7. DOORS & WINDOWS
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "8.1", "WIR-6", "Location of Doors as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000039"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "8.2", "WIR-6", "Direction of doors swing as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000039"), Guid.Parse("30000001-0000-0000-0000-000000000031"), seedDate), // Door Schedule
            CreateItem(sequence++, "8.3", "WIR-6", "Main entrance door as per App Drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000039"), Guid.Parse("30000001-0000-0000-0000-000000000031"), seedDate), // Door Schedule
            CreateItem(sequence++, "8.4", "WIR-6", "Lock of Main entrance door is installed",
                Guid.Parse("40000001-0000-0000-0000-000000000039"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 8. WOOD WORKS
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "8.10", "WIR-6", "Kitchen cabinets, counter top and accessories installed as per app drawing",
                Guid.Parse("40000001-0000-0000-0000-000000000040"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "8.11", "WIR-6", "Kitchen sink and sink mixer installed",
                Guid.Parse("40000001-0000-0000-0000-000000000040"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "8.12", "WIR-6", "Wardrobe installed as per approved drawings",
                Guid.Parse("40000001-0000-0000-0000-000000000040"), Guid.Parse("30000001-0000-0000-0000-000000000002"), seedDate),
            CreateItem(sequence++, "8.13", "WIR-6", "Wardrobe doors and drawers functioning smoothly and free from scratches",
                Guid.Parse("40000001-0000-0000-0000-000000000040"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 9. OTHER FINISHES
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "9.1", "WIR-6", "Mirror installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.2", "WIR-6", "Threshold installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.3", "WIR-6", "Glass Partition installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.4", "WIR-6", "Floor drain and covers installed and free from damages",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.5", "WIR-6", "Vanity installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.6", "WIR-6", "WC and cover installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.7", "WIR-6", "Shower installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.8", "WIR-6", "Toilet accessories installed and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.9", "WIR-6", "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000034"), seedDate), // Fire Rating Certificate
            CreateItem(sequence++, "9.10", "WIR-6", "Painted walls are clean and free from stains",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "9.11", "WIR-6", "Tiles are fixed with grouting properly and free from damage",
                Guid.Parse("40000001-0000-0000-0000-000000000041"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        // ============================================
        // 10. MEP SYSTEMS FINAL CHECK
        // ============================================
        items.AddRange(new[]
        {
            CreateItem(sequence++, "10.1", "WIR-6", "Check Final Condition of outside of the room and ensure its damage free",
                Guid.Parse("40000001-0000-0000-0000-000000000042"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate),
            CreateItem(sequence++, "10.2", "WIR-6", "Sign the delivery note for accepting the loading of precast modular in good condition",
                Guid.Parse("40000001-0000-0000-0000-000000000042"), Guid.Parse("30000001-0000-0000-0000-000000000001"), seedDate)
        });

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);
    }

    private static PredefinedChecklistItem CreateItem(int sequence, string itemNumber, string wirNumber,
        string description, Guid categoryId, Guid referenceId, DateTime createdDate)
    {
        return new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse($"20000001-0000-0000-0006-{sequence:D12}"),
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
