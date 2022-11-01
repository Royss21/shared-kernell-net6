
namespace SharedKernell.Helpers
{
    using Azure.Storage.Blobs;
    public static class StorageBlobHelper
    {
        public static async Task UploadAsync(string connectionString, string blobContainerName, string fileName,
            string fileBase64)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, blobContainerName);

            BlobClient blobClient = container.GetBlobClient(fileName);
            byte[] fileBytes = Convert.FromBase64String(fileBase64);

            await blobClient.UploadAsync(BinaryData.FromBytes(fileBytes));
        }

        public static async Task<bool> RemoveAsync(string connectionString, string blobContainerName, string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, blobContainerName);
            BlobClient blobClient = container.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public static async Task<byte[]> DownloadBlobArray(string connectionString, string blobContainerName,
            string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, blobContainerName);
            BlobClient blobClient = container.GetBlobClient(fileName);

            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            
            return stream.ToArray();
        }

        public static async Task<byte[]> DownloadBlobArray(BlobClient blobClient)
        {
            await using var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            return stream.ToArray();
        }

        internal static BlobClient GetBlobClient(string connectionString, string blobContainerName, string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, blobContainerName);
            BlobClient blobClient = container.GetBlobClient(fileName);

            return blobClient;
        }
    }
}