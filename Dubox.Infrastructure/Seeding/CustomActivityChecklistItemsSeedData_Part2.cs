using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Additional seeds for custom ActivityCheckListItem data
/// Generated automatically based on filter criteria
/// </summary>
public static class CustomActivityChecklistItemsSeedData_Part2
{
    private static readonly DateTime SeedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedAdditionalActivityChecklistItems(ModelBuilder modelBuilder)
    {
        SeedActivityMaster3(modelBuilder);
        SeedActivityMaster4(modelBuilder);
        SeedActivityMaster5(modelBuilder);
        SeedActivityMaster6(modelBuilder);
        SeedActivityMaster7(modelBuilder);
        SeedActivityMaster8(modelBuilder);
        SeedActivityMaster9(modelBuilder);
        SeedActivityMaster10(modelBuilder);
        SeedActivityMaster11(modelBuilder);
        SeedActivityMaster12(modelBuilder);
        SeedActivityMaster13(modelBuilder);
        SeedActivityMaster14(modelBuilder);
        SeedActivityMaster15(modelBuilder);
        SeedActivityMaster16(modelBuilder);
        SeedActivityMaster17(modelBuilder);
        SeedActivityMaster18(modelBuilder);
        SeedActivityMaster19(modelBuilder);
        SeedActivityMaster20(modelBuilder);
        SeedActivityMaster21(modelBuilder);
        SeedActivityMaster22(modelBuilder);
        SeedActivityMaster23(modelBuilder);
        SeedActivityMaster24(modelBuilder);
        SeedActivityMaster25(modelBuilder);
        SeedActivityMaster26(modelBuilder);
    }

