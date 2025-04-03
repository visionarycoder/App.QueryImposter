using vc.QueryImposter.Ifx;

namespace vc.QueryImposter.Services.DatabaseConfigService;

public interface IDbConfigService
{
    IReadOnlyList<DatabaseConfig> GetAll();
    void AddDatabaseConfig(DatabaseConfig cfg);
}