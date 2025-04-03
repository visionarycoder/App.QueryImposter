using vc.QueryImposter.Ifx;

namespace vc.QueryImposter.Services.DatabaseConfigService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbConfigs"></param>
    public sealed class DbConfigService(IOptions<List<DatabaseConfig>> dbConfigs, IOptions<SecretConfig> secretConfigs) 
        : IDbConfigService
    {

        private readonly List<DatabaseConfig> dbConfigs = dbConfigs.Value;
        private readonly SecretConfig secretConfig = secretConfigs.Value;

        public IReadOnlyList<DatabaseConfig> GetAll() => dbConfigs;

        public void AddDatabaseConfig(DatabaseConfig cfg)
        {
            
            var newIndex = dbConfigs.Count; // Next array index
            WriteUserSecret($"Databases:{newIndex}:Name", cfg.Name);
            WriteUserSecret($"Databases:{newIndex}:ConnectionString", cfg.ConnectionString);
            WriteUserSecret($"Databases:{newIndex}:DomainUser", cfg.DomainUser);
            WriteUserSecret($"Databases:{newIndex}:Password", cfg.Password);

        }

        private void InitUserSecret(SecretConfig secretConfig)
        {

            var filename = "dotnet";
            var arguments = $"user-secrets init --project \"{secretConfig.ProjectName}\"";
            ExecuteProcess(filename, arguments);
        }

        private void WriteUserSecret(string key, string value)
        {

            var filename = "dotnet";
            var arguments = $"user-secrets set \"{key}\" \"{value}\" --project \"{secretConfig.ProjectName}\"";

            ExecuteProcess(filename, arguments);

        }

        private static void ExecuteProcess(string filename, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var proc = Process.Start(psi);
            proc?.WaitForExit();

            var output = proc.StandardOutput.ReadToEnd();
            var error = proc.StandardError.ReadToEnd();
            Console.WriteLine($"[user-secrets init output] {output}");
            Console.WriteLine($"[user-secrets init error] {error}");
        }

    }

}
