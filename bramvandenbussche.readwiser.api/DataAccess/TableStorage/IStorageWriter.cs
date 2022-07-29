using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage;

public interface IStorageWriter
{
    Task Write(INote note);
}

public interface IStorageReader
{
    Task<INote[]> GetNotes(string[] partitionKeyValues, params Type[] eventTypes);
}