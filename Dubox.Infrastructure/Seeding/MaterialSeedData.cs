using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class MaterialSeedData
{
    public static void SeedMaterials(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        var materials = new List<Material>();

        // STRUCTURAL MATERIALS (1 month before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-STRUCT-001", "Insulation", "Structural Materials", "m²", 30, seedDate),
            CreateMaterial("MAT-STRUCT-002", "PC Strand", "Structural Materials", "ton", 30, seedDate),
            CreateMaterial("MAT-STRUCT-003", "Embedded Items", "Structural Materials", "pcs", 30, seedDate),
            CreateMaterial("MAT-STRUCT-004", "Pre-Cast", "Structural Materials", "m³", 7, seedDate),
            CreateMaterial("MAT-STRUCT-005", "Dry Mix Concrete", "Structural Materials", "m³", 7, seedDate),
            CreateMaterial("MAT-STRUCT-006", "Grout", "Structural Materials", "kg", 7, seedDate),
            CreateMaterial("MAT-STRUCT-007", "Sealant", "Structural Materials", "tube", 7, seedDate),
            CreateMaterial("MAT-STRUCT-008", "Silicon", "Structural Materials", "tube", 7, seedDate),
            CreateMaterial("MAT-STRUCT-009", "Backer Rod", "Structural Materials", "m", 7, seedDate),
            CreateMaterial("MAT-STRUCT-010", "PVC Shims", "Structural Materials", "pcs", 7, seedDate),
            CreateMaterial("MAT-STRUCT-011", "Bonding Agent", "Structural Materials", "liter", 7, seedDate),
            CreateMaterial("MAT-STRUCT-012", "Curing Compound", "Structural Materials", "liter", 7, seedDate),
            CreateMaterial("MAT-STRUCT-013", "Concrete Repair", "Structural Materials", "kg", 7, seedDate),
            CreateMaterial("MAT-STRUCT-014", "Steel Rebar", "Structural Materials", "ton", 7, seedDate),
            CreateMaterial("MAT-STRUCT-015", "Plywood", "Structural Materials", "sheet", 7, seedDate),
            CreateMaterial("MAT-STRUCT-016", "Cement", "Structural Materials", "bag", 7, seedDate)
        });

        // FINISHING MATERIALS (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-FINISH-001", "Paint", "Finishing Materials", "liter", 7, seedDate),
            CreateMaterial("MAT-FINISH-002", "Tiles", "Finishing Materials", "m²", 7, seedDate),
            CreateMaterial("MAT-FINISH-003", "Tile Grout", "Finishing Materials", "kg", 7, seedDate),
            CreateMaterial("MAT-FINISH-004", "Tile Adhesive", "Finishing Materials", "kg", 7, seedDate),
            CreateMaterial("MAT-FINISH-005", "Fire Sealant", "Finishing Materials", "tube", 7, seedDate),
            CreateMaterial("MAT-FINISH-006", "Bitumen", "Finishing Materials", "kg", 7, seedDate),
            CreateMaterial("MAT-FINISH-007", "Waterproof", "Finishing Materials", "liter", 7, seedDate),
            CreateMaterial("MAT-FINISH-008", "Gypsum Board", "Finishing Materials", "sheet", 7, seedDate),
            CreateMaterial("MAT-FINISH-009", "Threshold", "Finishing Materials", "pcs", 7, seedDate)
        });

        // DOORS & WINDOWS (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-DOOR-001", "Wooden Door", "Doors & Windows", "pcs", 7, seedDate),
            CreateMaterial("MAT-DOOR-002", "Metal Door", "Doors & Windows", "pcs", 7, seedDate),
            CreateMaterial("MAT-DOOR-003", "Door Hardware", "Doors & Windows", "set", 7, seedDate),
            CreateMaterial("MAT-DOOR-004", "Aluminum Door", "Doors & Windows", "pcs", 7, seedDate),
            CreateMaterial("MAT-DOOR-005", "Aluminum Window", "Doors & Windows", "pcs", 7, seedDate)
        });

        // CARPENTRY (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-CARP-001", "Kitchen Cabinet", "Carpentry", "set", 7, seedDate),
            CreateMaterial("MAT-CARP-002", "Countertop (Corian)", "Carpentry", "m²", 7, seedDate),
            CreateMaterial("MAT-CARP-003", "Wardrobe", "Carpentry", "set", 7, seedDate)
        });

        // MEP - HVAC (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-HVAC-001", "FCU", "MEP - HVAC", "unit", 7, seedDate),
            CreateMaterial("MAT-HVAC-002", "FAHU", "MEP - HVAC", "unit", 7, seedDate),
            CreateMaterial("MAT-HVAC-003", "Ventilation Fan", "MEP - HVAC", "unit", 7, seedDate),
            CreateMaterial("MAT-HVAC-004", "Duct", "MEP - HVAC", "m", 7, seedDate),
            CreateMaterial("MAT-HVAC-005", "Air Outlet", "MEP - HVAC", "pcs", 7, seedDate),
            CreateMaterial("MAT-HVAC-006", "Damper", "MEP - HVAC", "pcs", 7, seedDate),
            CreateMaterial("MAT-HVAC-007", "Exhaust Fan", "MEP - HVAC", "unit", 7, seedDate),
            CreateMaterial("MAT-HVAC-008", "Thermostat", "MEP - HVAC", "unit", 7, seedDate),
            CreateMaterial("MAT-HVAC-009", "Thermal Insulation", "MEP - HVAC", "m²", 7, seedDate)
        });

        // MEP - PLUMBING (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-PLUMB-001", "PPR Pipes & Fittings", "MEP - Plumbing", "m", 7, seedDate),
            CreateMaterial("MAT-PLUMB-002", "Water Hammer Arrestor", "MEP - Plumbing", "pcs", 7, seedDate),
            CreateMaterial("MAT-PLUMB-003", "Water Heaters", "MEP - Plumbing", "unit", 7, seedDate),
            CreateMaterial("MAT-PLUMB-004", "PVC Pipes & Fittings", "MEP - Plumbing", "m", 7, seedDate),
            CreateMaterial("MAT-PLUMB-005", "HDPE Pipes & Fittings", "MEP - Plumbing", "m", 7, seedDate),
            CreateMaterial("MAT-PLUMB-006", "Floor Drain", "MEP - Plumbing", "pcs", 7, seedDate),
            CreateMaterial("MAT-PLUMB-007", "Floor Cleanout", "MEP - Plumbing", "pcs", 7, seedDate),
            CreateMaterial("MAT-PLUMB-008", "Sanitary Fixture", "MEP - Plumbing", "set", 7, seedDate),
            CreateMaterial("MAT-PLUMB-009", "Mechanical Hangers & Support", "MEP - Plumbing", "set", 7, seedDate),
            CreateMaterial("MAT-PLUMB-010", "Bottle Trap", "MEP - Plumbing", "pcs", 7, seedDate),
            CreateMaterial("MAT-PLUMB-011", "Angle Valve", "MEP - Plumbing", "pcs", 7, seedDate),
            CreateMaterial("MAT-PLUMB-012", "PEX Pipe & Fittings", "MEP - Plumbing", "m", 7, seedDate),
            CreateMaterial("MAT-PLUMB-013", "Kitchen Mixer", "MEP - Plumbing", "unit", 7, seedDate),
            CreateMaterial("MAT-PLUMB-014", "Kitchen Sink", "MEP - Plumbing", "unit", 7, seedDate),
            CreateMaterial("MAT-PLUMB-015", "Kitchen Hood", "MEP - Plumbing", "unit", 7, seedDate),
            CreateMaterial("MAT-PLUMB-016", "Copper Pipe & Fitting", "MEP - Plumbing", "m", 7, seedDate),
            CreateMaterial("MAT-PLUMB-017", "Rain Water Down Spout", "MEP - Plumbing", "m", 7, seedDate)
        });

        // MEP - ELECTRICAL (1 week before, except PVC Conduits which is 1 month)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-ELEC-001", "LV Cables", "MEP - Electrical", "m", 7, seedDate),
            CreateMaterial("MAT-ELEC-002", "LV Panels", "MEP - Electrical", "unit", 7, seedDate),
            CreateMaterial("MAT-ELEC-003", "Wire Connectors", "MEP - Electrical", "pcs", 7, seedDate),
            CreateMaterial("MAT-ELEC-004", "Light Fittings", "MEP - Electrical", "unit", 7, seedDate),
            CreateMaterial("MAT-ELEC-005", "Earthing", "MEP - Electrical", "set", 7, seedDate),
            CreateMaterial("MAT-ELEC-006", "UPS", "MEP - Electrical", "unit", 7, seedDate),
            CreateMaterial("MAT-ELEC-007", "Isolators", "MEP - Electrical", "pcs", 7, seedDate),
            CreateMaterial("MAT-ELEC-008", "PVC Conduits, Box, GI Back Box", "MEP - Electrical", "m", 30, seedDate), // 1 month
            CreateMaterial("MAT-ELEC-009", "EMT Conduits & Acc", "MEP - Electrical", "m", 7, seedDate),
            CreateMaterial("MAT-ELEC-010", "GI Flexible Conduits & Acc", "MEP - Electrical", "m", 7, seedDate),
            CreateMaterial("MAT-ELEC-011", "Cable Management System (Trays)", "MEP - Electrical", "m", 7, seedDate),
            CreateMaterial("MAT-ELEC-012", "Floor Box", "MEP - Electrical", "pcs", 7, seedDate),
            CreateMaterial("MAT-ELEC-013", "Wiring Accessories", "MEP - Electrical", "set", 7, seedDate),
            CreateMaterial("MAT-ELEC-014", "Ceiling Fan", "MEP - Electrical", "unit", 7, seedDate),
            CreateMaterial("MAT-ELEC-015", "Control Panel", "MEP - Electrical", "unit", 7, seedDate)
        });

        // MEP - SYSTEMS (1 week before)
        materials.AddRange(new[]
        {
            CreateMaterial("MAT-SYS-001", "CCTV System", "MEP - Systems", "set", 7, seedDate),
            CreateMaterial("MAT-SYS-002", "Monitor Sensor", "MEP - Systems", "pcs", 7, seedDate),
            CreateMaterial("MAT-SYS-003", "Fire Alarm System", "MEP - Systems", "set", 7, seedDate),
            CreateMaterial("MAT-SYS-004", "IBMS", "MEP - Systems", "set", 7, seedDate),
            CreateMaterial("MAT-SYS-005", "Access Control System", "MEP - Systems", "set", 7, seedDate)
        });

        modelBuilder.Entity<Material>().HasData(materials);
    }

    private static int _materialIdCounter = 1;
    
    private static Material CreateMaterial(string code, string name, string category, string unit, int defaultRequiredBeforeDays, DateTime createdDate)
    {
        // Generate deterministic GUID based on counter for seeding
        var guidBytes = new byte[16];
        BitConverter.GetBytes(_materialIdCounter++).CopyTo(guidBytes, 0);
        
        return new Material
        {
            MaterialId = new Guid(guidBytes),
            MaterialCode = code,
            MaterialName = name,
            MaterialCategory = category,
            Unit = unit,
            DefaultRequiredBeforeDays = defaultRequiredBeforeDays,
            IsActive = true,
            CurrentStock = 0,
            AllocatedStock = 0,
            MinimumStock = 0,
            ReorderLevel = 0
        };
    }
}

