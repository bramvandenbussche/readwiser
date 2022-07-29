namespace bramvandenbussche.readwiser.api.Domain;

public class Highlight : AbstractNote
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public string Chapter { get; set; }

    public override string PartitionKey => $"{Author.ToLower().Replace(" ", "_")}-{Title.ToLower().Replace(" ", "_")}";
}

public abstract class AbstractNote : INote
{
    public Guid NoteId { get; set; } = Guid.NewGuid();
    public abstract string PartitionKey { get; }
    public DateTimeOffset RaisedTime { get; set; } = DateTimeOffset.UtcNow;
}