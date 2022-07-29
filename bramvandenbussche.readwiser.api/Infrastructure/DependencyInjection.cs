using bramvandenbussche.readwiser.api.DataAccess.TableStorage;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var tableStorageConnectionString = configuration["EventStore:TableStorage.ConnectionString"];

        var encryptionKey = configuration["InformatEncryption:EncryptionKey"];
        var encryptionInitializationVector = configuration["InformatEncryption:InitializationVector"];
        var salt = configuration["InformatEncryption:Salt"];

        //services.AddTransient<IStorageAccountSettings>(_ => new TableStorageConfiguration(tableStorageConnectionString));

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
        ConnectionString = configuration["EventStore:TableStorage.ConnectionString"];
;
    }
}