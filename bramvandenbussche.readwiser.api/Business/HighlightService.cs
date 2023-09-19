using bramvandenbussche.readwiser.api.DataAccess;
using bramvandenbussche.readwiser.api.Domain;
using bramvandenbussche.readwiser.api.Dto;

namespace bramvandenbussche.readwiser.api.Business;

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
    
    public async Task Add(List<CreateHighlightRequestDto.HighlightDto> notes)
    {
        // The cache is used to store the data already in storage for the books mentioned in the input
        var cache = new Dictionary<string, List<Highlight>>();
        var makeKey = (string author, string title) => $"{author}-{title}";

        foreach (var note in notes)
        {
            var key = makeKey(note.Author, note.Title);
            if (!cache.ContainsKey(key))
            {
                var data = await _repository.GetForBook(note.Title, note.Author);
                cache.Add(key, data.ToList());
            }

            // Prevent duplicates
            if (!cache[key].Any(x => x.Chapter == note.Chapter && x.Text == note.Text))
            {
                var value = note.ToDomain();
                cache[key].Add(value);
                await _repository.Save(value);
            }
        }
    }
}