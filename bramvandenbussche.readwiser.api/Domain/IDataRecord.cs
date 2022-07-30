namespace bramvandenbussche.readwiser.api.Domain;

public interface IDataRecord
{
    /// <summary>
    /// Unique Identifier for the event
    /// </summary>
    Guid NoteId { get; }

    /// <summary>
    /// Primary Index
    /// </summary>
    string TableName => GetType().Name;

    /// <summary>
    /// Secondary Index
    /// </summary>
    string PartitionKey { get; }

    /// <summary>
    /// Tertiary Index
    /// </summary>
    string RowId => NoteId.ToString();

    /// <summary>
    /// When did this shit raised?
    /// </summary>
    public DateTimeOffset RaisedTime { get; }
}