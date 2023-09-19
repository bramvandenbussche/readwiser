namespace bramvandenbussche.readwiser.api.Dto;

public class MigrateNotesRequest
{
    public string SourceConnection { get; set; }

    public int Timestamp { get; set; }
}