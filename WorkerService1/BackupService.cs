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

        public BackupService(WorkerOptions options)
        {
            _options = options;
        }

        public void Backup()
        {

            foreach (string path in _options.Files)
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        Log.Error("Soubor neexistuje. {path}", path);
                        return;
                    }

                    Log.Information("Záloha provedena v čase: {time} a soubor {dir} uložen do {backupDir}", DateTimeOffset.Now, path.ToString(), _options.BackupDirectory);
                    string ext = Path.GetExtension(path.ToString());
                    File.Copy(path.ToString(), Path.Combine(_options.BackupDirectory + Path.GetFileNameWithoutExtension(path.ToString()) + "-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ext), true);

                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }

            }
        }
    }
}
