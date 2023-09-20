namespace bramvandenbussche.readwiser.domain.Model;

public class ImportResult
{
    public bool IsSucces { get; set; }
    public string Reason { get; set; } = string.Empty;

    public int Notes { get; set; }
    public int Books { get; set; }
}