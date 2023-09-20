using bramvandenbussche.readwiser.data.tablestorage.Interface;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;

namespace bramvandenbussche.readwiser.data.tablestorage;

public class NoteRepositoryFactory : INoteRepositoryFactory
{
    private readonly IStorageAccountSettings _settings;
    private readonly ISerializer _serializer;

    public NoteRepositoryFactory(IStorageAccountSettings settings, ISerializer serializer)
    {
        _settings = settings;
        _serializer = serializer;
    }

    public INoteRepository Create(string connectionstring)
    {
        var storageClientFactory = new StorageClientFactory(connectionstring);
        var tableStorageData = new TableStorageData(_serializer, storageClientFactory);
        return new NoteRepository(tableStorageData, tableStorageData);
    }

    public INoteRepository Create() => Create(_settings.ConnectionString);
}