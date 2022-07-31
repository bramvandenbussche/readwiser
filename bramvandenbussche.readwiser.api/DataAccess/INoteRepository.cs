using bramvandenbussche.readwiser.api.Domain;

namespace bramvandenbussche.readwiser.api.DataAccess;

public interface INoteRepository
{
    public Task<IEnumerable<Highlight>> GetAll(int timestamp);
    public Task<IEnumerable<Highlight>> GetForBook(string title, string author);

    public Task Save(IDataRecord dataRecord);
}