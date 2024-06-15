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
}