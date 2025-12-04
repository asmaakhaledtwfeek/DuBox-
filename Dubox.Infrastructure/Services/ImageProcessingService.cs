using Dubox.Domain.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Dubox.Infrastructure.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageProcessingService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<byte[]?> GetImageBytesAsync(IFormFile? file, string? imageUrl, CancellationToken cancellationToken)
        {
            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms, cancellationToken);
                return ms.ToArray();
            }

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                using var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                var response = await httpClient.GetAsync(imageUrl, cancellationToken);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType;

                if (contentType == null || !contentType.StartsWith("image/"))
                    throw new Exception("The provided URL does not point to a valid image.");

                return await response.Content.ReadAsByteArrayAsync(cancellationToken);
            }

            return null;
        }
    }
}
