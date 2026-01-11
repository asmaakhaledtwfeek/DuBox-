using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Dubox.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly string _baseUrl;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _logger = logger;

            try
            {
                var connectionString = configuration["AzureBlobStorage:ConnectionString"];
                var containerName = configuration["AzureBlobStorage:ContainerName"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentException("Connection String is not configured in settings");

                if (string.IsNullOrEmpty(containerName))
                    throw new ArgumentException("Container Name is not configured in settings");

                // Extract account name from Connection String
                var parts = connectionString.Split(';');
                var accountName = parts.FirstOrDefault(p => p.StartsWith("AccountName="))?.Split('=')[1];

                if (string.IsNullOrEmpty(accountName))
                    throw new ArgumentException("Account Name not found in Connection String");

                // Build Base URL
                _baseUrl = $"https://{accountName}.blob.core.windows.net/{containerName}";

                // Create Blob Service Client
                var blobServiceClient = new BlobServiceClient(connectionString);
                _containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Create Container if it doesn't exist (Public access for direct access)
                _containerClient.CreateIfNotExists(PublicAccessType.Blob);

                _logger.LogInformation("Successfully connected to Blob Storage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to Blob Storage");
                throw;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is empty or does not exist");

                // Create unique file name
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                // Add folder if provided
                var fileName = string.IsNullOrEmpty(folderName)
                    ? uniqueFileName
                    : $"{folderName.TrimEnd('/')}/{uniqueFileName}";

                var blobClient = _containerClient.GetBlobClient(fileName);

                // Set content type
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType,
                    CacheControl = "public, max-age=31536000" // Cache for 1 year
                };

                // Upload file
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                _logger.LogInformation($"File uploaded successfully: {fileName}");

                // Return file name (not full URL)
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw;
            }
        }

        public string GetFileUrl(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            return $"{_baseUrl}/{fileName}";
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentException("File name is required");

                var blobClient = _containerClient.GetBlobClient(fileName);

                if (!await blobClient.ExistsAsync())
                    throw new FileNotFoundException($"File {fileName} not found");

                var download = await blobClient.DownloadAsync();

                _logger.LogInformation($"File downloaded: {fileName}");

                return download.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading file: {fileName}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                var blobClient = _containerClient.GetBlobClient(fileName);
                var deleted = await blobClient.DeleteIfExistsAsync();

                if (deleted)
                    _logger.LogInformation($"File deleted: {fileName}");
                else
                    _logger.LogWarning($"File not found: {fileName}");

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file: {fileName}");
                throw;
            }
        }

        public async Task<bool> FileExistsAsync(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                var blobClient = _containerClient.GetBlobClient(fileName);
                return await blobClient.ExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if file exists: {fileName}");
                return false;
            }
        }

        public async Task<List<string>> ListFilesAsync(string folderName = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var files = new List<string>();
                var prefix = string.IsNullOrEmpty(folderName) ? null : folderName.TrimEnd('/') + "/";

                await foreach (var blobItem in _containerClient.GetBlobsAsync(
                    BlobTraits.None,
                    BlobStates.None,
                    prefix: prefix,
                    cancellationToken: cancellationToken))
                {
                    files.Add(blobItem.Name);
                }

                _logger.LogInformation($"Listed {files.Count} file(s)");

                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files");
                throw;
            }
        }
    }

}

