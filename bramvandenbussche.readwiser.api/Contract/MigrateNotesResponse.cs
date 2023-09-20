namespace bramvandenbussche.readwiser.api.Contract;

public class MigrateNotesResponse : ApiResponse
{
    public int BooksMigrated { get; set; }
    public int NotesMigrated { get; set; }
}