using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using bramvandenbussche.readwiser.data.tablestorage.Interface;

namespace bramvandenbussche.readwiser.data.tablestorage
{
    public class StorageClientFactory : IStorageClientFactory
    {
        private readonly string _connectionstring;

        private readonly ConcurrentDictionary<string, TableClient> _tableClients =
            new ConcurrentDictionary<string, TableClient>();

        private readonly TableServiceClient _tableServiceClient;

        public StorageClientFactory(IStorageAccountSettings settings) : this(settings.ConnectionString) { }

        public StorageClientFactory(string connectionString)
        {
            _connectionstring = connectionString;
            _tableServiceClient = new TableServiceClient(_connectionstring);
        }

        public Task<TableClient> GetTableClient(string tableName)
        {
            return Task.FromResult(_tableClients.GetOrAdd(tableName, name =>
            {
                var tableClient = _tableServiceClient.GetTableClient(tableName);

                tableClient.CreateIfNotExists();

                return tableClient;
            }));
        }

        public async Task<BlobClient> GetBlobClient(string containerName, string blobName)
        {
            var container = await GetBlobContainerClient(containerName);
            return container.GetBlobClient(blobName);
        }

        private async Task<BlobContainerClient> GetBlobContainerClient(string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionstring);
            var containers = blobServiceClient.GetBlobContainers();
            var container = containers.FirstOrDefault(x => x.Name == containerName);
            if (container == null)
            {
                return await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            return new BlobContainerClient(_connectionstring, containerName);
        }
    }
}