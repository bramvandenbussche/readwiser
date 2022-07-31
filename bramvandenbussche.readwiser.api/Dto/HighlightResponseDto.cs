namespace bramvandenbussche.readwiser.api.Dto;

public class HighlightResponseDto
{
    public List<BookDto> Books { get; set; } = new();

    public class BookDto
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public List<HighlightDto> Highlights { get; set; } = new();
    }

    public class HighlightDto
    {
        public Guid Id { get; set; }

        public string Timestamp { get; set; } = string.Empty;
        public string HighlightText { get; set; } = string.Empty;
        public string? NoteText { get; set; }
        public string Location { get; set; } = string.Empty;
        public string LocationSort { get; set; } = string.Empty;
    }

}