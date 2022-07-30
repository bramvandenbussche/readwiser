namespace bramvandenbussche.readwiser.api.Dto;

public class HighlightResponseDto
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string HighlightText { get; set; }
    public string NoteText { get; set; }
    public string Location { get; set; }
    public string Timestamp { get; set; }
    public Guid Id { get; set; }
}