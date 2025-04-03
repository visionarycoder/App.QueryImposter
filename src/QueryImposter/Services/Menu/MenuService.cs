using vc.QueryImposter.Ifx;
using vc.QueryImposter.Services.DatabaseConfigService;
using vc.QueryImposter.Services.SsmsLauncher;

namespace vc.QueryImposter.Services.Menu;

public sealed class MenuService(IDbConfigService dbConfigs, ISsmsLauncher ssms) : IMenuService
{

    private readonly IDbConfigService dbConfigs = dbConfigs;

    public void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Query Imposter ===");
            Console.WriteLine("1] List & Use Existing Databases");
            Console.WriteLine("2] Add a New Database");
            Console.WriteLine("3] Exit");
            Console.Write("Selection: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UseExistingDbs();
                    break;
                case "2":
                    AddNewDbEntry();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private void UseExistingDbs()
    {
        var list = dbConfigs.GetAll();
        if (list.Count == 0)
        {
            Console.WriteLine("No databases found in user secrets.");
            return;
        }

        for (var i = 0; i < list.Count; i++)
            Console.WriteLine($"{i + 1}. {list[i].Name}");

        Console.Write("Choice: ");
        var input = Console.ReadLine() ?? "";
        if (!int.TryParse(input, out var idx) || idx < 1 || idx > list.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        var selected = list[idx - 1];
        ssms.Launch(selected);
    }

    private void AddNewDbEntry()
    {
        Console.Write("Enter DB name/label: ");
        var name = Console.ReadLine() ?? "";

        Console.Write("Enter ConnectionString (server name): ");
        var conn = Console.ReadLine() ?? "";

        Console.Write("Enter Domain\\User: ");
        var user = Console.ReadLine() ?? "";

        Console.Write("Enter Password: ");
        var pwd = ReadPasswordMasked();

        var cfg = new DatabaseConfig(name, conn, user, pwd);
        dbConfigs.AddDatabaseConfig(cfg);

        Console.WriteLine("New DB entry added. Re-run or reload config to see changes immediately.");
    }

    private string ReadPasswordMasked()
    {
        var pwd = string.Empty;
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) break;

            if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
            {
                pwd = pwd[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                pwd += key.KeyChar;
                Console.Write("*");
            }
        }
        Console.WriteLine();
        return pwd;
    }
}