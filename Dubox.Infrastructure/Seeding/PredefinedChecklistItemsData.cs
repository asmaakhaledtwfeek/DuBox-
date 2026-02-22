using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// AUTO-GENERATED - Contains all 698 checklist items from JSON
/// Generated: 2026-02-05 23:28:47
/// DO NOT EDIT MANUALLY - Regenerate using Generate-HasDataSeeding.ps1
/// </summary>
public static partial class ActivityCheckListItemSeedData
{
    /// <summary>
    /// Seeds all 698 PredefinedChecklistItems from JSON
    /// </summary>
    private static void SeedPredefinedChecklistItemsFromJSON(ModelBuilder modelBuilder, DateTime seedDate)
    {
        var predefinedItems = new List<PredefinedChecklistItem>
    {
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("61dbf221-b3ce-45db-ad29-16bc45261ab6"),
            Description = "Ensure method statement, materials and shop drawings are approved.",
            Reference = "Checklist 1 Part 1",
            Sequence = 101,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a3b8e083-476a-433f-b9ba-8de6c71d3987"),
            Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 1 Part 1",
            Sequence = 102,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e77b6ebc-457b-48cb-a85f-52ac3d188dcb"),
            Description = "Check the expiry date of the material prior to applications.",
            Reference = "Checklist 1 Part 1",
            Sequence = 103,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e3571af4-ff03-4ef2-ac48-b28072645f78"),
            Description = "Ensure Drawing Stamp, Signature, Element Tags are correct",
            Reference = "Checklist 1 Part 1",
            Sequence = 104,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ef9a8552-8712-4ba5-acaf-08b4785d067e"),
            Description = "Floor Setting Out/Layout as per drawing",
            Reference = "Checklist 1 Part 1",
            Sequence = 105,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("adcb3f21-4eec-4e5b-b1e4-157995e143b2"),
            Description = "Erection of external walls by Temporary Support (props and brackets) Outer",
            Reference = "Checklist 1 Part 2",
            Sequence = 106,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("28c53002-5447-48fb-bcf9-f957875807fe"),
            Description = "Dimensions, alignment and level are as per drawing.",
            Reference = "Checklist 1 Part 2",
            Sequence = 107,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("54849a4f-ae57-4b7c-b1f8-71ed34d995aa"),
            Description = "Sides and corners of boxes aligned with floor layout.",
            Reference = "Checklist 1 Part 2",
            Sequence = 108,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e5df2ee3-231f-4819-89e1-2fef578906a3"),
            Description = "Check if wall plumb of all sides are within the allowable tolerance",
            Reference = "Checklist 1 Part 2",
            Sequence = 109,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fa8fee07-e3ef-4be0-a4e4-f3dbd4e3c546"),
            Description = "Check height of the box at different location as per drawing",
            Reference = "Checklist 1 Part 2",
            Sequence = 110,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("631fedcb-4875-4def-9633-b89d7a04aaa4"),
            Description = "Check If all wet & mechanical connection done & repaired and has no wavy or bulge surface",
            Reference = "Checklist 1 Part 2",
            Sequence = 111,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0b218c7d-3952-4919-adf6-c4ea2796381b"),
            Description = "Check if panels are aligned to each other at joints.",
            Reference = "Checklist 1 Part 2",
            Sequence = 112,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bf9ed210-5464-4a25-88bd-2efea3e00929"),
            Description = "All joints on paint-able surfaces filled with sealant properly",
            Reference = "Checklist 1 Part 2",
            Sequence = 113,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1663d791-fc93-48f8-9bb6-f907ef708c77"),
            Description = "Check if external surface of the box is leveled and free from damages",
            Reference = "Checklist 1 Part 2",
            Sequence = 114,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5a6fcff3-aacd-46dc-b84b-d3369e3b5e3c"),
            Description = "Level Line to be marked at walls on 1000mm from FFL.",
            Reference = "Checklist 1 Part 3",
            Sequence = 115,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a74d7b03-d1d6-432b-abb3-1871d8a1c305"),
            Description = "Panel to Panel Connections are as per drawing",
            Reference = "Checklist 1 Part 3",
            Sequence = 116,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2643e140-a1d0-4ed8-9e93-35bf3f62e63e"),
            Description = "Erection & Connections of Floor Slab",
            Reference = "Checklist 1 Part 3",
            Sequence = 116,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ecd6ec00-531f-4d16-a7fd-381e466a6f38"),
            Description = "Dimensions (outer, inner and diagonal), Line and Level Grouting",
            Reference = "Checklist 1 Part 3",
            Sequence = 117,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d687bd92-0b37-452c-96ce-c0f3be44f178"),
            Description = "Bottom slab shall read 1020mm at 1000m FFL line or as per drawing",
            Reference = "Checklist 1 Part 3",
            Sequence = 117,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("026ad2af-90f5-4d93-a3df-1557fcefa60e"),
            Description = "Pod installed as per drawing - Level and alignment without any damages",
            Reference = "Checklist 1 Part 3",
            Sequence = 118,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("29b5036f-51ca-475c-aa6d-4de31094bfe3"),
            Description = "Erection of partition walls by Temporary Support",
            Reference = "Checklist 1 Part 3",
            Sequence = 118,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7bffb033-f8a8-4ce8-b04a-54c2b065afbc"),
            Description = "Erection & Connection of Roof Slab as per approved drawing and ensure box clear height",
            Reference = "Checklist 1 Part 3",
            Sequence = 119,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("82c75739-3247-4a93-bcb5-2b4a26160bc0"),
            Description = "Check If wet connections are proper and have no wavy, depressed or bulge surface.",
            Reference = "Checklist 1 Part 3",
            Sequence = 120,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3037c680-51ea-44b9-972c-60ab64ef1f63"),
            Description = "All joints on paint able surfaces filled with sealant properly.",
            Reference = "Checklist 1 Part 3",
            Sequence = 121,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("161db0eb-b332-4a4b-a4a4-5d40011d1acc"),
            Description = "All internal cracks are repaired (if any)",
            Reference = "Checklist 1 Part 3",
            Sequence = 122,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8ad24dee-22be-4fc4-9262-52aefd3fb8cb"),
            Description = "Internal and External Dimension of Box",
            Reference = "Checklist 1 Part 3",
            Sequence = 123,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a6ffd7ec-e8e8-413f-8855-d7db6fcea8a0"),
            Description = "Check for edges + Angles + grooves + chamfer + Pin holes + Cracks before moving to finishing area.",
            Reference = "Checklist 1 Part 3",
            Sequence = 124,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bee796aa-8146-409e-8792-834315d4afda"),
            Description = "The materials/type/model/capacity as per approved material submittal.",
            Reference = "Checklist 2",
            Sequence = 201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6ac5410e-4a6f-4fb5-a89f-0053a7d12bf1"),
            Description = "No visible damage on the materials.",
            Reference = "Checklist 2",
            Sequence = 202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("aff20e60-ac10-47c9-a106-cbbad6294d62"),
            Description = "Check the indoor unit location/height as per approved shop darwing.",
            Reference = "Checklist 2",
            Sequence = 203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5a7acb4b-0718-4c97-a683-3de6cdf1be2e"),
            Description = "Check the unit slope toward the condensate tray oulet.",
            Reference = "Checklist 2",
            Sequence = 204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5d78d34a-4d87-4fc9-8f98-9262534a60a8"),
            Description = "Check the approved type/capacity vibration isolator installed.",
            Reference = "Checklist 2",
            Sequence = 205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("22b095c4-2752-47a5-84e5-001ade385072"),
            Description = "Check the fans are freely rotating.",
            Reference = "Checklist 2",
            Sequence = 206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bbcb68ff-f912-46cc-ab6f-d7a37b8c4fcd"),
            Description = "Check the filters are installed, clean and accessible for maintenanace.",
            Reference = "Checklist 2",
            Sequence = 207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("96fa2a38-95e6-4906-86e6-c303bcec6e4a"),
            Description = "Check the motors are accessible for the maintenance.",
            Reference = "Checklist 2",
            Sequence = 208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4ef072b4-e4fa-4f9b-8720-99ddb5616b2c"),
            Description = "Check the dielectric unions/flexible connector are connected.",
            Reference = "Checklist 2",
            Sequence = 209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5971b29f-0df9-4b19-b35d-47e6a3958c12"),
            Description = "Check the chilled water/refrigerant piping are connectred with appropriate fittings",
            Reference = "Checklist 2",
            Sequence = 210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("01b1105d-23d3-4a68-9960-274a08ef35fe"),
            Description = "Check the valve package to the indoor unit as per approved.",
            Reference = "Checklist 2",
            Sequence = 211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7efe3e1d-84f6-451e-9e4a-a5ba9204bdeb"),
            Description = "Check the outdoor unit space around as per manufacturer recommendations.",
            Reference = "Checklist 2",
            Sequence = 212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e2b5f47c-7407-4941-821a-696f13580a32"),
            Description = "Pipe sizes are as per approved shop drawing.",
            Reference = "Checklist 2",
            Sequence = 213,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d2e84996-4bdc-4915-b126-fbeb409716a9"),
            Description = "Pipe layout/routing as per approved shop drawing.",
            Reference = "Checklist 2",
            Sequence = 214,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6ff2ba7c-2adc-48e9-bc22-56fc9d8840e1"),
            Description = "Sleeves are provided for the pipes passing through the walls/slabs.",
            Reference = "Checklist 2",
            Sequence = 215,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fd3237a4-b4c4-4a69-975a-f66e3d36ee2f"),
            Description = "Check the pipes are supported well with approved clamps.",
            Reference = "Checklist 2",
            Sequence = 216,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("58894a8d-1f59-46c0-b3a7-98d14c4c5aa2"),
            Description = "Check the insulation is properly done",
            Reference = "Checklist 2",
            Sequence = 217,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("67b9c827-8f14-46d0-af96-895a799bbc17"),
            Description = "Installed pipes are free of sag & bend.",
            Reference = "Checklist 2",
            Sequence = 218,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("773ce43b-1ff0-40d1-9be9-2a6c0c38e38a"),
            Description = "Check the condensate drain pipes are connected with flexible hose & p trap with proper slope.",
            Reference = "Checklist 2",
            Sequence = 219,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1b593558-70a9-4541-bcb2-2a2e6ece700e"),
            Description = "Check the duct connection to the unit, there is no deformity in the connector.",
            Reference = "Checklist 2",
            Sequence = 220,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9f7b29bb-0350-4fb5-b1f3-5f31024cf142"),
            Description = "Check the material used are as per the Approved material submittal.",
            Reference = "Checklist 3",
            Sequence = 301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fb87ef95-9f38-4742-af14-edab06e1d4b5"),
            Description = "Check conduit type and size as per approved shop drawings.",
            Reference = "Checklist 3",
            Sequence = 302,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1453996-58f8-46e6-bb8f-03d71514bf0b"),
            Description = "Check the drawing used for the installation are current and approved",
            Reference = "Checklist 3",
            Sequence = 303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2f10b5df-f070-4067-a502-fa9286017ea4"),
            Description = "Conduits installed in exposed areas subject to mechanical damage shall be IMC or RSC, and those exposed but not subject to mechanical damages shall be EMT. The conduits embedded in walls or encased in concrete and structural slabs shall be PVC conduits.",
            Reference = "Checklist 3",
            Sequence = 304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("23b2bdc4-e83f-48a6-a682-efafa59137d3"),
            Description = "Installation of boxes, pull and Junction boxes, Outlet boxes shall be accommodate orientation of wiring devices as indicated on drawings.",
            Reference = "Checklist 3",
            Sequence = 305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b7bc2c74-6feb-437e-aabd-005c4067a671"),
            Description = "The fittings used with embedded conduit shall be concrete tight and water tight.",
            Reference = "Checklist 3",
            Sequence = 306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("aadad362-9368-438c-a750-bb1d76b110c0"),
            Description = "Locknuts shall be provided to protect the wire from abrasion unles in the design.",
            Reference = "Checklist 3",
            Sequence = 307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("52e69220-82b5-48e7-8f49-c03ead60ac76"),
            Description = "Check that grounding has been done according to the project Specification",
            Reference = "Checklist 3",
            Sequence = 308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c9b64e80-1ad4-4476-b0a5-d3dff37cc2c3"),
            Description = "The conduits shall be securely fastened at intervals not exceeding 2m for 20 and 25mm rigid metal conduits and 3 meter for all larger sizes.",
            Reference = "Checklist 3",
            Sequence = 309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f1c59dd4-ea9c-4309-a2c2-53779925a105"),
            Description = "Interior boxes shall be cleaned to remove dust, derbis and other material. Exposed surfaces shall also be cleaned and finish restored.",
            Reference = "Checklist 3",
            Sequence = 310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6917f5c3-4626-46e6-89c4-bf50a70d7d21"),
            Description = "Verify that all conduit covers installed properly after the cable pulling activity.",
            Reference = "Checklist 3",
            Sequence = 311,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("720f23c1-de7a-4f3a-a933-e66756f81085"),
            Description = "Ensure method statement, materials and drawings are approved.",
            Reference = "Checklist 4 Part Part 1",
            Sequence = 401,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("717ec181-9fb8-4f3f-a1e3-6fa22e64273c"),
            Description = "Ensure materials are stored as per manufacturers recommendations.",
            Reference = "Checklist 4 Part Part 1",
            Sequence = 402,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2e6d9fd3-5e78-489c-9f40-e4445e6abc7a"),
            Description = "Verify the expiry date and number of coats of the material prior to applications.",
            Reference = "Checklist 4 Part Part 1",
            Sequence = 403,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3cc68162-5b07-4fd6-a1a3-e33a9c09699f"),
            Description = "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.",
            Reference = "Checklist 4 Part Part 1",
            Sequence = 404,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2b4c0a33-8f78-47af-8146-d9264b49f544"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 4 Part Part 2",
            Sequence = 405,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d00cd57e-f7f7-4165-8e01-4fa5b1e0e50b"),
            Description = "Check for repair of surface imperfection and protrusions (if any).",
            Reference = "Checklist 4 Part Part 2",
            Sequence = 406,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ed6a11d7-c24f-400d-984a-32e093530948"),
            Description = "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.",
            Reference = "Checklist 4 Part Part 2",
            Sequence = 407,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f70f733e-157f-48e1-a295-c366eeecf298"),
            Description = "Undulations or irregular corners are repaired and grinded as required.",
            Reference = "Checklist 4 Part Part 2",
            Sequence = 408,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("991876b8-6854-46aa-947e-5adadfe6874c"),
            Description = "Ensure application of Primer as per manufacturers recommendation",
            Reference = "Checklist 4 Part Part 3",
            Sequence = 409,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bed4eccc-1d32-42b6-9005-3c4c3b964a46"),
            Description = "Ensure application of Stuccoo as per manufecturers recommendation",
            Reference = "Checklist 4 Part Part 3",
            Sequence = 410,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6eadd7bd-f336-499d-93aa-adb7f8095cb3"),
            Description = "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.",
            Reference = "Checklist 4 Part Part 4",
            Sequence = 411,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e35eeacc-154a-4683-a1be-a80db49cbb24"),
            Description = "Application of final coat of Paint as per manufacturers recommendation.",
            Reference = "Checklist 4 Part Part 4",
            Sequence = 412,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("657781a7-a5c0-48f3-b8ea-f076f073d9d3"),
            Description = "Line between two color shades is straight, no Brush marks should be visible.",
            Reference = "Checklist 4 Part Part 4",
            Sequence = 413,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("926eff20-75bb-44aa-b000-6dcf1f97e765"),
            Description = "Ensure application of Primer as per manufacturers recommendation",
            Reference = "Checklist 4 Part Part 5",
            Sequence = 414,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7b6cb652-94ab-4b6d-bc9f-2c0f39b63911"),
            Description = "Touchup, grinding, undulations, corner repairs and pinholes are filled properly.",
            Reference = "Checklist 4 Part Part 6",
            Sequence = 415,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5f0d0363-e564-41bf-a1e3-cb413722a464"),
            Description = "Ensure application of Texturer Coat as per manufacturers recommendation",
            Reference = "Checklist 4 Part Part 6",
            Sequence = 416,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f904a018-938e-47a9-a6ed-6458eb65c6b6"),
            Description = "Application of final coat of Paint as per manufacturers recommendation.",
            Reference = "Checklist 4 Part Part 7",
            Sequence = 417,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("892c342c-7ed2-4aa5-ba6a-01ef0bff039e"),
            Description = "Approved shop drawings should be followed",
            Reference = "Checklist 5",
            Sequence = 501,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("069935b6-81a2-4be4-92ec-d06c72b342ad"),
            Description = "The materials are approved",
            Reference = "Checklist 5",
            Sequence = 502,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ffb783aa-f1a6-4745-a576-ae3839eea255"),
            Description = "No visible damage on the materials",
            Reference = "Checklist 5",
            Sequence = 503,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("59988b94-9042-4b79-8336-24a79a29679a"),
            Description = "Pipe sizes are as per approved shop drawing",
            Reference = "Checklist 5",
            Sequence = 504,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("36a128fd-e1e0-481c-8058-364cea0f0f65"),
            Description = "Pipe layout/routing as per approved shop drawing",
            Reference = "Checklist 5",
            Sequence = 505,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0ab3367a-c7dc-466f-9d5f-13f26b1991e8"),
            Description = "Slope should be maintained for all pipes",
            Reference = "Checklist 5",
            Sequence = 506,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8f6217b4-1852-4a21-b33f-f87463d91192"),
            Description = "Installation as per approved method statement",
            Reference = "Checklist 5",
            Sequence = 507,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("abd76b02-af66-4908-bc75-19b9cf608d52"),
            Description = "Sleeves are provided for the pipes passing the structural memebers",
            Reference = "Checklist 5",
            Sequence = 508,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fd29172b-3dae-46bc-b98d-186378bf122a"),
            Description = "Drainage pipes are not passing above electrical services",
            Reference = "Checklist 5",
            Sequence = 509,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3cc2cc5d-f8db-4223-ab0a-be9b2a0b66ca"),
            Description = "Installed pipes are free of sag & bend",
            Reference = "Checklist 5",
            Sequence = 510,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("02b3d45c-8f78-4bc5-88ff-3a62cc5e5d63"),
            Description = "Vent pipes are provided to the manhole as per approved drawing",
            Reference = "Checklist 5",
            Sequence = 511,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2c82a768-799c-429e-b5db-db1d00707f7e"),
            Description = "Pipe joints are properly made and are tight/secure",
            Reference = "Checklist 5",
            Sequence = 512,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("71c55beb-dc82-4f99-805b-64dbe17ab587"),
            Description = "Pipes are supported properly vertically&horizontally",
            Reference = "Checklist 5",
            Sequence = 513,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("56ddc743-2253-4d9e-9cb9-6adf03cc1562"),
            Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.",
            Reference = "Checklist 6 Part 1",
            Sequence = 601,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cea10df0-242b-4a53-b9c1-66dd6ccc42d5"),
            Description = "Ensure materials are stored as per manufacturers recommendations.",
            Reference = "Checklist 6 Part 1",
            Sequence = 602,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6d3d2c0a-9fd0-462e-a9e0-a28789303e7e"),
            Description = "Verify the expiry date of the material prior to applications.",
            Reference = "Checklist 6 Part 1",
            Sequence = 603,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("84597bdb-452d-4912-978a-cb56447cbed2"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 6 Part 1",
            Sequence = 604,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4f34ae92-39ac-41cd-b0cd-e13984bfa9d7"),
            Description = "Check surface is levelled and sloped as per the approved drawings.",
            Reference = "Checklist 6 Part 1",
            Sequence = 605,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("73c338fb-9162-41de-96b9-7c14ceb30251"),
            Description = "Check the surface roughness is appropriate as per the tile and tile adhesive manufacturer recommendations.",
            Reference = "Checklist 6 Part 1",
            Sequence = 606,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fcf30004-723a-464d-ba53-442ec79b1867"),
            Description = "Verify the application of wet area water proofing and leak test.",
            Reference = "Checklist 6 Part 1",
            Sequence = 607,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c3aadee8-5ec8-42c9-a279-47a0e0ecd308"),
            Description = "Verify the location and level of floor drain as per the approved drawings.",
            Reference = "Checklist 6 Part 1",
            Sequence = 608,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("23ae56ac-8cb1-4e5f-b9be-b198c0d800c1"),
            Description = "Check the MEP clearance prior to start Painting works.",
            Reference = "Checklist 6 Part 1",
            Sequence = 609,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c5ab8cab-e134-4564-959d-a7aee656a496"),
            Description = "Ensure proper mixing of material as per the manufacturer recommendations.",
            Reference = "Checklist 6 Part 2",
            Sequence = 610,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3018672f-f4e9-4982-9af5-6570666f6b63"),
            Description = "Check the Location, Colour & Type of tile as per the approved shop drawings / material submittal.",
            Reference = "Checklist 6 Part 2",
            Sequence = 611,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c3d22ad1-3839-4178-b547-e2aa0ef155d1"),
            Description = "Check the application of tile adhesive using notch trowel on the backside of tile and over the substrate.",
            Reference = "Checklist 6 Part 2",
            Sequence = 612,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("dd3e4ee1-68e1-4ecf-b344-800b85fdcca0"),
            Description = "Verify the tile spacers width is uniform and aligned as per the appoved drawings.",
            Reference = "Checklist 6 Part 2",
            Sequence = 613,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("893ae3d5-1738-4d43-b185-5950b6423687"),
            Description = "Check the setting out / pattern of wall and floor tiles as per the approved drawings.",
            Reference = "Checklist 6 Part 2",
            Sequence = 614,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("306ec504-6fd2-4d25-92e4-a468dc0a5f74"),
            Description = "Verify the slope and level of the tiles as per the approved drawings.",
            Reference = "Checklist 6 Part 3",
            Sequence = 615,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("049c0dcf-2415-4bea-a1e8-764306ef916d"),
            Description = "Check the laying of tiles above the false ceiling as per the approved drawings.",
            Reference = "Checklist 6 Part 3",
            Sequence = 616,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("32d1fc5f-1152-4474-afe9-c5f1971bda1e"),
            Description = "Check the pull off / adhesion test for the paint application as per the project specifications / manufacturer recommendations (if required).",
            Reference = "Checklist 6 Part 3",
            Sequence = 617,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("44fc7108-33d6-4ca2-b433-b2d444d67cce"),
            Description = "Approval to proceed with further works obtained from consultant / Client.",
            Reference = "Checklist 6 Part 3",
            Sequence = 618,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cc797cd6-8d1a-4c00-b66c-aa69023e0ba4"),
            Description = "Check the tile joints are clean and free from contaminants like dust etc.",
            Reference = "Checklist 6 Part 4",
            Sequence = 619,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bc75eb0e-1a37-42bc-982d-6ad8b1f4a2c0"),
            Description = "Ensure proper mixing of material as per the manufacturer recommendations.",
            Reference = "Checklist 6 Part 4",
            Sequence = 620,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c10ae965-efe4-4132-af3a-d5873a6441fa"),
            Description = "Check the Location, Colour & Type of grout as per the approved shop drawings / material submittal.",
            Reference = "Checklist 6 Part 4",
            Sequence = 621,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("757cce09-ea85-45b8-b54e-4f3f6a0d73fd"),
            Description = "Check the width of tile grout is proper and uniform.",
            Reference = "Checklist 6 Part 4",
            Sequence = 622,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6989a14b-0402-451d-85e7-0e74e418a0b5"),
            Description = "Check the application of tile grout as per the manufacturer recommendations.",
            Reference = "Checklist 6 Part 4",
            Sequence = 623,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("12741670-ff21-4d54-890a-dcf16830a502"),
            Description = "Approval to proceed with further works obtained from consultant / Client.",
            Reference = "Checklist 6 Part 4",
            Sequence = 624,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("df6d95d8-7b9f-4b40-b8c3-b5043e8d7747"),
            Description = "Check the material used are as per the Approved Material Submittal.",
            Reference = "Checklist 7",
            Sequence = 701,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a20a7a0c-69e9-490a-b4a0-22fbabb91137"),
            Description = "Pipe sizes are as per approved shop drawing.",
            Reference = "Checklist 7",
            Sequence = 702,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("22a965bb-1b50-44fe-9878-250d0840ce92"),
            Description = "Check the drawing used for the installation are current and approved.",
            Reference = "Checklist 7",
            Sequence = 703,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cd4abf01-5c94-4ef1-bb07-50d80292311c"),
            Description = "Check the Horizontal pipes are supported well and with approved clamps.",
            Reference = "Checklist 7",
            Sequence = 704,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3e6ec60a-5ffd-4452-a46d-35cf901f7e30"),
            Description = "Ensure the sleeves are provided on structural elements where the pipes are penetrating to & from the Building /Partician/ Walls / Slabs",
            Reference = "Checklist 7",
            Sequence = 705,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b4112462-7d49-478e-bc50-adc81aa08174"),
            Description = "Pipe Joints are properly made tight & secure, Check the vertical riser of the pipes are supported well with approved clamp.",
            Reference = "Checklist 7",
            Sequence = 706,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9cba4c7f-9368-4c80-8a87-3b2c47399bc5"),
            Description = "Water pipes are not passing above electrical services and minimum clearance provided as per project Spec's.",
            Reference = "Checklist 7",
            Sequence = 707,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1c5ba2b-faa5-4e7c-a9da-82bb9bd9a77b"),
            Description = "Check location / size of valves are provided as per approved shop drawing and specification",
            Reference = "Checklist 7",
            Sequence = 708,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d16cea31-413c-40c0-bd0c-6756eaef72da"),
            Description = "Shut off Valves shall be arranged closed, when rotating in clock wise direction and provided as per approved Shop drawings",
            Reference = "Checklist 7",
            Sequence = 709,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8f916deb-addb-4085-a9dc-fa723d99ef1e"),
            Description = "Hydrotest of piping shall included the valves, valves shall be in open position at the tome of testing. Hydrotest pressure shall be equivalent to piping test pressure (i.e) 1.5 Times the operating pressure.",
            Reference = "Checklist 7 Part 1",
            Sequence = 710,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cb52c317-595a-4b4b-bf49-8b37eaca4a87"),
            Description = "After Hydrotest Valves shall be fully wrapped with corrossion resistant adhesive tape up to spindle of the valve.",
            Reference = "Checklist 7",
            Sequence = 711,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d601a1f8-3452-443a-a599-137f30d29b63"),
            Description = "Water Hammer arrestor installed in upright position",
            Reference = "Checklist 7",
            Sequence = 712,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c65baecf-6df1-435f-a6ce-a859844f0cdb"),
            Description = "Insulation shall only be applied to piping after all testing has been completed.",
            Reference = "Checklist 7",
            Sequence = 713,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b42d3f72-13ca-4bc8-b897-9db19bd693cb"),
            Description = "All open pipe ends or fitting openings shall be plugged or capped immediately during construction.",
            Reference = "Checklist 7",
            Sequence = 714,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9f06aefc-5b8d-48f5-b69e-4ea15e436413"),
            Description = "All surface to be insulated shall be dry and free from loose scale, dirt, oil or water when insulation is applied.",
            Reference = "Checklist 7",
            Sequence = 715,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ef3a3b54-0c88-492b-9472-8bc965770a5a"),
            Description = "Check hot water pipes and fittings are insulated and damaged insulation is replaced & Masking Tape is provided on every joints of insulation.",
            Reference = "Checklist 7",
            Sequence = 716,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cd003b5e-94e2-4e90-b19d-8b0ae59936ad"),
            Description = "Check the all materials used have approved submittals.",
            Reference = "Checklist 8",
            Sequence = 801,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2e5c1134-9d70-4b88-bac1-ee3e90aaefb6"),
            Description = "Ensure drawings used for installation are current and approved.",
            Reference = "Checklist 8",
            Sequence = 802,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e95d42c9-5c26-4dfa-b7fe-7d234b1d63a1"),
            Description = "Inspect all the accessories used are new and undamaged.",
            Reference = "Checklist 8",
            Sequence = 803,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ed63ba28-4165-48c5-82b5-d98937f1ea9d"),
            Description = "Ensure the routing and layout is as per approved construction drawings.",
            Reference = "Checklist 8",
            Sequence = 804,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d5cd4b31-5313-41df-a50e-3b393320d80e"),
            Description = "Check the sleeves are installed for piping passing through walls / penetrations as required.",
            Reference = "Checklist 8",
            Sequence = 805,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("40c52768-3568-4b07-8216-50d09bcf3b2e"),
            Description = "Check the Pipe levels as per the approved shop / coordination drawing.",
            Reference = "Checklist 8",
            Sequence = 806,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e591842c-88e8-4942-b8b0-db5e73d5f4ad"),
            Description = "Check the pipe alignment.",
            Reference = "Checklist 8",
            Sequence = 807,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("152b49a2-ad74-4120-ab1e-d331cdb1a49e"),
            Description = "Check the marking, supports and pipes installation done as per approved shop drawings.",
            Reference = "Checklist 8",
            Sequence = 808,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1c1d88e4-1afd-4cc1-a9ad-216595215ca8"),
            Description = "Check the pipe painting as per approved method statement.",
            Reference = "Checklist 8",
            Sequence = 809,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("eec5f209-bdb2-46d6-9861-14112a288cc9"),
            Description = "Check that the identification labels are provided for the installed piping.",
            Reference = "Checklist 8",
            Sequence = 810,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3a1b0480-7ff3-4ef5-8fbb-dfe21b681e58"),
            Description = "Check the locations of zone control valve as per the approved drawing.",
            Reference = "Checklist 8",
            Sequence = 811,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c26fb3ab-072f-4fb1-afec-fe95711f72b3"),
            Description = "Check the Drain points are provided with approved valves. (Test & Drain system).",
            Reference = "Checklist 8",
            Sequence = 812,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cb75d150-8a53-49c0-a935-63cdd843a440"),
            Description = "Checks the location of Fire hose cabinet, Fire Hose reel, Pressure Reducing Landing valves, Fire hoses with Nozzles are as per the approved drawing.",
            Reference = "Checklist 8",
            Sequence = 813,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2b92ad8e-d7a0-4d9e-9c23-f95760fb7129"),
            Description = "Check the location of sprinkler heads as per the approved drawing.",
            Reference = "Checklist 8 Part 1",
            Sequence = 814,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f13cca20-ba72-4b28-bef3-0d47869f8af9"),
            Description = "Check the fire extinguisher location as per approved drawing",
            Reference = "Checklist 8",
            Sequence = 815,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f2a294c0-55c2-43f7-a343-650b1cc55816"),
            Description = "Check the Fire Blankets location as per approved drawing",
            Reference = "Checklist 8",
            Sequence = 816,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("96170a89-54b4-4779-99ad-91cf45346f40"),
            Description = "Check the location of O,S & Y Gate Valve, NRV, pressure reducing valve, pressure relief valve and pressure gauge as per the approved drawing and accessible for maintenance.",
            Reference = "Checklist 8",
            Sequence = 817,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b1e0bb19-0e2b-4e6e-982b-d4cb384336ce"),
            Description = "Check Automatic Air Release valves are provided on the highest pipe points on Risers.",
            Reference = "Checklist 8",
            Sequence = 818,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a8562dc1-0e53-48c3-b023-9f2e7cea4209"),
            Description = "The materials are approved.",
            Reference = "Checklist 9",
            Sequence = 901,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("004d668e-007e-4e74-8932-2fd476b72e45"),
            Description = "No visible damage on the materials.",
            Reference = "Checklist 9",
            Sequence = 902,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("72eedc0c-8085-486c-bd33-2b86f84edfb6"),
            Description = "Pipe sizes are as per approved shop drawing.",
            Reference = "Checklist 9",
            Sequence = 903,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("839ba238-3fff-4c3b-ae00-9eded558df16"),
            Description = "Pipe layout/routing as per approved shop drawing.",
            Reference = "Checklist 9",
            Sequence = 904,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e4598587-04d4-4cbb-978d-ebfc3d6aa3cc"),
            Description = "Alignment of the pipes as per approved shop drawing.",
            Reference = "Checklist 9",
            Sequence = 905,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("14d8c774-0ce7-4d32-9ffa-5bca94f71a4e"),
            Description = "Installation as per approved method statement.",
            Reference = "Checklist 9",
            Sequence = 906,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("59d155c0-470a-487d-961d-4487c61a0d74"),
            Description = "Sleeves are provided for the pipes passing through the walls/slabs.",
            Reference = "Checklist 9",
            Sequence = 907,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6f2ca44e-d722-4c4d-b4b3-97456d3ae463"),
            Description = "Check the pipes are supported well with approved clamps.",
            Reference = "Checklist 9",
            Sequence = 908,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4fc68421-08b9-4bd1-8c7e-6fe0d9213e0b"),
            Description = "Pipes are not passing above electrical services.",
            Reference = "Checklist 9",
            Sequence = 909,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("07095085-5014-4fc1-a911-8e15a27d0048"),
            Description = "Installed pipes are free of sag & bend.",
            Reference = "Checklist 9",
            Sequence = 910,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c7818010-1622-43a9-81a2-d6e83ef2202d"),
            Description = "Valves & accessories are intalled as per approved layout",
            Reference = "Checklist 9",
            Sequence = 911,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("43155025-7f6d-435e-81ea-3958a68a498d"),
            Description = "Pipe joints are properly made and are tight/secure.",
            Reference = "Checklist 9",
            Sequence = 912,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b40d990a-10a2-4da5-8d05-29449606e6e9"),
            Description = "Check the distance between the support as per specification.",
            Reference = "Checklist 9",
            Sequence = 913,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cbae421c-c4fe-4150-916d-27f1b370322d"),
            Description = "Check the pipe routing are coordinated with other services.",
            Reference = "Checklist 9",
            Sequence = 914,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("dc188967-d599-4c3f-9ae3-23f201bbba85"),
            Description = "Ensure that all pipes installation shall be as per manufacturer recommendation and project specs.",
            Reference = "Checklist 10",
            Sequence = 1001,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f76c18db-8644-4d91-be9f-8ad9c172c858"),
            Description = "Ensure that the piping accessories installed as per approved project specs.",
            Reference = "Checklist 10",
            Sequence = 1002,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3a81f6c2-231a-46f9-857e-17d23a119586"),
            Description = "Ensure that Pipe work to properly fixed and supported using a recognised and approved Manufacturer's support system.",
            Reference = "Checklist 10",
            Sequence = 1003,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("77cdad70-70ad-40b8-a191-15823c066d41"),
            Description = "Ensure that the gauges installed as per approved shop drawings.",
            Reference = "Checklist 10",
            Sequence = 1004,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1001d97f-e609-4631-b22f-5616baaf7198"),
            Description = "Ensure that the valves installed as per approved shop drawings, approved specs and as per manufacturer recommendation.",
            Reference = "Checklist 10",
            Sequence = 1005,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("414740e6-f97c-42f3-9a61-872c8d1ab8ec"),
            Description = "Ensure that pipe work shall be suitably identified and labeled where specified.",
            Reference = "Checklist 10",
            Sequence = 1006,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ff0a3b47-9679-47e4-94c6-6250e0291aa2"),
            Description = "Ensure that Leakage test done as per approved specs.",
            Reference = "Checklist 10",
            Sequence = 1007,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2195a9c1-2022-4ef1-83de-b0327435f906"),
            Description = "Ensure method statement and materials are approved.",
            Reference = "Checklist 11",
            Sequence = 1101,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7a16d424-a7aa-48c9-a15c-6d8a6f5562ba"),
            Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 11",
            Sequence = 1102,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("85a4c99f-35e4-4ca8-85f7-366cd2076525"),
            Description = "Check the expiry date of the material prior to applications.",
            Reference = "Checklist 11",
            Sequence = 1103,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8287c211-5676-4c02-be6d-e62ff7836540"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 11",
            Sequence = 1104,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f664e4ba-1fbb-4297-9d07-60bcfee9671f"),
            Description = "Check for repair of surface imperfection and protrusions (if any).",
            Reference = "Checklist 11",
            Sequence = 1105,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ba8c0ea3-20f1-43a5-88e3-97653e35be29"),
            Description = "Check the angle fillets and chamfering of all sharp edges (if required).",
            Reference = "Checklist 11",
            Sequence = 1106,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d9fc6fdc-2ffb-4bd4-af82-d91afecdc2b2"),
            Description = "Check the MEP clearance are obtained prior to start bitumen application.",
            Reference = "Checklist 11",
            Sequence = 1107,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7a423526-e5ee-4d6f-808c-1dc01130a412"),
            Description = "Ensure proper mixing as per the manufacturer recommandations.",
            Reference = "Checklist 11",
            Sequence = 1108,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a363610e-3d39-4882-bdd8-4c10c69ab008"),
            Description = "Check the application of primer coat (if required).",
            Reference = "Checklist 11",
            Sequence = 1109,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("459ca8e6-d443-4d10-812f-a8c3d00f5974"),
            Description = "Check the rate of application as per the manufacturer recommendation and method statement.",
            Reference = "Checklist 11",
            Sequence = 1110,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9d78085f-46bf-47dd-9d8d-f4097a0f99ac"),
            Description = "Check the application of coats as per project requirements / manufacturer recommandations.",
            Reference = "Checklist 11",
            Sequence = 1111,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e001b31f-dee9-4dc2-b974-d42ae14fe929"),
            Description = "Check the application of subsequent coats are carried out at right angle to the previous coat.",
            Reference = "Checklist 11",
            Sequence = 1112,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7ef294b5-4267-4020-98ff-d95c91820a81"),
            Description = "Check the WFT as per the project specifications / manufacturer recommandations.",
            Reference = "Checklist 11",
            Sequence = 1113,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6fd78816-190c-49c9-a046-a916a3e5096a"),
            Description = "Check the curing of application as per manufacturer recommendation.",
            Reference = "Checklist 11",
            Sequence = 1114,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("be8cf534-2bc0-4114-8804-1898a624b2b9"),
            Description = "Approval obtained from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 11",
            Sequence = 1115,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e1768d29-e5bf-433d-8463-cceeaf9d43ba"),
            Description = "Ensure the materials are as per approved material submitted.",
            Reference = "Checklist 12",
            Sequence = 1201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("192f93c2-19e8-4416-9c83-2f2eaaee8e1a"),
            Description = "Ensure drawings are used for installation are current and approved.",
            Reference = "Checklist 12",
            Sequence = 1202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("aba1ba5b-70f6-4f43-a402-c7292f08cf44"),
            Description = "Check that only in properly fabricated fittings are used for changes in directions, shapes,sizes and connections.",
            Reference = "Checklist 12",
            Sequence = 1203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("01fa0165-9a30-4f14-b07d-0030a5d81be4"),
            Description = "The joints and flanges are correctly made, jointed and sealed.",
            Reference = "Checklist 12",
            Sequence = 1204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("560567e0-3a43-4c4a-8907-13a599b3f376"),
            Description = "Check that duct joints are sealed externally before applying insulation with approved sealant.",
            Reference = "Checklist 12",
            Sequence = 1205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("58e3194f-d8e6-43be-87c5-5effc255579d"),
            Description = "Check fixing of supports and spacing as approved drawings & submittal.",
            Reference = "Checklist 12",
            Sequence = 1206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b32a6747-d042-42d1-bc3a-82510395069a"),
            Description = "Check acoustic lining is properly fastened and un damaged.",
            Reference = "Checklist 12",
            Sequence = 1207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a67cac3b-c555-4e95-82ac-ac9bc006c117"),
            Description = "Insulation is applied as per manufacturer's instructions anf finished smooth and straight without any damages.",
            Reference = "Checklist 12",
            Sequence = 1208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1d81723-1ea8-4465-96f5-e88d257b6aca"),
            Description = "Check nuts, bolts, screws, brackets drop rods etc. are tight and aligned properly.",
            Reference = "Checklist 12",
            Sequence = 1209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8a69c0d9-d0f7-46dd-83d1-db53f669e0d9"),
            Description = "All the access doors, fire dampers,VCDs etc are installed as per approved drawings, specification and manufacturer instructions as applicable.",
            Reference = "Checklist 12",
            Sequence = 1210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4ceb14da-adbe-4168-97b3-a9f81d059061"),
            Description = "Flexible duct connectors are provided at building expansion joints.",
            Reference = "Checklist 12",
            Sequence = 1211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d475c7c3-a442-43ca-b174-4e109fc3a63a"),
            Description = "Ensure that the identification & labeling are provided for duct works.",
            Reference = "Checklist 12",
            Sequence = 1212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("35746651-3a2e-451d-8630-f118ca112d5b"),
            Description = "Check the cables and accessories are as per approved material submittal",
            Reference = "Checklist 13",
            Sequence = 1301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4cdbc40e-da7c-4387-891c-946bed80f575"),
            Description = "Check and ensure the drawings used for installation are current and approved.",
            Reference = "Checklist 13",
            Sequence = 1302,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("62bcddb9-6f20-4398-9130-343bec17b840"),
            Description = "Ensure containment has been completed prior to start cable or wire pulling",
            Reference = "Checklist 13",
            Sequence = 1303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5bc8b482-c70f-4760-8c6f-088f2591d953"),
            Description = "Ensure containment has been cleaned prior to start pulling",
            Reference = "Checklist 13",
            Sequence = 1304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fc731547-8cb5-440e-8975-3baacbffcdd7"),
            Description = "Check the cable and other associated material are new and undamaged.",
            Reference = "Checklist 13",
            Sequence = 1305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("85bbd21a-8566-4959-ba4d-8db0ef4f909a"),
            Description = "Check insulation resistance test prior to start the cable pulling.",
            Reference = "Checklist 13",
            Sequence = 1306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c2bebfe1-18ad-4583-b30d-40cea622656f"),
            Description = "Check that the cables are securely fixed.",
            Reference = "Checklist 13",
            Sequence = 1307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7915ef1a-9945-4bdd-9043-470195ecdc0d"),
            Description = "Check and ensure that the cable sizes are as approved drawing.",
            Reference = "Checklist 13",
            Sequence = 1308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3715d4ab-769d-4c5e-b6ae-83e9b15248b7"),
            Description = "Give a temporary tagging before cable pulling to track the circuits during termination",
            Reference = "Checklist 13",
            Sequence = 1309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f779f4ec-1e54-4092-babd-66d7ff83f462"),
            Description = "Protect the edges of the cable after it has pulled.",
            Reference = "Checklist 13",
            Sequence = 1310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8ccf6166-b9c1-44ff-8067-e4cc27ff1db8"),
            Description = "Check the installation of cables is co-ordinated with other cables",
            Reference = "Checklist 13",
            Sequence = 1311,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("72f229ad-caa2-4a34-989d-1f2366d736bd"),
            Description = "Check the installation of cables as per approved drawings.",
            Reference = "Checklist 13",
            Sequence = 1312,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("40eb757f-abee-4fc1-8ae8-fda8c4ee35a5"),
            Description = "Ensure method statement, material submittal and drawings are approved.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1401,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("77bebd5b-be73-4222-aeb6-f0ead9ca3d50"),
            Description = "Ensure materials (Gypsum board, cement board, insulation material, supporting system, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1402,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("96cbc744-d028-447f-8095-d16a5f9129db"),
            Description = "Check the color, type, material, fire rating and thickness are as per approved material approval and project requirements.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1403,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1743b09c-66a2-423a-89d6-cdd3c562e23e"),
            Description = "Verify and record the DCL product confirmity certificate for the insulation materials.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1404,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1cf7703e-e801-438d-9a8b-d44f75dd418d"),
            Description = "Verify the marking and setting out of the partition walls as per the approved drawings.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1405,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9ec4582f-8977-483f-897f-b00fac69c83c"),
            Description = "Verify the completion of the required finishes of the adjacent substrates.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1406,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("972d77ec-f24f-4efa-93eb-06a726a82061"),
            Description = "Verify the location, spacing and fixation of the supporting grid (vertical and horizontal channel, wall angle, etc.) as per the approved drawings.",
            Reference = "Checklist 14 Part 1",
            Sequence = 1407,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("50a6984c-cc7f-41a3-8706-53d946319683"),
            Description = "Verify the fixation of the board (on one side of the supports) as per the approved drawings.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1408,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cbdc978f-0f4d-436c-ba0b-f26d408517c8"),
            Description = "Ensure the completion of all embedded MEP and other dicipline works prior to closure as per the approved drawings.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1409,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7007375b-1b9d-4652-839a-29e75bb8231d"),
            Description = "Ensure additional supports are provided for the wall mounted fixtures as applicable.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1410,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9c05dad0-b050-4dfa-bd9b-2bcf4c5b3828"),
            Description = "Verify the installation of insulation works (if applicable) as per the approved drawings.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1411,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f7aca766-9759-4400-9c8e-6276b7013f99"),
            Description = "Obtain approval (Civil / MEP) from consultant / Client to proceed with further works.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1412,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ccca5b4e-a539-40ce-a3f8-5c2ca26a29fe"),
            Description = "Verify the fixation of the board as per the approved drawings.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1413,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("dcf7f5cc-12f8-4419-a07f-c39d2d8bbee4"),
            Description = "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1414,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ea921132-c270-4a95-84c5-151f69859bf4"),
            Description = "Verify the marking, position and alignment of MEP & wall mounted fixtures in the wall as per the approved drawings.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1415,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c4db9ab6-44a4-4b73-8349-0c80b72b46b3"),
            Description = "Ensure the cutting of gypsum board on the marked locations as per the project requirements.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1416,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4c295d35-b989-47e4-a364-a782a02d5c45"),
            Description = "Verify the jointing & taping as per the manufacturer recommendations.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1417,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("42ad2e83-5e80-4955-817d-6daec262c4a8"),
            Description = "Approval obtain from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 14 Part 2",
            Sequence = 1418,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("153bb96b-2d77-4d6a-bec6-86866d4c4dcc"),
            Description = "Check cable & accessories, used for installation have approved submittals",
            Reference = "Checklist 15",
            Sequence = 1501,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("09757220-4a69-47e1-94a9-e71725b6d869"),
            Description = "Check the drawings used for installation are current & approved.",
            Reference = "Checklist 15",
            Sequence = 1502,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("30602cc2-18d3-4246-8b6e-2d1a713999b6"),
            Description = "Check the cables/Wires & accessories and materials are new & undamaged.",
            Reference = "Checklist 15",
            Sequence = 1503,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9cfa59bc-c77b-4058-b74e-acccaa29eeb5"),
            Description = "Check the cables/Wires & accessories pulled/dressed and adequate spaced within as per approved drawings.",
            Reference = "Checklist 15",
            Sequence = 1504,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f259c428-8dac-4a2b-b1d4-ca418a81995c"),
            Description = "Check the laying of the cable & accessories configuration is as per approved drawing",
            Reference = "Checklist 15",
            Sequence = 1505,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8dfca8a3-4617-4571-a778-74cab3b54dc6"),
            Description = "Check the connections are done correctly (as per approved methods)",
            Reference = "Checklist 15",
            Sequence = 1506,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ce4adaad-5fac-43e1-8c71-d4c325864a82"),
            Description = "Check the proper mounting & fitting of lugs & accessories",
            Reference = "Checklist 15",
            Sequence = 1507,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2a82a13f-6fcd-421a-aa2b-c08642ae7dd0"),
            Description = "Check that a proper tags/identifications are provided",
            Reference = "Checklist 15",
            Sequence = 1508,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2bb3c1e9-d478-4ae6-a863-ec689b0cc4d5"),
            Description = "Check for system is checked for Insulation resistance and continuity",
            Reference = "Checklist 15",
            Sequence = 1509,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("39307943-2b50-4d2a-bc9f-0033ec093ef6"),
            Description = "Check tidiness of system installations.",
            Reference = "Checklist 15",
            Sequence = 1510,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e141c78f-1aef-463e-a59d-4aaedbdf2858"),
            Description = "The materials are approved.",
            Reference = "Checklist 16 Part 1",
            Sequence = 1601,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7cbb7242-61b1-465c-b7fb-4d7b0aa96ad4"),
            Description = "No visible damage on the materials.",
            Reference = "Checklist 16 Part 1",
            Sequence = 1602,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("52e7f25c-dba8-456d-b691-e854b2b3c3fb"),
            Description = "Cable containment and accessories as per approved drawings.",
            Reference = "Checklist 16 Part 1",
            Sequence = 1603,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bbb17f81-acba-4e03-be89-846c12557791"),
            Description = "Cable indentification as per the approved drawings.",
            Reference = "Checklist 16 Part 1",
            Sequence = 1604,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4aef1692-e300-4a50-a0ff-e88e5fa9b2ee"),
            Description = "Is the cable properly dressed and labelled.",
            Reference = "Checklist 16 Part 1",
            Sequence = 1605,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("89f9c422-eb9a-4acf-9606-f959ab38992b"),
            Description = "Check and verify location, orientation, and mounting heights of data and telephone oulets as per approved drawing",
            Reference = "Checklist 16",
            Sequence = 1606,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8d0525c1-b3c5-4c1b-91a5-5080ecf72ea8"),
            Description = "Check the IDF rack is cleaned without any unwanted materials",
            Reference = "Checklist 16",
            Sequence = 1607,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e32f52c3-b5c2-4ce5-8f99-93e8267b2ab5"),
            Description = "Check Power Distribution Unit is available and power supply is provided",
            Reference = "Checklist 16",
            Sequence = 1608,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("45035e49-774a-4e6f-a3bb-2432cf03d122"),
            Description = "Check RJ 45conector and modules are fixed properly on data and telephone outlets",
            Reference = "Checklist 16",
            Sequence = 1609,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f331b56e-fe97-4dc3-80cb-feaac7cf2771"),
            Description = "Termination tools are properly managed.",
            Reference = "Checklist 16",
            Sequence = 1610,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1845dd3-c4f2-471a-9de7-dd80dcaee745"),
            Description = "Termination materials as per the approved MAR.",
            Reference = "Checklist 16",
            Sequence = 1611,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7ce97c7f-8a21-4a47-95b2-20e250a976ed"),
            Description = "Type of the cable as per approved drawings.",
            Reference = "Checklist 16",
            Sequence = 1612,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f0644fa7-a443-4002-92e6-849d6c1fedd2"),
            Description = "Location of the data socket as per approved drawings.",
            Reference = "Checklist 16",
            Sequence = 1613,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ec29d3bf-2186-4a54-bb15-9acc14cf2033"),
            Description = "Idetification on the data socket and IDF patch panel are same.",
            Reference = "Checklist 16",
            Sequence = 1614,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("42646f43-0cf2-4d1e-b503-7b33d80da696"),
            Description = "Patch panel installation as per the approved rack schedule",
            Reference = "Checklist 16",
            Sequence = 1615,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ec571a67-dc34-4c8c-94fb-d007b519914c"),
            Description = "Ensure relevant shop drawing is approved.",
            Reference = "Checklist 17",
            Sequence = 1701,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2e06cca0-d33e-49df-bcc0-acf00f191fb2"),
            Description = "Ensure H&S method statement is submitted",
            Reference = "Checklist 17",
            Sequence = 1702,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e4368d3d-f288-4df8-ba76-59bedb13f41d"),
            Description = "Check all the materials are approved by consultant.",
            Reference = "Checklist 17",
            Sequence = 1703,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d301828f-5361-4ad3-b1ab-6a34520aa425"),
            Description = "Check the availablity mockup approval for the fire sealing works (around penetrations, partitions, etc.).",
            Reference = "Checklist 17",
            Sequence = 1704,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cfb378ab-afdf-4484-8263-47777e53e23c"),
            Description = "Check the MEP penetrations are installed, inspected and approved.",
            Reference = "Checklist 17",
            Sequence = 1705,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("824f3d2f-77c9-4e52-9059-1f70382a070e"),
            Description = "Check the surface for any latiance, dirt, loose materials around the penetrations.",
            Reference = "Checklist 17",
            Sequence = 1706,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("55d2471d-c394-4926-992c-74fe20665a63"),
            Description = "Check the dampness of the substrate prior to application (if required as per manufaturer recommendations)",
            Reference = "Checklist 17",
            Sequence = 1707,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d9852df8-f38c-4540-9c08-ef4f9a1ad33b"),
            Description = "Check the mixing of materials as per the manufacturer recommandations",
            Reference = "Checklist 17",
            Sequence = 1708,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("72fcb551-bf36-483d-b12c-67b97dbf2ec0"),
            Description = "Check the application covers the whole penetrations without any gap.",
            Reference = "Checklist 17",
            Sequence = 1709,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8436de64-aea5-4448-8569-6c8de82e2eb4"),
            Description = "Finishing of the openings shall be as per the approved mockup.",
            Reference = "Checklist 17",
            Sequence = 1710,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d3cd81d6-d7cd-4e24-b4ab-fca369cbc532"),
            Description = "Check the final cleaning and sealant (if required) applied as per the approved mockup.",
            Reference = "Checklist 17",
            Sequence = 1711,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("abb9465e-4e44-4819-bc2e-7025d513eee3"),
            Description = "Ensure method statement, material submittal and drawings are approved.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1801,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f5283651-1110-4b42-95d7-f00d6191aadd"),
            Description = "Ensure materials (Gypsum board, tiles, suspension systems, etc.,) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1802,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8f3d3f93-4a43-4580-b588-be89f1b344e0"),
            Description = "Check the color, type, material, pattern and thickness are as per approved material approval and project requirements.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1803,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("34b1549c-f111-4a61-b6bb-d3505aa59f04"),
            Description = "Verify the marking of the false ceiling level on the walls as per the approved drawings.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1804,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("31f7f042-190b-4e88-85aa-33edb92bf603"),
            Description = "Verify the marking / location of suspension system supports at ceiling as per the approved drawings.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1805,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7dac1d98-58ae-40f6-bd62-9e155f4e4afd"),
            Description = "Verify the completion of the required finishes above the false ceiling level.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1806,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ff93dcc6-9390-4859-9794-0efa7ead5af8"),
            Description = "Verify the closing of shaft openings, MEP penetrations and other openings as per project requirements.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1807,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a585bf08-ee37-40b6-b8b8-3c118816f683"),
            Description = "Verify the application of fire sealant / fire protection works are carried out as per the project requirements (if applicable).",
            Reference = "Checklist 18 Part 1",
            Sequence = 1808,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("95f84518-392c-4907-bbee-138c9704f133"),
            Description = "Verify the location, spacing and fixation of the grid (main channel, Furing channel, wall angle, hanging wire with adjustable clip, main tee, cross tee, etc.,) as per the approved drawings.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1809,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("60f65ed3-ddfc-4278-abab-8892ff9f2325"),
            Description = "Ensure the suspension system supports are not in contact with the adjacent MEP services.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1810,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f348c32a-de9b-4d76-9c55-a4f51249ce17"),
            Description = "Ensure the completion of MEP and other dicipline works above the false ceiling level and obtain clearance to proceed for futher works.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1811,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ded1c4d0-acb2-4631-b5b7-c9bc144c8884"),
            Description = "Ensure additional supports are provided for the ceiling mounted fixtures as applicable.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1812,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a52bbf7c-baf4-439e-b76e-c61c4360342e"),
            Description = "Obtain approval from consultant / Client to proceed with further works.",
            Reference = "Checklist 18 Part 1",
            Sequence = 1813,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6fc36320-f826-4e12-bd7d-53f819f77123"),
            Description = "Verify the type, fixation, level and alignment of the false ceiling board / tiles as per the approved drawings.",
            Reference = "Checklist 18 Part 2",
            Sequence = 1814,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d741096c-39db-4856-a151-b8e6304e04fe"),
            Description = "Verify the marking, position and alignment of MEP & ceiling mounted fixtures (light, access panel, sprinklers, signages etc.) in the ceiling as per the approved drawings.",
            Reference = "Checklist 18 Part 2",
            Sequence = 1815,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("05b22657-2760-41c6-9d32-9d00776ad1ec"),
            Description = "Ensure the cutting of gypsum board / tiles on the marked locations as per the project requirements.",
            Reference = "Checklist 18 Part 2",
            Sequence = 1816,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("baa6b4f1-ab41-4e3d-b2fe-49ecfea16367"),
            Description = "Verify the jointing & taping as per the manufacturer recommendations.",
            Reference = "Checklist 18 Part 2",
            Sequence = 1817,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("04ebb69f-13bb-4de5-84bb-20e1e80f2e1d"),
            Description = "Approval obtained from Consultant/Client to proceed with further activites.",
            Reference = "Checklist 18 Part 2",
            Sequence = 1818,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("857f07cf-5783-43d5-abf4-11d85307585a"),
            Description = "Ensure method statement and materials are approved.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1901,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3b44ccdf-3162-4446-9a28-dd11a755eae5"),
            Description = "Ensure materials are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1902,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a2e86be4-11b9-41e9-a82c-6c61caa7baf6"),
            Description = "Check the expiry date of the material prior to applications.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1903,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("09b3e191-f758-4d59-8dc4-032a2265308f"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1904,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d41dc445-c082-4ffb-a307-a0a5c35cb8ff"),
            Description = "Ensure grouting, angle fillet provided all around the penetrations and the projected MEP services are neat and clean from any latiance.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1905,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ffd8da8c-4dd5-4997-9b41-4514e6f7ca75"),
            Description = "Ensure the angle fillets are provided at the floor and wall junctions of the area that to be treated.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1906,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("626b0f69-3c6c-4856-962f-054893755f28"),
            Description = "Obtain MEP clearance prior to start the wet area water proofing.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1907,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c381fdee-07f1-484e-a772-093300c14636"),
            Description = "Ensure check dam constructed at the entrance of the room for the stagnation of water for water leakage testing after the completion of the wet area water proofing.",
            Reference = "Checklist 19 Part 1",
            Sequence = 1908,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6cc6b9e1-4999-44e5-9f4d-95f2bbe4ff2f"),
            Description = "Ensure proper mixing of the material as per the manufacturer recommendations",
            Reference = "Checklist 19 Part 2",
            Sequence = 1909,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c082bd65-1df7-4f9e-a96a-c0293c61c59f"),
            Description = "Check the application of primer coat (if required).",
            Reference = "Checklist 19 Part 2",
            Sequence = 1910,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3260df31-c7f8-4223-a62e-55c451898a64"),
            Description = "Check the rate of application as per the manufacturer recommendation and method statement.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1911,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3ea5ad74-8675-4509-8233-68f6a323942e"),
            Description = "Check the application of coats as per project requirements / manufacturer recommandations.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1912,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9b88c41d-62f3-4406-8ab4-45c9453b0cda"),
            Description = "Check the application of subsequent coats are carried out at right angle to the previous coat.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1913,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b1553c84-23ef-441e-beee-11b9094dbef3"),
            Description = "Ensure the application of waterproofing on the vertical face extended upto 300mm from FFL or as per approved drawing / project requirements.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1914,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ce6638a5-a16f-4d39-8e9a-ed9d9a43bce7"),
            Description = "Check the WFT as per the project specifications / manufacturer recommendations.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1915,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("53c61e49-7fed-48d2-931a-095c83966051"),
            Description = "Check the curing of application as per manufacturer recommendation.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1916,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9e8fbbf1-ccd6-4a53-a92e-ca780ec99e6f"),
            Description = "Obtain approval for the application of waterproofing to proceed water leakage test.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1917,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5d6e0318-0ab5-471a-bd18-d4dba2eeb206"),
            Description = "Check the filling of water and monitor the level of filled water during the leakage test.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1918,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bd51c0c7-eaa4-4311-a88f-cbe9104f6617"),
            Description = "Check for any water seepage / leakage after 24 hours or as per the project requirements.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1919,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("35d166b2-5fb7-4087-9a08-5f72d674f920"),
            Description = "Approval obtained from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 19 Part 2",
            Sequence = 1920,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e955bb2f-5186-42de-94cf-a11f9cb5691f"),
            Description = "Verify the installed Distribution Boards have approved submittals.",
            Reference = "Checklist 20",
            Sequence = 2001,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c71e0271-8fbb-4dbd-8d71-12fc8f87b71a"),
            Description = "Ensure the drawings used for installation are correct and approved.",
            Reference = "Checklist 20",
            Sequence = 2002,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("132e4f76-7afe-473c-b03e-8aeb9fce1ba9"),
            Description = "Check the name plate and identification labels as per load schedule and approved submittals.",
            Reference = "Checklist 20",
            Sequence = 2003,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8feb0b44-38d0-4b59-85fe-6d3253358d50"),
            Description = "Check the mounting channels are even and free from any damage",
            Reference = "Checklist 20",
            Sequence = 2004,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3d066e17-0d13-49f6-8b4e-08e12c3115ba"),
            Description = "Check all meters, circuit breakers, indication lamp, handles and locks are correct and undamaged.",
            Reference = "Checklist 20 Part 1",
            Sequence = 2005,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a0171452-751f-45ab-984b-7c2b0abaac58"),
            Description = "Ensure adequate clearance available around panel as required and as per drawings.",
            Reference = "Checklist 20",
            Sequence = 2006,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ca7a0728-f817-4125-aaef-d8b8799fefcf"),
            Description = "Check the alignment of installed panels.",
            Reference = "Checklist 20",
            Sequence = 2007,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("75017889-476c-47f2-895e-c3ca065c2498"),
            Description = "All cables and wires including grounding are terminated and identified.",
            Reference = "Checklist 20",
            Sequence = 2008,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a26fd0c2-2af4-4404-add1-9376a90e6842"),
            Description = "Ensure panel interiors are cleaned and free from dust and small metallic particles.",
            Reference = "Checklist 20",
            Sequence = 2009,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8c662afd-1860-4ff5-9a4e-3aad29d6edb6"),
            Description = "Check the grounding has been provided as per approved drawing including the body and door of the panel.",
            Reference = "Checklist 20",
            Sequence = 2010,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("991e699d-6170-424c-b76a-4ebe99d6e1d0"),
            Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2101,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4e66eae5-9128-4e62-93fb-e17e71dacbf5"),
            Description = "Ensure materials are stored as per manufacturers recommendations.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2102,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("31f660d3-3e54-4eca-b203-53c071227146"),
            Description = "Verify the expiry date of the material prior to applications.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2103,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bfc036ce-c1b8-4688-872b-359ad84ee73a"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2104,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4e45cb51-721e-41ab-b8f4-d403ceae14a8"),
            Description = "Check for repair of surface imperfection and protrusions (if any).",
            Reference = "Checklist 21 Part 1",
            Sequence = 2105,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("80455251-7caf-4182-82de-eaa08705d2ff"),
            Description = "Moisture content for the substrate and environmental conditions as per manufacturer recommendations.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2106,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("46413943-3db0-4ef2-b42e-b9922da6a367"),
            Description = "Undulations or irregular corners are repaired and grinded as required.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2107,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("11e337d8-97ba-46db-9ed0-ace4436659ff"),
            Description = "Check the MEP clearance prior to start epoxy flooring works.",
            Reference = "Checklist 21 Part 1",
            Sequence = 2108,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("725a7ee5-8b8e-42a1-84b0-9ca55569d565"),
            Description = "Ensure application of primer as per manufecturers recommendation",
            Reference = "Checklist 21 Part 2",
            Sequence = 2109,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("77a43e94-3e18-4caa-880c-b83f77d54e92"),
            Description = "Ensure application of Hardner as per manufecturers recommendation",
            Reference = "Checklist 21 Part 2",
            Sequence = 2110,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("98117a8f-6421-4da5-b192-9a3f511930a0"),
            Description = "Ensure Application of uniform base as per manufecturers recommendations.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2111,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b9a0a290-f90d-4a1c-a040-088af6e7084c"),
            Description = "Tuchup, grinding, undulations, corner repairs and pinholes are filled properly.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2112,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5e5d02a3-4e16-4f22-bd0b-042bb248713e"),
            Description = "Ensure proper mixing of material as per the manufacturer recommendations.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2113,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6cfe7644-332f-470f-b3f7-2960ada83ad8"),
            Description = "Check the Location, Colour, Type of Painting as per the approved shop drawings / material submittal.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2114,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("829898ab-5330-44c2-9f1b-a8fc17fda87a"),
            Description = "Check the application of coats as per project requirements / manufacturer recommendations.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2115,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("72a13833-7807-469d-a4b2-fd11f1977098"),
            Description = "Check the rate of application as per the manufacturer recommendation and method statement.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2116,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("003369a3-9eb1-46a1-88a0-6ce1c8437c6a"),
            Description = "Check the curing at every stages of application as per manufacturer recommendation.",
            Reference = "Checklist 21 Part 2",
            Sequence = 2117,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cc48f840-9b7c-4a3e-9957-855844e7e0bb"),
            Description = "Cable ID",
            Reference = "Checklist 22",
            Sequence = 2201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ecb1967f-beab-4894-943e-394b3c9117cb"),
            Description = "Cable Size",
            Reference = "Checklist 22",
            Sequence = 2202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4a21b2b1-39f8-4457-9e40-2cc1fd1d4efa"),
            Description = "Circuit Number",
            Reference = "Checklist 22",
            Sequence = 2203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("953e25ef-5846-4f4e-a2ba-092109d5eb08"),
            Description = "Continuity Test",
            Reference = "Checklist 22",
            Sequence = 2204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("db77b66f-1451-4f43-b472-2b01f574e628"),
            Description = "Insulation Resistance in Mega Ohms (R-Y, Y-B, B-R, R-N, Y-N, B-N, R-E, Y-E, B-E, N-E)",
            Reference = "Checklist 22",
            Sequence = 2205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ecb07f00-4755-4ac7-b11c-15f1096b1040"),
            Description = "Test Instrument Details",
            Reference = "Checklist 22",
            Sequence = 2206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2fc6b0be-86ed-4bcf-9c07-ab0634205e6c"),
            Description = "Calibration Date",
            Reference = "Checklist 22",
            Sequence = 2207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ae77fb70-6109-4476-98c4-e389e98cfe1d"),
            Description = "Calibration Due Date",
            Reference = "Checklist 22",
            Sequence = 2208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c0e157df-7898-4cc2-87e6-eb52e844b38a"),
            Description = "Test Voltage",
            Reference = "Checklist 22",
            Sequence = 2209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e105122d-d12d-4a16-b02e-bae5a17d6c44"),
            Description = "Remarks",
            Reference = "Checklist 22",
            Sequence = 2210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2273e11c-615e-4551-ace4-e25966f39c6f"),
            Description = "Ensure method statement, materials and drawings are approved.",
            Reference = "Checklist 23 Part 1",
            Sequence = 2301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e4d77d5e-dd9d-49f1-90b1-5d31c11d0fd0"),
            Description = "Ensure materials are stored as per manufacturers recommendations.",
            Reference = "Checklist 23 Part 1",
            Sequence = 2302,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("01f4e629-5e23-4d2b-a5d8-60029a7eae86"),
            Description = "Ensure method of statement is being followed.",
            Reference = "Checklist 23 Part 1",
            Sequence = 2303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6d1639f1-385d-4461-a74c-3120bfcc8f03"),
            Description = "Check location,Size,Color and Thickness of the Cabinets",
            Reference = "Checklist 23 Part 1",
            Sequence = 2304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("61c65e30-d2f1-4b80-9b44-2bf14aa257b8"),
            Description = "Check Height,Level and Alignment of cabinets",
            Reference = "Checklist 23 Part 1",
            Sequence = 2305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0f518c21-55ef-4815-a19d-27fdfdf37be0"),
            Description = "Check Opening Size and Direction of cabinets",
            Reference = "Checklist 23 Part 1",
            Sequence = 2306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8aca9630-fef3-4aed-b099-86a7ce31af3f"),
            Description = "Check Shelf Supports (Brackets or Pins)",
            Reference = "Checklist 23 Part 1",
            Sequence = 2307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1f9227c8-09da-453d-8971-1e48a71036d7"),
            Description = "Check Joints of the Cabinets",
            Reference = "Checklist 23 Part 1",
            Sequence = 2308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("966cb3cc-bd72-48de-ada8-ad475e3c6a3a"),
            Description = "Check wethere Drawer and Shelf functioning properly",
            Reference = "Checklist 23 Part 1",
            Sequence = 2309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("63792d76-cc19-40c4-bd08-a44ec74435b2"),
            Description = "Fixing of supports for the countertop is as per approved drawings",
            Reference = "Checklist 23 Part 1",
            Sequence = 2310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f2b95828-e804-4f40-9728-a7175f43c0c9"),
            Description = "Check Ironmongery is installed as per drawing and free from damanges",
            Reference = "Checklist 23 Part 1",
            Sequence = 2311,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3446434b-fad5-4bdf-bf20-28f37c0f04d0"),
            Description = "Check Sink installed Properly and as per location on dwg.",
            Reference = "Checklist 23 Part 1",
            Sequence = 2312,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3efb3cf9-a0ee-4bbe-8cde-301552f811cf"),
            Description = "Check Stove installed Properly",
            Reference = "Checklist 23 Part 1",
            Sequence = 2313,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3e7c08f7-9069-4312-a671-a2c8dfba9444"),
            Description = "Completion of Nearby Finishes",
            Reference = "Checklist 23 Part 1",
            Sequence = 2314,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6ac48b7d-7614-48c7-a9b7-aba875e4a350"),
            Description = "Check location,Size,Color and Thickness",
            Reference = "Checklist 23 Part 2",
            Sequence = 2315,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b3d3d7a6-8d6e-4509-adcd-7bc65e0938e3"),
            Description = "Check Opening Size and location for kitchen Accessories",
            Reference = "Checklist 23 Part 2",
            Sequence = 2316,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ca69d287-c82e-4ba4-b0b8-9eabef369201"),
            Description = "Check Level of the surface",
            Reference = "Checklist 23 Part 2",
            Sequence = 2317,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ab833f03-90a0-41f9-8731-978e44f8194d"),
            Description = "Check Applied Sealant and its Color",
            Reference = "Checklist 23 Part 2",
            Sequence = 2318,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("569ff794-5e4e-4e0b-9ccb-8a08c7910215"),
            Description = "Check for any Surface joints visibility",
            Reference = "Checklist 23 Part 2",
            Sequence = 2319,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5083ea4c-f6ee-4f01-adbf-e33df6007217"),
            Description = "Ensure method statement, material submittal and drawings are approved.",
            Reference = "Checklist 24",
            Sequence = 2401,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7e2d13ac-d10a-46e5-9884-c0c8fa540f3b"),
            Description = "Ensure materials (wardrobe, leaf, drawers , iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 24",
            Sequence = 2402,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("04c7bd2f-5f0a-4cf8-a880-b01d51951c29"),
            Description = "Check the color, type, material, coating of leaf, materials are as per approved material approval and project requirements.",
            Reference = "Checklist 24",
            Sequence = 2403,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8be21309-49e7-422e-9ee3-49d6c607b919"),
            Description = "Verify and record the DCL product conformity certificate for the sealants to be used.",
            Reference = "Checklist 24",
            Sequence = 2404,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e358dbb9-8e21-4982-bee5-985fae95cd50"),
            Description = "Ensure the fire rating of the wardrobe are as per the project requirements.",
            Reference = "Checklist 24",
            Sequence = 2405,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3318c546-e8a4-4598-9b53-93537a256d0a"),
            Description = "Verify the level and alignment of the wardrobe frame leaf opening with reference to the surrounding wall / cladding elevations.",
            Reference = "Checklist 24",
            Sequence = 2406,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bade9f88-5966-46d8-af48-4a53f6ed2ac1"),
            Description = "Verify the location and clear opening of leafs and drawers are as per the approved drawings.",
            Reference = "Checklist 24",
            Sequence = 2407,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e22fa1c1-7c5e-4e49-b7db-5b773de5986b"),
            Description = "Verify the wardrobe jamb area are solid and rigid.",
            Reference = "Checklist 24",
            Sequence = 2408,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7fee0993-f3de-4d6b-b051-ea7ebfb14e43"),
            Description = "Verify the fixation of subframe with applicable moisture resistant coating.(if required).",
            Reference = "Checklist 24",
            Sequence = 2409,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0267437f-9173-449e-b9fe-f609dc9a4c3b"),
            Description = "Verity the fixation, level and alignment of the wardrobe is as per the approved details / drawings.",
            Reference = "Checklist 24",
            Sequence = 2410,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ac898ec5-e570-4af6-802b-d7d88bd7a180"),
            Description = "Ensure the wardrobe consolidated.",
            Reference = "Checklist 24",
            Sequence = 2411,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5220367b-21fd-4a78-ac24-9ea6c32ffde3"),
            Description = "Ensure the location and No. of leaf hinges provided as per the approved drawings.",
            Reference = "Checklist 24",
            Sequence = 2412,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("545ccf7e-c7a5-424e-8396-538e53d4cf9e"),
            Description = "Ensure required Iron mongery sets are provided as per drawings.",
            Reference = "Checklist 24",
            Sequence = 2413,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0235daa1-0967-4d66-b793-cbb6da1f07ff"),
            Description = "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.",
            Reference = "Checklist 24",
            Sequence = 2414,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bb414510-25ce-4807-8393-da74a697c946"),
            Description = "Ensure location/dimensions of drawers are per drawing",
            Reference = "Checklist 24",
            Sequence = 2415,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("426547ca-113d-46a1-b173-cc26713164db"),
            Description = "Ensure the roller slides/drawer runners as per the approved drawings.",
            Reference = "Checklist 24",
            Sequence = 2416,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2bcca6a8-ae11-42e2-806e-c29ab393261d"),
            Description = "Verify Cloth Hangers/Tie Holders are installed as per the approved drawings.",
            Reference = "Checklist 24",
            Sequence = 2417,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("682a59c7-2515-4201-adb5-cb7a8b9fa836"),
            Description = "Ensure the alignment, plumbness and protection of wardrobe to avoid any damages during activities.",
            Reference = "Checklist 24",
            Sequence = 2418,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7689d250-290e-4ad7-a944-6be49f0bbd8f"),
            Description = "Ensure wardrobe hanging rail set is installed.",
            Reference = "Checklist 24",
            Sequence = 2419,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9a8261ad-8899-411c-9707-add4fd3104b7"),
            Description = "Approval obtained from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 24",
            Sequence = 2420,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a74f3d8b-abc8-45c3-aee0-2110b4477dab"),
            Description = "Ensure method statement, material submittal and drawings are approved.",
            Reference = "Checklist 25",
            Sequence = 2501,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("94e77a4a-aee6-416d-b7f2-1223ff323e7f"),
            Description = "Ensure materials (Door frame, leaf, window frame, window leaf, iron mongeries and accessories, etc.) are stored under dry, clean, shaded area, away from sunlight and other sources of heat.",
            Reference = "Checklist 25",
            Sequence = 2502,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("90fe4284-e343-41a0-a582-f9e8dffe3620"),
            Description = "Check the color, type, material, coating of door and window materials are as per approved material approval and project requirements.",
            Reference = "Checklist 25",
            Sequence = 2503,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("162581fa-58a0-4cd1-8e37-d48e59a7cd08"),
            Description = "Verify and record the DCL product confirmity certificate for the sealants to be used.",
            Reference = "Checklist 25",
            Sequence = 2504,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6d346a95-04d0-4015-9ff8-883af1e052a4"),
            Description = "Ensure the fire rating of the doors are as per the project requirements.",
            Reference = "Checklist 25",
            Sequence = 2505,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("924b2bc3-d5d4-4490-a5f6-eff6a685c829"),
            Description = "Verify the location and clear opening of doors / windows are as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2506,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ff5f6c29-2d7f-44a3-9d35-f89efd75dbdd"),
            Description = "Verify the level and alignment of the door / window frame opening with reference to the surrounding wall / cladding elevations.",
            Reference = "Checklist 25",
            Sequence = 2507,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fbfb4cf1-7926-42fa-a0f0-44a2ae470eb5"),
            Description = "Verify the door / window jamb area are solid and rigid.",
            Reference = "Checklist 25",
            Sequence = 2508,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ec0a6b67-2f9c-4f3a-84f4-5d49caa3f555"),
            Description = "Verify the fixation of subframe with applicable moisture resistant coating.(if required).",
            Reference = "Checklist 25",
            Sequence = 2509,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8238dce0-9ff8-4bf9-8399-a228cb89a12e"),
            Description = "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.",
            Reference = "Checklist 25",
            Sequence = 2510,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4e79a845-dabc-4065-a9ef-ceab8025de15"),
            Description = "Ensure the door frames are consolidated using foam as per the material approval and project requirements.",
            Reference = "Checklist 25",
            Sequence = 2511,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b65907a1-ebc0-4f69-a19e-09675a87446a"),
            Description = "Ensure the location and No. of Door hinges provided as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2512,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9084fd9e-60f8-4061-98e0-b3af65bd5c8b"),
            Description = "Ensure required Iron mongery sets are provided as per the door schedule drawings.",
            Reference = "Checklist 25",
            Sequence = 2513,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ce776dcd-da1e-4d57-a030-462da9eed83f"),
            Description = "Ensure the level, orientation and position of the iron mongery fixed as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2514,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a04454f1-6332-4895-967c-bd296b16f63a"),
            Description = "Ensure the door stopper / door coordinator provided as per the approved door schedule.",
            Reference = "Checklist 25",
            Sequence = 2515,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("483810d3-bec0-4e86-8167-4d539737d4f8"),
            Description = "Ensure the fire rated doors are provided with fire rated sealant & fire rating tags as per the project requirements.",
            Reference = "Checklist 25",
            Sequence = 2516,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("98048334-85a9-4e51-9f08-c4c428d50ed1"),
            Description = "Ensure the rubber gaskets are provided at the door jamb as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2517,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9650dfba-9dca-4741-a667-bdb77c421428"),
            Description = "Verify the undercut for the door leaves as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2518,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e662b1e7-d162-4789-9e4e-bdd0d7b62172"),
            Description = "Ensure the alignment, plumbness and protection of door leafs to avoid any damages during construction activities.",
            Reference = "Checklist 25",
            Sequence = 2519,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7fbdf401-40c5-4729-9ef3-0b366d99904c"),
            Description = "Ensure the door architrave are fixed as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2520,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("37e450bb-c225-4a1d-86df-0cb878fa0b6d"),
            Description = "Approval obtained from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 25",
            Sequence = 2521,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d72b9550-2471-43f8-8414-2b3bfae917ec"),
            Description = "Ensure the completion of finishes around the jamb area prior to installation.",
            Reference = "Checklist 25",
            Sequence = 2522,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("061a0ef7-1fa4-4ed6-835f-52fe9e1365a1"),
            Description = "Ensure the drip flashings are provided properly around the frame as per the approved drawings",
            Reference = "Checklist 25",
            Sequence = 2523,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("989c8839-6951-4ca3-a07a-baf81d5b0dfe"),
            Description = "Ensure the overlapping of the flashings around the frame are as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2524,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("49f8ffac-6550-4b19-95a1-968a66202c80"),
            Description = "Verity the fixation, level and alignment of the door frame as per the approved details / drawings.",
            Reference = "Checklist 25",
            Sequence = 2525,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9156879d-35ea-47bc-bc39-d81852a802e1"),
            Description = "Ensure the location and No. of hinges, iron mongeries provided as per the approved drawings.",
            Reference = "Checklist 25",
            Sequence = 2526,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1edbdd4-e5e6-4057-b360-3138a95dd146"),
            Description = "Ensure the alignment, plumbness, opening direction and protection of window shutters to avoid any damages during construction activities.",
            Reference = "Checklist 25",
            Sequence = 2527,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("28004dc8-998e-4417-9f4a-577052f1e8da"),
            Description = "Verify the uniform thickness, colour and application of the sealant all around the window frame.",
            Reference = "Checklist 25",
            Sequence = 2528,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3a3f1926-85b8-4721-ade9-375b9906f6c2"),
            Description = "Approval obtained from Consultant/Client to proceed with further activities.",
            Reference = "Checklist 25",
            Sequence = 2529,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2294a7d0-211c-414a-bf65-704ac479d284"),
            Description = "Check the model of sanitary wares are as per approved material submittal",
            Reference = "Checklist 27",
            Sequence = 2701,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0435c429-4cb6-4649-94dd-977f7ba7e3b9"),
            Description = "Check the mounting dimensions as per approved typical installation/approved architectural drawings/manufacturer recommendations.",
            Reference = "Checklist 27",
            Sequence = 2702,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c959869d-f8e5-4a8d-a263-3d8d6ff05a89"),
            Description = "Ensure the fixtures are correctly levelled and bolted",
            Reference = "Checklist 27",
            Sequence = 2703,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b8b572f7-a6e9-4691-96d1-5c7b4cd89c3f"),
            Description = "Water supply line connected to the fixtures",
            Reference = "Checklist 27",
            Sequence = 2704,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a35d4a00-040e-4b0b-b748-5164f669a7d1"),
            Description = "Drainage pipe line connected to the sytem",
            Reference = "Checklist 27",
            Sequence = 2705,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2ee4cc95-fe97-4bd7-9da0-5b47a8401d88"),
            Description = "Check adequate space is available to operate the fixtures",
            Reference = "Checklist 27",
            Sequence = 2706,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ea719431-8cc4-4559-ab31-39b9cb8a89d8"),
            Description = "Check bracket/screws/angle valves/P-traps are properly installed",
            Reference = "Checklist 27",
            Sequence = 2707,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("af25b778-3c4b-4004-8d82-c99a87a941c2"),
            Description = "Check adequate protection is available for all fixtures during construction period",
            Reference = "Checklist 27",
            Sequence = 2708,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9152193f-727b-47b5-8d74-a2172db40e66"),
            Description = "Check the sealant are provided around the gaps of the fixtures",
            Reference = "Checklist 27",
            Sequence = 2709,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("098c5d12-bff8-4847-99b2-0c0d41ee6ce9"),
            Description = "Ensure all the fixtures are clean",
            Reference = "Checklist 27",
            Sequence = 2710,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5dec00fc-80fe-4410-a272-3fcb1725bb52"),
            Description = "Material used as per the approved MAR.",
            Reference = "Checklist 28",
            Sequence = 2801,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("093876f8-4b2a-419c-b69d-f5707f4c0d25"),
            Description = "Material used damage free.",
            Reference = "Checklist 28",
            Sequence = 2802,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0f2db243-99fc-45a9-b240-32e87ca0d853"),
            Description = "Confirm the mounting heights and location of card readers, Touch Screen Access Terminals, push buttons",
            Reference = "Checklist 28",
            Sequence = 2803,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b4d372a0-746f-48b8-abe9-08410eef047d"),
            Description = "Confirm installation and Cables are as per approved system schematic diagram",
            Reference = "Checklist 28",
            Sequence = 2804,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4a672d2f-98aa-44f0-92e5-a756383df52c"),
            Description = "Ensure equipment grounding connection is provided",
            Reference = "Checklist 28",
            Sequence = 2805,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7ad04890-c07b-41ab-bd97-b6aa37e778e3"),
            Description = "Identify panel Using machine printed labels",
            Reference = "Checklist 28",
            Sequence = 2806,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d2171521-9ea3-4d58-9b86-30c5c356904d"),
            Description = "Ensure proper cable tagging/identification are provided",
            Reference = "Checklist 28",
            Sequence = 2807,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f677ccf1-7f80-4dc4-b7e8-efd620a39324"),
            Description = "Confirm Cable are tested",
            Reference = "Checklist 28",
            Sequence = 2808,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e59b85aa-855b-46c5-b374-571a27ac2098"),
            Description = "Confirm the Devices are tested for operation and are to perform as intended at full load without any signs of heating",
            Reference = "Checklist 28",
            Sequence = 2809,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c8de2d1b-502f-4d10-8c92-083a67344c47"),
            Description = "Confirm the Devices are tested and working in the event of power outage",
            Reference = "Checklist 28",
            Sequence = 2810,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0a8e9f2a-678c-4498-9868-7597d558f54c"),
            Description = "Ensure that door lock open during the emergency",
            Reference = "Checklist 28",
            Sequence = 2811,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9a9891c6-4f06-473c-a911-a055c827edbd"),
            Description = "Ensure method statement, materials and drawings (finishing schedule) are approved.",
            Reference = "Checklist 29",
            Sequence = 2901,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f89866b0-968f-4576-9cc8-4e594f323c7a"),
            Description = "Ensure materials are stored as per manufacturers recommendations.",
            Reference = "Checklist 29",
            Sequence = 2902,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f8b313c8-aa33-4d13-adda-58cc0b74c249"),
            Description = "Verify the expiry date of the material prior to applications.",
            Reference = "Checklist 29",
            Sequence = 2903,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d0dc613f-011e-421f-9cf2-a4ccfcdd184c"),
            Description = "Check substrate is clean, free from contaminants like dust, traces of curing compound, oil and greese.",
            Reference = "Checklist 29",
            Sequence = 2904,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6623efba-8734-46d6-929a-18984d892ff0"),
            Description = "Check for repair of surface imperfection and protrusions (if any).",
            Reference = "Checklist 29",
            Sequence = 2905,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2c8964a3-0a04-4cec-98d4-cb38c7551176"),
            Description = "Ensure the protection of nearby finishes / MEP services.",
            Reference = "Checklist 29",
            Sequence = 2906,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b4d90cdd-41ae-45c1-871e-f94f510472ae"),
            Description = "Check the MEP clearance prior to start of aluminium works.",
            Reference = "Checklist 29",
            Sequence = 2907,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b7baa524-a2f9-4015-ae40-253750965448"),
            Description = "Profile and glass country of origin / Manufacturer",
            Reference = "Checklist 29",
            Sequence = 2908,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("af0f01cd-815d-4d41-9fc7-35434a04480e"),
            Description = "Type, Size, Colour, Thickness and Opening Direction",
            Reference = "Checklist 29",
            Sequence = 2909,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("28b83159-92be-45e9-b5ce-fb80661efc9a"),
            Description = "Varify Size of opening as per drawing",
            Reference = "Checklist 29",
            Sequence = 2910,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2a30aee7-a4b3-421f-a4d8-b128772870a0"),
            Description = "Varify Location of opening for curtain wall, door and windows as per drawing",
            Reference = "Checklist 29",
            Sequence = 2911,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2ffc8ed4-6c26-41b9-bcd9-286f5bf73d7b"),
            Description = "Ironmongery is installed and free from damages",
            Reference = "Checklist 29",
            Sequence = 2912,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6c4ab70e-662a-4de9-8dde-f1d82707ad0f"),
            Description = "Sealant is applied properly",
            Reference = "Checklist 29",
            Sequence = 2913,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f78f12d6-9542-45cd-8045-b30dc98089d0"),
            Description = "water leak test is performed and no leakage is found",
            Reference = "Checklist 29",
            Sequence = 2914,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("86a20818-3c5c-4dd7-bc7b-6bc10029a5ab"),
            Description = "varify Functioning/movement of panels is as free as required",
            Reference = "Checklist 29",
            Sequence = 2915,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("81fed5df-543e-4808-bf63-50b135ec4c1b"),
            Description = "Varify the Rigidity of frame and moveable panels",
            Reference = "Checklist 29",
            Sequence = 2916,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("014331e4-ed57-41a3-b179-523c251c7b1b"),
            Description = "Varify Line and Level",
            Reference = "Checklist 29",
            Sequence = 2917,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0ddd0235-6bde-4715-bf9a-4ba62541a707"),
            Description = "The materials/type/model/capacity as per approved material submittal.",
            Reference = "Checklist 30",
            Sequence = 3001,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d9c778bd-03f1-4df8-af23-e661f3b1a125"),
            Description = "No visible damage on the materials.",
            Reference = "Checklist 30",
            Sequence = 3002,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fe230d42-bd15-4180-a972-ec83a40cb279"),
            Description = "Check the indoor unit location/height as per approved shop drawing.",
            Reference = "Checklist 30",
            Sequence = 3003,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f6be66e6-08cd-4522-93e3-a7b99ca9fb12"),
            Description = "Check the approved type/capacity vibration isolator installed.",
            Reference = "Checklist 30",
            Sequence = 3004,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a30ed9c5-089f-4455-9ba4-924375583552"),
            Description = "Check the fans are freely rotating.",
            Reference = "Checklist 30",
            Sequence = 3005,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1790149d-16ac-485e-83f5-b3a1f81b92b3"),
            Description = "Check the filters are installed, clean.",
            Reference = "Checklist 30",
            Sequence = 3006,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6104b4b6-3c25-40ff-8325-68cf40a610db"),
            Description = "Check the refrigerant piping are connected with appropriate fittings.",
            Reference = "Checklist 30",
            Sequence = 3007,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("04ccf75f-fec7-4e6d-8aa5-486fe424dc9c"),
            Description = "Check the outdoor unit space around as per manufacturer recommendations.",
            Reference = "Checklist 30",
            Sequence = 3008,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("829c360c-a1ac-4ec7-8e13-b8e4ac7f01ef"),
            Description = "Pipe sizes are as per approved shop drawing.",
            Reference = "Checklist 30",
            Sequence = 3009,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7f2926f7-162e-4e4e-85f4-cdd4f55315f7"),
            Description = "Pipe layout/routing as per approved shop drawing.",
            Reference = "Checklist 30",
            Sequence = 3010,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9b230786-8f57-4e06-85a4-7283336dc450"),
            Description = "Sleeves are provided for the pipes passing through the walls/slabs.",
            Reference = "Checklist 30",
            Sequence = 3011,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("faf70235-6025-4200-9e03-66078b55a26a"),
            Description = "Check the pipes are supported well with approved clamps.",
            Reference = "Checklist 30",
            Sequence = 3012,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("205f336e-c1ec-4c2f-a4e5-5554f3986203"),
            Description = "Check the insulation is properly done.",
            Reference = "Checklist 30",
            Sequence = 3013,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e2594626-4699-4694-a266-f52e89c96db1"),
            Description = "Installed pipes are free of sag & bend.",
            Reference = "Checklist 30",
            Sequence = 3014,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("423f9531-99ff-41e4-8eb3-ae09d3410173"),
            Description = "Check the condensate drain pipes are connected with proper slope.",
            Reference = "Checklist 30",
            Sequence = 3015,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("50dab685-6ac5-4d1a-bbb8-24dc3b3eaaea"),
            Description = "IR Ref. No.",
            Reference = "Checklist 31",
            Sequence = 3101,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a883ac89-76e7-496c-9646-22f47bf5f1fa"),
            Description = "Reference Documents",
            Reference = "Checklist 31",
            Sequence = 3102,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5d4ecbca-1ef8-4aa0-9cef-324c0d28049f"),
            Description = "Area/Location",
            Reference = "Checklist 31",
            Sequence = 3103,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d6e459a3-16e3-411f-8c3e-8810171a1694"),
            Description = "Date",
            Reference = "Checklist 31",
            Sequence = 3104,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a30fe4f4-19bc-4ee5-8978-6762fe1a7e11"),
            Description = "Discipline",
            Reference = "Checklist 31",
            Sequence = 3105,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4f858e78-20d3-49f8-8cb8-2ed355da093a"),
            Description = "Element",
            Reference = "Checklist 31",
            Sequence = 3106,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("144d284f-6ea2-4735-9f33-a7fdf0f4f103"),
            Description = "System Description",
            Reference = "Checklist 31",
            Sequence = 3107,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e4d5831f-9a38-467f-aea7-9b156e17b5f1"),
            Description = "Piping System",
            Reference = "Checklist 31",
            Sequence = 3108,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d177cc96-a137-477e-90ab-01cdfe947dad"),
            Description = "Testing Fluid",
            Reference = "Checklist 31",
            Sequence = 3109,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("988b0aef-aa94-48cd-a344-7d3333d4a172"),
            Description = "Duration of Test",
            Reference = "Checklist 31",
            Sequence = 3110,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("449cf9c3-4f69-4ea3-8a2f-14b8c9289cc2"),
            Description = "Start Time of Test",
            Reference = "Checklist 31",
            Sequence = 3111,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8992744f-2b8e-4d83-b9ef-102e67b29b25"),
            Description = "Finish Time of Test",
            Reference = "Checklist 31",
            Sequence = 3112,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("60ab8567-c837-43de-96bf-aceaddd8f9ef"),
            Description = "Pressure(bar)",
            Reference = "Checklist 31",
            Sequence = 3113,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a955e608-82c4-4df4-a4a5-1a102ad7ec52"),
            Description = "Test Result (Satisfactory/Not Satisfactory)",
            Reference = "Checklist 31",
            Sequence = 3114,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("76739f98-6276-49ce-9602-5d4ab3074941"),
            Description = "Remarks If Any",
            Reference = "Checklist 31",
            Sequence = 3115,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0ca313a7-5fb9-45ec-b6ee-f2368a18f6c0"),
            Description = "Check Alignment of Wiring Devices",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ab3a6e39-8408-46a8-803a-766b822244a0"),
            Description = "Check Identification tag of the modular",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7489ac04-eb35-4e18-94ef-28d0be7f5cfc"),
            Description = "Installation of Supply Duct",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e0be43c5-01be-47f9-a53c-e8846f792fa8"),
            Description = "Check Alignment of Wiring Devices",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c7cbf075-bb71-47db-a70c-24ee1c9e8571"),
            Description = "Check identification tag of the modular",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("50e98af4-2709-4c63-961c-0f12d36ea899"),
            Description = "Installation of Supply Duct",
            Reference = "Checklist 32",
            Sequence = 3201,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2737526a-9e27-4755-8c89-c131e9409227"),
            Description = "Verify with complete test Services for any defects or damages",
            Reference = "Checklist 32",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f2293ad1-7bda-404a-b5ba-b70da32543d5"),
            Description = "Check For ONU Panel Door",
            Reference = "Checklist 32",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9dfb8408-633f-48a7-8c37-3ef30f945eb0"),
            Description = "Installation of Return Duct",
            Reference = "Checklist 32",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9aac0fef-bf19-4fcc-a7e8-fa739635785c"),
            Description = "Installation of Return Duct",
            Reference = "Checklist 32 Part 1",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("99313cf8-5b59-4fec-9719-e7ba7ce8d07d"),
            Description = "Check For ONU Panel Door",
            Reference = "Checklist 32",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ca7e2ea5-7cac-42d1-8ced-966a213c6562"),
            Description = "Visually inspect the MEP Services for any defects or damages",
            Reference = "Checklist 32",
            Sequence = 3202,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a6c45a0a-8b0d-4508-bdd8-d8d2bc8027ca"),
            Description = "Check For DB Panel Door",
            Reference = "Checklist 32",
            Sequence = 3203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6494382a-8448-419f-9565-563b6a8d2232"),
            Description = "Installation of Chilled Pipe",
            Reference = "Checklist 32",
            Sequence = 3203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6d0901a8-575d-469b-884c-82f9e0ee29f8"),
            Description = "Installation of Fresh Air Duct",
            Reference = "Checklist 32",
            Sequence = 3203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("131ca61d-e70c-4ae1-938e-aaa1bf5f084e"),
            Description = "Check For DB Panel Door",
            Reference = "Checklist 32",
            Sequence = 3203,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("33c57069-7323-4909-81f5-623b159c0cd4"),
            Description = "10A 1G, 1 Way switch",
            Reference = "Checklist 32 Part 4",
            Sequence = 3204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a8d1491f-edba-4c2a-915f-507c6633c3f5"),
            Description = "Installation of Exhaust Air Duct",
            Reference = "Checklist 32",
            Sequence = 3204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("09c1dffc-cb94-4988-92ee-763b35eb05d5"),
            Description = "Installation of Exhaust Air Duct",
            Reference = "Checklist 32",
            Sequence = 3204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("83c02e64-810a-41fa-a888-126ad1fe0b45"),
            Description = "10A 1G, 1 Way switch",
            Reference = "Checklist 32",
            Sequence = 3204,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f95a691b-c532-4b41-b8f7-89cf4d6b065d"),
            Description = "10A 2G, 1 Way switch",
            Reference = "Checklist 32 Part 4",
            Sequence = 3205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fb444b8d-0a06-48b9-9852-55797443841d"),
            Description = "10A 2G, 1 Way switch",
            Reference = "Checklist 32",
            Sequence = 3205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fb359fea-8410-4681-9750-043e3efbce1c"),
            Description = "Installation of Kitchen Hood and Flexible Duct",
            Reference = "Checklist 32",
            Sequence = 3205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("04263bce-ea06-445e-9cc0-13c816a44652"),
            Description = "Installation of Kitchen hood and Flexible Duct",
            Reference = "Checklist 32",
            Sequence = 3205,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2db4934f-f556-4df8-b7fb-364de3794866"),
            Description = "Installation of VCD",
            Reference = "Checklist 32",
            Sequence = 3206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ecb2ba61-6e06-4970-9b24-5a3d961f5714"),
            Description = "10A 3G, 1 Way switch",
            Reference = "Checklist 32",
            Sequence = 3206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5a1b39e5-ab34-487b-a78a-3591e0c63aa8"),
            Description = "Installation of VCD",
            Reference = "Checklist 32",
            Sequence = 3206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("99ec8677-fa0d-423a-b7e3-8f5da6abf7a0"),
            Description = "10A 3G, 1 Way switch",
            Reference = "Checklist 32 Part 4",
            Sequence = 3206,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("99e35e9a-d5b4-4991-9ccb-7b25307365f0"),
            Description = "10A 1G, 2 Way switch",
            Reference = "Checklist 32",
            Sequence = 3207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("77211d09-0c59-4251-bc09-b38de6d6b5c8"),
            Description = "Installation of Fan Coil Unit",
            Reference = "Checklist 32",
            Sequence = 3207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("11a8d5db-479a-4e7c-be57-665fd7f5c068"),
            Description = "10A 1G, 2 Way switch",
            Reference = "Checklist 32 Part 4",
            Sequence = 3207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cbc4a160-c647-4c8d-8bcb-0ca5d57e3ba8"),
            Description = "Installation of Fire Call out",
            Reference = "Checklist 32",
            Sequence = 3207,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("021671e3-16a9-4ba5-846c-a7fd2fba28f0"),
            Description = "Installation of Fire Damper and Back Draft Damper",
            Reference = "Checklist 32",
            Sequence = 3208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c3578f03-ea8e-4d2d-8173-1c0b084b455d"),
            Description = "Wireless Switch Janitor & Linen Room",
            Reference = "Checklist 32 Part 4",
            Sequence = 3208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("060f9403-5536-4495-b95a-6af6eb457327"),
            Description = "Installation of Fire Damper and Back Draft Damper",
            Reference = "Checklist 32",
            Sequence = 3208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d747f2a1-572c-45de-88af-1983faed6c81"),
            Description = "Wireless Switch Janitor & Linen Room",
            Reference = "Checklist 32",
            Sequence = 3208,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ab377b64-e955-498e-a9d1-464b59444463"),
            Description = "Installation of Grille/Defuser",
            Reference = "Checklist 32 Part 1",
            Sequence = 3209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2d21a924-9156-466a-babf-14ce375df320"),
            Description = "Override Switch Electrical Room",
            Reference = "Checklist 32 Part 4",
            Sequence = 3209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4242d0b2-78c1-4f90-a531-337bfd5a132a"),
            Description = "Override Switch Electrical Room",
            Reference = "Checklist 32",
            Sequence = 3209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a68bf77a-13f8-4472-ae56-ae922891d17f"),
            Description = "Installation of Grills/Diffuser",
            Reference = "Checklist 32 Part 1",
            Sequence = 3209,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ffcb1225-57fb-4f67-bc32-b8802da7c1fb"),
            Description = "Three Gang Override Switch Electrical Room",
            Reference = "Checklist 32",
            Sequence = 3210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6c0d46e2-12c7-429f-97b6-d5af2028246a"),
            Description = "Three Gang Override Switch Electrical Room",
            Reference = "Checklist 32 Part 4",
            Sequence = 3210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5a2e4d26-f4b0-4a61-9b84-9222a205202f"),
            Description = "Installation of Pipes and fittings",
            Reference = "Checklist 32",
            Sequence = 3210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3fe91317-1af6-4d30-b353-c5fd4969dc4a"),
            Description = "Installation of Pipes and fittings",
            Reference = "Checklist 32",
            Sequence = 3210,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7a65dfbe-b4c9-4ab7-9439-ed1338b79899"),
            Description = "Single Data Outlet -Euro face plate single keystone adaptor",
            Reference = "Checklist 32 Part 4",
            Sequence = 3211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ce82757e-fd46-46f8-8cae-f15686f89438"),
            Description = "Single Data Outlet -Euro face plate single keystone adaptor",
            Reference = "Checklist 32",
            Sequence = 3211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("76e172eb-f6d5-44f1-9c76-5853fdade679"),
            Description = "Insulation of pipe",
            Reference = "Checklist 32",
            Sequence = 3211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c743102b-a0b7-4e6b-b515-7c9559b08b03"),
            Description = "Insulation of pipe",
            Reference = "Checklist 32",
            Sequence = 3211,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e1ebc8bd-1952-4c42-b3c4-10b6a5c445d4"),
            Description = "Check Pipe Insulation and adhesive",
            Reference = "Checklist 32",
            Sequence = 3212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b7b5bdbf-3762-43a7-9ffc-6c0c9dcab281"),
            Description = "Twin Data Outlet -Euro face plate Duplex keystone adaptor",
            Reference = "Checklist 32 Part 4",
            Sequence = 3212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1e83ba95-68ee-4bd7-a869-d45f4c65bc1a"),
            Description = "Twin Data Outlet -Euro face plate Duplex keystone adaptor",
            Reference = "Checklist 32",
            Sequence = 3212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("817355c4-bd2d-47e1-89eb-77d2e49091fd"),
            Description = "Check Pipe Insulation and adhesive.",
            Reference = "Checklist 32",
            Sequence = 3212,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3815b1cd-edd6-439d-a0b6-67e612664968"),
            Description = "13 A ,Switch Single Socket outlet with Neon Indicator",
            Reference = "Checklist 32 Part 4",
            Sequence = 3213,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7b4228d1-4560-4b77-be49-3a84cbc9de7c"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32",
            Sequence = 3213,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("863eab66-6996-42c2-9684-a57442728083"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32",
            Sequence = 3213,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("836ce14b-45dd-4f16-a75d-d04166697a3b"),
            Description = "13 A ,Switch Single Socket outlet with Neon Indicator",
            Reference = "Checklist 32",
            Sequence = 3213,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e3b3c452-e2ce-41be-8445-de91a0eafa07"),
            Description = "PICV Installation with insulation box",
            Reference = "Checklist 32",
            Sequence = 3214,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3a2a3648-7e1d-47db-bfc5-b9b484bfae17"),
            Description = "13 A ,Switch Double Socket outlet with Neon Indicator",
            Reference = "Checklist 32 Part 4",
            Sequence = 3214,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("480a98fb-7f94-4304-9e83-17b31cb83f8f"),
            Description = "RCU Installation with Insulation box",
            Reference = "Checklist 32",
            Sequence = 3214,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("98f0b383-3e5f-41a1-9b0d-fe19033805c1"),
            Description = "13 A ,Switch Double Socket outlet with Neon Indicator",
            Reference = "Checklist 32",
            Sequence = 3214,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3f3eda9c-48f7-457c-a316-bca6d456b8d5"),
            Description = "13 A ,Switch Single/ Double Socket outlet with USB port with Neon Indicator",
            Reference = "Checklist 32 Part 4",
            Sequence = 3215,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b70ee639-e760-477f-9454-0afb85f202fb"),
            Description = "Application of Primer paint",
            Reference = "Checklist 32",
            Sequence = 3215,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e9921cec-8c92-4d66-86e6-f84df8835afb"),
            Description = "Application of Primer paint",
            Reference = "Checklist 32",
            Sequence = 3215,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("148d1009-40c9-4e8c-9b79-ee10d527ffb2"),
            Description = "13 A ,Switch Single/ Double Socket outlet with USB port with Neon Indicator",
            Reference = "Checklist 32",
            Sequence = 3215,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d2868661-9d2a-480b-a2bf-56b7439c7e98"),
            Description = "45 A, DP switch with flex outlet for cooker and appliances",
            Reference = "Checklist 32",
            Sequence = 3216,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a88ab841-2554-4e54-b0a1-df62925ae010"),
            Description = "Application of Red Paint/coating",
            Reference = "Checklist 32",
            Sequence = 3216,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b3e8acb5-2e7e-47f4-a6b4-f68fac55f902"),
            Description = "45 A, DP switch with flex outlet for cooker and appliances",
            Reference = "Checklist 32 Part 4",
            Sequence = 3216,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bcc9c57a-1025-4789-99e5-043a302f7866"),
            Description = "Application of Paint final coating",
            Reference = "Checklist 32",
            Sequence = 3216,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("89c64bde-b524-47b7-9492-68afbc82229b"),
            Description = "Fire Alarm Control panel -ELV room",
            Reference = "Checklist 32 Part 4",
            Sequence = 3217,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9ab9b7e8-e93f-45bf-a461-a0f6309627e8"),
            Description = "Installation of Sprinklers",
            Reference = "Checklist 32",
            Sequence = 3217,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("df3ebe62-8f79-4c58-92b3-c92233808e71"),
            Description = "Installation of Hose Reel",
            Reference = "Checklist 32",
            Sequence = 3217,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e257c305-e574-4124-ae53-b79b5bc402c3"),
            Description = "Fire Alarm Control panel -ELV room",
            Reference = "Checklist 32",
            Sequence = 3217,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ed390ab8-7e35-4ce6-8cae-1c57a4fa7623"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32",
            Sequence = 3218,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1405a6c8-a988-4583-b717-2c3f8cf3725f"),
            Description = "13 A ,Switched Flex outlet with Neon Indicator- Hood",
            Reference = "Checklist 32",
            Sequence = 3218,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b2121b2f-a67d-4436-90a6-e851582fc8e1"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32",
            Sequence = 3218,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7963b40f-a76b-41a6-8807-7235eb7ed63d"),
            Description = "13 A ,Switched Flex outlet with Neon Indicator- Hood",
            Reference = "Checklist 32 Part 4",
            Sequence = 3218,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2e58fb08-19ee-4661-86d4-c5a0b9064e70"),
            Description = "20 A, Cable/Flex outlet Washing Machine",
            Reference = "Checklist 32 Part 4",
            Sequence = 3219,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("73907d92-9a80-4ab2-bc96-54517f78bee0"),
            Description = "Installation of Pipes and fittings",
            Reference = "Checklist 32 Part 1",
            Sequence = 3219,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("35f98980-91e5-40ae-861b-0217aad4edd6"),
            Description = "Installation of Pipes and fittings",
            Reference = "Checklist 32",
            Sequence = 3219,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0e606e0a-f3cc-4c20-8281-e6b50fd67be6"),
            Description = "20 A, Cable/Flex outlet Washing Machine",
            Reference = "Checklist 32",
            Sequence = 3219,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("366a81c4-3690-4462-8b09-9d53005967da"),
            Description = "Door Bell -230V Electromechanical chime",
            Reference = "Checklist 32 Part 4",
            Sequence = 3220,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("332d047c-b05a-4216-8dc1-3e4e9f37baea"),
            Description = "Door Bell -230V Electromechanical chime",
            Reference = "Checklist 32",
            Sequence = 3220,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("095f4a93-5a54-4904-b372-a19e9161aa2d"),
            Description = "Water Hammer Arrestor installed in the approved location.",
            Reference = "Checklist 32 Part 1",
            Sequence = 3220,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("88d766d9-2e0f-459d-bf54-46e338e1d5a9"),
            Description = "Water Hammer Arrestor installed in the approved location.",
            Reference = "Checklist 32",
            Sequence = 3220,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7947b565-9198-43c3-83c3-4c79760a06e1"),
            Description = "13A, DP, Simplex Switched Spur Outlet for FCUs with Neon Indicator",
            Reference = "Checklist 32",
            Sequence = 3221,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f3b0e525-ed24-450c-8f5a-4615cc6d270a"),
            Description = "13A, DP, Simplex Switched Spur Outlet for FCUs with Neon Indicator",
            Reference = "Checklist 32 Part 4",
            Sequence = 3221,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("41bf1dc4-16d2-4ff2-bd98-de10de0cc427"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32 Part 1",
            Sequence = 3221,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4b7244bb-a0b8-4b6c-9cbe-de87104375b3"),
            Description = "Pressure testing of the Piping",
            Reference = "Checklist 32",
            Sequence = 3221,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ddcec500-52e9-4d2a-adf3-e961af46b90b"),
            Description = "Hot water pipes insulated.",
            Reference = "Checklist 32",
            Sequence = 3222,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d40c660a-853b-4f27-bf40-e525409811de"),
            Description = "All Wires pulled as per the approved Drawings.",
            Reference = "Checklist 32",
            Sequence = 3222,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("61cd91b5-6f41-47de-a50f-f5e752f0b734"),
            Description = "All Wires pulled as per the approved Drawings.",
            Reference = "Checklist 32",
            Sequence = 3222,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("626dde64-b18a-49fb-a413-5fa56e6c6805"),
            Description = "Hot water pipes Insulated",
            Reference = "Checklist 32 Part 1",
            Sequence = 3222,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e52b06af-1609-490f-aeba-3074af5c431f"),
            Description = "Get Value Installation",
            Reference = "Checklist 32 Part 1",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1ab36498-449c-461c-b9f1-43597c198d61"),
            Description = "CAT-6 Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1793b9d2-c32c-48f9-a75b-757cc4d811bf"),
            Description = "CAT-6 Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3b8169ac-84ef-4f5d-84f6-1cfdee792b5b"),
            Description = "Gate Valve Installation",
            Reference = "Checklist 32",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b5103665-5c3f-456b-90e3-1d7d3db9e1bb"),
            Description = "Installation of Floor Cleanout (FCO)",
            Reference = "Checklist 32",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("184b1752-1bbb-4b54-b9d3-1899fc3d1bf9"),
            Description = "Installation of Floor Cleanout (FCO)",
            Reference = "Checklist 32",
            Sequence = 3223,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ae3ab520-7cd8-414a-95b0-ed127b4aa2ef"),
            Description = "Installation of Floor Drain",
            Reference = "Checklist 32",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("87a800b3-3f95-4d4b-865c-1949d82adbc9"),
            Description = "Fire alram Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("baa3ce09-a01f-4a87-85c9-ce77f0e9046d"),
            Description = "Fire alram Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d7aaecf5-3327-453f-b627-c673ca55384f"),
            Description = "Installation of Floor Drain",
            Reference = "Checklist 32",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6cac54f3-f6de-4772-84c1-3a2132049cd9"),
            Description = "Kitchen sink and accessories",
            Reference = "Checklist 32 Part 1",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("275f19e4-ecce-491f-84d1-93ec52392b3c"),
            Description = "Kitchen sink and accessories",
            Reference = "Checklist 32",
            Sequence = 3224,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("05c755bc-fe3c-4c52-b820-159e5e405457"),
            Description = "Installation CDP Pipes",
            Reference = "Checklist 32",
            Sequence = 3225,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("74b2e374-29e5-411e-8518-4c1f659b7f49"),
            Description = "Main Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3225,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("272957f7-502e-4571-9078-2f3dcf4b1b79"),
            Description = "Installation CDP Pipes",
            Reference = "Checklist 32",
            Sequence = 3225,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c5deb73f-6028-4955-a5b0-55b4af44f6e1"),
            Description = "Main Cable pulled",
            Reference = "Checklist 32",
            Sequence = 3225,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1d2b607f-f4e8-4361-ad96-6541683da8f9"),
            Description = "Smoke detector",
            Reference = "Checklist 32",
            Sequence = 3226,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e483dc52-3eab-4d51-af7c-c948a731c481"),
            Description = "Smoke detector",
            Reference = "Checklist 32 Part 2",
            Sequence = 3226,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fddfd5a0-9bb3-439e-b3a0-f9cf064ae731"),
            Description = "Piping Leak test",
            Reference = "Checklist 32",
            Sequence = 3226,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("486c91ac-c8d6-4454-ae57-06374e4609f1"),
            Description = "Piping Leak test",
            Reference = "Checklist 32",
            Sequence = 3226,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5d89e9ef-fee6-4898-9164-20dd10818040"),
            Description = "Sleeves provided for drain pipes outlets.",
            Reference = "Checklist 32",
            Sequence = 3227,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("945a8d2d-c178-4d37-9c05-8dce903eb818"),
            Description = "Sleeves provided for drain pipes outlets",
            Reference = "Checklist 32",
            Sequence = 3227,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c120df5f-da58-4f40-ae90-c513d50bad57"),
            Description = "Heat detector",
            Reference = "Checklist 32 Part 2",
            Sequence = 3227,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("268612b9-e10d-44ff-85bc-6447a3a57f20"),
            Description = "Heat detector",
            Reference = "Checklist 32",
            Sequence = 3227,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b0eab117-826e-437a-8a33-cda95498306d"),
            Description = "Soil Pipe",
            Reference = "Checklist 32",
            Sequence = 3228,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e7e8c41b-c535-4539-9565-e93a8baf7bbc"),
            Description = "Sensors",
            Reference = "Checklist 32",
            Sequence = 3228,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f443fe9c-7530-45b8-af4f-6eed852b10da"),
            Description = "Sensors",
            Reference = "Checklist 32 Part 6",
            Sequence = 3228,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("50a32955-4736-4b10-aadf-30ebba82c0fe"),
            Description = "Soil Pipe",
            Reference = "Checklist 32",
            Sequence = 3228,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2a322c2c-8e24-4693-b4ee-5ade0b6df04d"),
            Description = "DB Panel Tags and identification.",
            Reference = "Checklist 32",
            Sequence = 3229,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1d8cf575-272e-4bd0-92d6-18d819482228"),
            Description = "Waste Pipe",
            Reference = "Checklist 32",
            Sequence = 3229,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fc5911b7-7924-428e-87d3-7fa722bf2737"),
            Description = "Waste Pipe",
            Reference = "Checklist 32",
            Sequence = 3229,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8f5fd61c-d1e5-414d-aa8e-8fc8865ceb22"),
            Description = "DB Panel Tags and identification.",
            Reference = "Checklist 32",
            Sequence = 3229,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4e8f143c-0eba-4c5f-abae-2bb8da4c90e8"),
            Description = "ONU Panel installation and termination",
            Reference = "Checklist 32",
            Sequence = 3230,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ef66e833-d403-4dd0-b270-5a81012913d6"),
            Description = "ONU Panel installation and termination",
            Reference = "Checklist 32",
            Sequence = 3230,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("67855cd7-8e24-40d0-9d47-d5ebf07b345b"),
            Description = "Vent Pipe",
            Reference = "Checklist 32",
            Sequence = 3230,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d5b022f2-4af9-4539-8d63-b2057dca51ad"),
            Description = "Vent Pipe",
            Reference = "Checklist 32",
            Sequence = 3230,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f6584d3d-36e1-4180-93d0-aa886cec61bb"),
            Description = "Water Supply Pipe",
            Reference = "Checklist 32",
            Sequence = 3231,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("32d02b82-abe1-4697-9f7a-be08a4e82f71"),
            Description = "DB Panel installation and termination",
            Reference = "Checklist 32",
            Sequence = 3231,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7337aa1d-6ebe-4f97-b64f-008b9e43aa93"),
            Description = "DB Panel installation and termination",
            Reference = "Checklist 32",
            Sequence = 3231,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("09ebf665-3bf1-4f95-a539-1cc5096cbf68"),
            Description = "Water Supply Pipe",
            Reference = "Checklist 32",
            Sequence = 3231,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6d0c4063-5bb3-4c7a-a15a-661f576b140a"),
            Description = "Thermostat",
            Reference = "Checklist 32",
            Sequence = 3232,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("cf28fe72-646b-4711-b699-38ea9dc44e1a"),
            Description = "Chilled Water pipe",
            Reference = "Checklist 32",
            Sequence = 3232,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6f675f3e-88a1-4641-aeaf-fb1cbdda6210"),
            Description = "Chilled Water pipe",
            Reference = "Checklist 32",
            Sequence = 3232,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bf9c69bd-dd68-4fb6-85d9-17c43359c447"),
            Description = "Thermostat",
            Reference = "Checklist 32 Part 5",
            Sequence = 3232,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3f919e8c-6beb-43e2-92b1-54983886cdc4"),
            Description = "Firefighting Pipe",
            Reference = "Checklist 32",
            Sequence = 3233,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0d7fcd09-7011-4afd-9fa7-2f401d5c7a9e"),
            Description = "Firefighting Pipe",
            Reference = "Checklist 32",
            Sequence = 3233,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3e2bf5c0-7241-4267-b2bb-c43a56c8ed98"),
            Description = "PMU",
            Reference = "Checklist 32",
            Sequence = 3233,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ed51a849-09b8-47f8-8c28-d4bfa11d343f"),
            Description = "PMU",
            Reference = "Checklist 32",
            Sequence = 3233,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3b4d582a-63b3-4351-a2d0-9673a47e9de1"),
            Description = "Duct Riser and connection",
            Reference = "Checklist 32",
            Sequence = 3234,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5a018a85-792f-49a5-8469-cc5d6fd14833"),
            Description = "D1-Ceiling Mounted Light Living and Bed Room",
            Reference = "Checklist 32 Part 3",
            Sequence = 3234,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b73be9ce-c277-4991-9434-8eaf90fa67c5"),
            Description = "Duct Riser and connection",
            Reference = "Checklist 32",
            Sequence = 3234,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3bd2e9bc-943e-409e-bc17-215d7d316806"),
            Description = "D1-Ceiling Mounted Light Living and Bed Room",
            Reference = "Checklist 32",
            Sequence = 3234,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("44cea87e-d98a-48cb-9508-ed3034504772"),
            Description = "D2-Ceiling Mounted Light Kitchen area",
            Reference = "Checklist 32",
            Sequence = 3235,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("1fad81d0-047c-494a-87ef-25e76a2b584d"),
            Description = "D2-Ceiling Mounted Light Kitchen area",
            Reference = "Checklist 32 Part 3",
            Sequence = 3235,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("68b530bc-20a8-4570-9aed-66d75e630f8b"),
            Description = "D3-Spot Ceiling Mounted light Corridor",
            Reference = "Checklist 32 Part 3",
            Sequence = 3236,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ea63c09f-dc3f-49e0-8509-d435c1a25f04"),
            Description = "D3-Spot Ceiling Mounted light Coridoor",
            Reference = "Checklist 32",
            Sequence = 3236,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ff61c9b7-b56a-4f61-9cb1-f02e0d40f17e"),
            Description = "D4-1 Spot Ceiling Mounted light Toilet",
            Reference = "Checklist 32 Part 3",
            Sequence = 3237,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("40bffd5a-55b4-483c-903a-fa52b8768bd8"),
            Description = "D4-1 Spot Ceiling Mounted light Toilet",
            Reference = "Checklist 32",
            Sequence = 3237,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2e4cba14-317f-471b-b2be-8349df12a5f4"),
            Description = "L1-Led Strip Light Under Kitchen Cabinet",
            Reference = "Checklist 32",
            Sequence = 3238,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("95efcaf4-b5aa-49b1-9d2d-6f6ac274d357"),
            Description = "L1-Led Strip Light Under Kitchen Cabinet",
            Reference = "Checklist 32 Part 3",
            Sequence = 3238,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ac0e4d52-da0d-4a70-a5d8-18a9aa225be6"),
            Description = "L2-Surface Mounted linear light electrical Room & Garbage room",
            Reference = "Checklist 32",
            Sequence = 3239,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("63d5f2d6-68f6-44fa-af64-facb3a8313da"),
            Description = "L2-Surface Mounted linear light electrical Room & Garbage room",
            Reference = "Checklist 32 Part 3",
            Sequence = 3239,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a52ca134-80d9-457f-9834-aa851afe710f"),
            Description = "W1-Wall mounted recessed balcony light",
            Reference = "Checklist 32 Part 3",
            Sequence = 3240,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("7aa7e83d-f7be-498f-9287-af0e1f7c5802"),
            Description = "W1-Wall mounted recessed balcony light",
            Reference = "Checklist 32",
            Sequence = 3240,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("24bd90c9-8d2d-4838-86c6-1782fd5e5627"),
            Description = "W2- Wall mounted toilet light above Mirror",
            Reference = "Checklist 32 Part 3",
            Sequence = 3241,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c1509923-9736-48f2-b507-65161bffbe90"),
            Description = "W2- Wall mounted toilet light above Mirror",
            Reference = "Checklist 32",
            Sequence = 3241,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6b4ddd06-27b2-4f2f-bb52-4339d9869752"),
            Description = "Visually inspect the modular for any defects or damages",
            Reference = "Checklist 33",
            Sequence = 3301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("22b5ed30-ada2-4f90-8c5a-fa6247b4e080"),
            Description = "Check identification tag of the modular",
            Reference = "Checklist 33",
            Sequence = 3301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("690dbf69-9d1f-496d-9536-0b1107fb5231"),
            Description = "Verify the method of loading as per the project / design requirements",
            Reference = "Checklist 33",
            Sequence = 3301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("eaf5bf09-3a32-4a99-af41-cd27ff9d2925"),
            Description = "Ensure method statement, ITP, materials and shop drawings are approved",
            Reference = "Checklist 33",
            Sequence = 3301,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9be9f457-f8c4-4795-84ba-64b7f9069fe8"),
            Description = "Internal and External Dimensions of the modular",
            Reference = "Checklist 33",
            Sequence = 3302,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("98ecef42-0ddd-4199-9c95-850b6b87d78e"),
            Description = "Internal Paint (Application of Primer, Stucco and 2nd Coat of Paint)",
            Reference = "Checklist 33",
            Sequence = 3303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d889dc8e-ab13-4fd0-9c31-960934966e56"),
            Description = "External Paint(Application of Primer, Texture)",
            Reference = "Checklist 33",
            Sequence = 3303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b406507a-4900-44a9-89bf-5615b20963ec"),
            Description = "Ensure Paint touch ups are completed around installed items.",
            Reference = "Checklist 33",
            Sequence = 3303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b7f441af-45b2-448e-b4ca-1723a10abbfb"),
            Description = "Location and color of Painting as per the App Drawing",
            Reference = "Checklist 33",
            Sequence = 3303,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a644a291-0219-4d4e-8a97-03964e90dfd0"),
            Description = "Line, Level and Spacer for the Installed Tiles",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("45442ac3-eb8c-4d3c-a869-c61d9845e6c4"),
            Description = "Skirting is installed/fixed properly and truly vertical",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9bab6704-0008-4177-85d3-d4202d0970de"),
            Description = "Elastomeric sealant under skirting is provided properly",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c3cc03ab-c2d3-4439-adca-d72ec2abc5d6"),
            Description = "Layout and Fixing of Tiles as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e76c0745-a6d1-46d6-80e8-883c9c0e747d"),
            Description = "Grouting of all Joints is done properly",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("b9be95bb-9eb4-4578-9b4f-0834532df91e"),
            Description = "Bitumin Applied at required Areas",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("8e89ca4e-9db3-4774-82f0-5ce36573742f"),
            Description = "Damages, If any",
            Reference = "Checklist 33",
            Sequence = 3304,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("d03abafd-b740-4ef5-a709-41c22b1d5f9e"),
            Description = "Ensure Drainhole are free from any debris and properly closed (if applicable)",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f4d17ba5-3da6-4377-a50a-97e3b28c01ce"),
            Description = "Ensure Gypsum surface are Crackfree at joints.",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f9105e05-8307-4d33-a8b7-0d2082b5ac33"),
            Description = "Cleaning of corners and edges removing exccessive paint on skirting",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("e056ae90-666d-4b93-a909-6afcb6d12ea5"),
            Description = "Thickness of Dry wall is as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3b22d8a3-c581-47a2-844f-040043b38c53"),
            Description = "Opening for MEP services are cut properly.",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("51b4c9d9-9320-4398-8dc7-2b4279ab3c2e"),
            Description = "Damages, if any",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("68c99db4-208f-4b5e-b64f-2c47c8f57bc4"),
            Description = "Layout, location and position of dry wall is as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3305,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("5c7c81f2-0c1d-4fc8-bb53-acdd29b73b68"),
            Description = "Ensure Gypsum surface are Crackfree at joints.",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3909af74-7e9a-45dd-a7e8-41a94bfefb8a"),
            Description = "Damages, if any",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9c318d7c-31bd-48ca-96e2-d2b5f32c3b41"),
            Description = "Access panels/ Ceiling acoustic tiles are Fixed Properly",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a99766ce-7de2-4652-bb90-e1218354f465"),
            Description = "Height of the False Ceiling as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("eb1217a8-39f9-44f5-a186-cb9b2cf4f780"),
            Description = "Damages, if any",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("2453ba13-7431-4775-bf4c-116b814a032c"),
            Description = "Layout of False Ceiling tiles and bulk head as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3306,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("4f84571b-bebc-434a-8701-70e118966908"),
            Description = "Fixing of Silicone Sealant",
            Reference = "Checklist 33",
            Sequence = 3307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0f259f4f-ce86-49b7-ac25-eccb993e7514"),
            Description = "Fixing of Glass/panels",
            Reference = "Checklist 33",
            Sequence = 3307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("67b325a9-d3cb-4270-83e9-8260bd5a40b1"),
            Description = "Location of Window/Sliding Door as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ba5ea50a-0efe-4089-b0b9-1115d49a1861"),
            Description = "Fixing of Iron-Mongery and Accessories",
            Reference = "Checklist 33",
            Sequence = 3307,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("03e92e0b-6532-43e3-afb0-e05565b32810"),
            Description = "Damages, if any",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3168b84f-b68e-4247-aa27-9993b45f32f2"),
            Description = "Water leak test performed and passed.",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("76b00e99-10e5-4c0e-ac13-b5f10fd10635"),
            Description = "Wardrobe accessories as per approved drawings",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("f7a2ecc3-0944-4fee-9365-277d00df890b"),
            Description = "Wardrobe doors and drawers funtioning smoothly and free from scratches",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("defd300b-5649-4a0f-8848-375eac71e15a"),
            Description = "Lock/Hardware of Main Entrance Door / Bedroom Door is installed",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ee27f000-09d8-463a-85e3-a60e23e5fe67"),
            Description = "Main Entrance Door / Bedroom Door as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9fc62452-8a78-453c-a0d7-ff27dc6f1e15"),
            Description = "Architraves are fixed as per Drawing around Main Entrance Door / Bedroom Door",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("dda4131f-bee9-4f33-aec2-9cafff51e587"),
            Description = "Kitchen cabinets accessories installed as per app drawing",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("00fa8f95-95f7-4f0c-8ddb-7aeaab75be85"),
            Description = "Kitchen sink and sink mixer installed",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("56b8ce72-c26a-44bd-a552-66071ead1055"),
            Description = "Direction of doors swing as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("519124ae-9660-4efc-9da5-83aa17eb40c2"),
            Description = "Wardrobe installed as per approved drawings",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0256c9d6-f188-424b-9dca-a15fbe850160"),
            Description = "Kitchen cabinets, counter top installed as per app drawing",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("077c3358-1ca1-4547-988a-e8560548ddee"),
            Description = "Paint touch completed around the frame.",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("9654fe83-0995-4b2c-b76b-a21e1c944104"),
            Description = "Location of Doors as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("a283e6e8-b9ed-4da4-b4dc-a5b76b1a6b07"),
            Description = "Damages, if any",
            Reference = "Checklist 33",
            Sequence = 3308,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ccb8617c-d1c4-42f0-91e0-4b5ccb497e5a"),
            Description = "Firestop sealant, fire rated sealant & General sealant applied around penetration pipes & MEP fittings.",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("02a98155-e15a-493a-a3a8-858f3714959f"),
            Description = "Pod Mirror installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3bfcfc0d-8e04-49ed-9a86-7bed791d76b9"),
            Description = "Threshold installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0bbfed09-e880-41f0-9da5-70402681338f"),
            Description = "Balcony floor drain installed as per approved drawing",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("3518f62d-9746-4a2a-81bd-3e6052455252"),
            Description = "Pod Painted walls are clean and free from stains.",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bc0c2d87-d206-4c7c-9851-44d69dbce151"),
            Description = "Pod Gypsum board are free from pealing off and Crack Free",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bf9934f3-9b9b-4a10-bd38-871d2ea17a2f"),
            Description = "Pod Tiles are fixed with grouting properly and free from damage",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("91303861-8891-4782-8294-e3cdbf28c7d3"),
            Description = "Lock/Hardware of Pod door is installed",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("6c4b2eec-d24e-4a2d-bb0f-d67128317f06"),
            Description = "Architraves are fixed as per Drawing around Pod door",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("da533379-f73b-457a-a46a-efd10be02ee4"),
            Description = "Toilet accessories installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("aefe4866-2a5b-4f12-aee6-efa61341b549"),
            Description = "Pod door is installed as per App Drawing",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("bafb22d8-37a3-4ca0-afba-73ebff24e3e2"),
            Description = "Locking of Doors and Shutters securely to avoid movement during transportation",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("50982c8e-99bf-4125-b648-c836339bab6f"),
            Description = "Gypsum curtain pelmet installed and free from damages/cracks",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c206fbed-de6a-4939-ab71-58b2608b38d1"),
            Description = "Kitchen Backsplash installed, grouted and free from damages",
            Reference = "Checklist 33",
            Sequence = 3309,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("fee5d4bd-a836-41a7-a717-4fb57422d2f0"),
            Description = "Check Final Condition of outside of the room and ensure its damage free",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("0b9f1a33-6451-4b53-b8ca-ef641a00bced"),
            Description = "Sign the delivery note for accepting the loading of precast modular in good condition",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("c03eb545-85e6-468d-a70f-ffec03ec27d9"),
            Description = "Floor drain and covers installed and free from damages",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("244ace99-fc3a-4479-8be8-3fb00f854411"),
            Description = "Pod Glass Partition installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("23f8fa00-38e5-4920-a197-fa2f56b39190"),
            Description = "Pod Vanity installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("ed8e4d7b-8483-4c83-9437-8f283bf33ca7"),
            Description = "Pod Shower installted and free from damage",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        },
        new PredefinedChecklistItem
        {
            PredefinedItemId = Guid.Parse("14d4dc59-7775-45e0-b7da-7c03e0e49380"),
            Description = "Pod WC and cover installed and free from damage",
            Reference = "Checklist 33",
            Sequence = 3310,
            IsActive = true,
            CreatedDate = seedDate
        }
    };

        modelBuilder.Entity<PredefinedChecklistItem>().HasData(predefinedItems);
    }
}