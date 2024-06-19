using System.Collections.Generic;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Interface.DataAccess;

public interface INoteRepository
{
    /// <summary>
    /// Retrieve all notes in the database
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public Task<IEnumerable<Highlight>> GetAll(int timestamp);

    /// <summary>
    /// Retrieve all notes related to a given book
    /// </summary>
    /// <param name="title">Title of the book</param>
    /// <param name="author">Author of the book</param>
    /// <returns></returns>
    public Task<IEnumerable<Highlight>> GetForBook(string title, string author);

    /// <summary>
    /// Create a new note
    /// </summary>
    public Task Save(Highlight note);

    /// <summary>
    /// Retrieve a list of all authors for which notes exit
    /// </summary>
    public Task<IEnumerable<string>> GetAllAuthors();

    /// <summary>
    /// Retrieve all notes for a given author
    /// </summary>
    /// <param name="author">The author to retrieve books for</param>
    /// <returns></returns>
    public Task<IEnumerable<Highlight>> GetNotesForAuthor(string author);

    /// <summary>
    /// Retrieve a list of the most recent highlights
    /// </summary>
    /// <param name="amount">The amount of items to retrieve</param>
    public Task<IEnumerable<Highlight>> GetRecent(int amount);
    
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


    public Task<List<string>?> GetAllTags();
    public Task AddTag(string noteId, string tag);
    public Task RemoveTag(string noteId, string tag);
}