using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class WIR1_MaterialVerificationSeedData
{
    public static void SeedWIR1Items(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var items = new List<PredefinedChecklistItem>
        {
            // ============================================
            // MATERIAL VERIFICATION INSPECTION (CIVIL)
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000001"),
                ItemNumber = "1",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Is there material approval for received item?",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000005"), // MIR
                Sequence = 1,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000002"),
                ItemNumber = "2",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Is Manufacturer Name as per material approval?",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000003"), // MA
                Sequence = 2,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000003"),
                ItemNumber = "3",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Is Supplier Name as per material approval?",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000003"), // MA
                Sequence = 3,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000004"),
                ItemNumber = "4",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Is received material matching with approved sample?",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000036"), // Sample Approval
                Sequence = 4,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000005"),
                ItemNumber = "5",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Related mill test certificate (or) test reports?",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000027"), // Mill Test Certificate
                Sequence = 5,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000006"),
                ItemNumber = "6",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check for any defects",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 6,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000007"),
                ItemNumber = "7",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check Expiry date",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 7,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000008"),
                ItemNumber = "8",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check Item / product description",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 8,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000009"),
                ItemNumber = "9",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check Item / product code",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 9,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000010"),
                ItemNumber = "10",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check Dimensions (length, width, thickness etc.)",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 10,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000011"),
                ItemNumber = "11",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check Colour",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000030"), // Color Schedule
                Sequence = 11,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000012"),
                ItemNumber = "12",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check for Packaging Conditions",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 12,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000013"),
                ItemNumber = "13",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check for received Quantity (approx) as per DO",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 13,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000014"),
                ItemNumber = "14",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the area of storage as per MSDS",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000015"), // WIR-1: Material Verification
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000025"), // MSDS
                Sequence = 14,
                IsActive = true,
                CreatedDate = seedDate
            },

            // ============================================
            // MATERIAL RECEIVING INSPECTION - MEP
            // ============================================
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000101"),
                ItemNumber = "1",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Review documents for received materials",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 101,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000102"),
                ItemNumber = "2",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Materials outside visual checking",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 102,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000103"),
                ItemNumber = "3",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check for any damages (General & Visual)",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 103,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000104"),
                ItemNumber = "4",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Verify original bill of landing / Delivery Note",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 104,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000105"),
                ItemNumber = "5",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Supplier Certificate / Warranty letter",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000026"), // Manufacturer Recommendations
                Sequence = 105,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000106"),
                ItemNumber = "6",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check and Verify the material as per delivery list / details",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 106,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000107"),
                ItemNumber = "7",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the accessories",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 107,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000108"),
                ItemNumber = "8",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the Name Plate",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 108,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000109"),
                ItemNumber = "9",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Materials Storage and preservation as per manufacturer",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000026"), // Manufacturer Recommendations
                Sequence = 109,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000110"),
                ItemNumber = "10",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the identification of components",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 110,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000111"),
                ItemNumber = "11",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the rating as per approved drawing",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 111,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000112"),
                ItemNumber = "12",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the loose part",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 112,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000113"),
                ItemNumber = "13",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the dimension of delivered equipment as per approved drawing",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 113,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000114"),
                ItemNumber = "14",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Check the availability of spare breakers / relays/ terminals",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"), // Approved Drawing
                Sequence = 114,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000115"),
                ItemNumber = "15",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Delivered material photos",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"), // General
                Sequence = 115,
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0001-000000000116"),
                ItemNumber = "16",
                WIRNumber = "WIR-1",
                CheckpointDescription = "Material Site test",
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000016"), // WIR-1: MEP Receiving
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000008"), // Test Report (existing)
                Sequence = 116,
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(items);
    }
}
