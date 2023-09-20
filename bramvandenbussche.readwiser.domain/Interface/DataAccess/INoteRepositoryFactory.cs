namespace bramvandenbussche.readwiser.domain.Interface.DataAccess;

public interface INoteRepositoryFactory
{
    INoteRepository Create(string connectionstring);
    INoteRepository Create();
}