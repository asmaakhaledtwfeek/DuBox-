using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName = null);
        string GetFileUrl(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<bool> FileExistsAsync(string fileName);
        Task<List<string>> ListFilesAsync(string folderName = null, CancellationToken cancellationToken = default);
    }
}
