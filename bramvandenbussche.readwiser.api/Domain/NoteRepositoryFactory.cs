using bramvandenbussche.readwiser.api.DataAccess;
using bramvandenbussche.readwiser.api.DataAccess.TableStorage;

namespace bramvandenbussche.readwiser.api.Domain;

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