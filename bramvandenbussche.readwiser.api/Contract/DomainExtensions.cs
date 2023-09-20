using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.api.Contract;

public static class DomainExtensions
{
    public static HighlightResponseDto ToApiContract(this IEnumerable<Highlight> data)
    {
        return new HighlightResponseDto()
        {
            Books = data
                .GroupBy(x => new { x.Author, x.Title })
                .Select(x => new HighlightResponseDto.BookDto()
                {
                    Id = Guid.NewGuid(),
                    Author = x.Key.Author,
                    Title = x.Key.Title,
                    Highlights = x.Select(h => h.ToApiContract()).ToList()
                })
                .ToList()
        };
    }

    public static HighlightResponseDto.HighlightDto ToApiContract(this Highlight data)
    {
        return new HighlightResponseDto.HighlightDto()
        {
            Id = data.NoteId,
            HighlightText = data.Text,
            NoteText = data.Note,
            Location = data.Chapter,
            LocationSort = data.Chapter.FindChapterNumber(),
            Timestamp = data.RaisedTime.ToString("s")
        };
    }
}