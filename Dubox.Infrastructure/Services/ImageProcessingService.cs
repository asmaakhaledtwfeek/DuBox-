using Dubox.Domain.Abstraction;
using Dubox.Domain.Abstractions;
using Dubox.Domain.Services;
using Dubox.Domain.Services.ImageEntityConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Dubox.Infrastructure.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContext _dbContext;
        private readonly IImageEntityConfigFactory _factory;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<ImageProcessingService> _logger;
        public ImageProcessingService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, IDbContext dbContext, IImageEntityConfigFactory factory , IBlobStorageService blobStorageService, ILogger<ImageProcessingService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _factory = factory;
            _blobStorageService = blobStorageService;
            _logger = logger;
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

       
        public async Task<(bool IsSuccess, string ErrorMessage)> ProcessImagesAsync<TEntity>(
            Guid parentId,
            List<IFormFile>? files, 
            List<string>? imageUrls, 
            CancellationToken ct, 
            int sequence = 0, 
            List<string>? fileNames = null,
            List<TEntity>? existingImagesForVersioning = null) where TEntity : BaseImageEntity, new()
        {
            var images = new List<TEntity>();
            var uploadedFileNames = new List<string>();
            _logger.LogInformation($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: ProcessImagesAsync called with:");
            _logger.LogInformation($"   - Parent ID: {parentId}");
            _logger.LogInformation($"   - Files count: {files?.Count ?? 0}");
            _logger.LogInformation($"   - FileNames count: {fileNames?.Count ?? 0}");
            _logger.LogInformation($"   - FileNames: {(fileNames != null ? string.Join(", ", fileNames) : "NULL")}");
            _logger.LogInformation($"   - Existing images provided for versioning: {existingImagesForVersioning?.Count ?? 0}");

            // Get existing images for version checking
            IReadOnlyList<TEntity> existingImages;
            
            if (existingImagesForVersioning != null)
            {
                // Use provided existing images (for cross-entity version checking like all ProgressUpdates in a Box)
                existingImages = existingImagesForVersioning;
                _logger.LogInformation($"🔍 VERSION DEBUG - Using {existingImages.Count} pre-loaded images for version checking");
            }
            else
            {
                // Query for existing images by parent ID (default behavior)
            var config = _factory.GetConfig<TEntity>();
                _logger.LogInformation($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: Looking for existing images with {config.ForeignKeyName} = {parentId}");
                
            var predicate = BuildForeignKeyPredicate<TEntity>(config.ForeignKeyName, parentId);
                existingImages = await _unitOfWork.Repository<TEntity>().FindAsync(
                predicate, 
                ct
            );
            }

            _logger.LogInformation($"🔍 VERSION DEBUG - {typeof(TEntity).Name}: Found {existingImages.Count()} existing images for version checking");
            if (existingImages.Any())
            {
                _logger.LogInformation($"🔍 VERSION DEBUG - Existing images: {string.Join(", ", existingImages.Select(img => $"{img.OriginalName} (V{img.Version})"))}");
            }
            else
            {
                _logger.LogInformation($"🔍 VERSION DEBUG - No existing images found. This might be the first upload.");
            }

            // Track versions for files being uploaded in this batch to avoid conflicts
            var versionTracker = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (files?.Any() == true)
            {
                var folderName = GetFolderNameForEntity<TEntity>(parentId);

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];

                    // Determine filename
                    string originalName = "Attachment";
                    if (fileNames != null && i < fileNames.Count && !string.IsNullOrWhiteSpace(fileNames[i]))
                    {
                        originalName = fileNames[i];
                    }
                    else if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        originalName = file.FileName;
                    }
                    else
                    {
                        originalName = $"Attachment_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{i}{GetFileExtension(file.ContentType)}";
                    }

                    // Calculate version
                    int version = CalculateVersion(originalName, existingImages, versionTracker);
                    _logger.LogInformation($"✅ Final version for '{originalName}': V{version}");

                    string blobFileName;
                    try
                    {
                        blobFileName = await _blobStorageService.UploadFileAsync(file, folderName);
                        uploadedFileNames.Add(blobFileName); 
                        _logger.LogInformation($"✅ Uploaded file to Blob: {blobFileName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Failed to upload file: {originalName}");
                        throw new Exception($"Failed to upload file '{originalName}': {ex.Message}");
                    }

                    var entity = new TEntity
                    {
                        ImageFileName = blobFileName, 
                        OriginalName = originalName,
                        ImageType = "file",
                        FileSize = file.Length,
                        Sequence = sequence++,
                        CreatedDate = DateTime.UtcNow,
                        Version = version
                    };

                    SetForeignKey(entity, parentId);
                    images.Add(entity);
                }
            }
            if (imageUrls?.Any() == true)
            {
                var folderName = GetFolderNameForEntity<TEntity>(parentId);

                foreach (var url in imageUrls)
                {
                    var trimmed = url.Trim();
                    string originalName;
                    int version = 1;
                    string blobFileName;

                    if (trimmed.StartsWith("data:image/"))
                    {
                        var commaIndex = trimmed.IndexOf(',');
                        var base64Data = trimmed.Substring(commaIndex + 1);
                        var bytes = Convert.FromBase64String(base64Data);

                        originalName = $"Captured_Image_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jpg";
                        version = CalculateVersion(originalName, existingImages, versionTracker);

                        try
                        {
                            var formFile = ConvertBase64ToFormFile(bytes, originalName, "image/jpeg");
                            blobFileName = await _blobStorageService.UploadFileAsync(formFile, folderName);
                            uploadedFileNames.Add(blobFileName);
                            _logger.LogInformation($"✅ Uploaded base64 image to Blob: {blobFileName}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"❌ Failed to upload base64 image");
                            throw new Exception($"Failed to upload captured image: {ex.Message}");
                        }

                        var entity = new TEntity
                        {
                            ImageFileName = blobFileName,
                            OriginalName = originalName,
                            ImageType = "file",
                            FileSize = bytes.Length,
                            Sequence = sequence++,
                            CreatedDate = DateTime.UtcNow,
                            Version = version
                        };

                        SetForeignKey(entity, parentId);
                        images.Add(entity);
                    }
                    else
                    {
                        byte[] bytes;
                        try
                        {
                            bytes = await DownloadImageFromUrlAsync(trimmed, ct);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"❌ Failed to download image from URL: {trimmed}");
                            throw new Exception($"Failed to download image from URL: {ex.Message}");
                        }

                        originalName = ExtractFileNameFromUrl(trimmed);
                        version = CalculateVersion(originalName, existingImages, versionTracker);

                        try
                        {
                            var formFile = ConvertBytesToFormFile(bytes, originalName, "image/jpeg");
                            blobFileName = await _blobStorageService.UploadFileAsync(formFile, folderName);
                            uploadedFileNames.Add(blobFileName);
                            _logger.LogInformation($"✅ Uploaded URL image to Blob: {blobFileName}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"❌ Failed to upload downloaded image");
                            throw new Exception($"Failed to upload image from URL: {ex.Message}");
                        }

                        var entity = new TEntity
                        {
                            ImageFileName = blobFileName,
                            OriginalName = originalName,
                            ImageType = "url",
                            FileSize = bytes.Length,
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
               _logger.LogInformation($"💾 VERSION DEBUG - Saving {images.Count} images to database");
                await _unitOfWork.Repository<TEntity>().AddRangeAsync(images, ct);
                _logger.LogInformation($"✅ VERSION DEBUG - Successfully saved {images.Count} images");
            }

            return (true, string.Empty);
        }
        private void SetForeignKey<TEntity>(TEntity entity, Guid parentId)
        {
            var config = _factory.GetConfig<TEntity>();
            var fkProp = typeof(TEntity).GetProperty(config.ForeignKeyName);

            fkProp?.SetValue(entity, parentId);
        }

        private string GetFolderNameForEntity<TEntity>(Guid parentId) where TEntity : BaseImageEntity
        {
            var entityType = typeof(TEntity).Name.Replace("Image", "").ToLower();
            return $"{entityType}s/{parentId}"; 
        }

        private IFormFile ConvertBase64ToFormFile(byte[] bytes, string fileName, string contentType)
        {
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        private IFormFile ConvertBytesToFormFile(byte[] bytes, string fileName, string contentType)
        {
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        private string GetFileExtension(string contentType)
        {
            return contentType?.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/webp" => ".webp",
                _ => ".jpg"
            };
        }

        private string ExtractFileNameFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var fileName = Path.GetFileName(uri.LocalPath);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = $"Downloaded_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jpg";
                }
                return fileName;
            }
            catch
            {
                return $"Downloaded_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jpg";
            }
        }

        private async Task<byte[]> DownloadImageFromUrlAsync(string url, CancellationToken ct)
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            return await httpClient.GetByteArrayAsync(url, ct);
        }

        private Expression<Func<TEntity, bool>> BuildForeignKeyPredicate<TEntity>(
            string foreignKeyName,
            Guid parentId) where TEntity : BaseImageEntity
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, foreignKeyName);
            var constant = Expression.Constant(parentId);
            var equals = Expression.Equal(property, constant);
            return Expression.Lambda<Func<TEntity, bool>>(equals, parameter);
        }
        private int CalculateVersion<TEntity>(
        string originalName,
        IReadOnlyList<TEntity> existingImages,
        Dictionary<string, int> versionTracker) where TEntity : BaseImageEntity
        {
            int version = 1;
            var originalNameLower = originalName.ToLower();

            _logger.LogInformation($"🔍 Calculating version for: {originalName}");

            // Check existing database records
            var sameNameImages = existingImages
                .Where(img => img.OriginalName != null &&
                             img.OriginalName.ToLower() == originalNameLower)
                .ToList();

            if (sameNameImages.Any())
            {
                var maxExistingVersion = sameNameImages.Max(img => img.Version);
                version = maxExistingVersion + 1;
                _logger.LogInformation($"🔍 Found {sameNameImages.Count} existing files, max version: V{maxExistingVersion}");
            }

            // Check if already processed in this batch
            if (versionTracker.ContainsKey(originalName))
            {
                var batchVersion = versionTracker[originalName];
                version = Math.Max(version, batchVersion + 1);
                _logger.LogInformation($"🔍 Already in batch with V{batchVersion}, updating to V{version}");
            }

            versionTracker[originalName] = version;
            return version;
        }
    }
     
    }
