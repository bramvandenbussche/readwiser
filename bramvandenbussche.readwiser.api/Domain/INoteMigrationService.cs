using bramvandenbussche.readwiser.api.Dto;

namespace bramvandenbussche.readwiser.api.Domain;

public interface INoteMigrationService
{
    Task<ImportResult> ImportAll(string sourceConnectionString, int timestamp);
}