namespace Dubox.Domain.Abstractions
{
    public abstract class BaseImageEntity
    {
        public Guid Id { get; set; }
        public string? ImageData { get; set; }
        public string? ImageType { get; set; }
        public string? OriginalName { get; set; }
        public long FileSize { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
