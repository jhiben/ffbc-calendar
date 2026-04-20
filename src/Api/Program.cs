using FFBC.Options;
using FFBC.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddMemoryCache();
        services.AddHttpClient("ffbc");
        services.AddHttpClient("nominatim", client =>
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("FFBC-WebApp/1.0 (mountain-biking-events)");
        });
        services.Configure<FfbcEventStoreOptions>(
            context.Configuration.GetSection(FfbcEventStoreOptions.SectionName));
        services.AddSingleton<IEventStore, FfbcWebEventStore>();
        services.AddSingleton<IGeocodingService, NominatimGeocodingService>();
        services.AddSingleton<IEventDetailsService, FfbcEventDetailsService>();
    })
    .Build();

host.Run();
