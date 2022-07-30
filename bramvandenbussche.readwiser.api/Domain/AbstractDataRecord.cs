namespace bramvandenbussche.readwiser.api.Domain;

public abstract class AbstractDataRecord : IDataRecord
{
    public Guid NoteId { get; set; } = Guid.NewGuid();
    public abstract string PartitionKey { get; }
    public DateTimeOffset RaisedTime { get; set; } = DateTimeOffset.UtcNow;
}