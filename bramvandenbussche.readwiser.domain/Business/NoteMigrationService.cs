using System.Linq;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Interface.Business;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Business;

public class NoteMigrationService : INoteMigrationService
{
    private readonly INoteRepository _noteRepository;
    private readonly INoteRepositoryFactory _noteRepositoryFactory;

    public NoteMigrationService(INoteRepository noteRepository, INoteRepositoryFactory noteRepositoryFactory)
    {
        _noteRepository = noteRepository;
        _noteRepositoryFactory = noteRepositoryFactory;
    }

    public async Task<ImportResult> ImportAll(string sourceConnectionString, int timestamp)
    {
        var source = _noteRepositoryFactory.Create(sourceConnectionString);
        var target = _noteRepositoryFactory.Create();

        var notes = await source.GetAll(timestamp);

        var books = notes
            .GroupBy(note => new { Author = note.Author, Title = note.Title })
            .Select(grp => new { Author = grp.Key.Author, Title = grp.Key.Title, Notes = grp.ToList() });

        foreach (var book in books)
        {
            
        }

        return new ImportResult()
        {
            IsSucces = true,
            Notes = 10,
            Books = 10
        };
    }
}