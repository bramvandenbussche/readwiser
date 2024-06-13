using bramvandenbussche.readwiser.data.mongodb;
using bramvandenbussche.readwiser.domain.Model;
using MongoDB.Thin;

namespace bramvandenbussche.readwiser.api.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        // Load executable / API
        services.Scan(scan => scan
             .FromAssemblyOf<Program>()
             .AddClasses()
             .AsSelf()
             .AsImplementedInterfaces()
             .WithSingletonLifetime());

        // Load Domain
        services.Scan(scan => scan
            .FromAssemblyOf<Highlight>()
            .AddClasses()
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        // Load DataAccess

        //  - TableStorage
        //services.Scan(scan => scan
        //    .FromAssemblyOf<IStorageAccountSettings>()
        //    .AddClasses()
        //    .AsSelf()
        //    .AsImplementedInterfaces()
        //    .WithSingletonLifetime());

        //  - MongoDb
        services.AddMongo(configuration["DataStore:MongoDb:ConnectionString"],
            configuration["DataStore:MongoDb:Database"]);
        services.Scan(scan => scan
            .FromAssemblyOf<MongoDbNoteRepository>()
            .AddClasses()
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());


    }
}