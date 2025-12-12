using System.ComponentModel.DataAnnotations;

namespace Dubox.Domain.Abstractions
{
    public abstract class BaseImageEntity
    {
        [Required]
        public string ImageData { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string ImageType { get; set; } = "file"; // "file" or "url"

        [MaxLength(500)]
        public string? OriginalName { get; set; }

        public long? FileSize { get; set; }

        public int Sequence { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

}
