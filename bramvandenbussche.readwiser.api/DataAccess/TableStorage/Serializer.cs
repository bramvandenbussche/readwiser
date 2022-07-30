using System.Text.Json;
using Azure.Data.Tables;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public class Serializer : ISerializer
    {
        private readonly IStoreBigThings _storeBigThings;
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions();

        public Serializer(IStoreBigThings storeBigThings)
        {
            _storeBigThings = storeBigThings;
        }

        public async Task<TableEntity> Serialize(IDataRecord dataRecord)
        {
            var entity = new TableEntity(dataRecord.PartitionKey, dataRecord.RowId);

            var eventType = dataRecord.GetType();

            var rawData = JsonSerializer.Serialize(dataRecord, eventType, Options);
            if (rawData.Length > 30000)
            {
                var reference = await _storeBigThings.StoreBigString(dataRecord.TableName, dataRecord.NoteId, rawData);
                rawData = "Ref|" + reference;
            }

            entity.Add("RawData", rawData);
            entity.Add("TypeName", eventType.Name);
            entity.Add("TypeFullName", eventType.FullName);

            foreach (var property in eventType.GetProperties()
                         .Where(p => p.PropertyType.IsClass == false || p.PropertyType == typeof(string)))
            {
                var name = entity.ContainsKey(property.Name) ? $"prop_{property.Name}" : property.Name;

                var value = property.GetValue(dataRecord);

                if (property.PropertyType == typeof(DateTime))
                {
                    entity.Add(name, ((DateTime) value!).ToUniversalTime());
                }
                else if (property.PropertyType.IsEnum)
                {
                    entity.Add(name, value!.ToString());
                }
                else
                {
                    entity.Add(name, value);
                }
            }

            return entity;
        }

        public async Task<IDataRecord> Deserialize(TableEntity entity, Type eventType)
        {
            var rawData = (string)entity["RawData"];
            if (rawData.StartsWith("Ref|"))
            {
                var reference = rawData.Substring(4);
                rawData = await _storeBigThings.GetBigString(reference);
            }
            return ((IDataRecord)JsonSerializer.Deserialize(rawData, eventType, Options)!);
        }
    }
}