using Microsoft.AspNetCore.Http;

namespace Dubox.Domain.Abstraction
{
    public interface IImageProcessingService
    {
        Task<byte[]?> GetImageBytesAsync(IFormFile? file, string? imageUrl, CancellationToken cancellationToken);
    }
}
