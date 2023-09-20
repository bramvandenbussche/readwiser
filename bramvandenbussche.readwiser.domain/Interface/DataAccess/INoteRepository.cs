using System.Collections.Generic;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Interface.DataAccess;

public interface INoteRepository
{
    public Task<IEnumerable<Highlight>> GetAll(int timestamp);
    public Task<IEnumerable<Highlight>> GetForBook(string title, string author);

    public Task Save(IDataRecord dataRecord);
}