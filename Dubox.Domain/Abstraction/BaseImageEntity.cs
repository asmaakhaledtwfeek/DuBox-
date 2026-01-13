using System.ComponentModel.DataAnnotations;

namespace Dubox.Domain.Abstractions
{
    public abstract class BaseImageEntity
    {

        [Required]
        [MaxLength(10)]
        public string ImageType { get; set; } = "file"; // "file" or "url"

        [MaxLength(500)]
        public string? OriginalName { get; set; }
        public string? ImageFileName { get; set; }
        public long? FileSize { get; set; }

        public int Sequence { get; set; } = 0;

        public int Version { get; set; } = 1; // Version number for files with same name

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

}
