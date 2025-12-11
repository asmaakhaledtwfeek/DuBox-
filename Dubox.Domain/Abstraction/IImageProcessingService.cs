using Dubox.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
namespace Dubox.Domain.Abstraction
{
    public interface IImageProcessingService
    {
        Task<byte[]?> GetImageBytesAsync(IFormFile? file, string? imageUrl, CancellationToken cancellationToken);

        Task<byte[]?> DownloadImageFromUrlAsync(string imageUrl, CancellationToken cancellationToken);
        Task<(bool IsSuccess, string ErrorMessage)> ProcessImagesAsync<TEntity>(Guid parentId, List<byte[]>? files, List<string>? imageUrls, CancellationToken ct, int sequence = 0) where TEntity : BaseImageEntity, new();
    }
}
