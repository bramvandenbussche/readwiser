using System.Text.RegularExpressions;

namespace bramvandenbussche.readwiser.domain.Model;

public class Highlight : AbstractDataRecord
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

    public override string PartitionKey => GetPartitionKey(Title, Author);

    public static string GetPartitionKey(string title, string author)
    {
        var rgx = new Regex("[^a-zA-Z_]");

        return $"{rgx.Replace(author.ToLower().Replace(" ", "_"), "")}-{rgx.Replace(title.ToLower().Replace(" ", "_"), "")}";
    }
}