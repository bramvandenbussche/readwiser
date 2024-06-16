using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.api.Contract;

public static class ContractExtensions
{
    public static Highlight ToDomain(this CreateHighlightRequest.HighlightDto dto)
    {
        return new Highlight()
        {
            RaisedTime = DateTimeOffset.UtcNow,
            Author = dto.Author,
            Title = dto.Title,
            Chapter = dto.Chapter ?? string.Empty,
            Text = dto.Text,
            Note = dto.Note
        };
    }

    public static IEnumerable<Highlight> ToDomain(this IEnumerable<CreateHighlightRequest.HighlightDto> dto) => dto.Select(ToDomain);
}