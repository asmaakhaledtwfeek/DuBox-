using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        private BlobContainerClient GetContainerClient(string containerName)
        {
            return _containerClients.GetOrAdd(containerName, name =>
            {
                var client = _blobServiceClient.GetBlobContainerClient(name);
                client.CreateIfNotExists(PublicAccessType.Blob);
                return client;
            });
        }
        public async Task<string> UploadFileAsync(string containerName, IFormFile file, string folderName = null)
        {
            var containerClient = GetContainerClient(containerName);

            // Your existing upload logic here, but use containerClient
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

            return fileName;
        }

        public string GetFileUrl(string containerName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            return $"https://{_accountName}.blob.core.windows.net/{containerName}/{fileName}";
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string fileName)
        {
            var containerClient = GetContainerClient(containerName);
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentException("File name is required");

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
            var containerClient = GetContainerClient(containerName);
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

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
            var containerClient = GetContainerClient(containerName);

            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

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
            var containerClient = GetContainerClient(containerName);

            try
            {
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

                _logger.LogInformation($"Listed {files.Count} file(s)");

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

