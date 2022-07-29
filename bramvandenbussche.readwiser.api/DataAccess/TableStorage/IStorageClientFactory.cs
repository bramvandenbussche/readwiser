using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public interface IStorageClientFactory
    {
        Task<TableClient> GetTableClient(string tableName);
        Task<BlobClient> GetBlobClient(string containerName, string blobName);
    }
}