using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace WorkerService1
{
    public class BackupService : IBackupService
    {
        private readonly WorkerOptions _options;
        private readonly ILogger<BackupService> _logger;

        public BackupService(WorkerOptions options, ILogger<BackupService> logger)
        {
            _options = options;
            _logger = logger;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(_options.BackupDirectory + "/logs/"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void Backup()
        {
            foreach (string path in _options.Files)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        Log.Information("Záloha provedena v čase: {time} a soubor {dir} uložen do {backupDir}", DateTimeOffset.Now, path.ToString(), _options.BackupDirectory);
                        string ext = Path.GetExtension(path.ToString());
                        File.Copy(path.ToString(), Path.Combine(_options.BackupDirectory + Path.GetFileNameWithoutExtension(path.ToString()) + "-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ext), true);
                    }
                    else Log.Error("Soubor neexistuje. {path}", path);

                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }

            }
        }
    }
}
