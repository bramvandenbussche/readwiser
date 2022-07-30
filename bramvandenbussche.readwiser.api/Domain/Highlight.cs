namespace bramvandenbussche.readwiser.api.Domain;

public class Highlight : AbstractDataRecord
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public string Chapter { get; set; }

    public override string PartitionKey => GetPartitionKey(Title, Author);

    public static string GetPartitionKey(string title, string author) =>
        $"{author.ToLower().Replace(" ", "_")}-{title.ToLower().Replace(" ", "_")}";
}