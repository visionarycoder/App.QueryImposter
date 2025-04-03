using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using vc.QueryImposter.Services.Menu;
using vc.QueryImposter.Services.SsmsLauncher;

// 1) Create Host
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Add user secrets
        config.AddUserSecrets(typeof(Program).Assembly);
    })
    .ConfigureServices((context, services) =>
    {
        // Bind "Databases" to a List<DatabaseConfig>
        services.Configure<List<DatabaseConfig>>(context.Configuration.GetSection("Databases"));

        // Register the services
        services.AddSingleton<IDbConfigService, DbConfigService>();
        services.AddSingleton<ISsmsLauncher, SsmsLauncher>();
        services.AddSingleton<IMenuService, MenuService>();
    })
    .Build();

// 2) Retrieve MenuService and show the menu
var menu = host.Services.GetRequiredService<IMenuService>();
menu.ShowMainMenu();