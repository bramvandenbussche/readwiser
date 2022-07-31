using Azure.Data.Tables;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public interface ISerializer
    {
        Task<TableEntity> Serialize(IDataRecord dataRecord);
        Task<IDataRecord> Deserialize(TableEntity entity, Type dataRecordType);
    }
}