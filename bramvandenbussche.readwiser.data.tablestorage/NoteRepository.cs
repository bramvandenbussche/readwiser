using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.data.tablestorage.Interface;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.data.tablestorage;

public class NoteRepository : INoteRepository
{
    private readonly IStorageReader _reader;
    private readonly IStorageWriter _writer;

    public NoteRepository(IStorageReader reader, IStorageWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<IEnumerable<Highlight>> GetAll(int timestamp)
    {
        IDataRecord[] data;
        if (timestamp > 0)
            data = await _reader.GetOrderedNotes(new string[] { }, DateTimeOffset.FromUnixTimeSeconds(timestamp), typeof(Highlight));
        else
            data = await _reader.GetOrderedNotes(new string[] { }, typeof(Highlight));

        return data.Select(d => d as Highlight)!;
    }

    public async Task<IEnumerable<Highlight>> GetForBook(string title, string author)
    {
        var data = await _reader.GetOrderedNotes(new string[] { Highlight.GetPartitionKey(title, author) }, typeof(Highlight));

        return data.Select(d => d as Highlight)!;
    }

    public Task Save(Highlight dataRecord)
    {
        return _writer.Write(dataRecord);
    }
}