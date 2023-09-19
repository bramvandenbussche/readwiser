using System.Collections.Concurrent;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public class StorageClientFactory : IStorageClientFactory
    {
        private readonly IStorageAccountSettings _settings;

        private readonly ConcurrentDictionary<string, TableClient> _tableClients =
            new ConcurrentDictionary<string, TableClient>();

        private readonly TableServiceClient _tableServiceClient;

        public StorageClientFactory(IStorageAccountSettings settings)
        {
            _settings = settings;
            _tableServiceClient = new TableServiceClient(_settings.ConnectionString);
        }

        public StorageClientFactory(string connectionString)
        {
            _tableServiceClient = new TableServiceClient(connectionString);
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
            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            var containers = blobServiceClient.GetBlobContainers();
            var container = containers.FirstOrDefault(x => x.Name == containerName);
            if (container == null)
            {
                return await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            return new BlobContainerClient(_settings.ConnectionString, containerName);
        }


    }
}