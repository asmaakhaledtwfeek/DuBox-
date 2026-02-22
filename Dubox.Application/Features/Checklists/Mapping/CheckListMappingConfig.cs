using Dubox.Application.Features.Checklists.Queries;
using Dubox.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Features.Checklists.Mapping
{
    public class CheckListMappingConfig
    {
        public static void Configure()
        {
            // Configure ChecklistItem mapping
            TypeAdapterConfig<PredefinedChecklistItem, ChecklistItemDto>
                .NewConfig()
                .Map(dest => dest.ChecklistItemId, src => src.PredefinedItemId)
                .Map(dest => dest.Order, src => src.Sequence);

            // Configure ChecklistSection mapping
            TypeAdapterConfig<ChecklistSection, ChecklistSectionDto>
                .NewConfig()
                .Map(dest => dest.ItemCount, src => src.Items.Count)
                .Map(dest => dest.Items, src => src.Items.OrderBy(i => i.Sequence));

            // Configure Checklist mapping
            TypeAdapterConfig<Checklist, ChecklistDto>
                .NewConfig()
                .Map(dest => dest.Sections, src => src.Sections.OrderBy(s => s.Order));
        }
    }
}
