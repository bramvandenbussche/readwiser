using System.Diagnostics;
using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.Dto;

public static class DtoExtensions
{
    public static Highlight ToDomain(this CreateHighlightRequestDto.HighlightDto dto)
    {
        return new Highlight()
        {
            NoteId = Guid.NewGuid(),
            RaisedTime = DateTimeOffset.UtcNow,
            Author = dto.Author,
            Title = dto.Title,
            Chapter = dto.Chapter ?? string.Empty,
            Text = dto.Text,
            Note = dto.Note
        };
    }

    public static HighlightResponseDto ToDto(this IEnumerable<Highlight> data)
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
                    Highlights = x.Select(h => h.ToDto()).ToList()
                })
                .ToList()
        };
    }

    public static HighlightResponseDto.HighlightDto ToDto(this Highlight value)
    {
        return new HighlightResponseDto.HighlightDto()
        {
            Id = value.NoteId,
            HighlightText = value.Text,
            NoteText = value.Note,
            Location = value.Chapter,
            LocationSort = value.Chapter.FindChapterNumber(),
            Timestamp = value.RaisedTime.ToString("s")
        };
    }

}