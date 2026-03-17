using Lmbtfy.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        config.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddSingleton<DailyKeywordService>();
        services.AddSingleton<ShareUrlService>();
        services.AddSingleton<TinyUrlService>();
        services.AddSingleton<UnsplashClient>();
        services.AddSingleton<BackgroundImageService>();
    })
    .Build();

host.Run();