    private static void SeedActivityMaster3(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000003");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("08d47879-25c8-4009-8a5a-43bb1ce2d4ad"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4ec4fcb2-4ae4-488c-8ea7-2e041f084c48"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("93e285c6-dd4c-4802-a930-265142a77481"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f7528494-9836-4455-9da9-351a053b529d"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2c28fec3-3c39-4cd9-bae3-f4e3ad082d99"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("07b8aa1e-ed4e-4665-ab9f-d0d600d154a9"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7212ab28-1323-49b7-a37c-6cd4db4e8102"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7803530a-d508-4040-9697-e3275b740558"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2307dd77-092a-459c-8cf8-9dd7d4052532"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c795abab-561e-48f4-8c35-4f199e8bd06a"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("13a573ec-e219-4be6-a570-ada6431e3002"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("27cbb526-4d6a-43d8-8907-604d684b7734"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e7a0e349-3f2d-4b3b-9858-de097e44bb8d"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("965995a7-1d68-4978-86c2-10cd2265f36d"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88b5db1d-f0fc-4b91-888b-6ded612088cb"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b5ef6f8-17de-4154-a59a-b624864e69a9"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b45d19b1-8bcd-4f25-91d4-4728530503c1"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8180d017-c10c-4cd0-b99a-a7d838b019af"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ff990c53-fa5c-4f77-b19f-36ada11518b0"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f113debb-cab9-44ad-b00e-b5e4018e1f5c"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster4(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000001-0000-0000-0000-000000000004");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("955140b4-00aa-4ab2-a1f1-5ab51fc3f08d"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09b3e941-a3fc-426f-ba68-980e3a7a6a27"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("db840b56-f8a1-42bc-b984-df7e134ad5a5"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2c65ce6c-4aeb-4dcf-adf5-79fd96a74fef"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("93edeff9-3c3f-4090-bf19-edce625c26c8"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2b6382a0-29a5-485b-918c-4b04719ac277"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a372b33c-04f9-4d86-8316-f9264775ccda"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("40cc3cb6-1bc2-4c4c-83e8-c79b360f0e4a"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("16df93a1-6764-46e4-ae7c-f7c5e9cc094f"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f134eeef-7a92-4274-9115-125cf9d5373f"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("26f83daf-f89d-4ece-a1b3-a9d6c00307a5"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e05bc064-2c78-4dce-bb83-caa05e894ca0"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bfaebe11-4b44-4046-8353-05b4aa658c74"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7bfda768-0861-456f-bb67-81e44547f005"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f24a7845-057a-4b89-b83b-817822f8e4c6"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7e4c3f32-9931-4cca-9672-9edbd662a8d9"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ab057b3a-5b18-46d3-8af1-c608cbd564e0"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b5a597a9-1cbf-49d8-af83-0cf43698e8e8"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("441afc7f-a0cb-4cb2-b386-312c14f24160"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("66cdd933-7c82-466d-a3c0-43c1651064ec"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c76908fd-b7f1-4ace-b87c-ea30bafda313"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cd97222f-0ae5-4822-84a9-398a52188da1"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1416a5de-152c-4c95-bcc9-f983c1e4ad88"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0dcd763b-9cbf-43c8-93a3-49b24a0592b5"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6d78ce26-de03-4b4e-9198-dedd505a35c4"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("681d9573-580c-47ad-a080-6b4e544c8ec4"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0c083254-bbf4-4496-85c5-173dec86ddef"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8d757c01-965e-4e07-80d1-bafbb637683b"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("578cb175-3ca9-4ae4-a51a-dddfe7833fe3"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad656132-ed15-45bf-b23e-8afd7c92040e"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a0c868d0-f7d4-4db3-8f69-170f488afa6c"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("45f74fc8-45c1-40fc-a63d-0aff25455051"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c2008d59-9c5f-430b-88ec-ac1032eb4d90"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ef4c44ac-4039-4bcc-9826-e5325a1a1007"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b0325b54-d58e-476d-a2ea-25f5cd79e5b6"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d26383d5-2148-45ef-b7f3-9fff778842af"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b75c69b-fa43-4774-a272-65eb7b913369"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4A00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1fc02631-b0c8-4697-946e-e37a8d22af7a"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster5(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000001");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04c346e9-ba12-4d6d-aa06-60eef6c346d0"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf0e5c9-774d-4edb-a7d2-f53012694063"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6dec26f6-15c6-4330-9363-5fc625b26e1f"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ff097399-f610-463b-8dbc-2d5ec3ff2a13"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("40ed5516-f7be-4b5b-999f-7cecf0a439e0"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e5deec6c-d0b4-4baa-adb8-5993e3df37b3"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5bde543b-a8f4-4c38-816d-b3fe7fd1dcc3"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f6d11a24-c99c-491b-aa13-156010ff8f28"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04fe8436-2097-447c-b9bf-0abe2be0b48a"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f51cfbac-61d2-41b6-9acf-c06f819d3ef1"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cf8948ba-a70c-44e9-86a1-37d658113833"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9583ebb9-bca8-4cd1-abeb-0a04f0a47a2d"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5A00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster6(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000002");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fc9a846f-3894-4023-b0ce-0ab2a8a99909"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e80d2f89-8c4c-4afe-b396-7709593f2e61"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("16ad46e9-8938-49dc-81c5-1b9ae6aae443"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("df162ee4-0c3b-4878-be2b-fb7d1f32a190"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9b306fde-5f0e-4647-a4e7-33a1adaa976b"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1ba35898-fa1c-4e8d-9f2d-d2729089870c"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5f165a36-6b93-4988-ab62-3bf9dd4c2814"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f155fbaa-f686-4517-92bd-4dc759374bdb"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cdcad9b8-cc0b-4d4d-b811-52b405235bd4"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("50143032-0002-4d9b-ae22-d53cd37cfdc8"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("27ba937a-9c7f-4ecc-b9f3-9a9b161eb76c"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("20284c8e-b358-49ab-8638-c91e42bed6c3"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b212cab-2c31-4594-9787-4221f854a705"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d061408-580a-4678-b261-4bf8c6e27464"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3ec3a9e0-b3bf-461b-987d-7aebd0a58371"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c4ed391c-9b92-43d0-8077-9deb2c561cca"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1772d3a5-c9cf-4b90-84c2-0495e59caa57"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b8466988-21ee-4712-83b5-56b1d1403f27"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4674359e-6b09-46ce-8f11-dcf690d8b575"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("21bd8d7d-5c9e-4296-9946-31744d8a6d73"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b168451-bbb5-432b-81cd-6856015d72ec"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c0f1e219-e407-4460-8634-ba833192ac99"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6d1fbc32-d882-4b52-9f6f-fdd69e60403e"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("211ce4b1-ed70-497f-9fc4-cfc3dc2ee3f8"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64d8d399-55a5-46cc-8dbc-27495181bcdc"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64feeab6-9865-4c8a-ad1b-63388dbc0aad"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("898b9c95-004f-4ab5-912c-436c41b9af4b"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ec6ac050-354f-40f5-870e-cb26cbd38dc8"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("02e45a45-e6d3-4e09-ac80-5402e7ea8322"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b22e1c1-4e06-4bf8-88db-2a5f3a730755"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("052a5cf4-6021-408e-adb1-9c7a7764a6c4"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f545fd0a-f082-4279-bd73-d5ea4c5c4ddf"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("be931ba4-5b37-4296-96bd-680e77d389ab"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("56334ad3-57c0-4e76-8e3f-22b208c8949c"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17aeccc1-bbaa-4ef6-b59d-af67e31ddece"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b6197f0d-4eca-403e-864e-0744f12f143f"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b544e716-5038-4a01-ac5f-ba30cecb452f"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("585e23d7-1214-4e49-9a47-7b75c9c9e591"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9ea0df82-d199-439a-8b7c-de0862cefe12"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5be605c2-c610-4956-abc7-7fb403011004"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8dd4e197-d197-405d-bbc0-0c9d9c6cfe66"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc126c65-f23e-4403-a9b3-ff1fd148b5cd"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09b3f44e-67bc-43bc-8b88-7bf51e8a7950"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9a91541a-d201-4b3e-9321-6f062185cc4b"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f7bd3c96-fb5c-4281-9bb4-5b631aca5290"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0c13ecc1-3223-4c16-9abb-51e69e1e727d"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d7394cb4-78be-4d87-b56f-3b570983111b"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("081b58c5-4cbc-4750-b165-1b79d70c2c72"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4d9a2c2a-ec21-489a-a5df-1892b56029fa"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e6e4da3a-a8b5-4dcf-a891-4b2c3b1c4d88"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("514366d3-9f52-403a-907b-0eb230a3d09a"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("86a61a68-6503-436a-92b8-0da353de43dd"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c13aa488-69e4-4ec4-bc55-1de3ab7577c4"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a0655886-1f05-4450-b322-561f7ab54234"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b43498f7-280e-4dd0-9dd4-e4799aad482f"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("33b19000-dcff-4054-a9fd-31e1b3f95233"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("470f9567-f0f6-4722-aa32-788992c47f25"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("925099a5-d33d-4596-9879-d65775da638a"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e89acfc1-4629-4cb4-8cbd-96c514f826aa"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6343fbd1-4500-4478-a986-430077720e5f"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3a916071-80b9-44de-aaf1-8277f78f6625"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cda90f80-a64a-4d43-833d-a9af574dc069"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa858ddc-40e4-430e-8cd8-a3b2ee09818c"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("172c06d1-2f4b-4308-bc82-294745d500d1"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6ac29bce-c031-4b67-9d5d-bfc683b5fc62"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b35b0411-9005-46ba-9615-d7a90a727686"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("430bf6e6-17d8-4a63-8106-91280499b3e9"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("43adc193-73e6-4cd3-8a27-5f6f42e919b4"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("06c99882-5fea-46db-b48f-f6d68f6d52b2"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("979a3394-a7a2-4aaa-88bc-69a8f075671a"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b6e205a2-9e64-4a1d-b5ee-4e2e582af750"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("db275e55-9586-4f2d-84a5-7865d47f6be9"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c7a47373-9854-4971-b477-5d52b20598c2"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb4a6ae1-4a8b-47a3-9fde-220e926ed20f"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("02ced833-2e61-430d-b297-a885c3c9e8d7"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("73ba10b2-be36-4ef0-85fa-eda0f41df3c6"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5e1c762f-bd20-42f2-b2ee-dcbfcbe5e421"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da71752b-8646-46f9-aa7d-a9c1aa536dea"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37634b72-32c2-4c01-b122-7f9724a8320a"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e45b067d-582b-4e49-815e-3eca574fcee0"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de0683c3-e647-47a0-8f41-0d8b1c8cb7a0"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("606a1ab0-3f50-477f-928e-2c004dbf6099"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3e75cb7f-0ae6-410b-93b4-105d000f4e30"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f5b4ca81-f3f8-43e4-be6f-e7480fbbf78e"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3e8804eb-5eca-4c1a-b593-631961a7447d"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("721e6fd9-3dc4-4b31-90cc-079d32f87b58"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("43b81437-5409-4e34-824e-985cf80fe25c"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8d0a4c90-4155-4bf6-8189-977a6126371b"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78e52a23-8de2-4f14-8644-61a0e4a838ec"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d2e32821-15ef-4d1a-a62b-668ebb3bd724"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1ce8f571-d81c-45c8-bc95-a91733b63f0d"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000092"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("989e32f4-a50c-401e-a787-250467407afc"),
                Sequence = 92,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000093"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 93,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000094"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 94,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000095"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 95,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000096"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 96,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000097"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 97,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000098"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 98,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000099"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 99,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000100"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 100,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000101"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 101,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000102"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 102,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000103"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 103,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000104"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 104,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000105"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 105,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000106"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 106,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000107"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 107,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000108"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 108,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000109"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 109,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000110"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 110,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000111"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 111,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000112"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 112,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000113"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 113,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000114"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 114,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000115"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 115,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000116"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 116,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000117"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 117,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000118"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 118,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000119"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 119,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000120"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 120,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000121"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 121,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000122"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 122,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000123"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 123,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000124"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 124,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000125"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 125,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000126"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 126,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000127"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 127,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000128"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 128,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000129"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 129,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000130"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 130,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000131"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 131,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000132"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 132,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000133"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 133,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000134"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 134,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000135"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 135,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000136"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 136,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000137"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 137,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000138"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 138,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000139"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 139,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000140"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 140,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000141"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 141,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000142"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 142,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000143"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 143,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000144"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 144,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000145"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 145,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000146"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 146,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000147"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 147,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000148"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 148,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000149"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 149,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000150"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 150,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000151"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 151,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000152"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 152,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000153"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 153,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000154"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 154,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000155"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 155,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000156"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 156,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000157"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 157,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000158"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 158,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000159"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 159,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000160"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 160,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000161"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 161,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000162"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 162,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000163"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 163,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000164"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 164,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000165"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 165,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000166"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 166,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000167"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 167,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000168"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 168,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000169"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 169,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000170"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 170,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6A00000-0000-0000-0000-000000000171"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 171,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster7(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000003");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("58208b50-c7ab-4aab-8abf-d12e0bc499b8"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ede76791-a5a8-41cb-8fc7-bf9d94c7a91f"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0c9e3cff-e7a2-4432-9831-430d56a1d002"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("caed0266-0388-41af-8a93-fdcc084b2907"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("45efd065-5c15-435d-9328-1b94f681c251"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c0bfe921-cb9a-4dce-b27e-51701233d06f"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9b096988-5ff1-4b44-a0a6-77b6cb2c14c9"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f502097-445b-4934-ad76-0e45f16a7d76"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("062fda0b-afba-44df-8476-7779f48df1a7"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cf6d1e8c-3730-4527-a151-d3c15eecfffe"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4d7872fd-5fc7-4605-819a-7707b041d18e"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bb900f09-a78e-4079-b98c-b7ded3c8e9f2"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da066bca-1cbb-4546-b3de-ef557bd83220"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f3007f58-fd72-4809-b5ab-e6f36f257f07"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dce86551-5429-4c69-9c7b-c2a19dc21106"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7c829bf3-d0ec-40e2-b0d0-9f70dfbc1e9f"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d061408-580a-4678-b261-4bf8c6e27464"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3ec3a9e0-b3bf-461b-987d-7aebd0a58371"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c4ed391c-9b92-43d0-8077-9deb2c561cca"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1772d3a5-c9cf-4b90-84c2-0495e59caa57"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b8466988-21ee-4712-83b5-56b1d1403f27"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4674359e-6b09-46ce-8f11-dcf690d8b575"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("21bd8d7d-5c9e-4296-9946-31744d8a6d73"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b168451-bbb5-432b-81cd-6856015d72ec"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c0f1e219-e407-4460-8634-ba833192ac99"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6d1fbc32-d882-4b52-9f6f-fdd69e60403e"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("211ce4b1-ed70-497f-9fc4-cfc3dc2ee3f8"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64d8d399-55a5-46cc-8dbc-27495181bcdc"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64feeab6-9865-4c8a-ad1b-63388dbc0aad"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("898b9c95-004f-4ab5-912c-436c41b9af4b"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ec6ac050-354f-40f5-870e-cb26cbd38dc8"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("02e45a45-e6d3-4e09-ac80-5402e7ea8322"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b22e1c1-4e06-4bf8-88db-2a5f3a730755"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("052a5cf4-6021-408e-adb1-9c7a7764a6c4"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f545fd0a-f082-4279-bd73-d5ea4c5c4ddf"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("be931ba4-5b37-4296-96bd-680e77d389ab"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("56334ad3-57c0-4e76-8e3f-22b208c8949c"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17aeccc1-bbaa-4ef6-b59d-af67e31ddece"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b6197f0d-4eca-403e-864e-0744f12f143f"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b544e716-5038-4a01-ac5f-ba30cecb452f"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("585e23d7-1214-4e49-9a47-7b75c9c9e591"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9ea0df82-d199-439a-8b7c-de0862cefe12"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5be605c2-c610-4956-abc7-7fb403011004"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8dd4e197-d197-405d-bbc0-0c9d9c6cfe66"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc126c65-f23e-4403-a9b3-ff1fd148b5cd"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09b3f44e-67bc-43bc-8b88-7bf51e8a7950"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9a91541a-d201-4b3e-9321-6f062185cc4b"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f7bd3c96-fb5c-4281-9bb4-5b631aca5290"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0c13ecc1-3223-4c16-9abb-51e69e1e727d"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d7394cb4-78be-4d87-b56f-3b570983111b"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("081b58c5-4cbc-4750-b165-1b79d70c2c72"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4d9a2c2a-ec21-489a-a5df-1892b56029fa"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e6e4da3a-a8b5-4dcf-a891-4b2c3b1c4d88"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("514366d3-9f52-403a-907b-0eb230a3d09a"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("86a61a68-6503-436a-92b8-0da353de43dd"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c13aa488-69e4-4ec4-bc55-1de3ab7577c4"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a0655886-1f05-4450-b322-561f7ab54234"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b43498f7-280e-4dd0-9dd4-e4799aad482f"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("33b19000-dcff-4054-a9fd-31e1b3f95233"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("470f9567-f0f6-4722-aa32-788992c47f25"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("925099a5-d33d-4596-9879-d65775da638a"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e89acfc1-4629-4cb4-8cbd-96c514f826aa"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6343fbd1-4500-4478-a986-430077720e5f"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3a916071-80b9-44de-aaf1-8277f78f6625"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cda90f80-a64a-4d43-833d-a9af574dc069"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa858ddc-40e4-430e-8cd8-a3b2ee09818c"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("172c06d1-2f4b-4308-bc82-294745d500d1"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6ac29bce-c031-4b67-9d5d-bfc683b5fc62"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b35b0411-9005-46ba-9615-d7a90a727686"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("430bf6e6-17d8-4a63-8106-91280499b3e9"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("43adc193-73e6-4cd3-8a27-5f6f42e919b4"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("06c99882-5fea-46db-b48f-f6d68f6d52b2"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("979a3394-a7a2-4aaa-88bc-69a8f075671a"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b6e205a2-9e64-4a1d-b5ee-4e2e582af750"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("db275e55-9586-4f2d-84a5-7865d47f6be9"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c7a47373-9854-4971-b477-5d52b20598c2"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb4a6ae1-4a8b-47a3-9fde-220e926ed20f"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("02ced833-2e61-430d-b297-a885c3c9e8d7"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("73ba10b2-be36-4ef0-85fa-eda0f41df3c6"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5e1c762f-bd20-42f2-b2ee-dcbfcbe5e421"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da71752b-8646-46f9-aa7d-a9c1aa536dea"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37634b72-32c2-4c01-b122-7f9724a8320a"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e45b067d-582b-4e49-815e-3eca574fcee0"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de0683c3-e647-47a0-8f41-0d8b1c8cb7a0"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("606a1ab0-3f50-477f-928e-2c004dbf6099"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3e75cb7f-0ae6-410b-93b4-105d000f4e30"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f5b4ca81-f3f8-43e4-be6f-e7480fbbf78e"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3e8804eb-5eca-4c1a-b593-631961a7447d"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("721e6fd9-3dc4-4b31-90cc-079d32f87b58"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("43b81437-5409-4e34-824e-985cf80fe25c"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8d0a4c90-4155-4bf6-8189-977a6126371b"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000092"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78e52a23-8de2-4f14-8644-61a0e4a838ec"),
                Sequence = 92,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000093"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d2e32821-15ef-4d1a-a62b-668ebb3bd724"),
                Sequence = 93,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000094"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1ce8f571-d81c-45c8-bc95-a91733b63f0d"),
                Sequence = 94,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000095"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("989e32f4-a50c-401e-a787-250467407afc"),
                Sequence = 95,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000096"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 96,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000097"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 97,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000098"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 98,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000099"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 99,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000100"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 100,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000101"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 101,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000102"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 102,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000103"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 103,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000104"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 104,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000105"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 105,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000106"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 106,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000107"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 107,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000108"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 108,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000109"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 109,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000110"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 110,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000111"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 111,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000112"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 112,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000113"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 113,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000114"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 114,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000115"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 115,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000116"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 116,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000117"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 117,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000118"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 118,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000119"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 119,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000120"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 120,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000121"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 121,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000122"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 122,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000123"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 123,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000124"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 124,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000125"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 125,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000126"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 126,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000127"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 127,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000128"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 128,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000129"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 129,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000130"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 130,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000131"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 131,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000132"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 132,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000133"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 133,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000134"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 134,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000135"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 135,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000136"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 136,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000137"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 137,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000138"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 138,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000139"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 139,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000140"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 140,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000141"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 141,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000142"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 142,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000143"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 143,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000144"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 144,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000145"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 145,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000146"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 146,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000147"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 147,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000148"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 148,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000149"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 149,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000150"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 150,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000151"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 151,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000152"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 152,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000153"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 153,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000154"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 154,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000155"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 155,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000156"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 156,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000157"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 157,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000158"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 158,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000159"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 159,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000160"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 160,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000161"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 161,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000162"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 162,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000163"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 163,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000164"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 164,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000165"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 165,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000166"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 166,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000167"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 167,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000168"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 168,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000169"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 169,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000170"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 170,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000171"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 171,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000172"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 172,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000173"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 173,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7A00000-0000-0000-0000-000000000174"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 174,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster8(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000002-0000-0000-0000-000000000004");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("403cf397-2ad9-4f6e-889d-0d574a5f4ad4"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("65f1df0f-b6bb-4c35-9c66-d3c52cef90ef"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("74a948fc-e34b-4a22-b136-d19fbe6af766"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("085bb58e-540d-4b6c-84cc-54776d64fc89"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("57212d2b-4fb7-414c-a1b0-e05abafff7bf"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("18d723c7-436d-4711-8cbc-ea31d259bf39"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6bbba1c9-0e88-446b-bba6-80b1bb0f25d9"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a43c8dd4-f883-45a3-bdae-e8260442c89c"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("853c9e38-26b0-4693-a767-f26e4eeec52e"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f522dac1-ebfd-41f5-b806-da3809cc4eb6"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ab3f8145-4e56-429f-afc8-36fc8f591bef"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4d6ac49a-5417-4dd8-b419-e4bfb3934a9e"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2f20a7a6-7402-4bf5-b3a4-76ff474b3ec3"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1db870d3-d222-4d0f-a085-fa9541527f02"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("540425a1-235d-43cf-b6c6-243fbcd3633d"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("12c6986e-441d-431c-b9e1-b4fe891025ab"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f6f49553-2d9d-4807-88fb-8754e1b6b7bf"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("224dac96-da3b-4b43-ba41-35ff26695f69"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000092"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 92,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000093"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 93,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000094"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 94,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000095"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 95,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000096"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 96,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8A00000-0000-0000-0000-000000000097"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 97,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster9(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000001");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7febb68a-bbdc-4fb1-851b-fdddf0b71cff"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb88439c-f3d1-4999-ad00-08c69aaeb797"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d38ccb25-66d5-41f7-bfff-1ebaadb40941"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce231580-a82b-4bf5-a80f-60f8f43472c0"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("62d88f73-f5c8-4cb3-893d-f561d836cc5e"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fad895d-32b6-4595-aa83-4d6c937efe8e"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddb86f1c-d13b-44ae-aae0-4e0e508acbb8"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("73d5aa6c-30a2-473d-8c77-c8390104bb40"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("21a096df-ea8e-47e1-bebe-9e1c5b79af43"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c5af12e5-445a-4598-8a43-9c7e322444e6"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15629b0b-5874-4820-aaca-2110feb63147"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a1ff291e-a81a-453a-aca3-81c13b8489e4"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c2ee20b6-2096-452e-9f42-97affdbe75a4"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b26a149-0192-456f-ade5-27f3e27860e0"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9A00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("512447f2-f057-49a9-9b20-8adf07bd285f"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster10(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000002");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("21a83d6e-3a43-497c-bc26-84622f6dd12d"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f8d1caa3-df11-43af-a593-77e2b72c440e"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5614599f-a50d-4493-9b16-0906caa3cb28"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecc9d7dc-847b-4a94-896c-596e9d9b8186"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7d405e5a-ddab-4d18-bfd4-5b458437ab3d"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b2d6bb03-7edc-4965-92c2-39ad84ee6da7"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3be38508-28d0-4da7-ab04-a4309ac3a16a"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c025e02c-bf77-46c3-a1fb-cb54b019185e"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d9547769-376d-4f26-93a1-8fce5bdbf80b"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ab517858-1181-498e-a42d-43ffc9cbbc0a"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0d573c4b-545c-445d-a0b2-e145c1a7f9aa"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("abe52242-c1c6-4efe-9771-a9196274b423"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dcb733c0-b4d6-4911-9c3d-502cb00bc9f8"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d162b339-6395-419a-a780-fab677a1a43b"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c1c555b7-7771-45db-a267-ce7b6fddf3e5"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad9b045c-4ed0-480b-b3f9-038995566517"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("97ebe931-848b-476d-b2ab-10492e54e4d3"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("43c7e739-2e6e-4aa9-966f-55544237d3f8"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64125285-8631-4209-b991-7f032da5a3f6"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c4f8a775-496e-4f4b-b14c-e848043ff388"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("681fd4d5-16b2-4f26-81d6-d26664366a90"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d905cf97-5b30-46bf-8f54-12f2fca5a007"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7febb68a-bbdc-4fb1-851b-fdddf0b71cff"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb88439c-f3d1-4999-ad00-08c69aaeb797"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d38ccb25-66d5-41f7-bfff-1ebaadb40941"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce231580-a82b-4bf5-a80f-60f8f43472c0"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("62d88f73-f5c8-4cb3-893d-f561d836cc5e"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fad895d-32b6-4595-aa83-4d6c937efe8e"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddb86f1c-d13b-44ae-aae0-4e0e508acbb8"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("73d5aa6c-30a2-473d-8c77-c8390104bb40"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("21a096df-ea8e-47e1-bebe-9e1c5b79af43"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c5af12e5-445a-4598-8a43-9c7e322444e6"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15629b0b-5874-4820-aaca-2110feb63147"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a1ff291e-a81a-453a-aca3-81c13b8489e4"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c2ee20b6-2096-452e-9f42-97affdbe75a4"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b26a149-0192-456f-ade5-27f3e27860e0"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("512447f2-f057-49a9-9b20-8adf07bd285f"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5c9734db-cc01-423c-b813-f70c874d1f93"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f7b1dd13-c8a9-4079-a522-ad4d414432a4"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("288645d9-3492-43e0-82bd-490517fd0c6f"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d8dc1b76-e2df-4b01-a431-26bdc7bdf5b9"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("35a436a3-69fe-44ed-ae55-37d57a2707bf"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1eeaa530-b6cb-44e7-ae74-3718b1e5e872"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d1260a09-491d-4341-9e69-14c1d5d9abcd"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0e4990a2-a019-448a-9e4d-a8075fb704ef"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d1bf6e74-6579-4410-a0e8-96670c387d09"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAA00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e70fbe5d-3ae4-4871-9698-91c816aea508"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster11(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000003");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e6a5a23c-9801-4888-bfac-54d0fbdfd35f"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4baf7b8f-a015-4076-9b8e-91801d77a5aa"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04d7af06-438e-450c-bfe9-eb37e06f5e6b"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9cca1846-e662-467e-9ef6-cd81e1ffede8"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5dc23780-10f1-4fdd-866f-4e56d254e050"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b346074a-ee4a-48e5-a007-ffb1fb299c08"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b14ea2fe-5ca2-4d69-81d2-5a871fb4d18e"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb353b87-d350-476a-bea5-706999583474"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7541e031-0162-4a3f-affe-b15bc6bcd374"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d8683107-69be-46a3-be3c-0bf9c7c55359"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("aea84fbe-06c5-428c-9967-6212a909d10d"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7ce2a769-44b4-43b7-b3e6-25ba7d0757b8"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b706902-8f64-41b5-8816-1170f03dfb81"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("85ce2c11-1394-4f61-a04e-2af869da95f0"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("52e4c536-a852-4100-a959-340427f8a202"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("968021b6-350c-4d0b-85a9-848557a4bea4"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d91e0777-034c-44e1-9053-01eb3e02040c"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBA00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a2289bc1-e11b-45b2-98ac-85a8eb35c37b"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster12(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000003-0000-0000-0000-000000000004");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0f111bb0-0aeb-4000-bb91-f398ef931222"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2cc08f24-5f4d-421a-ae3a-cd8613d9e859"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("66ec5514-f5bd-4346-b277-3dc4efa61453"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4bee9e3-62d7-4096-bd95-3fb9881ac02b"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("46de0b44-619e-4283-ada6-5fbb026ca41a"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("49cb5eb3-b64d-4e6f-b512-217892c688f5"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c25a5fba-d27f-456d-beec-7914265ff91c"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("79b31440-1ebe-41c9-954f-669b1a66918f"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce34e1ed-00c5-4ee8-b88c-bfd5d72e65de"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4f15b0b2-0b65-46ed-9666-d8718ed18631"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster13(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000001");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cd23880b-d8d5-4906-8bbe-524b6fb2a026"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b3c59182-7948-4c6c-a6c4-955089429d72"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cb79f23d-81a7-49f4-b810-e730814446ff"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c9b7cf99-40b5-4519-986b-86bbc0268dee"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d200b52b-da7d-423f-af36-e8572d69a711"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("469b11c3-9b29-432e-a40e-41f288e6bad6"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("79cb402a-73e1-412b-a217-fe74bfcf88e8"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d16913a-856c-4bcd-89fe-332d7b62e41f"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("946db233-fc09-4c54-b459-ee4a7ecbface"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9926b25-f332-4514-be49-3d12c2545096"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e1231319-bdfd-492e-be42-8d54eda27a64"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4e6dd28d-ce0b-493a-ac7d-3acf97bacb9c"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("19a7f51f-b924-4082-9248-7e58393e202d"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("182b9f7c-e4ba-4d4c-845b-c6572c78460d"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fef0be3f-fbc9-4976-9d08-1dbd21fc6105"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fb4c4e21-b0a4-4fb1-aa46-378b38b4d769"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a5df3e69-c336-4d9e-8d58-3b5362338cb4"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("52793a37-42d0-4912-b482-4f00fe58808a"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("594b64c5-e8b8-48df-a1fd-7c4cff067732"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8585aba3-f549-40ae-a3ad-27bc0b7f379b"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3e6d1e9e-7597-4f12-a30e-f5848931e418"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6d149393-9070-4f01-857b-70c9c0c51cb8"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad45bf90-1109-4cc3-ab85-2619da1d9101"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1566dece-d7b0-4e35-9d57-8e6a42642b3d"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ec66f868-c7d8-4309-8e13-7f686470236d"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f4406a32-3e71-47b7-a307-373e83246d5d"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cfefdbd3-fb55-40e2-8c69-436c7262b42e"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("82d17236-eba3-416d-bac6-e1fbc2cd527b"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDA00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("69491148-22ea-492c-ac6f-adddc0b410b2"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster14(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000002");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f72519f8-49b9-4876-82e7-d46fc87e8818"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("42fdc704-b5e7-44b2-b45a-d8199af4d07b"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("864d236d-ce8e-4e01-af52-aa3b765d48d4"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7eafc3ba-549c-4a4c-a2e3-b7bc3d4e0940"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d5f41566-352c-4247-b12f-59b664d1f0b2"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4e12e9de-426f-4bbb-b512-8611c61a635c"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("867faaf8-e5df-4458-9fdc-5f4b598ebc82"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f75036e3-6dad-4313-9e9b-4e693cc1e88f"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1ca61259-3a0c-46b1-8d26-1554ffea85f1"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b05e5faf-afce-468d-9f8d-2fcebaf0f6fe"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b44896bc-fd7d-4a0b-a47b-280a50c7327f"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d559d0e-0c62-4731-82a0-b65eacccb862"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FEA00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7d603add-7f92-4a9b-9935-f88951c077fa"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster15(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000003");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bc89becf-fabc-4491-811b-7717fe102680"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6faf8916-81be-4553-9614-356e0252ef61"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9392fbd4-16f4-4c00-acec-f323e8a40e3d"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d8a00696-ebfb-4112-8688-1e3d3786927e"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("658f2282-bbfa-4273-b550-478b78965c2f"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("52389a97-13cb-4105-b765-d9f9042e1c28"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88e1c6b7-ec67-4579-af62-a3f27e2dce6d"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1eb79f3e-cb02-44f2-a80d-9ed634952cd0"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bf403728-0d5d-4ab4-9d90-16c26aa88bcc"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6ea6173e-a66d-4894-af82-e296d53d4f8d"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8f3b376e-a9c9-412c-9176-410878f6c35e"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ec96cb4d-0a87-423c-851f-76fa7f64fde3"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc9487a4-4ac2-4a8f-90b0-40181bf660fb"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5cb6906f-3fdf-4626-a667-9ff053be78b9"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d014b5eb-d37b-4c57-ae02-7e2253ccaf8d"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59dbe1e9-14dd-4498-b9fb-767b8a428a62"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("77072599-a0dd-4018-9922-6a3e80e7319e"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("39c663f5-cb16-4127-ade1-867d3a58a617"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f5bab168-1f30-42d2-a704-cfdd6a0a6aa7"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f2603bb4-f950-49fa-9706-e1c0a37ee75e"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc63b3a7-0ece-4efb-80d0-0c1db4a272bd"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a0e5b184-0a4a-47cc-a64c-03429beb5c5f"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8768930e-a8b5-42aa-8606-4859dd18da32"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FFA00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("478bf621-712d-42bf-9da7-7b51a085562e"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster16(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000005");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("53e28149-b4ea-4505-a216-88d94d17bda1"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a040cd39-7f2a-4c4d-ae2d-9be050aa8fe7"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("352c036d-d372-4e89-92dc-764f8c64349a"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5dc52daf-bffa-417f-a1e8-6715f815fdab"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6845689f-4ba7-4c30-9793-80103f0f2723"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0c00f837-611d-446d-b658-2c3a601eb921"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("95e0441f-ab15-48c9-80d0-21672b96bcec"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a7bdf2ed-c39f-47b4-ad62-72c5817bb162"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("96a4199e-0dd8-4daa-9b67-5380143f0d74"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dd082a2c-d35e-45a4-b861-5b551519ddbb"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83f4a4ee-75b0-418a-9d03-ec77d0f0f6e9"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7c6f6b88-4eae-4346-9a4d-e38a4ce588f4"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e40c65ef-3658-493e-af56-031c223df3ad"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c3053643-fbe2-4ff4-96b5-9098697431de"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a954cea8-945c-4b93-90ba-f41d1a58576b"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1c955271-6607-4f41-bb6b-f070821c5375"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e5dcf94a-08f2-4f2f-9355-fbff7b252a36"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2da4b5d8-2d4c-4e69-a8cc-7ad9028abcb0"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b3848625-18e2-4025-acf4-f3d9ac0d47ca"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387154b1-94a1-42a9-acb0-e8b5f784179a"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa17f164-ca80-4ea1-a1a5-50044a94ba96"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d0837def-c9cb-4ee2-8a0e-d1786b147fd0"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cf2b96fc-b547-4bfc-aea9-ac2a53e626b0"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("140cc6e1-5532-4bf3-a0d2-b7520efd62e3"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("abb745e8-b1a1-44e2-8932-6f42997333f8"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e31bfb18-ab34-43db-a4cd-bc17739f0073"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a0e2f6fe-1c80-4aef-a4e4-f3558333c916"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("77825f8e-98e1-4c39-89dd-d9d494e1ada4"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F3B00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("599a1b61-2cc0-4f5c-b1cd-a29aa3b4fda9"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster17(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000004-0000-0000-0000-000000000006");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("129bc385-a35c-4168-9957-2b389fc2a4aa"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2c036e3d-7375-4a49-af8a-75725ac8467e"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f5a1c3e7-a35f-46e7-9571-a146012d4a33"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a8f96a49-e70e-418d-8406-35bbbcc6b32c"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("993aa6ba-0edc-472b-b4ab-1f25754c9bf9"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cd6456a4-e734-4044-b31b-0ef06170232e"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8492bd88-5615-4ae6-aed4-afc8454fb934"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bce9a766-6a7a-4a33-bf9a-cdf890b7fa1a"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e8e82513-ed02-406c-94ff-bcb6abec9be5"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c84d19b4-30cc-4dd6-8f28-a967e176a5a7"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d23ff95e-ac50-4ca3-9d43-017bc279f545"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6c02392a-e8e8-4164-b019-f31ba44a8166"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5da2b26a-6b12-47bb-9520-e918dc397478"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09272c94-f0e9-44b3-8313-c067d777eebe"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F4B00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("103c2da9-1b8b-4e09-80f4-1bfc2c173295"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster18(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000001");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F5B00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster19(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000002");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F6B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster20(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000003");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("bbe2d49a-8f18-4d83-ba50-2255bfc2f332"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0e825a1e-597c-4249-969c-d9bf8ecbf217"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5951afd7-c2e3-466d-9a92-a4c2f7fb54e8"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1e8d2475-09b2-4432-b759-546956257b4f"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cd8c0c83-3458-401a-ac03-781f2a4e2873"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("85a1755e-c267-4414-8212-7388f41c23a5"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2ddd69b8-b5b6-4194-8205-45b75b615e37"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8c456f71-b17e-43b4-8862-12e23ba1ad1b"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7894ae06-2b59-4b01-861f-b1d31a86c6a5"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f14d841d-7b32-41e6-8133-0d6d7603f40a"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fbe8cd1d-3428-4f5c-89db-df03ab0be959"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d3acc70b-d069-45bb-aa23-6b580d5798d3"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b0763c83-2bc1-42d1-aa70-adc0f6998653"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("eef55547-7f41-4087-9f28-eeb5d059b98f"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d6e6598c-93d7-46b5-a1f5-6925b03ddff0"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1ab21f95-1979-4859-aefc-64df110e42bc"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("97738c09-af82-4fbb-aa44-2d9d49a3e0e2"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9a97608c-d973-4df6-ae9b-b09301805f73"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4af9ee4e-e534-4724-b5ad-fc01a37a4ba3"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("34742979-182c-419f-b838-32265f835f45"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4cf893c7-8ab5-407f-8f37-954f580d197e"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000092"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 92,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000093"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 93,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000094"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 94,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000095"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 95,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000096"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 96,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000097"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 97,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000098"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 98,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000099"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 99,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F7B00000-0000-0000-0000-000000000100"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 100,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster21(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000004");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68afb347-f6f4-4129-be0b-2cc62dfc82f2"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9f4a2292-f98f-4a76-aee8-cdb91ede9624"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e5bc132a-d5f8-46cf-a462-7d149fe2c698"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f56c730c-7b84-4f92-b7da-c8f9aafb5481"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d84680f-b347-4632-9caa-40f755c9f28a"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a4ed943d-30a8-4bf5-9684-0265ac0664f4"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4d0d208f-72e9-46a0-b4af-cb811303a757"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5a993c86-32c7-472a-a39d-b58fd2fc1c02"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("922d888c-00f2-455d-92a0-427415001514"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c8e475f1-cd7a-4dbf-a902-49e6480baa06"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F8B00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("aac8ff6d-ce32-472f-ba22-be8f5656f374"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster22(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000005");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("F9B00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster23(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000006");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FAB00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster24(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000007");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FBB00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster25(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000005-0000-0000-0000-000000000008");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCB00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCB00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FCB00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }

    private static void SeedActivityMaster26(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("10000006-0000-0000-0000-000000000002");
        
        var activityCheckListItems = new[]
        {
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000001"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("88cc1dbf-d471-4216-ae6f-a9ed341400d0"),
                Sequence = 1,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000002"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f12fb28-c711-4937-8341-1a030c6a996f"),
                Sequence = 2,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000003"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7852e526-2969-49ea-8ca1-4028a99d0679"),
                Sequence = 3,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000004"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8323dcd7-2fed-41dd-8b0e-aa21393ee086"),
                Sequence = 4,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000005"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1af7c222-6b43-43a7-868b-c3f41cb29d46"),
                Sequence = 5,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000006"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("408dfdc3-f5fb-460a-bfa6-035782f57535"),
                Sequence = 6,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000007"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("59136623-44ad-4d1d-bb7c-92dd241da546"),
                Sequence = 7,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000008"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb58d2a-60d0-4138-afb4-ea16ebbfdc4e"),
                Sequence = 8,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000009"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5677ef27-1524-4d76-be31-60682e67dcc1"),
                Sequence = 9,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000010"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c468d5dd-3a81-4512-ac31-60aa0d667abc"),
                Sequence = 10,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000011"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("190be8df-7b70-4c79-a737-1df209ba7e6d"),
                Sequence = 11,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000012"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("210828ab-eb2a-4b2a-a4d9-cf22cba5ac74"),
                Sequence = 12,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000013"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6e8d71d1-3de1-4e35-876b-6fd2a7f540fb"),
                Sequence = 13,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000014"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d103945-bcd7-484c-a3e1-ea63598bad72"),
                Sequence = 14,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000015"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e876f8f0-fc0f-4feb-93af-22afb87e851d"),
                Sequence = 15,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000016"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a3986e4-0079-4e49-81e9-7973dd0b888f"),
                Sequence = 16,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000017"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("17780e95-9d21-43ee-9cea-df7209a59b59"),
                Sequence = 17,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000018"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cce7b248-d364-41fa-84f4-489bbe5ba3f6"),
                Sequence = 18,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000019"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ce5a9bcd-5ff2-4418-b3d3-88e62149d329"),
                Sequence = 19,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000020"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4a4686eb-39a9-4881-b6f7-cbed17fb5bc7"),
                Sequence = 20,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000021"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("78b295bf-d82c-4ffd-ad4f-189bc2a0fd5e"),
                Sequence = 21,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000022"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a62eed08-be48-4b31-880a-c478c8e3b671"),
                Sequence = 22,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000023"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f3f8664-1c77-4f87-b7b7-87b7c7250569"),
                Sequence = 23,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000024"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3823cbdb-efb4-42dd-9c9f-8c1d24471a3d"),
                Sequence = 24,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000025"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3d2e1461-488c-46ef-b662-019f097a515a"),
                Sequence = 25,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000026"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a65828cb-6694-46ff-b252-f92626d6d6c3"),
                Sequence = 26,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000027"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("67ddfe9b-8c23-4cea-8216-22a5168c94d3"),
                Sequence = 27,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000028"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9e3a024d-57ad-46ce-b27b-1fc403f68b15"),
                Sequence = 28,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000029"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d442a0f-a5ed-49ad-8afb-5192f1d15a54"),
                Sequence = 29,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000030"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4b602baf-0fcf-4985-a31d-64707c18558a"),
                Sequence = 30,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000031"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("da4112c9-e84a-48ed-9ee0-67ddf655fd04"),
                Sequence = 31,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000032"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("861dad54-79db-42f7-ab37-68466a23f743"),
                Sequence = 32,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000033"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("498e4400-83af-4482-932f-8716bc72aaa1"),
                Sequence = 33,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000034"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("727cfca0-cfaf-4d6d-a61a-38dbbb5ab6d0"),
                Sequence = 34,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000035"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e4f673d6-7685-48d7-b00f-5b909c181493"),
                Sequence = 35,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000036"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("32ca540d-0700-4e5d-9b33-fe084dbecf04"),
                Sequence = 36,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000037"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("683581ae-9676-4651-b627-28663576d682"),
                Sequence = 37,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000038"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3b481135-7d60-408f-81d9-25cf9363ee8d"),
                Sequence = 38,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000039"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa5eae9f-6947-4eaf-8f5f-fcc6516488ab"),
                Sequence = 39,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000040"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2d7fbe6b-2411-4413-9294-1b807e6da3f0"),
                Sequence = 40,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000041"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8fe1b6f2-b823-4677-9827-23fa5f523e61"),
                Sequence = 41,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000042"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("15c7f5fd-b05c-49a0-a35f-a548d9f86126"),
                Sequence = 42,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000043"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9b4eb9a-5062-4a6b-a3ea-98bb4db4dfdb"),
                Sequence = 43,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000044"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dc1347fa-997e-4a1f-ab5e-ba8c1c286b3f"),
                Sequence = 44,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000045"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ddf6192a-38b4-4eab-b81b-ac7178214cc1"),
                Sequence = 45,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000046"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6f868e6e-7a11-4c65-9cef-f2aeae768964"),
                Sequence = 46,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000047"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("09749601-1157-43d5-bd9b-43ae7994d520"),
                Sequence = 47,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000048"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e66380f3-38a2-4392-b82b-fced24df44d9"),
                Sequence = 48,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000049"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("acfc8c2c-7b88-4a40-a78e-4a545e855ab0"),
                Sequence = 49,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000050"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de5ae193-0f9a-4a87-800b-186dd1ad03e2"),
                Sequence = 50,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000051"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("de8bc96d-aaa0-4176-8415-1f5c222116c6"),
                Sequence = 51,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000052"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0cd699e5-ca4d-4797-93a1-029308ade190"),
                Sequence = 52,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000053"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("83770710-147d-46d3-8b71-414f5c09b677"),
                Sequence = 53,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000054"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("113778e7-dca1-43e7-9539-22c3caf843dc"),
                Sequence = 54,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000055"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("387e21fd-ad8c-432a-b08e-059a790522a4"),
                Sequence = 55,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000056"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ba041522-26d3-48cd-acae-729cb0b667ab"),
                Sequence = 56,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000057"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("9c4da2e4-6734-4e94-9037-d2e8f88f931f"),
                Sequence = 57,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000058"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("54c1ece9-f4f7-4b9a-a76f-a6c22b4729ec"),
                Sequence = 58,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000059"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6deff94b-68f3-4827-8af9-06b37affbaec"),
                Sequence = 59,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000060"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04b4344e-91bd-4386-8218-626d7abddba0"),
                Sequence = 60,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000061"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5b9f0de3-a164-47c0-9ae3-2def089f998e"),
                Sequence = 61,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000062"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca1ab126-cd47-4d96-9819-de6c0c931649"),
                Sequence = 62,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000063"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37901174-a631-4d12-8335-dda5bd07b626"),
                Sequence = 63,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000064"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cbb97e1d-c8df-443e-bbf7-84e98c937d71"),
                Sequence = 64,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000065"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("147809cd-7f6e-4f39-9426-d74b9db3b490"),
                Sequence = 65,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000066"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("64bc0366-95f0-4de8-a817-4dba4fdf20c2"),
                Sequence = 66,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000067"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("68f28143-1d00-4a4e-b853-7724bcbc46ba"),
                Sequence = 67,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000068"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecb258bf-f26b-4232-afa7-457d523569db"),
                Sequence = 68,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000069"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ad5de0d1-e0f8-4b85-b8ae-4c3c45621c35"),
                Sequence = 69,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000070"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("72cf8727-5ae5-4c63-9640-483853577ea9"),
                Sequence = 70,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000071"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bdab2ef-3541-4e57-bd37-c092e66cc603"),
                Sequence = 71,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000072"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5d730e37-85be-462c-9170-41a5e3d711b2"),
                Sequence = 72,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000073"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37360f3d-9505-4382-a4c3-e3b7ce068acd"),
                Sequence = 73,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000074"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("2bbe6d9e-1c54-4111-84e9-dd5242d6853c"),
                Sequence = 74,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000075"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f339ecfd-447b-483b-92b1-31b24987de87"),
                Sequence = 75,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000076"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7762e342-6be1-4849-a614-b59106040118"),
                Sequence = 76,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000077"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0a0669f6-ce5a-4c00-b9f6-0d90dbd16c1e"),
                Sequence = 77,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000078"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("025a157c-d1d2-46cb-9a70-6af87031c935"),
                Sequence = 78,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000079"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d938728b-f6f4-4006-ad53-df88d75800ae"),
                Sequence = 79,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000080"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5754a1ad-7e69-452b-a749-ed982ff0bd49"),
                Sequence = 80,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000081"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6913ca24-239d-497c-825e-ae75f5593dab"),
                Sequence = 81,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000082"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5e055348-85e7-438a-bf16-58ef08ad0299"),
                Sequence = 82,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000083"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("fa367570-0059-4e5d-914b-ea736d74d929"),
                Sequence = 83,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000084"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("38cbe975-ccca-4392-a4df-696b155663dd"),
                Sequence = 84,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000085"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5c829863-31c8-4f21-a39f-b31a139886fc"),
                Sequence = 85,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000086"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("3c3552b8-391b-4e50-a2c4-fbe64ad258a6"),
                Sequence = 86,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000087"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("593f6152-0b33-4ffc-9db4-40eef486ef60"),
                Sequence = 87,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000088"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("38a7b53b-6f7b-467f-a8a7-19f15c50d7ea"),
                Sequence = 88,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000089"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f78928e3-4a97-4ad6-8e9c-0ee1d4e9c533"),
                Sequence = 89,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000090"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("36aea065-6d5c-4745-8b25-5e45d1922bd1"),
                Sequence = 90,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000091"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a6c9d888-06bc-4a22-9682-ed2ac3ca76a0"),
                Sequence = 91,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000092"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ecc10bd2-ad34-48ba-8bc3-8a40d44fa0a4"),
                Sequence = 92,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000093"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("729fdd41-54d6-476d-b771-b6fe144a77b1"),
                Sequence = 93,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000094"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f9fe2e06-3ad3-42a6-842c-9efac4bccad0"),
                Sequence = 94,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000095"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a058982e-398c-489e-9737-fc5294b1fa4b"),
                Sequence = 95,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000096"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a9f2380a-44c8-4961-a2c3-5bb31d8aedf4"),
                Sequence = 96,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000097"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a6acdd96-32d0-41e3-a91d-7e72a467e24f"),
                Sequence = 97,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000098"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a2bd1dd4-5df1-4b34-a623-bbe5f677d839"),
                Sequence = 98,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000099"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e7496dc0-9775-4ca4-beca-053b892659c5"),
                Sequence = 99,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000100"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("13724d8f-e010-42ea-82fd-183a6e9d05bc"),
                Sequence = 100,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000101"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ccf78f91-1eda-4ad1-a8e2-9004105d77bd"),
                Sequence = 101,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000102"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5c90ef3b-9568-43bb-888b-2b6f0c3a6d61"),
                Sequence = 102,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000103"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c5264e1d-04e7-4ecf-ba81-cb01b30d7b7c"),
                Sequence = 103,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000104"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("69c8ef21-5d6a-4053-8664-a4bbf67f2002"),
                Sequence = 104,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000105"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("53ea654d-85d1-45a3-a724-36a211ed2002"),
                Sequence = 105,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000106"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("af94ae15-6ae0-49d7-a2d3-d277a3a48bff"),
                Sequence = 106,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000107"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("98220e2e-b428-4fd3-a8a0-8cc428a7b7d7"),
                Sequence = 107,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000108"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("adbd1799-6f64-44e0-bb55-142bf4d25e98"),
                Sequence = 108,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000109"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("be8e4474-f481-48b1-9104-9060f18e5a38"),
                Sequence = 109,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000110"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8bee12ea-99c0-4b2b-85fe-72d3f2960c86"),
                Sequence = 110,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000111"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("a30e8f27-06c6-450b-8ce5-ac325a5f782e"),
                Sequence = 111,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000112"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("640de90f-e5f6-4b2b-ad3c-83198e6454dc"),
                Sequence = 112,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000113"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("d2729cb6-566d-456f-b375-bd21557428e4"),
                Sequence = 113,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000114"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("494f70ea-6edc-4323-bd7b-9f0871b2fb91"),
                Sequence = 114,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000115"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e7cf3ce6-4886-4a56-88a0-29eb6cb33b22"),
                Sequence = 115,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000116"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("94132e61-1f89-4e2b-ae0c-579f18e07a54"),
                Sequence = 116,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000117"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f56fd362-0129-4572-8472-fc212d7097e5"),
                Sequence = 117,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000118"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b435bcd5-93ab-48e8-8d70-611c966df1a5"),
                Sequence = 118,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000119"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("af868546-4f3b-47ae-94e3-67e5d7f60cef"),
                Sequence = 119,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000120"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e1686ab9-a5a8-4390-9ff6-14c1efbfb798"),
                Sequence = 120,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000121"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e2690a8c-bb7e-4af1-97af-4bdc2f0dc11b"),
                Sequence = 121,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000122"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("79d97660-c6f6-4f43-a1e5-f19c87678d79"),
                Sequence = 122,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000123"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("f0429c4f-e6f4-4f60-813d-3fb03aebf219"),
                Sequence = 123,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000124"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("b4d75187-8828-4249-a467-7513a65d1b11"),
                Sequence = 124,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000125"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("03e78227-dbf6-478e-8c01-f9d22c823fca"),
                Sequence = 125,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000126"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("12af8392-002a-4405-885a-b366a78058c1"),
                Sequence = 126,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000127"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("77751558-dc8c-4b53-a628-b370c7de2372"),
                Sequence = 127,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000128"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("81aa004d-85e3-4938-94ca-32aa23ce4814"),
                Sequence = 128,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000129"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7a10f754-bd03-45b9-b3ad-849441baaa85"),
                Sequence = 129,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000130"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("c7764709-9dd4-44b1-8555-dd0ba57d7d5f"),
                Sequence = 130,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000131"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("37ece6ee-5b00-4f59-99d7-793316077609"),
                Sequence = 131,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000132"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("7b91df5f-6edd-4042-a106-43b290732bc5"),
                Sequence = 132,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000133"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("4642025b-d9a5-42b2-991e-bd1110e5a444"),
                Sequence = 133,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000134"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("dd47d609-400e-4cb0-89ca-b543861e7dc3"),
                Sequence = 134,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000135"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("58df44d8-fc05-497c-9fba-aac2bb0812e5"),
                Sequence = 135,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000136"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ea95acb3-aac2-4337-8121-59e25743c831"),
                Sequence = 136,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000137"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1aa26ac0-0531-4db8-b9f2-f62ae6963262"),
                Sequence = 137,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000138"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("33adc13d-0a8a-4d1f-9b45-5e29351b5495"),
                Sequence = 138,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000139"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("553f5c81-5d40-4a60-8164-e918a0c480aa"),
                Sequence = 139,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000140"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("cc8cfe61-66bf-46e7-abcd-547be6c11d63"),
                Sequence = 140,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000141"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("8f7c8795-ea1f-41fe-9689-458d5641939f"),
                Sequence = 141,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000142"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1d1aeb89-68aa-49c6-b2c0-020ef825deda"),
                Sequence = 142,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000143"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("04698a62-e44f-4e89-acc6-cbcded5c5002"),
                Sequence = 143,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000144"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("92094210-da61-47e3-a50b-d9fd847ddf8d"),
                Sequence = 144,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000145"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("5ee62650-4a35-4195-9bf0-07d53c074fa5"),
                Sequence = 145,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000146"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ff776cae-3f8f-4fe3-af3d-c286ccc7bc6c"),
                Sequence = 146,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000147"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("6c91891f-f7c4-474f-9e6d-21983a0df8c5"),
                Sequence = 147,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000148"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ca7c0427-e7d5-4b81-b5ff-f8e2ba7d9116"),
                Sequence = 148,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000149"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ab04eac7-0ab2-46bd-89a8-08cf2554790d"),
                Sequence = 149,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000150"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("81493c56-6ed3-41c1-9998-f828b91f7799"),
                Sequence = 150,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000151"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ec889fee-8818-4413-8617-699c7e3768c4"),
                Sequence = 151,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000152"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("41d00092-1bfc-4b42-bbd2-cb003193800a"),
                Sequence = 152,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000153"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("e14c918d-0431-4b7d-9ff6-80ff3a1b9bec"),
                Sequence = 153,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000154"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("0fd1838f-30f3-4548-862a-36e0cf28e7ea"),
                Sequence = 154,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000155"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("111d0079-f84b-46e7-b2ff-dbf0b74f3ba1"),
                Sequence = 155,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000156"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("33d53b69-a990-4140-bfa1-73e94fccc585"),
                Sequence = 156,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000157"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("ffe42576-2b31-4f7d-890b-78015bb52ccb"),
                Sequence = 157,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("FDB00000-0000-0000-0000-000000000158"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("1f8f3d81-bf07-487f-b306-f3f861967a64"),
                Sequence = 158,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }
}
