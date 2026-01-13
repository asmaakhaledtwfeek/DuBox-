using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, IFormFile file, string folderName = null);
        string GetImageUrl(string containerName, string fileName, int expiryMinutes = 60);
        Task<Stream> DownloadFileAsync(string containerName, string fileName);
        Task<bool> DeleteFileAsync(string containerName, string fileName);
        Task<bool> FileExistsAsync(string containerName, string fileName);
        Task<List<string>> ListFilesAsync(string containerName, string folderName = null, CancellationToken cancellationToken = default);
    }
}
