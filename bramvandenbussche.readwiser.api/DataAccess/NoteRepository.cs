using bramvandenbussche.readwiser.api.DataAccess.TableStorage;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess;

public class NoteRepository : INoteRepository
{
    private readonly IStorageReader _reader;
    private readonly IStorageWriter _writer;

    public NoteRepository(IStorageReader reader, IStorageWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public Task<IEnumerable<INote>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Save(INote note)
    {
        return _writer.Write(note);
    }
}

public interface INoteRepository
{
    public Task<IEnumerable<INote>> GetAll();
    public Task Save(INote note);
}