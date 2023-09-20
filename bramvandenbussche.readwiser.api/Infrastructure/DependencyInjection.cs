using bramvandenbussche.readwiser.data.tablestorage.Interface;
using bramvandenbussche.readwiser.domain.Model;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
       services.Scan(scan => scan
            .FromAssemblyOf<Program>()
            .AddClasses()
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

       services.Scan(scan => scan
           .FromAssemblyOf<Highlight>()
           .AddClasses()
           .AsSelf()
           .AsImplementedInterfaces()
           .WithSingletonLifetime());

       services.Scan(scan => scan
           .FromAssemblyOf<IStorageAccountSettings>()
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