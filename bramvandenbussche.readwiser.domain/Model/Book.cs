using System;
using System.Collections.Generic;

namespace bramvandenbussche.readwiser.domain.Model;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public IList<Highlight> Highlights { get; set; } = new List<Highlight>();
}