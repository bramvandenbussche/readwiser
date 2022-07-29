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

        public async Task Write(INote note)
        {
            var serialized = await _serializer.Serialize(note);
            var client = await _clientFactory.GetTableClient(GetTableName(note));
            await client.AddEntityAsync(serialized);
        }

        public Task<INote[]> GetOrderedEvents(string[] partitionKeyValues, params Type[] eventTypes) =>
            GetOrderedEvents(partitionKeyValues, null, eventTypes);


        
        public async Task<INote[]> GetOrderedEvents(string[] partitionKeyValues, DateTimeOffset? atSpecificTimeUtc,
            params Type[] eventTypes)
        {
            var result = new ConcurrentBag<INote>();

            var filter = string.Join(" or ", partitionKeyValues.Select(p => $"(PartitionKey eq '{p}')"));
            var timeFilter = atSpecificTimeUtc.HasValue
                ? $" and (Timestamp lt datetime'{atSpecificTimeUtc.Value.ToString(TableStorageData.DateTimeFormat)}')"
                : "";
            if (filter != "") filter = $"({filter}) and ";

            foreach (var eventType in eventTypes)
            {
                var tableName = GetTableName(eventType);
                var client = await _clientFactory.GetTableClient(tableName);

                var tableEntityCacheKey = $"{tableName}--{filter}--TableEntities";
                var eventsCacheKey = $"{tableName}--{filter}--Events";

                var tableEntities =
                    await _cache.GetOrCreateAsync(tableEntityCacheKey, _ => Task.FromResult(new ConcurrentDictionary<string, TableEntity>()));

                var events = await _cache.GetOrCreateAsync(eventsCacheKey, entry => Task.FromResult(new ConcurrentDictionary<string, INote>()));

                var latestCachedEventTime = tableEntities.Any() ? tableEntities.Values.Max(x => x.Timestamp.GetValueOrDefault()) : new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                var newTableEntities = client.Query<TableEntity>($"{filter} (Timestamp gt datetime'{latestCachedEventTime.ToString(DateTimeFormat)}') {timeFilter}");

                var addTasks = newTableEntities.Select(async tableEntity =>
                {
                    var key = $"{tableEntity.PartitionKey}|{tableEntity.RowKey}";
                    tableEntities.TryAdd(key, tableEntity);
                    var value = await _serializer.Deserialize(tableEntity, eventType);
                    events.GetOrAdd(key, _ => value);
                });
                await Task.WhenAll(addTasks);

                _cache.Set(tableEntityCacheKey, tableEntities, TimeSpan.FromMinutes(10));
                _cache.Set(eventsCacheKey, events, TimeSpan.FromMinutes(10));

                Parallel.ForEach(events.Values, e => result.Add(e));
            }
            

            return result
                .Where(x => !atSpecificTimeUtc.HasValue || x.RaisedTime <= atSpecificTimeUtc)
                .OrderBy(x => x.RaisedTime).ToArray();
        }




        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        #region EventsToTableName
        private ConcurrentDictionary<Type, string> EventsTableNameCache = new();
        private string GetTableName(INote note) =>
            EventsTableNameCache.GetOrAdd(note.GetType(), type => note.TableName);

        private string GetTableName(Type eventType) =>
            EventsTableNameCache.GetOrAdd(eventType, type =>
            {
                var theNote = (INote)Activator.CreateInstance(type);
                return theNote.TableName;
            }); 
        #endregion

        public Task<INote[]> GetNotes(string[] partitionKeyValues, params Type[] eventTypes)
        {
            throw new NotImplementedException();
        }
    }
}