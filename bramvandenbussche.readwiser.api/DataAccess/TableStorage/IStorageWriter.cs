using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public interface IStorageWriter
{
    Task Write(IDataRecord dataRecord);
}

public interface IStorageReader
{
    Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, params Type[] noteTypes);
    Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, DateTimeOffset startTime, params Type[] noteTypes);
}