using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EventEase.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureStorage:ConnectionString"] ?? string.Empty;
            _containerName = configuration["AzureStorage:ContainerName"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("AzureStorage:ConnectionString is missing from appsettings.json.");
            }

            if (string.IsNullOrWhiteSpace(_containerName))
            {
                throw new InvalidOperationException("AzureStorage:ContainerName is missing from appsettings.json.");
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2023_11_03);

            var containerClient = new BlobContainerClient(
                _connectionString,
                _containerName,
                options
            );

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var blobClient = containerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();

            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2023_11_03);

            var containerClient = new BlobContainerClient(
                _connectionString,
                _containerName,
                options
            );

            string fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}