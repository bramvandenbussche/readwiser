using System.Diagnostics;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.Dto;

public static class DtoExtensions
{
    public static Highlight ToDomain(this HighlightDto dto)
    {
        return new Highlight()
        {
            NoteId = Guid.NewGuid(),
            RaisedTime = DateTimeOffset.UtcNow,
            Author = dto.Author,
            Title = dto.Title,
            Chapter = dto.Chapter,
            Text = dto.Text
        };
    }

}