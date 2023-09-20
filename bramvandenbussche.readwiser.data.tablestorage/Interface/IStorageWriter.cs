using System;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.data.tablestorage.Interface;

public interface IStorageWriter
{
    Task Write(IDataRecord dataRecord);
}

public interface IStorageReader
{
    Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, params Type[] recordTypes);
    Task<IDataRecord[]> GetOrderedNotes(string[] partitionKeyValues, DateTimeOffset? startTime, params Type[] noteTypes);
}