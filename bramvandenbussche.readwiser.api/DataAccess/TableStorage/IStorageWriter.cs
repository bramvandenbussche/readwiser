using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public interface IStorageWriter
{
    Task Write(IDataRecord dataRecord);
}

public interface IStorageReader
{
    Task<IDataRecord[]> GetNotes(string[] partitionKeyValues, params Type[] noteTypes);
}