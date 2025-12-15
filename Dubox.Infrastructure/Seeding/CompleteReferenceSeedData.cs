using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class CompleteReferenceSeedData
{
    public static void SeedAllReferences(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

        var references = new List<Reference>
        {
            // ============================================
            // GENERAL REFERENCES
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000001"),
                ReferenceName = "General",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000002"),
                ReferenceName = "Approved Drawing",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000003"),
                ReferenceName = "Approved Shop Drawing",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000004"),
                ReferenceName = "Approved Method Statement",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000005"),
                ReferenceName = "Project Specification",
                CreatedDate = seedDate
            },

            // ============================================
            // MATERIAL REFERENCES
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000010"),
                ReferenceName = "MA (Material Approval)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000011"),
                ReferenceName = "MIR (Material Inspection Record)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000012"),
                ReferenceName = "MAR (Material Approval Record)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000013"),
                ReferenceName = "Material Submittal",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000014"),
                ReferenceName = "MSDS (Material Safety Data Sheet)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000015"),
                ReferenceName = "Manufacturer Recommendations",
                CreatedDate = seedDate
            },

            // ============================================
            // TESTING & CALIBRATION REFERENCES
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000020"),
                ReferenceName = "Calibration Certificate",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000021"),
                ReferenceName = "Test Report",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000022"),
                ReferenceName = "Mill Test Certificate",
                CreatedDate = seedDate
            },

            // ============================================
            // MEP SPECIFICATIONS
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000100"),
                ReferenceName = "Project Spec. - Section 230713 (HVAC)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000101"),
                ReferenceName = "Project Spec. - Section 233113 (HVAC Duct)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000102"),
                ReferenceName = "Project Spec. - Section 221300 (Drainage)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000103"),
                ReferenceName = "Project Spec. - Section 221116 (Water Supply)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000104"),
                ReferenceName = "Project Spec. - Section 211100 (Fire Fighting)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000105"),
                ReferenceName = "Project Spec. - Section 232300 (Refrigerant Piping)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000106"),
                ReferenceName = "Project Spec. - Section 237400 & 238129 (HVAC Equipment)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000107"),
                ReferenceName = "NFPA 13 (Fire Protection Standard)",
                CreatedDate = seedDate
            },

            // ============================================
            // ELECTRICAL SPECIFICATIONS
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000200"),
                ReferenceName = "Project Spec. - Section 260513 (LV Cables)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000201"),
                ReferenceName = "Project Spec. - Section 260513-1.7 C (Cable Testing)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000202"),
                ReferenceName = "Project Spec. - Section 262416 (LV Panels)",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000203"),
                ReferenceName = "Project Spec. - Section 260533 (Conduits)",
                CreatedDate = seedDate
            },

            // ============================================
            // FINISHING SPECIFICATIONS
            // ============================================
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000300"),
                ReferenceName = "Finishing Schedule",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000301"),
                ReferenceName = "Color Schedule",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000302"),
                ReferenceName = "Door Schedule",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000303"),
                ReferenceName = "Window Schedule",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000304"),
                ReferenceName = "DCL Product Conformity Certificate",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000305"),
                ReferenceName = "Fire Rating Certificate",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000306"),
                ReferenceName = "Mockup Approval",
                CreatedDate = seedDate
            },
            new Reference
            {
                ReferenceId = Guid.Parse("30000001-0000-0000-0000-000000000307"),
                ReferenceName = "Sample Approval",
                CreatedDate = seedDate
            }
        };

        modelBuilder.Entity<Reference>().HasData(references);
    }
}
