using System.Collections.Concurrent;
using Azure.Data.Tables;
using bramvandenbussche.readwiser.api.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public class TableStorageData : IStorageWriter, IStorageReader
    {
        private readonly IStorageClientFactory _clientFactory;
        private readonly ISerializer _serializer;
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());

        public TableStorageData(ISerializer serializer, IStorageClientFactory clientFactory)
        {
            _serializer = serializer;
            _clientFactory = clientFactory;
        }

        public async Task Write(IDataRecord dataRecord)
        {
            var serialized = await _serializer.Serialize(dataRecord);
            var client = await _clientFactory.GetTableClient(GetTableName(dataRecord));
            await client.AddEntityAsync(serialized);
        }

        public Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, params Type[] recordTypes) =>
            GetOrderedNotes(partitionKeyValues, null, recordTypes);
        

        public async Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, DateTimeOffset? atSpecificTimeUtc,
            params Type[] recordTypes)
        {
            var result = new ConcurrentBag<IDataRecord>();

            var filter = string.Join(" or ", partitionKeyValues.Select(p => $"(PartitionKey eq '{p}')"));
            var timeFilter = atSpecificTimeUtc.HasValue
                ? $" and (Timestamp gt datetime'{atSpecificTimeUtc.Value.ToString(TableStorageData.DateTimeFormat)}')"
                : "";
            if (filter != "") filter = $"({filter}) and ";

            foreach (var recordType in recordTypes)
            {
                var tableName = GetTableName(recordType);
                var client = await _clientFactory.GetTableClient(tableName);

                var tableEntityCacheKey = $"{tableName}--{filter}--TableEntities";
                var recordsCacheKey = $"{tableName}--{filter}--Records";

                var tableEntities =
                    await _cache.GetOrCreateAsync(tableEntityCacheKey, _ => Task.FromResult(new ConcurrentDictionary<string, TableEntity>()));

                var records = await _cache.GetOrCreateAsync(recordsCacheKey, entry => Task.FromResult(new ConcurrentDictionary<string, IDataRecord>()));

                var latestCachedRecordTime = tableEntities.Any() ? tableEntities.Values.Max(x => x.Timestamp.GetValueOrDefault()) : new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                var newTableEntities = client.Query<TableEntity>($"{filter} (Timestamp gt datetime'{latestCachedRecordTime.ToString(DateTimeFormat)}') {timeFilter}");

                var addTasks = newTableEntities.Select(async tableEntity =>
                {
                    var key = $"{tableEntity.PartitionKey}|{tableEntity.RowKey}";
                    tableEntities.TryAdd(key, tableEntity);
                    var value = await _serializer.Deserialize(tableEntity, recordType);
                    records.GetOrAdd(key, _ => value);
                });
                await Task.WhenAll(addTasks);

                _cache.Set(tableEntityCacheKey, tableEntities, TimeSpan.FromMinutes(10));
                _cache.Set(recordsCacheKey, records, TimeSpan.FromMinutes(10));

                Parallel.ForEach(records.Values, r => result.Add(r));
            }
            

            return result
                .Where(x => !atSpecificTimeUtc.HasValue || x.RaisedTime >= atSpecificTimeUtc)
                .OrderBy(x => x.RaisedTime).ToArray();
        }




        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        #region DataRecordsToTableName
        private ConcurrentDictionary<Type, string> DataRecordsTableNameCache = new();
        private string GetTableName(IDataRecord dataRecord) =>
            DataRecordsTableNameCache.GetOrAdd(dataRecord.GetType(), type => dataRecord.TableName);

        private string GetTableName(Type recordType) =>
            DataRecordsTableNameCache.GetOrAdd(recordType, type =>
            {
                var theNote = (IDataRecord)Activator.CreateInstance(type)!;
                return theNote!.TableName;
            }); 
        #endregion
        
    }
}