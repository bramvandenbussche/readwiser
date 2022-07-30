namespace bramvandenbussche.readwiser.api.Domain;

public class Highlight : AbstractDataRecord
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Chapter { get; set; } = string.Empty;

    public override string PartitionKey => GetPartitionKey(Title, Author);

    public static string GetPartitionKey(string title, string author) =>
        $"{author.ToLower().Replace(" ", "_")}-{title.ToLower().Replace(" ", "_")}";
}