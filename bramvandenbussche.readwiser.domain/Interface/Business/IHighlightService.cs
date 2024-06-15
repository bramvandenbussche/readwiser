using System.Collections.Generic;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Interface.Business;

public interface IHighlightService
{
    Task<IEnumerable<Highlight>> GetAll(int timestamp);
    Task<IEnumerable<Highlight>> GetForBook(string title, string author);
    Task Add(IEnumerable<Highlight> notes);

    /// <summary>
    /// Retrieve a list of all books for which notes exist for the given author
    /// </summary>
    /// <param name="author">The author to retrieve books for</param>
    public Task<IEnumerable<Book>> GetBooksForAuthor(string author);

    /// <summary>
    /// Retrieve a list of all authors for which notes are stored
    /// </summary>
    public Task<IEnumerable<string>> GetAllAuthors();

    /// <summary>
    /// Retrieve a list of all books for which notes exist
    /// </summary>
    public Task<IEnumerable<Book>> GetAllBooks();

    /// <summary>
    /// Retrieve a list of the most recent highlights
    /// </summary>
    /// <param name="amount">The amount of items to retrieve</param>
    Task<IEnumerable<Highlight>> GetRecentHighlights(int amount);
}