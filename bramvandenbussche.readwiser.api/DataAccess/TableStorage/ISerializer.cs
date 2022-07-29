using Azure.Data.Tables;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess.TableStorage
{
    public interface ISerializer
    {
        Task<TableEntity> Serialize(INote note);
        Task<INote> Deserialize(TableEntity entity, Type eventType);
    }
}