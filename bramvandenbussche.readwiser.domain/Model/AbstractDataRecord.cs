using System;

namespace bramvandenbussche.readwiser.domain.Model;

public abstract class AbstractDataRecord : IDataRecord
{
    public Guid NoteId { get; set; } = Guid.NewGuid();
    public abstract string PartitionKey { get; }
    public DateTimeOffset RaisedTime { get; set; } = DateTimeOffset.UtcNow;
}