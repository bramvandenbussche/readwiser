using bramvandenbussche.readwiser.data.tablestorage.Interface;
using bramvandenbussche.readwiser.domain.Model;

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

       services.Scan(scan => scan
           .FromAssemblyOf<Highlight>()
           .AddClasses()
           .AsSelf()
           .AsImplementedInterfaces()
           .WithSingletonLifetime());

        //  - TableStorage
        //services.Scan(scan => scan
        //    .FromAssemblyOf<IStorageAccountSettings>()
        //    .AddClasses()
        //    .AsSelf()
        //    .AsImplementedInterfaces()
        //    .WithSingletonLifetime());



    }
}