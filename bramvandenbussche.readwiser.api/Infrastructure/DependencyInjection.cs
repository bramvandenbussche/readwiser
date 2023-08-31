using bramvandenbussche.readwiser.api.DataAccess.TableStorage;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
       services.Scan(scan => scan
            .FromAssemblyOf<IStorageWriter>()
            .AddClasses()
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        
    }
}

public class TableStorageConfiguration : IStorageAccountSettings
{
    public string ConnectionString { get; set; }
    
    public TableStorageConfiguration(IConfiguration configuration)
    {
        ConnectionString = configuration["DataStore:TableStorage.ConnectionString"];
    }
}