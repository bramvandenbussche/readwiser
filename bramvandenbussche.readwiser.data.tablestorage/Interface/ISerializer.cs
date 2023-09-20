using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.data.tablestorage.Interface
{
    public interface ISerializer
    {
        Task<TableEntity> Serialize(IDataRecord dataRecord);
        Task<IDataRecord> Deserialize(TableEntity entity, Type dataRecordType);
    }
}