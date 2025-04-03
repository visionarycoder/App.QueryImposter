using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Add user secrets for local dev
        config.AddUserSecrets(typeof(Program).Assembly);
    })
    .ConfigureServices((context, services) =>
    {
        // Bind a list of DatabaseConfig objects from config
        services.Configure<List<DatabaseConfig>>(context.Configuration.GetSection("Databases"));

        // Register our "launcher" service
        services.AddTransient<DatabaseLauncher>();
    })
    .Build();

// 2. Retrieve and run
var launcher = host.Services.GetRequiredService<DatabaseLauncher>();
launcher.ShowMainMenu();