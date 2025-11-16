using Dubox.Domain.Enums;

namespace Dubox.Application.DTOs
{
    public class BoxMaterialDto
    {
        public Guid BoxMaterialId { get; set; }
        public Guid BoxId { get; set; }
        public string BoxTag { get; set; } = string.Empty;
        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public decimal? RequiredQuantity { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public decimal? ConsumedQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public bool IsShort { get; set; }
        public BoxMaterialStatusEnum Status { get; set; } = BoxMaterialStatusEnum.Pending; // Pending, Allocated, Consumed, Short
        public DateTime? AllocatedDate { get; set; }
        public DateTime? ConsumedDate { get; set; }
    }
}
