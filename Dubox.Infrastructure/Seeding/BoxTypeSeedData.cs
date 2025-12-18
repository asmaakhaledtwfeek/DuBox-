using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding
{
    public static class BoxTypeSeedData
    {
        public static void SeedBoxTypes(ModelBuilder modelBuilder)
        {
            // Categories
            var categories = new List<ProjectTypeCategory>
            {
                new ProjectTypeCategory { CategoryId = 1, CategoryName = "Residential", Abbreviation = "RES" },
                new ProjectTypeCategory { CategoryId = 2, CategoryName = "Commercial", Abbreviation = "COM" },
                new ProjectTypeCategory { CategoryId = 3, CategoryName = "Industrial", Abbreviation = "IND" },
                new ProjectTypeCategory { CategoryId = 4, CategoryName = "Institutional", Abbreviation = "INS" },
                new ProjectTypeCategory { CategoryId = 5, CategoryName = "Heavy Civil/Infrastructure", Abbreviation = "HCI" },
                new ProjectTypeCategory { CategoryId = 6, CategoryName = "Mixed-Use", Abbreviation = "MXU" },
                new ProjectTypeCategory { CategoryId = 7, CategoryName = "Specialized Construction", Abbreviation = "SPC" }
            };

            modelBuilder.Entity<ProjectTypeCategory>().HasData(categories);

            // Box Types
            var boxTypes = new List<BoxType>
            {
                // Residential (CategoryId = 1)
                new BoxType { BoxTypeId = 1, BoxTypeName = "Kitchen", Abbreviation = "KIT", CategoryId = 1 },
                new BoxType { BoxTypeId = 2, BoxTypeName = "Living Room", Abbreviation = "LIV", CategoryId = 1 },
                new BoxType { BoxTypeId = 3, BoxTypeName = "Dining Room", Abbreviation = "DIN", CategoryId = 1 },
                new BoxType { BoxTypeId = 4, BoxTypeName = "Bedrooms", Abbreviation = "BED", CategoryId = 1 },
                new BoxType { BoxTypeId = 5, BoxTypeName = "Bathrooms", Abbreviation = "BTH", CategoryId = 1 },
                new BoxType { BoxTypeId = 6, BoxTypeName = "Laundry Room", Abbreviation = "LAU", CategoryId = 1 },
                new BoxType { BoxTypeId = 7, BoxTypeName = "Garage", Abbreviation = "GAR", CategoryId = 1 },
                new BoxType { BoxTypeId = 8, BoxTypeName = "Basement", Abbreviation = "BSM", CategoryId = 1 },
                new BoxType { BoxTypeId = 9, BoxTypeName = "Attic", Abbreviation = "ATT", CategoryId = 1 },
                new BoxType { BoxTypeId = 10, BoxTypeName = "Entry Hall", Abbreviation = "ENT", CategoryId = 1 },
                new BoxType { BoxTypeId = 11, BoxTypeName = "Closets Rooms", Abbreviation = "CLO", CategoryId = 1 },
                new BoxType { BoxTypeId = 12, BoxTypeName = "Home Office / Study", Abbreviation = "HOM", CategoryId = 1 },
                new BoxType { BoxTypeId = 13, BoxTypeName = "Mudroom", Abbreviation = "MUD", CategoryId = 1 },
                new BoxType { BoxTypeId = 14, BoxTypeName = "Pantry", Abbreviation = "PAN", CategoryId = 1 },
                new BoxType { BoxTypeId = 15, BoxTypeName = "Balcony", Abbreviation = "BAL", CategoryId = 1 },
                new BoxType { BoxTypeId = 16, BoxTypeName = "Utility Room", Abbreviation = "UTL", CategoryId = 1 },

                // Commercial (CategoryId = 2)
                new BoxType { BoxTypeId = 17, BoxTypeName = "Office Spaces", Abbreviation = "OFF", CategoryId = 2 },
                new BoxType { BoxTypeId = 18, BoxTypeName = "Meeting Rooms", Abbreviation = "MTG", CategoryId = 2 },
                new BoxType { BoxTypeId = 19, BoxTypeName = "Reception", Abbreviation = "REC", CategoryId = 2 },
                new BoxType { BoxTypeId = 20, BoxTypeName = "Sales Floor", Abbreviation = "SAL", CategoryId = 2 },
                new BoxType { BoxTypeId = 21, BoxTypeName = "Dressing Rooms", Abbreviation = "DRS", CategoryId = 2 },
                new BoxType { BoxTypeId = 22, BoxTypeName = "Stock Rooms", Abbreviation = "STK", CategoryId = 2 },
                new BoxType { BoxTypeId = 23, BoxTypeName = "Staff Room", Abbreviation = "STF", CategoryId = 2 },
                new BoxType { BoxTypeId = 24, BoxTypeName = "Restrooms", Abbreviation = "RST", CategoryId = 2 },
                new BoxType { BoxTypeId = 25, BoxTypeName = "Server Room", Abbreviation = "SRV", CategoryId = 2 },
                new BoxType { BoxTypeId = 26, BoxTypeName = "Mail Room", Abbreviation = "MAL", CategoryId = 2 },
                new BoxType { BoxTypeId = 27, BoxTypeName = "Storage Areas", Abbreviation = "STO", CategoryId = 2 },
                new BoxType { BoxTypeId = 28, BoxTypeName = "Loading Dock", Abbreviation = "LOD", CategoryId = 2 },
                new BoxType { BoxTypeId = 29, BoxTypeName = "Parking Garage", Abbreviation = "PRK", CategoryId = 2 },
                new BoxType { BoxTypeId = 30, BoxTypeName = "Restaurant Dining Area", Abbreviation = "RDA", CategoryId = 2 },
                new BoxType { BoxTypeId = 31, BoxTypeName = "Commercial Kitchen", Abbreviation = "CKT", CategoryId = 2 },
                new BoxType { BoxTypeId = 32, BoxTypeName = "Hotel Rooms", Abbreviation = "HTL", CategoryId = 2 },
                new BoxType { BoxTypeId = 33, BoxTypeName = "Fitness Center", Abbreviation = "FIT", CategoryId = 2 },
                new BoxType { BoxTypeId = 34, BoxTypeName = "Elevators", Abbreviation = "ELV", CategoryId = 2 },

                // Industrial (CategoryId = 3)
                new BoxType { BoxTypeId = 35, BoxTypeName = "Production Floor", Abbreviation = "PRD", CategoryId = 3 },
                new BoxType { BoxTypeId = 36, BoxTypeName = "Assembly Line", Abbreviation = "ASM", CategoryId = 3 },
                new BoxType { BoxTypeId = 37, BoxTypeName = "Quality Control", Abbreviation = "QC", CategoryId = 3 },
                new BoxType { BoxTypeId = 38, BoxTypeName = "Warehouse", Abbreviation = "WHS", CategoryId = 3 },
                new BoxType { BoxTypeId = 39, BoxTypeName = "Loading", Abbreviation = "LDG", CategoryId = 3 },
                new BoxType { BoxTypeId = 40, BoxTypeName = "Machine Shop", Abbreviation = "MCH", CategoryId = 3 },
                new BoxType { BoxTypeId = 41, BoxTypeName = "Maintenance Workshop", Abbreviation = "MNT", CategoryId = 3 },
                new BoxType { BoxTypeId = 42, BoxTypeName = "Control Room", Abbreviation = "CTL", CategoryId = 3 },
                new BoxType { BoxTypeId = 43, BoxTypeName = "Raw Material Storage", Abbreviation = "RMS", CategoryId = 3 },
                new BoxType { BoxTypeId = 44, BoxTypeName = "Finished Goods Storage", Abbreviation = "FGS", CategoryId = 3 },
                new BoxType { BoxTypeId = 45, BoxTypeName = "Cleanroom", Abbreviation = "CLN", CategoryId = 3 },
                new BoxType { BoxTypeId = 46, BoxTypeName = "Chemical Storage", Abbreviation = "CHM", CategoryId = 3 },
                new BoxType { BoxTypeId = 47, BoxTypeName = "Break Room", Abbreviation = "BRK", CategoryId = 3 },
                new BoxType { BoxTypeId = 48, BoxTypeName = "Locker Rooms", Abbreviation = "LCK", CategoryId = 3 },
                new BoxType { BoxTypeId = 49, BoxTypeName = "Shipping", Abbreviation = "SHP", CategoryId = 3 },
                new BoxType { BoxTypeId = 50, BoxTypeName = "Utility Room", Abbreviation = "UTL", CategoryId = 3 },
                new BoxType { BoxTypeId = 51, BoxTypeName = "Office Area", Abbreviation = "OFC", CategoryId = 3 },
                new BoxType { BoxTypeId = 52, BoxTypeName = "Laboratory", Abbreviation = "LAB", CategoryId = 3 },

                // Institutional (CategoryId = 4)
                new BoxType { BoxTypeId = 53, BoxTypeName = "Schools", Abbreviation = "SCH", CategoryId = 4 },
                new BoxType { BoxTypeId = 54, BoxTypeName = "Hospitals", Abbreviation = "HOS", CategoryId = 4 },
                new BoxType { BoxTypeId = 55, BoxTypeName = "Government Buildings", Abbreviation = "GOV", CategoryId = 4 },

                // Heavy Civil/Infrastructure (CategoryId = 5)
                new BoxType { BoxTypeId = 56, BoxTypeName = "Toll Booths", Abbreviation = "TOL", CategoryId = 5 },
                new BoxType { BoxTypeId = 57, BoxTypeName = "Rest Areas", Abbreviation = "RST", CategoryId = 5 },
                new BoxType { BoxTypeId = 58, BoxTypeName = "Maintenance Yards", Abbreviation = "MYD", CategoryId = 5 },
                new BoxType { BoxTypeId = 59, BoxTypeName = "Equipment Storage Facilities", Abbreviation = "ESF", CategoryId = 5 },
                new BoxType { BoxTypeId = 60, BoxTypeName = "Control Centers", Abbreviation = "CCT", CategoryId = 5 },
                new BoxType { BoxTypeId = 61, BoxTypeName = "Pump Stations", Abbreviation = "PMP", CategoryId = 5 },
                new BoxType { BoxTypeId = 62, BoxTypeName = "Treatment Facilities", Abbreviation = "TRT", CategoryId = 5 },
                new BoxType { BoxTypeId = 63, BoxTypeName = "Tunnel Sections", Abbreviation = "TUN", CategoryId = 5 },
                new BoxType { BoxTypeId = 64, BoxTypeName = "Bridge Inspection Areas", Abbreviation = "BIA", CategoryId = 5 },
                new BoxType { BoxTypeId = 65, BoxTypeName = "Airport Terminals", Abbreviation = "APT", CategoryId = 5 },
                new BoxType { BoxTypeId = 66, BoxTypeName = "Concourses", Abbreviation = "CON", CategoryId = 5 },
                new BoxType { BoxTypeId = 67, BoxTypeName = "Baggage Claim", Abbreviation = "BAG", CategoryId = 5 },
                new BoxType { BoxTypeId = 68, BoxTypeName = "Runways", Abbreviation = "RWY", CategoryId = 5 },
                new BoxType { BoxTypeId = 69, BoxTypeName = "Train Platforms", Abbreviation = "TRN", CategoryId = 5 },
                new BoxType { BoxTypeId = 70, BoxTypeName = "Station Houses", Abbreviation = "STN", CategoryId = 5 },

                // Mixed-Use (CategoryId = 6)
                new BoxType { BoxTypeId = 71, BoxTypeName = "Residential Units", Abbreviation = "RSU", CategoryId = 6 },
                new BoxType { BoxTypeId = 72, BoxTypeName = "Retail Spaces", Abbreviation = "RTL", CategoryId = 6 },
                new BoxType { BoxTypeId = 73, BoxTypeName = "Office Spaces", Abbreviation = "OFS", CategoryId = 6 },
                new BoxType { BoxTypeId = 74, BoxTypeName = "Restaurant Spaces", Abbreviation = "RES", CategoryId = 6 },
                new BoxType { BoxTypeId = 75, BoxTypeName = "Common Areas", Abbreviation = "CMA", CategoryId = 6 },
                new BoxType { BoxTypeId = 76, BoxTypeName = "Shared Amenity Spaces", Abbreviation = "SAS", CategoryId = 6 },
                new BoxType { BoxTypeId = 77, BoxTypeName = "Parking Structures", Abbreviation = "PKS", CategoryId = 6 },
                new BoxType { BoxTypeId = 78, BoxTypeName = "Transit Connections", Abbreviation = "TRC", CategoryId = 6 },

                // Specialized Construction (CategoryId = 7)
                new BoxType { BoxTypeId = 79, BoxTypeName = "Green Buildings", Abbreviation = "GRN", CategoryId = 7 },
                new BoxType { BoxTypeId = 80, BoxTypeName = "Communication Towers", Abbreviation = "CMT", CategoryId = 7 }
            };

            modelBuilder.Entity<BoxType>().HasData(boxTypes);

            // Box SubTypes
            var boxSubTypes = new List<BoxSubType>
            {
                // Bedrooms subtypes (BoxTypeId = 4)
                new BoxSubType { BoxSubTypeId = 1, BoxSubTypeName = "Master", Abbreviation = "MBR", BoxTypeId = 4 },
                new BoxSubType { BoxSubTypeId = 2, BoxSubTypeName = "Guest", Abbreviation = "GBR", BoxTypeId = 4 },
                new BoxSubType { BoxSubTypeId = 3, BoxSubTypeName = "Children's", Abbreviation = "CBR", BoxTypeId = 4 },

                // Bathrooms subtypes (BoxTypeId = 5)
                new BoxSubType { BoxSubTypeId = 4, BoxSubTypeName = "Master", Abbreviation = "MBA", BoxTypeId = 5 },
                new BoxSubType { BoxSubTypeId = 5, BoxSubTypeName = "Guest", Abbreviation = "GBA", BoxTypeId = 5 },
                new BoxSubType { BoxSubTypeId = 6, BoxSubTypeName = "Powder Room", Abbreviation = "PWD", BoxTypeId = 5 },

                // Schools subtypes (BoxTypeId = 53)
                new BoxSubType { BoxSubTypeId = 7, BoxSubTypeName = "Classrooms", Abbreviation = "CLS", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 8, BoxSubTypeName = "Science Labs", Abbreviation = "SCI", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 9, BoxSubTypeName = "Computer Labs", Abbreviation = "CMP", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 10, BoxSubTypeName = "Library Center", Abbreviation = "LIB", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 11, BoxSubTypeName = "Gymnasium", Abbreviation = "GYM", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 12, BoxSubTypeName = "Cafeteria", Abbreviation = "CAF", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 13, BoxSubTypeName = "Auditorium", Abbreviation = "AUD", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 14, BoxSubTypeName = "Music Room", Abbreviation = "MUS", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 15, BoxSubTypeName = "Art Room", Abbreviation = "ART", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 16, BoxSubTypeName = "Administrative Offices", Abbreviation = "ADM", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 17, BoxSubTypeName = "Nurse's Office", Abbreviation = "NRS", BoxTypeId = 53 },
                new BoxSubType { BoxSubTypeId = 18, BoxSubTypeName = "Corridors", Abbreviation = "COR", BoxTypeId = 53 },

                // Hospitals subtypes (BoxTypeId = 54)
                new BoxSubType { BoxSubTypeId = 19, BoxSubTypeName = "Patient Rooms", Abbreviation = "PAT", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 20, BoxSubTypeName = "Operating Rooms", Abbreviation = "OR", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 21, BoxSubTypeName = "Emergency Room", Abbreviation = "ER", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 22, BoxSubTypeName = "Intensive Care Unit", Abbreviation = "ICU", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 23, BoxSubTypeName = "Examination Rooms", Abbreviation = "EXM", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 24, BoxSubTypeName = "Radiology", Abbreviation = "RAD", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 25, BoxSubTypeName = "Laboratory", Abbreviation = "LAB", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 26, BoxSubTypeName = "Pharmacy", Abbreviation = "PHM", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 27, BoxSubTypeName = "Waiting Rooms", Abbreviation = "WAT", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 28, BoxSubTypeName = "Nurses' Stations", Abbreviation = "NST", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 29, BoxSubTypeName = "Surgery Recovery", Abbreviation = "SRG", BoxTypeId = 54 },
                new BoxSubType { BoxSubTypeId = 30, BoxSubTypeName = "Cafeteria", Abbreviation = "CAF", BoxTypeId = 54 },

                // Government Buildings subtypes (BoxTypeId = 55)
                new BoxSubType { BoxSubTypeId = 31, BoxSubTypeName = "Offices", Abbreviation = "OFF", BoxTypeId = 55 },
                new BoxSubType { BoxSubTypeId = 32, BoxSubTypeName = "Courtrooms", Abbreviation = "CRT", BoxTypeId = 55 },
                new BoxSubType { BoxSubTypeId = 33, BoxSubTypeName = "Council Chambers", Abbreviation = "CNC", BoxTypeId = 55 },
                new BoxSubType { BoxSubTypeId = 34, BoxSubTypeName = "Public Service Areas", Abbreviation = "PSA", BoxTypeId = 55 },
                new BoxSubType { BoxSubTypeId = 35, BoxSubTypeName = "Archive/Records Room", Abbreviation = "ARC", BoxTypeId = 55 },
                new BoxSubType { BoxSubTypeId = 36, BoxSubTypeName = "Security Checkpoints", Abbreviation = "SEC", BoxTypeId = 55 },

                // Retail Spaces subtypes (BoxTypeId = 72)
                new BoxSubType { BoxSubTypeId = 37, BoxSubTypeName = "Ground Floor", Abbreviation = "GFL", BoxTypeId = 72 },

                // Office Spaces (Mixed-Use) subtypes (BoxTypeId = 73)
                new BoxSubType { BoxSubTypeId = 38, BoxSubTypeName = "Mid Floors", Abbreviation = "MFL", BoxTypeId = 73 },

                // Green Buildings subtypes (BoxTypeId = 79)
                new BoxSubType { BoxSubTypeId = 39, BoxSubTypeName = "Solar Panel Arrays", Abbreviation = "SOL", BoxTypeId = 79 },
                new BoxSubType { BoxSubTypeId = 40, BoxSubTypeName = "Green Roof Areas", Abbreviation = "GRF", BoxTypeId = 79 },
                new BoxSubType { BoxSubTypeId = 41, BoxSubTypeName = "Rainwater Harvesting Areas", Abbreviation = "RWH", BoxTypeId = 79 },
                new BoxSubType { BoxSubTypeId = 42, BoxSubTypeName = "Geothermal Equipment Rooms", Abbreviation = "GEO", BoxTypeId = 79 },

                // Communication Towers subtypes (BoxTypeId = 80)
                new BoxSubType { BoxSubTypeId = 43, BoxSubTypeName = "Equipment Shelters", Abbreviation = "EQS", BoxTypeId = 80 },
                new BoxSubType { BoxSubTypeId = 44, BoxSubTypeName = "Generator Rooms", Abbreviation = "GEN", BoxTypeId = 80 },
                new BoxSubType { BoxSubTypeId = 45, BoxSubTypeName = "Battery Backup Rooms", Abbreviation = "BAT", BoxTypeId = 80 },
                new BoxSubType { BoxSubTypeId = 46, BoxSubTypeName = "Antenna Platforms", Abbreviation = "ANT", BoxTypeId = 80 }
            };

            modelBuilder.Entity<BoxSubType>().HasData(boxSubTypes);
        }
    }
}
