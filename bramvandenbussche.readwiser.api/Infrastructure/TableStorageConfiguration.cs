using bramvandenbussche.readwiser.data.tablestorage.Interface;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public class TableStorageConfiguration : IStorageAccountSettings
{
    public string ConnectionString { get; set; }

    public TableStorageConfiguration(IConfiguration configuration)
    {
        ConnectionString = configuration["DataStore:TableStorage:ConnectionString"];
    }
}