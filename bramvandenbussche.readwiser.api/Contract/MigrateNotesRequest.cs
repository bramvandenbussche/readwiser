namespace bramvandenbussche.readwiser.api.Contract;

public class MigrateNotesRequest : IApiRequest
{
    /// <summary>
    /// The connection string of the storage location of the source data
    /// </summary>
    public string SourceConnection { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp from which to get the notes from.
    /// Use 0 for all notes.
    /// </summary>
    public int Timestamp { get; set; }

    /// <summary>
    /// Verifies that the request is valid
    /// </summary>
    public bool IsValid => !string.IsNullOrEmpty(SourceConnection);
    
}