using System.Threading.Tasks;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.domain.Interface.Business;

public interface INoteMigrationService
{
    Task<ImportResult> ImportAll(string sourceConnectionString, int timestamp);
}