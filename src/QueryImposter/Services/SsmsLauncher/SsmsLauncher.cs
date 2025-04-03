using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using vc.QueryImposter.Ifx;

namespace vc.QueryImposter.Services.SsmsLauncher
{
    public sealed class SsmsLauncher : ISsmsLauncher
    {
        public void Launch(DatabaseConfig db)
        {
            using var securePwd = new SecureString();
            foreach (var ch in db.Password) securePwd.AppendChar(ch);
            securePwd.MakeReadOnly();

            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe",
                Arguments = $"-S {db.ConnectionString}",
                UserName = db.DomainUser,
                Password = securePwd,
                UseShellExecute = false,
                LoadUserProfile = true
            };

            try
            {
                using var proc = Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch SSMS: {ex.Message}");
            }
        }
    }
}
