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
    public Task<IEnumerable<Highlight>> GetRecentHighlights(int amount);

    /// <summary>
    /// Updates a highlight object.
    /// Only the Note property can be updated.
    /// </summary>
    /// <param name="highlight"></param>
    /// <returns></returns>
    public Task UpdateHighlight(Highlight highlight);

    /// <summary>
    /// Remove highlight from database
    /// </summary>
    /// <param name="noteId">Id of the record to delete</param>
    public Task DeleteHighlight(string noteId);

    public Task<List<string>> GetAllTags();

    /// <summary>
    /// Add a tag to a note
    /// </summary>
    /// <param name="noteId">The ID of the note</param>
    /// <param name="tag">The tag to add</param>
    public Task AddTag(string noteId, string tag);

    /// <summary>
    /// Remove a tag from a note
    /// </summary>
    /// <param name="noteId">The ID of the note</param>
    /// <param name="tag">The tag to remove</param>
    /// <returns></returns>
    public Task RemoveTag(string noteId, string tag);
}