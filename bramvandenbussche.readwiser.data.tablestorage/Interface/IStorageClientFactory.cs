using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace bramvandenbussche.readwiser.data.tablestorage.Interface
{
    public interface IStorageClientFactory
    {
        Task<TableClient> GetTableClient(string tableName);
        Task<BlobClient> GetBlobClient(string containerName, string blobName);
    }
}