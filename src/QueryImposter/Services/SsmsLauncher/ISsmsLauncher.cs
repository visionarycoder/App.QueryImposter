using vc.QueryImposter.Ifx;

namespace vc.QueryImposter.Services.SsmsLauncher;

public interface ISsmsLauncher
{
    void Launch(DatabaseConfig db);
}