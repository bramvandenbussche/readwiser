using System.Collections.Generic;
using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Interface.Business;

public interface IHighlightService
{
    Task<IEnumerable<Highlight>> GetAll(int timestamp);
    Task<IEnumerable<Highlight>> GetForBook(string title, string author);
    Task Add(IEnumerable<Highlight> notes);
}