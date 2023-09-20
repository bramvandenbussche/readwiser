namespace bramvandenbussche.readwiser.domain.Model;

public class ImportResult
{
    public bool IsSucces { get; set; }
    public string Reason { get; set; }

    public int Notes { get; set; }
    public int Books { get; set; }
}