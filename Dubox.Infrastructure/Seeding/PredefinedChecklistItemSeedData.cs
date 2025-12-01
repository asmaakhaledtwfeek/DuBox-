using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class PredefinedChecklistItemSeedData
{
    public static void SeedPredefinedChecklistItems(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var predefinedItems = new List<PredefinedChecklistItem>
        {
            // A. General
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000001"),
                CheckpointDescription = "Ensure method statement, material submittal and drawings are approved.",
                ReferenceDocument = null,
                Sequence = 1,
                Category = "General",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000002"),
                CheckpointDescription = "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
                ReferenceDocument = null,
                Sequence = 2,
                Category = "General",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000003"),
                CheckpointDescription = "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.",
                ReferenceDocument = null,
                Sequence = 3,
                Category = "General",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000004"),
                CheckpointDescription = "Verify and record the DCL product conformity certificate for the insulation materials.",
                ReferenceDocument = null,
                Sequence = 4,
                Category = "General",
                IsActive = true,
                CreatedDate = seedDate
            },
            // B. Setting Out
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000005"),
                CheckpointDescription = "Verify the marking and setting out of the partition walls as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 5,
                Category = "Setting Out",
                IsActive = true,
                CreatedDate = seedDate
            },
            // C. Installation Activity
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000006"),
                CheckpointDescription = "Verify the completion of the required finishes of the adjacent substrates.",
                ReferenceDocument = null,
                Sequence = 6,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000007"),
                CheckpointDescription = "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 7,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000008"),
                CheckpointDescription = "Verify the fixation of the board (on one side of the supports) as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 8,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000009"),
                CheckpointDescription = "Ensure the completion of all embedded MEP and other discipline works prior to closure as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 9,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000010"),
                CheckpointDescription = "Ensure additional supports are provided for the wall mounted fixtures as applicable.",
                ReferenceDocument = null,
                Sequence = 10,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000011"),
                CheckpointDescription = "Verify the installation of insulation works (if applicable) as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 11,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000012"),
                CheckpointDescription = "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.",
                ReferenceDocument = null,
                Sequence = 12,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000013"),
                CheckpointDescription = "Verify the fixation of the board as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 13,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000014"),
                CheckpointDescription = "Ensure the completion of MEP and other discipline works above the false ceiling level and obtain clearance to proceed for further works.",
                ReferenceDocument = null,
                Sequence = 14,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000015"),
                CheckpointDescription = "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.",
                ReferenceDocument = null,
                Sequence = 15,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000016"),
                CheckpointDescription = "Ensure the cutting of gypsum board on the marked locations as per the project requirements.",
                ReferenceDocument = null,
                Sequence = 16,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000017"),
                CheckpointDescription = "Verify the jointing & taping as per the manufacturer recommendations.",
                ReferenceDocument = null,
                Sequence = 17,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            },
            new PredefinedChecklistItem
            {
                PredefinedItemId = Guid.Parse("20000001-0000-0000-0000-000000000018"),
                CheckpointDescription = "Approval obtain from Consultant/Client to proceed with further activities.",
                ReferenceDocument = null,
                Sequence = 18,
                Category = "Installation Activity",
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(predefinedItems);
    }
}

