namespace bramvandenbussche.readwiser.api.Contract;

public class MigrateNotesRequest
{
    /// <summary>
    /// The connection string of the storage location of the source data
    /// </summary>
    public string SourceConnection { get; set; }

    /// <summary>
    /// The timestamp from which to get the notes from.
    /// Use 0 for all notes.
    /// </summary>
    public int Timestamp { get; set; }
}