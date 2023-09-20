using System;

namespace bramvandenbussche.readwiser.domain.Model;

public class Book : AbstractDataRecord
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    public override string PartitionKey => "books"; //GetPartitionKey(Title, Author);
}