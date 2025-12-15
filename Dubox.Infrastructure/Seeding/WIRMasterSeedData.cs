using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class WIRMasterSeedData
{
    public static void SeedWIRMasters(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var wirMasters = new List<WIRMaster>
        {
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000001"),
                WIRNumber = "WIR-1",
                WIRName = "Material Receiving & Verification",
                Description = "Verify all materials meet specifications and are properly documented before use in production",
                Sequence = 1,
                Discipline = "Both",
                Phase = "Material",
                IsActive = true,
                CreatedDate = seedDate
            },
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000002"),
                WIRNumber = "WIR-2",
                WIRName = "MEP Installation & Testing",
                Description = "Install and test all MEP systems including HVAC, plumbing, fire fighting, and refrigerant piping",
                Sequence = 2,
                Discipline = "MEP",
                Phase = "Installation",
                IsActive = true,
                CreatedDate = seedDate
            },
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000003"),
                WIRNumber = "WIR-3",
                WIRName = "Electrical Installation & Testing",
                Description = "Install and test all electrical systems including cables, wires, panels, and conduits",
                Sequence = 3,
                Discipline = "Electrical",
                Phase = "Installation",
                IsActive = true,
                CreatedDate = seedDate
            },
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000004"),
                WIRNumber = "WIR-4",
                WIRName = "Structural Assembly & Erection",
                Description = "Assemble and erect precast concrete modular elements including walls, slabs, and partitions",
                Sequence = 4,
                Discipline = "Civil",
                Phase = "Assembly",
                IsActive = true,
                CreatedDate = seedDate
            },
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000005"),
                WIRNumber = "WIR-5",
                WIRName = "Finishing Works",
                Description = "Apply all interior and exterior finishes including painting, tiling, ceilings, doors, windows, and woodwork",
                Sequence = 5,
                Discipline = "Civil",
                Phase = "Finishing",
                IsActive = true,
                CreatedDate = seedDate
            },
            new WIRMaster
            {
                WIRMasterId = Guid.Parse("10000001-0000-0000-0000-000000000006"),
                WIRNumber = "WIR-6",
                WIRName = "Final Pre-Loading Inspection",
                Description = "Comprehensive final inspection of completed modular before loading and transportation to site",
                Sequence = 6,
                Discipline = "Both",
                Phase = "Final",
                IsActive = true,
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<WIRMaster>().HasData(wirMasters);
    }
}
