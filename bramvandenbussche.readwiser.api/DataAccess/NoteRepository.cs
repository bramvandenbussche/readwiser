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

    public async Task<IEnumerable<Highlight>> GetAll()
    {
        var data = await _reader.GetOrderedNotes(new string[]{ }, typeof(Highlight));

        return data.Select(d => d as Highlight)!;
    }

    public async Task<IEnumerable<Highlight>> GetForBook(string title, string author)
    {
        var data = await _reader.GetOrderedNotes(new string[] { Highlight.GetPartitionKey(title, author) }, typeof(Highlight));

        return data.Select(d => d as Highlight)!;
    }

    public Task Save(IDataRecord dataRecord)
    {
        return _writer.Write(dataRecord);
    }
}