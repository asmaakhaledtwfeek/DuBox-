using Dubox.Domain.Abstraction;
using Dubox.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Dubox.Infrastructure.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ImageProcessingService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
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

        public async Task<byte[]?> DownloadImageFromUrlAsync(string imageUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return null;

            try
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
            catch (Exception ex)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}. Error: {ex.Message}", ex);
            }
        }
        public async Task<(bool IsSuccess, string ErrorMessage)> ProcessImagesAsync<TEntity>(Guid parentId, List<byte[]>? files, List<string>? imageUrls, CancellationToken ct) where TEntity : BaseImageEntity, new()
        {
            var images = new List<TEntity>();
            int sequence = 0;

            if (files?.Any() == true)
            {
                foreach (var fileBytes in files)
                {
                    var base64 = Convert.ToBase64String(fileBytes);
                    var imageData = $"data:image/jpeg;base64,{base64}";

                    var entity = new TEntity
                    {
                        ImageData = imageData,
                        ImageType = "file",
                        FileSize = fileBytes.Length,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow
                    };

                    SetForeignKey(entity, parentId);
                    images.Add(entity);
                }
            }

            if (imageUrls?.Any() == true)
            {
                foreach (var url in imageUrls)
                {
                    var trimmed = url.Trim();
                    byte[] bytes;

                    if (trimmed.StartsWith("data:image/"))
                    {
                        var commaIndex = trimmed.IndexOf(',');
                        bytes = Convert.FromBase64String(trimmed.Substring(commaIndex + 1));

                        var entity = new TEntity
                        {
                            ImageData = trimmed,
                            ImageType = "file",
                            FileSize = bytes.Length,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow
                        };

                        SetForeignKey(entity, parentId);
                        images.Add(entity);
                    }
                    else
                    {
                        bytes = await DownloadImageFromUrlAsync(trimmed, ct);

                        var base64 = Convert.ToBase64String(bytes);
                        var entity = new TEntity
                        {
                            ImageData = $"data:image/jpeg;base64,{base64}",
                            ImageType = "url",
                            FileSize = bytes.Length,
                            OriginalName = trimmed.Length > 500 ? trimmed[..500] : trimmed,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow
                        };

                        SetForeignKey(entity, parentId);
                        images.Add(entity);
                    }
                }
            }

            if (images.Any())
                await _unitOfWork.Repository<TEntity>().AddRangeAsync(images, ct);

            return (true, string.Empty);
        }
        private void SetForeignKey<TEntity>(TEntity entity, Guid parentId)
        {
            var fkProp = typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id") && p.PropertyType == typeof(Guid));

            fkProp?.SetValue(entity, parentId);
        }

    }
}
