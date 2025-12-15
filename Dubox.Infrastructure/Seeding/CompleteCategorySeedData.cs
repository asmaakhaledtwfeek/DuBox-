using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class CompleteCategorySeedData
{
    public static void SeedAllCategories(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var categories = new List<Category>
        {
            // ============================================
            // WIR-1: MATERIAL RECEIVING & VERIFICATION
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0001-000000000001"),
                CategoryName = "WIR-1: Material Verification Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0001-000000000002"),
                CategoryName = "WIR-1: Material Receiving Inspection - MEP",
                CreatedDate = seedDate
            },

            // ============================================
            // WIR-2: MEP INSTALLATION (Existing - Enhanced)
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000001"),
                CategoryName = "WIR-2: Installation of HVAC Duct",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000002"),
                CategoryName = "WIR-2: Installation of Above Ground Drainage Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000003"),
                CategoryName = "WIR-2: Leak Test of Above Ground Drainage Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000004"),
                CategoryName = "WIR-2: Installation of Above Ground Water Supply Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000005"),
                CategoryName = "WIR-2: Testing of Above Ground Water Supply Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000006"),
                CategoryName = "WIR-2: Installation of Above Ground Fire Fighting Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000007"),
                CategoryName = "WIR-2: Testing of Above Ground Fire Fighting Pipes",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000008"),
                CategoryName = "WIR-2: Installation of Refrigerant Pipe",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0002-000000000009"),
                CategoryName = "WIR-2: Pressure Testing of Refrigerant Pipe",
                CreatedDate = seedDate
            },

            // ============================================
            // WIR-3: ELECTRICAL INSTALLATION (Existing - Enhanced)
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0003-000000000001"),
                CategoryName = "WIR-3: Installation of LV Cables & Wires",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0003-000000000002"),
                CategoryName = "WIR-3: Testing of LV Cables & Wires",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0003-000000000003"),
                CategoryName = "WIR-3: Installation of LV Panels",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0003-000000000004"),
                CategoryName = "WIR-3: Installation of Conduits & Accessories",
                CreatedDate = seedDate
            },

            // ============================================
            // WIR-4: STRUCTURAL ASSEMBLY
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000001"),
                CategoryName = "WIR-4: General Requirements",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000002"),
                CategoryName = "WIR-4: Preparation & Setting Out",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000003"),
                CategoryName = "WIR-4: External Walls Erection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000004"),
                CategoryName = "WIR-4: Floor Slab Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000005"),
                CategoryName = "WIR-4: Internal Partition Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000006"),
                CategoryName = "WIR-4: Top Slab Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0004-000000000007"),
                CategoryName = "WIR-4: Final Verification",
                CreatedDate = seedDate
            },

            // ============================================
            // WIR-5: FINISHING WORKS
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000001"),
                CategoryName = "WIR-5: Painting - General",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000002"),
                CategoryName = "WIR-5: Painting - Surface Preparation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000003"),
                CategoryName = "WIR-5: Painting - Internal Application",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000004"),
                CategoryName = "WIR-5: Painting - External Application",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000005"),
                CategoryName = "WIR-5: Ceramic Tiling",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000006"),
                CategoryName = "WIR-5: Bitumen Paint Application",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000007"),
                CategoryName = "WIR-5: Gypsum Partition Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000008"),
                CategoryName = "WIR-5: False Ceiling Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000009"),
                CategoryName = "WIR-5: Wet Area Waterproofing",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000010"),
                CategoryName = "WIR-5: Epoxy Flooring",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000011"),
                CategoryName = "WIR-5: Kitchen Cabinets Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000012"),
                CategoryName = "WIR-5: Wardrobe Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000013"),
                CategoryName = "WIR-5: Doors & Windows Installation",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000014"),
                CategoryName = "WIR-5: Fire Sealing Works",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0005-000000000015"),
                CategoryName = "WIR-5: Aluminium & Glazing",
                CreatedDate = seedDate
            },

            // ============================================
            // WIR-6: FINAL PRE-LOADING INSPECTION
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000001"),
                CategoryName = "WIR-6: General & Structural Verification",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000002"),
                CategoryName = "WIR-6: Painting Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000003"),
                CategoryName = "WIR-6: Floor & Wall Tiling",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000004"),
                CategoryName = "WIR-6: Dry Wall Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000005"),
                CategoryName = "WIR-6: False Ceiling Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000006"),
                CategoryName = "WIR-6: Aluminium & Glazing Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000007"),
                CategoryName = "WIR-6: Doors & Windows Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000008"),
                CategoryName = "WIR-6: Wood Works Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000009"),
                CategoryName = "WIR-6: Other Finishes Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000010"),
                CategoryName = "WIR-6: MEP Systems Inspection",
                CreatedDate = seedDate
            },
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0006-000000000011"),
                CategoryName = "WIR-6: Balance Work & Final Check",
                CreatedDate = seedDate
            },

            // ============================================
            // GENERAL CATEGORIES (Cross-WIR)
            // ============================================
            new Category
            {
                CategoryId = Guid.Parse("40000001-0000-0000-0000-000000000001"),
                CategoryName = "General Requirements",
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}
