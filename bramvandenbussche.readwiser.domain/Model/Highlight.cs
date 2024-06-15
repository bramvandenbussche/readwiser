using System;
using System.Text.RegularExpressions;

namespace bramvandenbussche.readwiser.domain.Model;

public class Highlight
{
    /// <summary>
    /// Book title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Book author
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The highlighted text
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Optional note
    /// </summary>
    public string? Note { get; set; }
    
    /// <summary>
    /// The chapter where the highlight was made
    /// </summary>
    public string Chapter { get; set; } = string.Empty;

    public DateTimeOffset RaisedTime { get; set; } = DateTimeOffset.UtcNow;

    public Guid NoteId { get; set; }
    
    public string SortOrder { get; set; }
}