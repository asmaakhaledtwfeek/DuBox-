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
        public async Task<(bool IsSuccess, string ErrorMessage)> ProcessImagesAsync<TEntity>(
            Guid parentId, 
            List<byte[]>? files, 
            List<string>? imageUrls, 
            CancellationToken ct, 
            int sequence = 0, 
            List<string>? fileNames = null,
            List<TEntity>? existingImagesForVersioning = null) where TEntity : BaseImageEntity, new()
        {
            var images = new List<TEntity>();

            Console.WriteLine($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: ProcessImagesAsync called with:");
            Console.WriteLine($"   - Parent ID: {parentId}");
            Console.WriteLine($"   - Files count: {files?.Count ?? 0}");
            Console.WriteLine($"   - FileNames count: {fileNames?.Count ?? 0}");
            Console.WriteLine($"   - FileNames: {(fileNames != null ? string.Join(", ", fileNames) : "NULL")}");
            Console.WriteLine($"   - Existing images provided for versioning: {existingImagesForVersioning?.Count ?? 0}");

            // Get existing images for version checking
            IReadOnlyList<TEntity> existingImages;
            
            if (existingImagesForVersioning != null)
            {
                // Use provided existing images (for cross-entity version checking like all ProgressUpdates in a Box)
                existingImages = existingImagesForVersioning;
                Console.WriteLine($"🔍 VERSION DEBUG - Using {existingImages.Count} pre-loaded images for version checking");
            }
            else
            {
                // Query for existing images by parent ID (default behavior)
            var config = _factory.GetConfig<TEntity>();
                Console.WriteLine($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: Looking for existing images with {config.ForeignKeyName} = {parentId}");
                
            var predicate = BuildForeignKeyPredicate<TEntity>(config.ForeignKeyName, parentId);
                existingImages = await _unitOfWork.Repository<TEntity>().FindAsync(
                predicate, 
                ct
            );
            }

            Console.WriteLine($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: Found {existingImages.Count()} existing images for version checking");
            if (existingImages.Any())
            {
                Console.WriteLine($"🔍 VERSION DEBUG - Existing images: {string.Join(", ", existingImages.Select(img => $"{img.OriginalName} (V{img.Version})"))}");
            }
            else
            {
                Console.WriteLine($"🔍 VERSION DEBUG - No existing images found. This might be the first upload.");
            }

            // Track versions for files being uploaded in this batch to avoid conflicts
            var versionTracker = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

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
                    var originalNameLower = originalName.ToLower();
                    
                    Console.WriteLine($"🔍 VERSION DEBUG - Processing file: {originalName}");
                    
                    // First, check existing database records (in-memory comparison)
                    var sameNameImages = existingImages
                        .Where(img => img.OriginalName != null && 
                                     img.OriginalName.ToLower() == originalNameLower)
                        .ToList();
                    
                    if (sameNameImages.Any())
                    {
                        var maxExistingVersion = sameNameImages.Max(img => img.Version);
                        version = maxExistingVersion + 1;
                        Console.WriteLine($"🔍 VERSION DEBUG - Found {sameNameImages.Count} existing files with name '{originalName}', max version: V{maxExistingVersion}, assigning V{version}");
                    }
                    else
                    {
                        Console.WriteLine($"🔍 VERSION DEBUG - No existing files with name '{originalName}', assigning V1");
                    }
                    
                    // Second, check if this filename was already processed in this batch
                    if (versionTracker.ContainsKey(originalName))
                    {
                        var batchVersion = versionTracker[originalName];
                        version = Math.Max(version, batchVersion + 1);
                        Console.WriteLine($"🔍 VERSION DEBUG - File '{originalName}' already processed in this batch with V{batchVersion}, updating to V{version}");
                    }
                    
                    // Update tracker with the version we're using
                    versionTracker[originalName] = version;
                    Console.WriteLine($"✅ VERSION DEBUG - Final version for '{originalName}': V{version}");

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
                        var originalNameLower = originalName.ToLower();

                        // Check for existing images with same name (in-memory comparison)
                        var sameNameImages = existingImages
                            .Where(img => img.OriginalName != null && 
                                         img.OriginalName.ToLower() == originalNameLower)
                            .ToList();
                        
                        if (sameNameImages.Any())
                        {
                            version = sameNameImages.Max(img => img.Version) + 1;
                        }
                        
                        // Check if this filename was already processed in this batch
                        if (versionTracker.ContainsKey(originalName))
                        {
                            version = Math.Max(version, versionTracker[originalName] + 1);
                        }
                        
                        // Update tracker
                        versionTracker[originalName] = version;

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
                        var originalNameLower = originalName.ToLower();

                        // Check for existing images with same URL (in-memory comparison)
                        var sameNameImages = existingImages
                            .Where(img => img.OriginalName != null && 
                                         img.OriginalName.ToLower() == originalNameLower)
                            .ToList();
                        
                        if (sameNameImages.Any())
                        {
                            version = sameNameImages.Max(img => img.Version) + 1;
                        }
                        
                        // Check if this URL was already processed in this batch
                        if (versionTracker.ContainsKey(originalName))
                        {
                            version = Math.Max(version, versionTracker[originalName] + 1);
                        }
                        
                        // Update tracker
                        versionTracker[originalName] = version;

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
            {
                Console.WriteLine($"💾 VERSION DEBUG - Saving {images.Count} images to database");
                await _unitOfWork.Repository<TEntity>().AddRangeAsync(images, ct);
                Console.WriteLine($"✅ VERSION DEBUG - Successfully saved {images.Count} images");
            }

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
