using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Interface.Business;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;
using Microsoft.Extensions.Logging;

namespace bramvandenbussche.readwiser.domain.Business;

public class HighlightService : IHighlightService
{
    private readonly INoteRepository _repository;
    private readonly ILogger<HighlightService> _logger;

    public HighlightService(INoteRepository repository, ILogger<HighlightService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Highlight>> GetAll(int timestamp)
    {
        var data = await _repository.GetAll(timestamp);

        return data;
    }

    public async Task<IEnumerable<Highlight>> GetForBook(string title, string author)
    {
        var data = await _repository.GetForBook(title, author);

        return data;
    }
    
    public async Task Add(IEnumerable<Highlight> notes)
    {
        // The cache is used to store the data already in storage for the books mentioned in the input
        var cache = new Dictionary<string, List<Highlight>>();
        var makeKey = (string author, string title) => $"{author}-{title}";

        foreach (var note in notes)
        {
            // Get all existing notes for book
            var key = makeKey(note.Author, note.Title);
            if (!cache.ContainsKey(key))
            {
                var data = await _repository.GetForBook(note.Title, note.Author);
                cache.Add(key, data.ToList());
            }

            // Prevent duplicates
            if (!cache[key].Any(x => x.Chapter == note.Chapter && x.Text == note.Text))
            {
                var value = note;
                cache[key].Add(value);
                await _repository.Save(value);
            }
        }
    }
}