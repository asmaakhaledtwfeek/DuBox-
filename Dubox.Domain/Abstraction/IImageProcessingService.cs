using Microsoft.AspNetCore.Http;

namespace Dubox.Domain.Abstraction
{
    public interface IImageProcessingService
    {
        Task<byte[]?> GetImageBytesAsync(IFormFile? file, string? imageUrl, CancellationToken cancellationToken);
        
        /// <summary>
        /// Downloads an image from a URL and returns it as byte array
        /// </summary>
        Task<byte[]?> DownloadImageFromUrlAsync(string imageUrl, CancellationToken cancellationToken);
    }
}
