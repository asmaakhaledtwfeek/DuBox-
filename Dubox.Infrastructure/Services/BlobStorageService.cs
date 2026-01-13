using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Dubox.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _accountName;
        private readonly ILogger<BlobStorageService> _logger;
        private readonly ConcurrentDictionary<string, BlobContainerClient> _containerClients;
        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _logger = logger;
            _containerClients = new ConcurrentDictionary<string, BlobContainerClient>();

            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection String is not configured");

            // Extract account name
            var parts = connectionString.Split(';');
            _accountName = parts.FirstOrDefault(p => p.StartsWith("AccountName="))?.Split('=')[1];

            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        private async Task<BlobContainerClient> GetOrCreateContainerClientAsync(string containerName)
        {
            if (_containerClients.TryGetValue(containerName, out var existingClient))
            {
                return existingClient;
            }

            var client = _blobServiceClient.GetBlobContainerClient(containerName);
            
            try
            {
                // Try to create container with public access
                await client.CreateIfNotExistsAsync(PublicAccessType.None);
                _logger.LogInformation($"Container '{containerName}' ensured with public access");
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 403)
            {
                // If public access fails due to permissions, try creating without public access
                _logger.LogWarning($"Cannot set public access for container '{containerName}'. Attempting to create with private access.");
                try
                {
                    await client.CreateIfNotExistsAsync(PublicAccessType.None);
                    _logger.LogInformation($"Container '{containerName}' created with private access");
                }
                catch (Azure.RequestFailedException createEx) when (createEx.Status == 403)
                {
                    // Container might already exist, just log and continue
                    _logger.LogWarning($"Cannot create container '{containerName}'. It may already exist. Continuing...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error ensuring container '{containerName}' exists");
                throw;
            }

            _containerClients.TryAdd(containerName, client);
            return client;
        }
        
        private BlobContainerClient GetContainerClient(string containerName)
        {
            // For synchronous operations, return existing or create new client without ensuring container exists
            return _containerClients.GetOrAdd(containerName, name =>
            {
                return _blobServiceClient.GetBlobContainerClient(name);
            });
        }
        public async Task<string> UploadFileAsync(string containerName, IFormFile file, string folderName = null)
        {
            try
            {
                var containerClient = await GetOrCreateContainerClientAsync(containerName);

                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fileName = string.IsNullOrEmpty(folderName)
                    ? uniqueFileName
                    : $"{folderName.TrimEnd('/')}/{uniqueFileName}";

                var blobClient = containerClient.GetBlobClient(fileName);

                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType,
                    CacheControl = "public, max-age=31536000"
                };

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                _logger.LogInformation($"Successfully uploaded file: {fileName} to container: {containerName}");
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading file to container '{containerName}': {ex.Message}");
                throw;
            }
        }

        public string GetImageUrl(string containerName, string fileName, int expiryMinutes = 60)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (!blobClient.CanGenerateSasUri)
                throw new InvalidOperationException("Cannot generate SAS");

            return blobClient.GenerateSasUri(
                BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddHours(2)
            ).ToString();
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentException("File name is required");

                var containerClient = await GetOrCreateContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

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

        public async Task<bool> DeleteFileAsync(string containerName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                var containerClient = await GetOrCreateContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
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

        public async Task<bool> FileExistsAsync(string containerName, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                var containerClient = await GetOrCreateContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                return await blobClient.ExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if file exists: {fileName}");
                return false;
            }
        }

        public async Task<List<string>> ListFilesAsync(string containerName, string folderName = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerClient = await GetOrCreateContainerClientAsync(containerName);
                var files = new List<string>();
                var prefix = string.IsNullOrEmpty(folderName) ? null : folderName.TrimEnd('/') + "/";

                await foreach (var blobItem in containerClient.GetBlobsAsync(
                    BlobTraits.None,
                    BlobStates.None,
                    prefix: prefix,
                    cancellationToken: cancellationToken))
                {
                    files.Add(blobItem.Name);
                }

                _logger.LogInformation($"Listed {files.Count} file(s) in container '{containerName}'");

                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files");
                throw;
            }
        }

        public Task<List<string>> ListFilesAsync(string containerName, string folderName = null)
        {
            throw new NotImplementedException();
        }
    }

}

