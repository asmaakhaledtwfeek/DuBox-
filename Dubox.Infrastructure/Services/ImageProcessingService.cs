using Dubox.Domain.Abstraction;
using Dubox.Domain.Abstractions;
using Dubox.Domain.Services.ImageEntityConfig;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Dubox.Infrastructure.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IImageEntityConfigFactory _factory;
        public ImageProcessingService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, IDbContext dbContext, IImageEntityConfigFactory factory)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _factory = factory;
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
        public async Task<(bool IsSuccess, string ErrorMessage)> ProcessImagesAsync<TEntity>(Guid parentId, List<byte[]>? files, List<string>? imageUrls, CancellationToken ct, int sequence = 0, List<string>? fileNames = null) where TEntity : BaseImageEntity, new()
        {
            var images = new List<TEntity>();

            // Get existing images for version checking
            var config = _factory.GetConfig<TEntity>();
            var predicate = BuildForeignKeyPredicate<TEntity>(config.ForeignKeyName, parentId);
            var existingImages = await _unitOfWork.Repository<TEntity>().FindAsync(
                predicate, 
                ct
            );

            if (files?.Any() == true)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var fileBytes = files[i];
                    var base64 = Convert.ToBase64String(fileBytes);
                    var imageData = $"data:image/jpeg;base64,{base64}";

                    // Determine filename: use provided name or generate default
                    string originalName = "Attachment";
                    if (fileNames != null && i < fileNames.Count && !string.IsNullOrWhiteSpace(fileNames[i]))
                    {
                        originalName = fileNames[i];
                    }
                    else
                    {
                        // Generate a default name based on timestamp
                        originalName = $"Attachment_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{i}.jpg";
                    }

                    // Check for existing file with same name and determine version
                    int version = 1;
                    var sameNameImages = existingImages
                        .Where(img => img.OriginalName != null && 
                                     img.OriginalName.Equals(originalName, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    
                    if (sameNameImages.Any())
                    {
                        version = sameNameImages.Max(img => img.Version) + 1;
                    }

                    var entity = new TEntity
                    {
                        ImageData = imageData,
                        ImageType = "file",
                        FileSize = fileBytes.Length,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow,
                        OriginalName = originalName,
                        Version = version
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
                    string originalName;
                    int version = 1;

                    if (trimmed.StartsWith("data:image/"))
                    {
                        var commaIndex = trimmed.IndexOf(',');
                        bytes = Convert.FromBase64String(trimmed.Substring(commaIndex + 1));
                        originalName = $"Captured_Image_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jpg";

                        // Check for existing images with same name
                        var sameNameImages = existingImages
                            .Where(img => img.OriginalName != null && 
                                         img.OriginalName.Equals(originalName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        
                        if (sameNameImages.Any())
                        {
                            version = sameNameImages.Max(img => img.Version) + 1;
                        }

                        var entity = new TEntity
                        {
                            ImageData = trimmed,
                            ImageType = "file",
                            FileSize = bytes.Length,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow,
                            OriginalName = originalName,
                            Version = version
                        };

                        SetForeignKey(entity, parentId);
                        images.Add(entity);
                    }
                    else
                    {
                        bytes = await DownloadImageFromUrlAsync(trimmed, ct);

                        var base64 = Convert.ToBase64String(bytes);
                        originalName = trimmed.Length > 500 ? trimmed[..500] : trimmed;

                        // Check for existing images with same URL
                        var sameNameImages = existingImages
                            .Where(img => img.OriginalName != null && 
                                         img.OriginalName.Equals(originalName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        
                        if (sameNameImages.Any())
                        {
                            version = sameNameImages.Max(img => img.Version) + 1;
                        }

                        var entity = new TEntity
                        {
                            ImageData = $"data:image/jpeg;base64,{base64}",
                            ImageType = "url",
                            FileSize = bytes.Length,
                            OriginalName = originalName,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow,
                            Version = version
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
            var config = _factory.GetConfig<TEntity>();
            var fkProp = typeof(TEntity).GetProperty(config.ForeignKeyName);

            fkProp?.SetValue(entity, parentId);
        }

        private Expression<Func<TEntity, bool>> BuildForeignKeyPredicate<TEntity>(string foreignKeyName, Guid parentId)
        {
            // Create parameter: e
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            
            // Create property access: e.ForeignKeyName (e.g., e.ProgressUpdateId)
            var property = Expression.Property(parameter, foreignKeyName);
            
            // Create constant: parentId
            var constant = Expression.Constant(parentId, typeof(Guid));
            
            // Create equality comparison: e.ForeignKeyName == parentId
            var equality = Expression.Equal(property, constant);
            
            // Create lambda expression: e => e.ForeignKeyName == parentId
            return Expression.Lambda<Func<TEntity, bool>>(equality, parameter);
        }

    }
}
