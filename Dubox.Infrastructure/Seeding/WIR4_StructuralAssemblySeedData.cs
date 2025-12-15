using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class WIR4_StructuralAssemblySeedData
{
    public static void SeedWIR4Items(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var items = new List<PredefinedChecklistItem>
        {
            // ============================================
            // A. GENERAL REQUIREMENTS
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000001"),
                ItemNumber = "A1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Ensure method statement, materials and shop drawings are approved",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000017"), // WIR-4: General Requirements
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000007"), // Approved MTS
                Sequence = 1,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000002"),
                ItemNumber = "A2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000017"), // WIR-4: General Requirements
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 2,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000003"),
                ItemNumber = "A3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Check the expiry date of the material prior to applications",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000017"), // WIR-4: General Requirements
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 3,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // B. PREPARATION & SETTING OUT
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000004"),
                ItemNumber = "B1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Drawing Stamp & Signature",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000018"), // WIR-4: Preparation & Setting Out
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 4,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000005"),
                ItemNumber = "B2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Element Tag, QC Approval for Element",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000018"), // WIR-4: Preparation & Setting Out
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 5,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000006"),
                ItemNumber = "B3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Floor Setting Out",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000018"), // WIR-4: Preparation & Setting Out
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 6,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // C. EXTERNAL WALLS ERECTION
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000007"),
                ItemNumber = "C1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Erection with temporary support",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000019"), // WIR-4: External Walls
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 7,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000008"),
                ItemNumber = "C2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Panel to panel connections",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000019"), // WIR-4: External Walls
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 8,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000009"),
                ItemNumber = "C3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Dimensions (outer, inner and diagonal), line and level",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000019"), // WIR-4: External Walls
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 9,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // D. FLOOR SLAB INSTALLATION
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000010"),
                ItemNumber = "D1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Backer Rod / Shim Pad",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 10,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000011"),
                ItemNumber = "D2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Slab Position / Level",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 11,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000012"),
                ItemNumber = "D3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Panel to Slab Connections",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 12,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000013"),
                ItemNumber = "D4",
                WIRNumber = "WIR-4",
                CheckpointDescription = "1000mm FFL to be marked clearly",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 13,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000014"),
                ItemNumber = "D5",
                WIRNumber = "WIR-4",
                CheckpointDescription = "MEP Clearance",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 14,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000015"),
                ItemNumber = "D6",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Check the Slab level (shall read 1016mm at 1000m FFL line)",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 15,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000016"),
                ItemNumber = "D7",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Grouting",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000020"), // WIR-4: Floor Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"), // Project Specification
                Sequence = 16,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // E. INTERNAL PARTITION INSTALLATION
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000017"),
                ItemNumber = "E1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Erection with Temporary Support",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000021"), // WIR-4: Internal Partition
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 17,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000018"),
                ItemNumber = "E2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Panel to Panel Connections",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000021"), // WIR-4: Internal Partition
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 18,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000019"),
                ItemNumber = "E3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Dimensions (outer, inner and diagonal), Line and Level",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000021"), // WIR-4: Internal Partition
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 19,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000020"),
                ItemNumber = "E4",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Grouting",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000021"), // WIR-4: Internal Partition
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"), // Project Specification
                Sequence = 20,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // F. TOP SLAB INSTALLATION
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000021"),
                ItemNumber = "F1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Backer Rod / Shim Pad",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 21,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000022"),
                ItemNumber = "F2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Slab Position / Top Height",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 22,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000023"),
                ItemNumber = "F3",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Panel to Slab Connections",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 23,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000024"),
                ItemNumber = "F4",
                WIRNumber = "WIR-4",
                CheckpointDescription = "MEP Clearance",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 24,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000025"),
                ItemNumber = "F5",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Grouting",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"), // Project Specification
                Sequence = 25,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000026"),
                ItemNumber = "F6",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Curing",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000022"), // WIR-4: Top Slab
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"), // Project Specification
                Sequence = 26,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // G. FINAL VERIFICATION
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000027"),
                ItemNumber = "G1",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Internal and External Dimension of Box",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000023"), // WIR-4: Final Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 27,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0004-000000000028"),
                ItemNumber = "G2",
                WIRNumber = "WIR-4",
                CheckpointDescription = "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000023"), // WIR-4: Final Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 28,
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);
    }
}
